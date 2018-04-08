using Kros.TroubleShooterCommon;
using Kros.TroubleShooterCommon.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using ElipticCurves;

namespace Kros.TroubleShooterClient.Update
{
    /// <summary>
    /// Compares server files with clients files and updates changes.
    /// </summary>
    public class UpdateController
    {
        /// <summary>
        /// The update directory
        /// </summary>
        private string _updateDir;

        /// <summary>
        /// clients file version config file
        /// </summary>
        private const string VERSION_CONFIG_FILE = "FileVersions.json";

        /// <summary>
        /// Provider used to verify EcSignature.
        /// </summary>
        private ECSignature _verifier;

        /// <summary>
        /// Provider used to generate keypair. keypair is used in EcDiffieHelman key Exchange
        /// </summary>
        private ECKeysGenerator _keyGen;

        /// <summary>
        /// Provider used to derive common secret. See Eliptic Curve Diffie-Helman key Exchange
        /// </summary>
        private ECDiffieHelman _diffieHelman;

        /// <summary>
        /// The servers public key used to verify source file signature.
        /// </summary>
        private byte[] _signatureKey = Convert.FromBase64String("A0mbdQ20EsLzFyiFwr58QrdLFqmIAA==");

        /// <summary>
        /// Troubleshooter client
        /// </summary>
        private TroubleShooterClient _client;

        /// <summary>
        /// initialise updater.
        /// </summary>
        /// <param name="updateDir"></param>
        public UpdateController(string updateDir, TroubleShooterClient client)
        {
            this._client = client;
            _updateDir = updateDir;
            ElipticCurve curve = ElipticCurve.secp160r1();
            _keyGen = new ECKeysGenerator(curve);
            _verifier = new ECSignature(curve);
            _diffieHelman = new ECDiffieHelman(curve);
        }

        /// <summary>
        /// Run update if server is running.
        /// </summary>
        /// <returns>
        /// false - files are already up to date so no compilation is needed
        /// true - files changed - recompilation needed
        /// </returns>
        public bool Execute()
        {
            if (!Directory.Exists(_updateDir))
                Directory.CreateDirectory(_updateDir);
            List<SourceFileInfo> serverFiles = GetServerFiles().ToList();
            if (serverFiles == null)
                return false;
            List<SourceFileInfo> myFiles = GetClientFiles().ToList();
            if (CompareFiles(serverFiles, myFiles))
                return false;
            ActualizeFiles(myFiles, serverFiles);
            SaveConfig(myFiles);
            return true;
        }

        /// <summary>
        /// Compares server files versions with clients files versions
        /// </summary>
        /// <param name="myFiles">list of client files versions</param>
        /// <param name="serverFiles">list of server files versions</param>
        /// <returns>
        /// true - client files are already up to date
        /// false - actualisation needed
        /// </returns>
        private bool CompareFiles(List<SourceFileInfo> myFiles, List<SourceFileInfo> serverFiles)
        {
            if (myFiles.Count != serverFiles.Count)
                return false;
            if (!myFiles.All(mf => serverFiles.Exists(sf => sf.FileName == mf.FileName && sf.Version == mf.Version)))
                return false;
            if (!serverFiles.All(sf => myFiles.Exists(mf => sf.FileName == mf.FileName && sf.Version == mf.Version)))
                return false;
            return true;
        }

        /// <summary>
        /// Gets server source files with versions
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SourceFileInfo> GetServerFiles()
        {
            string uri = (TroubleShooterClient.SERVICE_PATH + "/updateInfo");
            HttpResponseMessage response = _client.GetAsync(uri).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<IEnumerable<SourceFileInfo>>().GetAwaiter().GetResult();
            else
                return null;
        }

        /// <summary>
        /// Gets client source files with versions
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SourceFileInfo> GetClientFiles()
        {
            string configFile = Path.Combine(_updateDir, VERSION_CONFIG_FILE);
            if (File.Exists(configFile))
            {
                string json = File.ReadAllText(configFile);
                return JsonConvert.DeserializeObject<IEnumerable<SourceFileInfo>>(json);
            }
            else
            {
                return new List<SourceFileInfo>();
            }
        }

        /// <summary>
        /// Actualise changes in files
        /// - delete unactual files
        /// - replace changed files
        /// - add new files
        /// also keeps actual version of files versions config file
        /// </summary>
        /// <param name="myFiles">client files informations</param>
        /// <param name="serverFiles">server files informations</param>
        private void ActualizeFiles(List<SourceFileInfo> myFiles, List<SourceFileInfo> serverFiles)
        {
            //remove unactual files from directory and config
            foreach (string file in Directory.GetFiles(_updateDir, "*.cs"))
            {
                string fileName = Path.GetFileName(file);
                if (serverFiles.Where(sf => sf.FileName == fileName).Count() == 0)
                {
                    File.Delete(file);
                    myFiles.RemoveAll(mf => mf.FileName == fileName);
                }
            }

            //add new soueces into dir and config
            IEnumerable<SourceFileInfo> newFiles = serverFiles.Where(sf => !myFiles.Exists(mf => sf.FileName == mf.FileName));
            foreach (SourceFileInfo newFile in newFiles)
            {
                string decrypted = DecryptSourceFromServer(newFile);
                if (!string.IsNullOrEmpty(decrypted))
                {
                    File.WriteAllText(Path.Combine(_updateDir, newFile.FileName), decrypted);
                    myFiles.Add(newFile);
                }
            }

            //modify diferent version files and config
            IEnumerable<SourceFileInfo> modifiedFiles = serverFiles.Where(sf => myFiles.Exists(mf => sf.FileName == mf.FileName && sf.Version != mf.Version));
            foreach (SourceFileInfo modifiedFile in modifiedFiles)
            {
                string decrypted = DecryptSourceFromServer(modifiedFile);
                if (!string.IsNullOrEmpty(decrypted))
                {
                    File.WriteAllText(Path.Combine(_updateDir, modifiedFile.FileName), decrypted);
                    myFiles.Find(mf => mf.FileName == modifiedFile.FileName).Version = modifiedFile.Version;
                }
            }
        }

        /// <summary>
        /// saves specific config in update dir
        /// </summary>
        /// <param name="fileConfig">file informations to save on client</param>
        private void SaveConfig(IEnumerable<SourceFileInfo> fileConfig)
        {
            string json = JsonConvert.SerializeObject(fileConfig);
            File.WriteAllText(Path.Combine(_updateDir, VERSION_CONFIG_FILE), json);
        }

        /// <summary>
        /// - Gets specific encrypted source file from server. 
        /// - Encryption and Decription using AES symetric and commonSecret derived from EC Diffie-Helman key Exchange
        /// - Decrypts source files
        /// - verifies digital signature of source code
        /// </summary>
        /// <param name="sourceFileInfo">the file to get</param>
        /// <returns>verified and decrypted source file or null if operation unsuccessfull</returns>
        private string DecryptSourceFromServer(SourceFileInfo sourceFileInfo)
        {
            byte[] dhClientPublic;
            byte[] dhClientPrivate;
            _keyGen.GenerateKeyPair(out dhClientPrivate, out dhClientPublic);

            string uri = (TroubleShooterClient.SERVICE_PATH + "/sources");
            ProtectedSourceRequest request = new ProtectedSourceRequest() { DhClientPublic = dhClientPublic, FileName = sourceFileInfo.FileName };
            HttpResponseMessage response = _client.PostAsJsonAsync(uri, request).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                ProtectedSource source = response.Content.ReadAsAsync<ProtectedSource>().GetAwaiter().GetResult();
                byte[] sharedSecret = _diffieHelman.SharedSecret(dhClientPrivate, source.DhPublicServer);
                string decryptedSource = AesHandler.DecryptStringFromBytes_Aes(source.SourceCode, sharedSecret);
                if (_verifier.VerifySignature(decryptedSource, source.Signature, _signatureKey))
                    return decryptedSource;
                else
                    return null;
            }
            return null;
        }
    }
}

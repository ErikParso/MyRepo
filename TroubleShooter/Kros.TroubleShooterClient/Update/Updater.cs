using ElipticCurves;
using Kros.TroubleShooterCommon;
using Kros.TroubleShooterCommon.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;

namespace Kros.TroubleShooterClient.Update
{
    public class Updater
    {
        private string _updateDir;
        private const string URI_GET_VERSION = "api/updateFiles";
        private const string VERSION_CONFIG_FILE = "FileVersions.json";
        private ECEncryption decryptor;
        private ECSignature verifier;
        private ECKeysGenerator keyGen;
        private ECDiffieHelman diffieHelman;
        private byte[] signatureKey = Convert.FromBase64String("A0mbdQ20EsLzFyiFwr58QrdLFqmIAA==");

        private HttpClient client = new HttpClient();

        public Updater(string updateDir)
        {
            _updateDir = updateDir;
            client.BaseAddress = new Uri("http://localhost:51131/");
            ElipticCurve curve = ElipticCurve.secp160r1();
            decryptor = new ECEncryption(curve);
            keyGen = new ECKeysGenerator(curve);
            verifier = new ECSignature(curve);
            diffieHelman = new ECDiffieHelman(curve);
        }

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

        private IEnumerable<SourceFileInfo> GetServerFiles()
        {
            string uri = (URI_GET_VERSION + "/updateInfo");
            HttpResponseMessage response = client.GetAsync(uri).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<IEnumerable<SourceFileInfo>>().GetAwaiter().GetResult();
            else
                return null;
        }

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

        private void ActualizeFiles(List<SourceFileInfo> myFiles, List<SourceFileInfo> serverFiles)
        {
            //remove unactual files from directory and config
            foreach (string file in Directory.GetFiles(_updateDir, ".cs"))
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

        private void SaveConfig(IEnumerable<SourceFileInfo> fileConfig)
        {
            string json = JsonConvert.SerializeObject(fileConfig);
            File.WriteAllText(Path.Combine(_updateDir, VERSION_CONFIG_FILE), json);
        }

        private string DecryptSourceFromServer(SourceFileInfo sourceFileInfo)
        {
            byte[] dhClientPublic;
            byte[] dhClientPrivate;
            keyGen.GenerateKeyPair(out dhClientPrivate, out dhClientPublic);

            string uri = (URI_GET_VERSION + "/sources");
            ProtectedSourceRequest request = new ProtectedSourceRequest() { DhClientPublic = dhClientPublic, FileName = sourceFileInfo.FileName };
            HttpResponseMessage response = client.PostAsJsonAsync(uri, request).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                ProtectedSource source = response.Content.ReadAsAsync<ProtectedSource>().GetAwaiter().GetResult();
                byte[] sharedSecret = diffieHelman.SharedSecret(dhClientPrivate, source.DhPublicServer);
                string decryptedSource = AesHandler.DecryptStringFromBytes_Aes(source.SourceCode, sharedSecret);
                if (verifier.VerifySignature(decryptedSource, source.Signature, signatureKey))
                    return decryptedSource;
                else
                    return null;
            }
            return null;
        }
    }
}

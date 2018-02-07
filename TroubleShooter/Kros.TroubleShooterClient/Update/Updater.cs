﻿using ElipticCurves;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Numerics;
using System.Text;

namespace Kros.TroubleShooterClient.Update
{
    public class Updater
    {
        private string _updateDir;
        private const string URI_GET_VERSION = "api/updateFiles";
        private ECEncryption decryptor;
        private ECKeysGenerator keyGen;

        private HttpClient client = new HttpClient();

        public Updater(string updateDir)
        {
            _updateDir = updateDir;
            client.BaseAddress = new Uri("http://localhost:51131/");
            ElipticCurve curve = ElipticCurve.secp160r1();
            decryptor = new ECEncryption(curve);
            keyGen = new ECKeysGenerator(curve);
        }

        public void Execute()
        {
            string publicKey;
            BigInteger privateKey;
            keyGen.GenerateKeyPair(out privateKey, out publicKey);

            IEnumerable<ProtectedSource> sources = new List<ProtectedSource>();
            string uri = (URI_GET_VERSION + "/protected?publicKey=" + publicKey);
            HttpResponseMessage response = client.GetAsync(uri).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
                sources = response.Content.ReadAsAsync<IEnumerable<ProtectedSource>>().GetAwaiter().GetResult();
            if (!Directory.Exists(_updateDir))
                Directory.CreateDirectory(_updateDir);
            foreach (ProtectedSource source in sources)
            {
                string decryptedSource = decryptor.Decrypt(source.SourceCode, privateKey, Encoding.Unicode);
                File.WriteAllText(Path.Combine(_updateDir, source.FileName), decryptedSource);
            }
        }
    }
}
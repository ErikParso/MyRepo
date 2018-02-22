using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kros.TroubleShooterInput
{
    /// <summary>
    /// Contains data which will be passed to troubleshooter.
    /// Data is serialised and deserialised in json <see cref="Runner"/>
    /// </summary>
    [Serializable]
    public class RunData
    {
        [JsonProperty]
        private Dictionary<string, string> Data;

        [JsonProperty]
        private List<string> Flags;

        [JsonProperty]
        public Dictionary<string, ServisObject> ServisObjects { get; private set; }

        public Exception Exception { get; set; }

        public string ErrorMessage { get; set; }

        public List<string> Attachments { get; private set; }

        public int ErrNumber { get; set; }

        public StartupMode StartupMode { get; set; }

        public RunData()
        {
            Data = new Dictionary<string, string>();
            Flags = new List<string>();
            Exception = null;
            ErrorMessage = null;
            Attachments = new List<string>();
            ServisObjects = new Dictionary<string, ServisObject>();
            StartupMode = StartupMode.COMPLEX;
        }

        public void Set(string name, string value)
        {
            if (Data.ContainsKey(name))
                Data[name] = value;
            else
                Data.Add(name, value);
        }

        public void WithFlags(params string[] flags)
        {
            Flags.Clear();
            Flags.AddRange(flags);
        }

        public string Get(string name)
        {
            if (Data.ContainsKey(name))
                return Data[name];
            else return null;
        }

        public void SetServisObject(string identifier, Action<ServisObject> setter)
        {
            if (ServisObjects.ContainsKey(identifier))
                setter(ServisObjects[identifier]);
            else
            {
                ServisObject newObject = new ServisObject();
                ServisObjects.Add(identifier, newObject);
                setter(newObject);
            }
        }
    }

    public class ServisObject
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public bool Editable { get; set; } 
        public bool IsPath { get; set; }
        public string[] PossibleValues { get; set; }
    }

    public enum StartupMode
    {
        MINIMALISTIC, COMPLEX, SERVIS_ONLY
    }
}

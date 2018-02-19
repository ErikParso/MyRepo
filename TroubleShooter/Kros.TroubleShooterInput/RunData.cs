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
        public Dictionary<string, string> Data { get; set; }

        public Exception Exception { get; set; }
    }
}

using Kros.TroubleShooterClient.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kros.TroubleShooterClient.ViewModel
{
    /// <summary>
    /// model for servis mode
    /// </summary>
    public class ServiceModeVM : ObservableObject
    {
        /// <summary>
        /// the list of servis parameters
        /// </summary>
        public List<OptionalServiceProp> Properties { get; private set; }

        /// <summary>
        /// init view model 
        /// </summary>
        public ServiceModeVM()
        {
            Properties = new List<OptionalServiceProp>();
        }

        /// <summary>
        /// defines servis data
        /// </summary>
        public void DefineProperties()
        {
            Properties.Add(new OptionalServiceProp()
            {
                Name = "Kontakt"
            });

            Properties.Add(new OptionalServiceProp()
            {
                Name = "Popis chyby"
            });

            Properties.Add(new OptionalServiceProp()
            {
                Name = "Cesta k Olympu",
                Value = Environment.CurrentDirectory,
                IsPath = true,
                Editable = false
            });

            Properties.Add(new OptionalServiceProp()
            {
                Name = "Cesta k databáze",
                Value = Environment.CurrentDirectory + "\\data\\...",
                IsPath = true
            });

            Properties.Add(new OptionalServiceProp()
            {
                Name = "Typ databázy",
                PossibleValues = new List<string>() { "Neznámy", "Access", "SQL" },
                Value = "Neznámy"
            });

            Properties.Add(new OptionalServiceProp()
            {
                Name = "Operačný systém",
                Value = Environment.OSVersion.VersionString,
                Editable = false
            });

            Properties.Add(new OptionalServiceProp()
            {
                Name = "Log",
                Value = FlattenException(TroubleShooter.Current.RunData?.Exception),
                Editable = false
            });
        }

        /// <summary>
        /// gets log
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string FlattenException(Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }
    }
}

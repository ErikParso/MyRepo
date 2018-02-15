using Kros.TroubleShooterClient.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kros.TroubleShooterClient.ViewModel
{
    public class ServiceModeVM : ObservableObject
    {
        public List<OptionalServiceProp> Properties { get; private set; }

        public ServiceModeVM()
        {
            Properties = new List<OptionalServiceProp>();
        }

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
                IsPath = true, Editable = false
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
                Value = FlattenException(TroubleShooter.Current.Exception),
                Editable = false
            });
        }

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

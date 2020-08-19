using System;
using System.Configuration;

namespace SHE_Document_converter
{
    public class ReadSettings
    {
        /// <summary>
        /// Gibt den Wert aus der App.Config-appSettings anhand des angegebenen System.String aus.
        /// </summary>
        /// <param name="name">Schlüssel des Wertes.</param>
        /// <returns>Der Wert als System.String.</returns>
        public static string ReadAppSettings(string name)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
            catch
            {
                throw;
            }
        }
    }
}

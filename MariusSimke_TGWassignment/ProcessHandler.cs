using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Xml;
using System.Windows.Forms;
using MariusSimke_TGWassignment.DataModels.Entities;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace MariusSimke_TGWassignment
{
    public static class ProcessHandler
    {

        public static List<SettingEntry> ReadAllSettings()
        {
            
            List<SettingEntry> configEntryList = new List<SettingEntry>();
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                if (appSettings.Count == 0)
                    return null;              
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        SettingEntry configEntry = new SettingEntry();                       
                        configEntry.Key = key;
                        configEntry.Value = appSettings[key];                       
                        configEntry.Value = configEntry.Value.ToString();
                        configEntryList.Add(configEntry);
                    }
                    return configEntryList;
                }
            }
            catch (ConfigurationErrorsException)
            {
                MessageBox.Show("Error reading app settings");
                Console.WriteLine("Error reading app settings");
            }

            return configEntryList;
        }

        public static void AddNewConfigKey(string strKey, string value)
        {                    

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config");

            XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");
            // Create new <add> node
            XmlNode nodeNewKey = xmlDoc.CreateElement("add");
            // Create new attribute for key=""
            XmlAttribute attributeKey = xmlDoc.CreateAttribute("key");
            // Create new attribute for value=""
            XmlAttribute attributeValue = xmlDoc.CreateAttribute("value");
            // Assign values to both - the key and the value attributes:
            attributeKey.Value = strKey;
            attributeValue.Value = value;
            // Add both attributes to the newly created node:
            nodeNewKey.Attributes.Append(attributeKey);
            nodeNewKey.Attributes.Append(attributeValue);

            // Add the node under the 
            appSettingsNode.AppendChild(nodeNewKey);
            xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config");
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

        }

        public static void UpdateConfigKey(bool addNewSetting, string strKey, string newValue)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config");

            if (!ConfigKeyExists(strKey) && addNewSetting)
            {
                AddNewConfigKey( strKey, newValue);               
            }
            else if (!ConfigKeyExists(strKey))
            {
                AddNewConfigKey(strKey, newValue);
                throw new ArgumentNullException("Key", "<" + strKey + "> not find in the configuration.");
            }


            XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");
          

            foreach (XmlNode childNode in appSettingsNode)
            {               
                if (childNode.Attributes["key"].Value == strKey)
                {
                    childNode.Attributes["value"].Value = newValue;
                }
            }
            xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config");
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            
        }

        public static bool ConfigKeyExists(string strKey)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config");

            XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");

            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.Attributes["key"].Value == strKey)
                {
                    return true;
                }
            }
            return false;
        }


        public static List<SettingEntry> ReadFile(string filename) {
            //Will be passed back to update
            List<SettingEntry> settingsFoundList = new List<SettingEntry> ();
            settingsFoundList.Clear();
            //To awoid nullRef execption
            if (filename == "")
                return settingsFoundList;

            var lines = File.ReadAllLines(filename);
            
            foreach (string line in lines)
            {
                SettingEntry settingEntry = new SettingEntry();
                string keyName = String.Empty;
                string value = String.Empty;

                //Some lines has only 1 tab and to buil a better regex expresion need to remove
                string lineText = line.Replace("\t", "");
                
                //resultStartTime line has >1 ':' and regex fails, need to remove all text after this 
                if (lineText.Contains('/'))
                    lineText = lineText.Substring(0, lineText.LastIndexOf('/')+1);

                if (lineText.Contains(":"))
                {
                    int tmp = lineText.IndexOf(':');
                    StringBuilder sb = new StringBuilder(lineText);
                    sb[tmp] = '!'; // index starts at 0!
                    lineText = sb.ToString();
                }


                var keyRegex = new Regex("(.*)!.*");
                if (keyRegex.IsMatch(lineText))
                    keyName = keyRegex.Match(lineText).Groups[1].Value; 
                
                var valueRegex = new Regex(keyName+"!(.*)//.*");
                if (valueRegex.IsMatch(lineText))
                    value = valueRegex.Match(lineText).Groups[1].Value;

                //Just incase it's null or empty
                keyName = keyName != "" ? FirstCharToUpper(keyName) : "";
                //Remove the line that is not Setting Key
                if (keyName != "" && (keyName.Substring(0, 1) == "-" || keyName.Substring(0, 1) == "="))
                    keyName = String.Empty;


                if (keyName != "")
                {
                    settingEntry.Key = keyName;
                    settingEntry.Value = value;
                    settingsFoundList.Add(settingEntry);
                }

            }
            return settingsFoundList;
        }

        public static string ValidateByType(string settingKey, string objValue, string value) {
            string valueTypeName = String.Empty;
            string validationMessage = String.Empty;
            string valueInput = String.Empty;
            int aNumber;
            TimeSpan aTime;
            //string aString;

            //ONLY for Base Settings
            if (value == null)
            {
                if (objValue == "" || objValue == null)
                    return " Value Not Found";

                // For Base setting there are SET rules based on Get;Set; data types
                valueTypeName = BaseValidationRules(settingKey);
                valueInput = objValue;
            }
            else {//For the rest
                if (value == null || value == "")
                    return " Value Not Found";

                var valueObj = ParseString(value);
                valueTypeName = valueObj.GetType().Name;
                valueInput = value;
            }

            switch (valueTypeName)
            {
                case "Int32":
                    {                      
                        if (int.TryParse(valueInput, out aNumber))
                            validationMessage =  "Valid";                        
                        else
                            validationMessage = " Invalid Value: " + valueInput;                        
                    }
                    break;
                case "TimeSpan":
                    {
                        if (TimeSpan.TryParse(valueInput, out aTime))
                            validationMessage = "Valid";
                        else
                            validationMessage = " Invalid Value: " + valueInput;
                    }
                    break;
                case "String":
                    {
                        if (!Regex.IsMatch(valueInput, @"^[a-zA-Z]+$"))                       
                            validationMessage = "NOTE: Value Contain Not Alphabets: " + valueInput;
                        else
                            validationMessage = "Valid";
                    }
                    break;
                default:
                    validationMessage = " Invalid Data Type ";
                    break;

            }

            return validationMessage;
        }

        public static object ParseString(string str)
        {

            Int32 intValue;
            Int64 bigintValue;
            TimeSpan timeSpanValue;
            double doubleValue;
            bool boolValue;
            DateTime dateValue;

            // Place checks higher in if-else statement to give higher priority to type.

            if (Int32.TryParse(str, out intValue))
                return intValue;
            else if (TimeSpan.TryParse(str, out timeSpanValue))
                return timeSpanValue;
            else if (Int64.TryParse(str, out bigintValue))
                return bigintValue;            
            else if (double.TryParse(str, out doubleValue))
                return doubleValue;
            else if (bool.TryParse(str, out boolValue))
                return boolValue;           
            else if (DateTime.TryParse(str, out dateValue))
                return dateValue;
            else return str;

        }
        //This is just based on ConfigEntry.cs and for Base only
        public static string BaseValidationRules(string settingKey) {
            string rule = String.Empty;
            var ruleArray = new[] { "Int32", "TimeSpan", "String"};
            var intArray = new []{ "NumberOfSystems","OrdersPerHour","OrderLinesPerOrder"};
            var timeArray = new[] { "ResultStartTime"};
            var stringArry = new[] { "" };

            if (intArray.Contains(settingKey))
                rule = ruleArray[0];

            if (timeArray.Contains(settingKey))
                rule = ruleArray[1];

            return rule;
        }
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}

using MariusSimke_TGWassignment.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MariusSimke_TGWassignment
{
    public partial class Form1 : Form
    { 
          
        private static SettingEntry settingEntry = new SettingEntry();
        private static List<SettingEntry> settingEntryList = new List<SettingEntry>();
        public static ConfigEntry configEntry = new ConfigEntry();
        public static List<string> baseSettingNameList = new List<string>();
        public static List<SettingEntry> settingsFoundInFilesList = new List<SettingEntry>();
        public static bool updateAllSettings = false;
        public static bool addNewSetting = false;

        public Form1()
        {
            InitializeComponent();            
            GetBaseSettingsNames();
            DisplayParameters();
        }

        public void GetBaseSettingsNames() {
            ConfigEntry configEntry = new ConfigEntry();
            var allBaseSettings = configEntry.GetType().GetProperties();
            foreach (var item in allBaseSettings)
            {
                baseSettingNameList.Add(item.Name);
            }
        }

        public void CreateConfigEntry (string settingItemKey, string settingItemValue) {

                addNewSetting = chbAddNew.Checked;
                ProcessHandler.UpdateConfigKey(addNewSetting, settingItemKey, settingItemValue);

        }

        public void ValidateAndCreate(List<SettingEntry> settingEntry) {
            string settingValue = String.Empty;
            string settingKey = String.Empty;
            string objValueStr = String.Empty;
            string currentValuesLineText = String.Empty;
            bool displayAll = chbDisplayAll.Checked;
            //Validation Text variables
            string validationMessage = String.Empty;
            string notFounEndMsgText = String.Empty;
            string notFoundMsgText = String.Empty;
            lblValidationMessage.Text = String.Empty;


            //CONFIRM BASE Config Setting was found in Layes/Files
            if (settingsFoundInFilesList.Count > 0)
            {
                foreach (var baseSetting in baseSettingNameList)
                {
                    var oneOfBase = settingsFoundInFilesList.Where(x => x.Key == baseSetting).FirstOrDefault();
                    if (oneOfBase == null)
                    {
                        notFoundMsgText += baseSetting + " - " + "COULD NOT BE FOUND" + '\n' + '\n';
                        //SET_TO-ERROR 
                        //CreateConfigEntry(baseSetting, "");
                        //OR
                        notFounEndMsgText = "VALUES REMAIN UNCHANGED " + '\n' + '\n' + '\n';                       
                    }
                }
            }

            notFoundMsgText += notFounEndMsgText;

            if (notFoundMsgText.Length > 0)
                lblValidationMessage.Text += notFoundMsgText;

            foreach (var entry in settingsFoundInFilesList)
            {
                
                settingKey = entry.Key.ToString();
                settingValue = entry.Value.ToString();
                validationMessage = "";
                //Validate Base
                if (baseSettingNameList.Contains(settingKey)) {
                    object valueObj = settingEntry.Where(x => x.Key == settingKey).Select(x => x.Value).First();
                    objValueStr = valueObj.ToString();
                    validationMessage = ProcessHandler.ValidateByType(settingKey, objValueStr, null);

                    if (validationMessage == "Valid")
                    {
                        if (settingKey == "NumberOfSystems")
                            configEntry.NumberOfSystems = Convert.ToInt32(valueObj);
                        if (settingKey == "OrdersPerHour")
                            configEntry.OrdersPerHour = Convert.ToInt32(valueObj);
                        if (settingKey == "OrderLinesPerOrder")
                            configEntry.OrderLinesPerOrder = Convert.ToInt32(valueObj);
                        if (settingKey == "ResultStartTime")
                            configEntry.ResultStartTime = TimeSpan.Parse(objValueStr);

                        CreateConfigEntry(settingKey, settingValue);
                    }
                    else {                                              
                        lblValidationMessage.Text += settingKey + " - " + validationMessage + '\n' + '\n';
                    }                                            
                }              

                //Validate the rest and display
                if (!baseSettingNameList.Contains(settingKey))
                {
                    //If ADD NEW is checked and it doesn't exist create new // 
                    if (chbAddNew.Checked == true)
                        ProcessHandler.AddNewConfigKey(settingKey, "");                 
                    else
                        validationMessage = ProcessHandler.ConfigKeyExists(settingKey).ToString();

                    //If Key EXISTS validate value
                    if (validationMessage.ToLower() == "true" || validationMessage == "")
                        validationMessage = ProcessHandler.ValidateByType(settingKey, null, settingValue);
                    else
                        validationMessage = " DOES NOT EXIST. Consider 'Add New'" + '\n';


                    if (validationMessage == "Valid")
                    {
                        CreateConfigEntry(settingKey, settingValue);
                    }   
                    else                   
                        lblValidationMessage.Text += settingKey + " - " + validationMessage + '\n';
                }
                
            }


            int numLines = lblValidationMessage.Text.Count(c => c.Equals('\n')) + 1;
            DisplayParameters();
        }

        public void DisplayParameters() {
            string settingValue = String.Empty;           
            string currentValuesLineText = String.Empty;
            bool displayAll = chbDisplayAll.Checked;
            //Create a lisf appSettings based on current appSettings in App.Config
            settingEntryList = ProcessHandler.ReadAllSettings();
            lblResult.Text = settingEntryList.Count > 0 ? "" : "null : null";

            //DISPLAY Values and show message if invalid 
            foreach (var settingItem in settingEntryList)
            {

                if (displayAll)
                {
                    //Check if NON Base is valid
                    if (!baseSettingNameList.Contains(settingItem.Key.ToString()))
                    {
                        var value = ProcessHandler.ParseString(settingItem.Value.ToString());
                        string typeName = value.GetType().Name;

                    }
                    //Display
                    settingValue = settingItem.Value.ToString() != "" ? settingItem.Value.ToString() : "ERROR";
                    currentValuesLineText = settingItem.Key.ToString() + " : " + settingValue;
                    lblResult.Text += currentValuesLineText + '\n' + '\n';
                }
                else if (baseSettingNameList.Contains(settingItem.Key.ToString()))
                {
                    //Display
                    settingValue = settingItem.Value.ToString() != "" ? settingItem.Value.ToString() : "ERROR";
                    currentValuesLineText = settingItem.Key.ToString() + " : " + settingValue;
                    lblResult.Text += currentValuesLineText + '\n' + '\n';
                }
            }

        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            //Reset Found list;
            settingsFoundInFilesList.Clear();
            lblValidationMessage.Text = "";
           

           //openFileDialog1.ShowDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in openFileDialog1.FileNames)
                {
                    //Get a list of found SttingsKey key values. 
                    List<SettingEntry> settingsFoundList = ProcessHandler.ReadFile(file);
                    settingsFoundInFilesList.AddRange(settingsFoundList);

                }
            }

            ValidateAndCreate(settingsFoundInFilesList);
           
        }

        private void chbDisplayAll_CheckedChanged(object sender, EventArgs e)
        {
            DisplayParameters();
        }
    }

}

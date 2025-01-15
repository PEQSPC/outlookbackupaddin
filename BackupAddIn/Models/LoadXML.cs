using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackupAddIn.Models
{
    public class LoadXML
    {
        /// <summary>
        /// config do scanner
        /// </summary>
        public string block { get; private set; }
        public string ip { get; private set; }

        /// <summary>
        /// ip da impressora da zebra
        /// </summary>
        //public string ipZebraPrinter { get; private set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public int portZebraprinter { get; private set; }

        public static async Task SendAPIRequest()
        {
            try
            {
                LoadXML loadXML = new LoadXML();
                loadXML.LoadingXMLFILE();


                // Retrieve PC name and current date
                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                string pcName = Environment.MachineName;
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");



                Registo registo = new Registo();
                registo.Sigla = userName;
                registo.PCName = pcName;
                registo.LastBackupDate = currentDate;


                // Convert the JSON data to a string
                string jsonString = JsonSerializer.Serialize(registo);


                //sends http post request ,if the api is down it saves the request in a file, and sends it when the api is back up
                HttpClient client = new HttpClient();
                //sends http post request ,if the api is down it saves the request in a file, and sends it when the api is back up
                var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(loadXML.ip, content);


            }
            catch (Exception ex)
            {
                // Log any exception that occurs
                MessageBox.Show("error send api request" + ex);
            }
        }

        public bool LoadingXMLFILE()
        {

            string sSettingsZebraPrinterPath = AppDomain.CurrentDomain.BaseDirectory + "\\FILES\\Settings.xml";

            // Dataset table
            string tbParameters = "Parameters";
            //string cIP = "IP";
            //string cPort = "Port";
            string cPortCom = "block";
            string cIp = "ip";


            DataSet dsSettingsZebra = new DataSet();

            // Path to your XML configuration file
            if (File.Exists(sSettingsZebraPrinterPath))
            {
                try
                {
                    dsSettingsZebra.Clear();
                    dsSettingsZebra.ReadXml(sSettingsZebraPrinterPath, XmlReadMode.ReadSchema);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                // write program sequence in configuration file
                dsSettingsZebra.WriteXml(sSettingsZebraPrinterPath, XmlWriteMode.WriteSchema);
                return false;
            }

            if (dsSettingsZebra.Tables[tbParameters].Rows.Count == 0)
            {
                dsSettingsZebra.Tables[tbParameters].Rows.Add(dsSettingsZebra.Tables[tbParameters].NewRow());
                dsSettingsZebra.WriteXml(sSettingsZebraPrinterPath, XmlWriteMode.WriteSchema);
            }

            try
            {
                //ipZebraPrinter = dsSettingsZebra.Tables[tbParameters].Rows[0][cIP].ToString();
                //portZebraprinter = Convert.ToInt32(dsSettingsZebra.Tables[tbParameters].Rows[0][cPort].ToString());
                block = dsSettingsZebra.Tables[tbParameters].Rows[0][cPortCom].ToString();
                ip = dsSettingsZebra.Tables[tbParameters].Rows[0][cIp].ToString();

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}


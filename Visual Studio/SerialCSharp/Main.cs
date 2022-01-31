using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using ComponentFactory.Krypton.Toolkit;
using System.Configuration;

namespace SerialCSharp
{
    public partial class Main : KryptonForm
    {
        public Main()
        {
            InitializeComponent();
        }
        private static SimpleCryptograph oSimpleCryptograph = new SimpleCryptograph(ConfigurationManager.AppSettings["KeyHash"]);
        private static SerialPort port = new SerialPort(ConfigurationManager.AppSettings["PORT"], Convert.ToInt32(ConfigurationManager.AppSettings["Baudrate"]));
        private void Bt_Setup_Click(object sender, EventArgs e)
        {
            try
            {
                Tb_ListBox.Items.Add($"SSID WIFI:\t{Tb_SSID.Text}\nPassword WIFI :\t{oSimpleCryptograph.EncryptData(Tb_Password.Text)}\nMQTT SERVER :\t{Tb_MQTTServer.Text}\nMQTT PORT\t{Tb_MQTPort.Text}\nMQTT User :\t{Tb_MQTTUser.Text}\nMQTT Pass :\t{oSimpleCryptograph.EncryptData(Tb_Password.Text)}\n");
                //MessageBox.Show(oSimpleCryptograph.DecryptData(oSimpleCryptograph.EncryptData(Tb_Password.Text)));
                port.Open();
                port.Write($"{Tb_SSID.Text}\n{Tb_Password.Text}\n{Tb_MQTTServer.Text}\n{Tb_MQTPort.Text}\n{Tb_MQTTUser.Text}\n{Tb_Password.Text}\n");
                port.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

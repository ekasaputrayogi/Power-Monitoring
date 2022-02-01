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

        private void Tb_MQTTServer_TextChanged(object sender, EventArgs e)
        {

        }

        private void Bt_PinOK_Click(object sender, EventArgs e)
        {
            if (Cb_StatusCode.Text == ($"PIN {Cb_StatusCode.SelectedIndex}"))
            {
                Tb_ListBox.Items.RemoveAt(Cb_StatusCode.SelectedIndex);
                Tb_ListBox.Items.Insert(Cb_StatusCode.SelectedIndex, $"{Cb_StatusCode.Text} - {Tb_SensorName.Text} {Tb_RangeMin.Text} {Tb_RangeMax.Text} {Tb_Unit.Text} {Tb_AlarmBelow.Text} {Tb_AlarmMore.Text}");
            }
            Cb_StatusCode.SelectedIndex = -1;
            Tb_SensorName.Text = "";
            Tb_RangeMin.Text = "";
            Tb_RangeMax.Text = "";
            Tb_Unit.Text = "";
            Tb_AlarmBelow.Text = "";
            Tb_AlarmMore.Text = "";
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Tb_ListBox.Items.Insert(Tb_ListBox.SelectedIndex, $"PIN {Tb_ListBox.SelectedIndex} - No Data");
                Tb_ListBox.Items.RemoveAt(Tb_ListBox.SelectedIndex);
            }
            catch (Exception ex)
            {
                
            }
            
        }

        private void Tb_RangeMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void Tb_RangeMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void Tb_AlarmBelow_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void Tb_AlarmMore_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}

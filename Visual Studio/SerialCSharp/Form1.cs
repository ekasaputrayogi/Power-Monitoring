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

namespace SerialCSharp
{
    public partial class Form1 : Form
    {
        MqttClient mqttClient;
        public Form1()
        {
            InitializeComponent();
        }
        SerialPort port = new SerialPort("COM5", 115200);
        private void Form1_Load(object sender, EventArgs e)
        {
            
            Task.Run(() =>
            {
                mqttClient = new MqttClient("broker.emqx.io");
                mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;
                mqttClient.Subscribe(new string[] { "IOTBOX" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
                mqttClient.Connect("");
            });

        }

        private void MqttClient_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message);
            textBox6.Invoke((MethodInvoker)(() => textBox6.Text = message));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            port.Open();
            port.Write(textBox1.Text + "\n" + textBox2.Text);
            port.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                if (mqttClient != null && mqttClient.IsConnected)
                {
                    mqttClient.Publish("TESA", Encoding.UTF8.GetBytes(textBox3.Text));
                }
            });
        }
    }
}

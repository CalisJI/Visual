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
namespace Camera_Check_Component
{
    public partial class Com_setting : Form
    {
        public Com_setting()
        {
            InitializeComponent();
        }

        private void Com_setting_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports) 
            {
                Com_setting_box.Items.Add(port);
            }
            if (ports.Length > 0) Com_setting_box.SelectedIndex = 0;
            string[] Baudrate = {"9600", "19200", "38400", "57600", "115200" };
            foreach (string baud in Baudrate) 
            {
                Baudrate_box.Items.Add(baud);
            }
            if (Baudrate.Length > 0) Baudrate_box.SelectedIndex = 0;
        }

        private void connect_com_btn_Click(object sender, EventArgs e)
        {
            try 
            {
                if (serialPort1.IsOpen) serialPort1.Close();
                serialPort1.PortName = Com_setting_box.Text;
                serialPort1.BaudRate = Convert.ToInt32(Baudrate_box.Text);
                serialPort1.Open();
                DialogResult result = MessageBox.Show("Opem " + Com_setting_box.Text + " Successfully!");
                if (result == DialogResult.OK) 
                {
                    serialPort1.Close();
                }
            }
            catch( Exception )
            {
                MessageBox.Show(Com_setting_box.Text + " Not Existing or Available, Try other one");

            }
        }

        private void SAVE_btn_Click(object sender, EventArgs e)
        {
            bool success = true;
            if (Com_setting_box.Items.Count > 0) 
            {
                Program_Configuration.UpdateSystem_Config("DefaultComport", Com_setting_box.Text);
            }
            else 
            {
                MessageBox.Show("Select COM port first");
                success = false;
            }
            if (Baudrate_box.Items.Count > 0) 
            {
                Program_Configuration.UpdateSystem_Config("DefaultCOMBaudrate",Baudrate_box.Text);
            }
            else 
            {
                MessageBox.Show("Select Baudrate first");
                success = false;
            }
            if (success) MessageBox.Show("Com Setting is updated Successfully!");
            this.Close();
        }
    }
}

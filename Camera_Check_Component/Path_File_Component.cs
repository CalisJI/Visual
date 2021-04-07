using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Camera_Check_Component;

namespace Camera_Check_Component
{
    public partial class Path_File_Component : Form
    {
        private OpenFileDialog opendialog1 = new OpenFileDialog();
        private System_config system_config;
        private FolderBrowserDialog openfolder1 = new FolderBrowserDialog();
        public Path_File_Component()
        {
            InitializeComponent();
        }

        private void Path_File_Component_Load(object sender, EventArgs e)
        {
            system_config = Program_Configuration.GetSystem_Config();
            TextBox_PathFile.Text = system_config.Map_Path_File;
            openfolder1.RootFolder = Environment.SpecialFolder.MyComputer;
            
        }

        private void Open_Dialog_btn_Click(object sender, EventArgs e)
        {
            if (openfolder1.ShowDialog() == DialogResult.OK) 
            {
                TextBox_PathFile.Text = openfolder1.SelectedPath.ToString();
            }
        }

        private void Cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Saving_btn_Click(object sender, EventArgs e)
        {
            Program_Configuration.UpdateSystem_Config("Map_Path_File", TextBox_PathFile.Text);
            this.Close();
        }
    }
}

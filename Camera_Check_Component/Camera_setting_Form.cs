﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Imaging;
using AForge.Video.DirectShow;
using System.IO;

namespace Camera_Check_Component
{
    public partial class Camera_setting_Form : Form
    {
        private System_config system_config;
        private FilterInfoCollection filterInfoCollection;
        private VideoCaptureDevice videoCaptureDevice1;
        private VideoCaptureDevice videoCaptureDevice2;
        private VideoCaptureDevice videoCaptureDevice3;
        private VideoCaptureDevice videoCaptureDevice4;
        private VideoCaptureDevice videoCaptureDevice5;
        private VideoCaptureDevice videoCaptureDevice6;
        private VideoCaptureDevice videoCaptureDevice7;
        public Camera_setting_Form()
        {
            InitializeComponent();
            system_config = Program_Configuration.GetSystem_Config();

            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (filterInfoCollection.Count > 0)
            {
                foreach (FilterInfo filterInfo in filterInfoCollection)
                {
                    Cambox1.Items.Add(filterInfo.Name);
                    Cambox2.Items.Add(filterInfo.Name);
                    Cambox3.Items.Add(filterInfo.Name);
                    Cambox4.Items.Add(filterInfo.Name);
                    Cambox5.Items.Add(filterInfo.Name);
                    Cambox6.Items.Add(filterInfo.Name);
                    if (system_config.add_cam.ToString() == "true")
                    {
                        Cambox7.Items.Add(filterInfo.Name);
                    }

                }
                Cambox1.SelectedIndex = 0;
                Cambox2.SelectedIndex = 0;
                Cambox3.SelectedIndex = 0;
                Cambox4.SelectedIndex = 0;
                Cambox5.SelectedIndex = 0;
                Cambox6.SelectedIndex = 0;
                if (system_config.add_cam == "true")
                {
                    Cambox7.SelectedIndex = 0;
                }


                videoCaptureDevice1 = new VideoCaptureDevice();
                videoCaptureDevice2 = new VideoCaptureDevice();
                videoCaptureDevice3 = new VideoCaptureDevice();
                videoCaptureDevice4 = new VideoCaptureDevice();
                videoCaptureDevice5 = new VideoCaptureDevice();
                videoCaptureDevice6 = new VideoCaptureDevice();
                videoCaptureDevice7 = new VideoCaptureDevice();


                system_config = Program_Configuration.GetSystem_Config();
                if (system_config.Camera1 < filterInfoCollection.Count) Cambox1.SelectedIndex = system_config.Camera1;
                if (system_config.Camera2 < filterInfoCollection.Count) Cambox2.SelectedIndex = system_config.Camera2;
                if (system_config.Camera3 < filterInfoCollection.Count) Cambox3.SelectedIndex = system_config.Camera3;
                if (system_config.Camera4 < filterInfoCollection.Count) Cambox4.SelectedIndex = system_config.Camera4;
                if (system_config.Camera5 < filterInfoCollection.Count) Cambox5.SelectedIndex = system_config.Camera5;
                if (system_config.Camera6 < filterInfoCollection.Count) Cambox6.SelectedIndex = system_config.Camera6;

                if (system_config.add_cam == "true")
                {
                    if (system_config.Camera7 < filterInfoCollection.Count) Cambox7.SelectedIndex = system_config.Camera7;
                }


            }
            else
            {
                Cambox1.Text = "NO CAMERA";
                Cambox2.Text = "NO CAMERA";
                Cambox3.Text = "NO CAMERA";
                Cambox4.Text = "NO CAMERA";
                Cambox5.Text = "NO CAMERA";
                Cambox6.Text = "NO CAMERA";
                Cam1.Enabled = false;
                Cam2.Enabled = false;
                Cam3.Enabled = false;
                Cam4.Enabled = false;
                Cam5.Enabled = false;
                Cam6.Enabled = false;
            }
            //videoCaptureDevice1 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera1].MonikerString);
            // videoCaptureDevice2 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera2].MonikerString);
            //videoCaptureDevice3 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera3].MonikerString);
            //videoCaptureDevice4 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera4].MonikerString);
            //videoCaptureDevice5 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera5].MonikerString);
            //videoCaptureDevice6 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera6].MonikerString);
            if (videoCaptureDevice1 != null || videoCaptureDevice2 != null || videoCaptureDevice3 != null || videoCaptureDevice4 != null || videoCaptureDevice5 != null || videoCaptureDevice6 != null || videoCaptureDevice7 != null)
            {
                system_config = Program_Configuration.GetSystem_Config();
                if (system_config.pixel_cam1 < videoCaptureDevice1.VideoCapabilities.Length) comboBox1.SelectedIndex = system_config.pixel_cam1;
                if (system_config.pixel_cam2 < videoCaptureDevice2.VideoCapabilities.Length) comboBox2.SelectedIndex = system_config.pixel_cam2;
                if (system_config.pixel_cam3 < videoCaptureDevice3.VideoCapabilities.Length) comboBox3.SelectedIndex = system_config.pixel_cam3;
                if (system_config.pixel_cam4 < videoCaptureDevice4.VideoCapabilities.Length) comboBox4.SelectedIndex = system_config.pixel_cam4;
                if (system_config.pixel_cam5 < videoCaptureDevice5.VideoCapabilities.Length) comboBox5.SelectedIndex = system_config.pixel_cam5;
                if (system_config.pixel_cam6 < videoCaptureDevice6.VideoCapabilities.Length) comboBox6.SelectedIndex = system_config.pixel_cam6;
                if (system_config.add_cam == "true")
                {
                    if (system_config.pixel_cam7 < videoCaptureDevice7.VideoCapabilities.Length) comboBox7.SelectedIndex = system_config.pixel_cam7;
                }
            }

            if (system_config.add_cam == "true")
            {
                videoCaptureDevice7 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera7].MonikerString);
            }
            if (videoCaptureDevice1 != null && videoCaptureDevice1.VideoCapabilities.Length > 0 ) 
            {
                foreach (VideoCapabilities videoCapabilities in videoCaptureDevice1.VideoCapabilities)
                {
                    comboBox1.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "X" + videoCapabilities.FrameSize.Height.ToString());

                }
            }
            if (videoCaptureDevice2 != null && videoCaptureDevice2.VideoCapabilities.Length > 0)
            {
                foreach (VideoCapabilities videoCapabilities in videoCaptureDevice2.VideoCapabilities)
                {
                    comboBox2.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "X" + videoCapabilities.FrameSize.Height.ToString());

                }
            }
            if (videoCaptureDevice3 != null && videoCaptureDevice3.VideoCapabilities.Length > 0 )
            {
                foreach (VideoCapabilities videoCapabilities in videoCaptureDevice3.VideoCapabilities)
                {
                    comboBox3.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "X" + videoCapabilities.FrameSize.Height.ToString());

                }
            }
            if (videoCaptureDevice4 != null && videoCaptureDevice4.VideoCapabilities.Length > 0)
            {
                foreach (VideoCapabilities videoCapabilities in videoCaptureDevice4.VideoCapabilities)
                {
                    comboBox4.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "X" + videoCapabilities.FrameSize.Height.ToString());

                }
            }
            if (videoCaptureDevice5 != null && videoCaptureDevice5.VideoCapabilities.Length > 0 )
            {
                foreach (VideoCapabilities videoCapabilities in videoCaptureDevice5.VideoCapabilities)
                {
                    comboBox5.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "X" + videoCapabilities.FrameSize.Height.ToString());

                }
            }
            if (videoCaptureDevice6 != null && videoCaptureDevice6.VideoCapabilities.Length > 0)
            {
                foreach (VideoCapabilities videoCapabilities in videoCaptureDevice6.VideoCapabilities)
                {
                    comboBox6.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "X" + videoCapabilities.FrameSize.Height.ToString());

                }
            }
            if (system_config.add_cam == "true")
            {
                if ( videoCaptureDevice7 != null && videoCaptureDevice7.VideoCapabilities.Length > 0)
                {
                    foreach (VideoCapabilities videoCapabilities in videoCaptureDevice7.VideoCapabilities)
                    {
                        comboBox7.Items.Add(videoCapabilities.FrameSize.Width.ToString() + " X " + videoCapabilities.FrameSize.Height.ToString());
                    }
                }
            }
            

        }

        private void Cam1_Click(object sender, EventArgs e)
        {
            Form Cam1_Form = new Cam_Review(Cambox1.SelectedIndex, Camera1.Name, comboBox1.SelectedIndex);
            Cam1_Form.FormClosed += Cam1_Form_FormClosed;
            this.Hide();
            Cam1_Form.Show();
        }
        private void Cam1_Form_FormClosed(object sender1, FormClosedEventArgs e1)
        {
            this.Show();
        }
        private void Cam2_Click(object sender, EventArgs e)
        {
            Form Cam2_Form = new Cam_Review(Cambox2.SelectedIndex, Camera2.Name, comboBox2.SelectedIndex);
            Cam2_Form.FormClosed += Cam2_Form_FormClosed;
            this.Hide();
            Cam2_Form.Show();
        }
        private void Cam2_Form_FormClosed(object sender1, FormClosedEventArgs e1)
        {
            this.Show();
        }
        private void Cam3_Click(object sender, EventArgs e)
        {
            Form Cam3_Form = new Cam_Review(Cambox3.SelectedIndex, Camera3.Name, comboBox3.SelectedIndex);
            Cam3_Form.FormClosed += Cam3_Form_FormClosed;
            this.Hide();
            Cam3_Form.Show();
        }
        private void Cam3_Form_FormClosed(object sender1, FormClosedEventArgs e1)
        {
            this.Show();
        }
        private void Cam4_Click(object sender, EventArgs e)
        {
            Form Cam4_Form = new Cam_Review(Cambox4.SelectedIndex, Camera4.Name, comboBox4.SelectedIndex);
            Cam4_Form.FormClosed += Cam4_Form_FormClosed;
            this.Hide();
            Cam4_Form.Show();
        }
        private void Cam4_Form_FormClosed(object sender1, FormClosedEventArgs e1)
        {
            this.Show();
        }
        private void Cam5_Click(object sender, EventArgs e)
        {
            Form Cam5_Form = new Cam_Review(Cambox5.SelectedIndex, Camera5.Name, comboBox5.SelectedIndex);
            Cam5_Form.FormClosed += Cam5_Form_FormClosed;
            this.Hide();
            Cam5_Form.Show();
        }
        private void Cam5_Form_FormClosed(object sender1, FormClosedEventArgs e1)
        {
            this.Show();
        }

        private void Cam6_Click(object sender, EventArgs e)
        {
            Form Cam6_Form = new Cam_Review(Cambox6.SelectedIndex, Camera6.Name, comboBox6.SelectedIndex);
            Cam6_Form.FormClosed += Cam6_Form_FormClosed;
            this.Hide();
            Cam6_Form.Show();
        }
        private void Cam6_Form_FormClosed(object sender1, FormClosedEventArgs e1)
        {
            this.Show();
        }
        private void Cam7_Click(object sender, EventArgs e)
        {
            Form Cam7_Form = new Cam_Review(Cambox7.SelectedIndex, Camera7.Name, comboBox7.SelectedIndex);
            Cam7_Form.FormClosed += Cam7_Form_FormClosed;
            this.Hide();
            Cam7_Form.Show();
        }

        private void Cam7_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void Saving_btn_Click(object sender, EventArgs e)
        {
            bool Save_success = true;
            if (Cambox1.Items.Count > 0)
            {
                Program_Configuration.UpdateSystem_Config("Camera1", Cambox1.SelectedIndex.ToString());
                Program_Configuration.UpdateSystem_Config("pixel_cam1", comboBox1.SelectedIndex.ToString());
            }
            else
            {
                MessageBox.Show("Camera1 is not available");
                Save_success = false;
            }
            if (Cambox2.Items.Count > 0)
            {
                Program_Configuration.UpdateSystem_Config("Camera2", Cambox2.SelectedIndex.ToString());
                Program_Configuration.UpdateSystem_Config("pixel_cam2", comboBox2.SelectedIndex.ToString());
            }
            else
            {
                MessageBox.Show("Camera2 is not available");
                Save_success = false;
            }
            if (Cambox3.Items.Count > 0)
            {
                Program_Configuration.UpdateSystem_Config("Camera3", Cambox3.SelectedIndex.ToString());
                Program_Configuration.UpdateSystem_Config("pixel_cam3", comboBox3.SelectedIndex.ToString());
            }
            else
            {
                MessageBox.Show("Camera3 is not available");
                Save_success = false;
            }
            if (Cambox4.Items.Count > 0)
            {
                Program_Configuration.UpdateSystem_Config("Camera4", Cambox4.SelectedIndex.ToString());
                Program_Configuration.UpdateSystem_Config("pixel_cam4", comboBox4.SelectedIndex.ToString());
            }
            else
            {
                MessageBox.Show("Camera4 is not available");
                Save_success = false;
            }
            if (Cambox5.Items.Count > 0)
            {
                Program_Configuration.UpdateSystem_Config("Camera5", Cambox5.SelectedIndex.ToString());
                Program_Configuration.UpdateSystem_Config("pixel_cam5", comboBox5.SelectedIndex.ToString());
            }
            else
            {
                MessageBox.Show("Camera5 is not available");
                Save_success = false;
            }
            if (Cambox6.Items.Count > 0)
            {
                Program_Configuration.UpdateSystem_Config("Camera6", Cambox6.SelectedIndex.ToString());
                Program_Configuration.UpdateSystem_Config("pixel_cam6", comboBox6.SelectedIndex.ToString());
            }
            else
            {
                MessageBox.Show("Camera6 is not available");
                Save_success = false;
            }
            if (Cambox7.Items.Count > 0 && system_config.add_cam == "true")
            {
                Program_Configuration.UpdateSystem_Config("Camera7", Cambox6.SelectedIndex.ToString());
                Program_Configuration.UpdateSystem_Config("pixel_cam7", comboBox6.SelectedIndex.ToString());
            }
            else if (Cambox7.Items.Count < 0 && system_config.add_cam == "true")
            {
                MessageBox.Show("Camera7 is not available");
                Save_success = false;
            }
            system_config = Program_Configuration.GetSystem_Config();
            if (Save_success) MessageBox.Show("Camera setting are updated successfully");
            this.Close();

        }

        private void Camera_setting_Form_Load(object sender, EventArgs e)
        {

            if (system_config.add_cam == "false")
            {
                Add_cam.Text = "Add Camera";
                Camera7.Visible = false;
                Cambox7.Visible = false;
                Cam7.Visible = false;
                comboBox7.Visible = false;
            }
            if (system_config.add_cam == "true")
            {
                Add_cam.Text = "Disable Explore Camera";
                Camera7.Visible = true;
                Cambox7.Visible = true;
                Cam7.Visible = true;
                comboBox7.Visible = true;
            }
            system_config = Program_Configuration.GetSystem_Config();
            if (system_config.Camera1 < filterInfoCollection.Count) Cambox1.SelectedIndex = system_config.Camera1;
            if (system_config.Camera2 < filterInfoCollection.Count) Cambox2.SelectedIndex = system_config.Camera2;
            if (system_config.Camera3 < filterInfoCollection.Count) Cambox3.SelectedIndex = system_config.Camera3;
            if (system_config.Camera4 < filterInfoCollection.Count) Cambox4.SelectedIndex = system_config.Camera4;
            if (system_config.Camera5 < filterInfoCollection.Count) Cambox5.SelectedIndex = system_config.Camera5;
            if (system_config.Camera6 < filterInfoCollection.Count) Cambox6.SelectedIndex = system_config.Camera6;

            if (system_config.add_cam == "true")
            {
                if (system_config.Camera7 < filterInfoCollection.Count) Cambox7.SelectedIndex = system_config.Camera7;
            }
            else if (filterInfoCollection.Count < 0)
            {
                Cambox1.Text = "NO CAMERA";
                Cambox2.Text = "NO CAMERA";
                Cambox3.Text = "NO CAMERA";
                Cambox4.Text = "NO CAMERA";
                Cambox5.Text = "NO CAMERA";
                Cambox6.Text = "NO CAMERA";
                Cam1.Enabled = false;
                Cam2.Enabled = false;
                Cam3.Enabled = false;
                Cam4.Enabled = false;
                Cam5.Enabled = false;
                Cam6.Enabled = false;
            }
            if (videoCaptureDevice1 != null || videoCaptureDevice2 != null || videoCaptureDevice3 != null || videoCaptureDevice4 != null || videoCaptureDevice5 != null || videoCaptureDevice6 != null || videoCaptureDevice7 != null)
            {
                system_config = Program_Configuration.GetSystem_Config();
                videoCaptureDevice1 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera1].MonikerString);
                if (videoCaptureDevice1 != null && videoCaptureDevice1.VideoCapabilities.Length > 0)
                    {
                        foreach (VideoCapabilities videoCapabilities in videoCaptureDevice1.VideoCapabilities)
                        {
                            comboBox1.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "X" + videoCapabilities.FrameSize.Height.ToString());

                        }
                        comboBox1.SelectedIndex = system_config.pixel_cam1;
                    }
                videoCaptureDevice2 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera2].MonikerString);
                if (system_config.pixel_cam2 < videoCaptureDevice2.VideoCapabilities.Length) 
                {
                    if (videoCaptureDevice2 != null && videoCaptureDevice2.VideoCapabilities.Length > 0)
                    {
                        foreach (VideoCapabilities videoCapabilities in videoCaptureDevice2.VideoCapabilities)
                        {
                            comboBox2.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "X" + videoCapabilities.FrameSize.Height.ToString());

                        }
                        comboBox2.SelectedIndex = system_config.pixel_cam2;
                    }
                }
                videoCaptureDevice3 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera3].MonikerString);
                if (system_config.pixel_cam3 < videoCaptureDevice3.VideoCapabilities.Length) 
                {
                    if (videoCaptureDevice3 != null && videoCaptureDevice3.VideoCapabilities.Length > 0)
                    {
                        foreach (VideoCapabilities videoCapabilities in videoCaptureDevice3.VideoCapabilities)
                        {
                            comboBox3.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "X" + videoCapabilities.FrameSize.Height.ToString());

                        }
                        comboBox3.SelectedIndex = system_config.pixel_cam3;
                    }
                   
                }
                videoCaptureDevice4 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera4].MonikerString);
                if (system_config.pixel_cam4 < videoCaptureDevice4.VideoCapabilities.Length) 
                {
                    if (videoCaptureDevice4 != null && videoCaptureDevice4.VideoCapabilities.Length > 0)
                    {
                        foreach (VideoCapabilities videoCapabilities in videoCaptureDevice4.VideoCapabilities)
                        {
                            comboBox4.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "X" + videoCapabilities.FrameSize.Height.ToString());

                        }
                        comboBox4.SelectedIndex = system_config.pixel_cam4;
                    }
                }
                videoCaptureDevice5 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera5].MonikerString);
                if (system_config.pixel_cam5 < videoCaptureDevice5.VideoCapabilities.Length) 
                {
                    if (videoCaptureDevice5 != null && videoCaptureDevice5.VideoCapabilities.Length > 0)
                    {
                        foreach (VideoCapabilities videoCapabilities in videoCaptureDevice5.VideoCapabilities)
                        {
                            comboBox5.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "X" + videoCapabilities.FrameSize.Height.ToString());

                        }
                        comboBox5.SelectedIndex = system_config.pixel_cam5;
                    }
                }
                videoCaptureDevice6 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera6].MonikerString);
                if (system_config.pixel_cam6 < videoCaptureDevice6.VideoCapabilities.Length) 
                {
                    if (videoCaptureDevice6 != null && videoCaptureDevice6.VideoCapabilities.Length > 0)
                    {
                        foreach (VideoCapabilities videoCapabilities in videoCaptureDevice6.VideoCapabilities)
                        {
                            comboBox6.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "X" + videoCapabilities.FrameSize.Height.ToString());

                        }
                        comboBox6.SelectedIndex = system_config.pixel_cam6;
                    }
                }
                if (system_config.add_cam == "true")
                {
                    videoCaptureDevice7 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera7].MonikerString);
                    if (system_config.pixel_cam7 < videoCaptureDevice7.VideoCapabilities.Length)
                    {
                        comboBox7.Items.Clear();
                        if (videoCaptureDevice7 != null && videoCaptureDevice7.VideoCapabilities.Length > 0)
                        {
                            foreach (VideoCapabilities videoCapabilities in videoCaptureDevice7.VideoCapabilities)
                            {
                                comboBox7.Items.Add(videoCapabilities.FrameSize.Width.ToString() + " X " + videoCapabilities.FrameSize.Height.ToString());
                            }
                            comboBox7.SelectedIndex = system_config.pixel_cam7;
                        }

                    }
                }
            }
            else if(filterInfoCollection.Count < 0)
            {
                Cambox1.Text = "NO CAMERA";
                Cambox2.Text = "NO CAMERA";
                Cambox3.Text = "NO CAMERA";
                Cambox4.Text = "NO CAMERA";
                Cambox5.Text = "NO CAMERA";
                Cambox6.Text = "NO CAMERA";
                Cam1.Enabled = false;
                Cam2.Enabled = false;
                Cam3.Enabled = false;
                Cam4.Enabled = false;
                Cam5.Enabled = false;
                Cam6.Enabled = false;
            }

        } 

        private void Cambox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (load) 
            {
                while (load) 
                {
                    comboBox1.Items.Clear();
                    system_config.Camera1 = Cambox1.SelectedIndex;
                    Program_Configuration.UpdateSystem_Config("Camera1", Cambox1.SelectedIndex.ToString());
                    system_config = Program_Configuration.GetSystem_Config();
                    videoCaptureDevice1 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera1].MonikerString);
                    if (videoCaptureDevice1.VideoCapabilities.Length > 0)
                    {
                        foreach (VideoCapabilities videoCapabilities in videoCaptureDevice1.VideoCapabilities)
                        {
                            comboBox1.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "x" + videoCapabilities.FrameSize.Height.ToString());

                        }
                    }
                    Program_Configuration.UpdateSystem_Config("pixel_cam1", comboBox1.SelectedIndex.ToString());                  
                    load = false;
                    break;
                }
               
            }    
        }

        private void Cambox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load1)
            {
                while (load1)
                {
                    comboBox2.Items.Clear();
                    system_config.Camera2 = Cambox1.SelectedIndex;
                    Program_Configuration.UpdateSystem_Config("Camera2", Cambox2.SelectedIndex.ToString());
                    system_config = Program_Configuration.GetSystem_Config();
                    videoCaptureDevice2 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera2].MonikerString);
                    if (videoCaptureDevice1.VideoCapabilities.Length > 0)
                    {
                        foreach (VideoCapabilities videoCapabilities in videoCaptureDevice2.VideoCapabilities)
                        {
                            comboBox2.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "x" + videoCapabilities.FrameSize.Height.ToString());

                        }
                    }
                    Program_Configuration.UpdateSystem_Config("pixel_cam2", comboBox2.SelectedIndex.ToString());
                    load1 = false;
                    break;
                }

            }
        }

        private void Cambox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load2)
            {
                while (load2)
                {
                    comboBox3.Items.Clear();
                    system_config.Camera3 = Cambox3.SelectedIndex;
                    Program_Configuration.UpdateSystem_Config("Camera3", Cambox3.SelectedIndex.ToString());
                    system_config = Program_Configuration.GetSystem_Config();
                    videoCaptureDevice3 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera3].MonikerString);
                    if (videoCaptureDevice3.VideoCapabilities.Length > 0)
                    {
                        foreach (VideoCapabilities videoCapabilities in videoCaptureDevice3.VideoCapabilities)
                        {
                            comboBox3.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "x" + videoCapabilities.FrameSize.Height.ToString());
                        }
                    }
                    Program_Configuration.UpdateSystem_Config("pixel_cam3", comboBox3.SelectedIndex.ToString());                   
                    load2 = false;
                    break;
                }

            }
        }

        private void Cambox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load3)
            {
                while (load3)
                {
                    comboBox4.Items.Clear();
                    system_config.Camera4 = Cambox4.SelectedIndex;
                    Program_Configuration.UpdateSystem_Config("Camera4", Cambox4.SelectedIndex.ToString());
                    system_config = Program_Configuration.GetSystem_Config();
                    videoCaptureDevice4 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera4].MonikerString);
                    if (videoCaptureDevice4.VideoCapabilities.Length > 0)
                    {
                        foreach (VideoCapabilities videoCapabilities in videoCaptureDevice4.VideoCapabilities)
                        {
                            comboBox4.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "x" + videoCapabilities.FrameSize.Height.ToString());

                        }
                    }
                    Program_Configuration.UpdateSystem_Config("pixel_cam4", comboBox4.SelectedIndex.ToString());
                    load3 = false;
                    break;
                }

            }
        }

        private void Cambox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load4)
            {
                while (load4)
                {
                    comboBox5.Items.Clear();
                    system_config.Camera5 = Cambox5.SelectedIndex;
                    Program_Configuration.UpdateSystem_Config("Camera5", Cambox5.SelectedIndex.ToString());
                    system_config = Program_Configuration.GetSystem_Config();
                    videoCaptureDevice5 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera5].MonikerString);
                    if (videoCaptureDevice5.VideoCapabilities.Length > 0)
                    {
                        foreach (VideoCapabilities videoCapabilities in videoCaptureDevice5.VideoCapabilities)
                        {
                            comboBox5.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "x" + videoCapabilities.FrameSize.Height.ToString());

                        }
                    }
                    Program_Configuration.UpdateSystem_Config("pixel_cam5", comboBox5.SelectedIndex.ToString());
                    load4 = false;
                    break;
                }
            }
        }

        private void Cambox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load5)
            {
                while (load5)
                {
                    comboBox6.Items.Clear();
                    system_config.Camera6 = Cambox6.SelectedIndex;
                    Program_Configuration.UpdateSystem_Config("Camera6", Cambox6.SelectedIndex.ToString());
                    system_config = Program_Configuration.GetSystem_Config();
                    videoCaptureDevice6 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera6].MonikerString);
                    if (videoCaptureDevice6.VideoCapabilities.Length > 0)
                    {
                        foreach (VideoCapabilities videoCapabilities in videoCaptureDevice6.VideoCapabilities)
                        {
                            comboBox6.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "x" + videoCapabilities.FrameSize.Height.ToString());

                        }
                    }
                    Program_Configuration.UpdateSystem_Config("pixel_cam6", comboBox6.SelectedIndex.ToString());
                    load5 = false;
                    break;
                }

            }
        }
        private void Cambox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load6)
            {
                while (load6)
                {
                    comboBox7.Items.Clear();
                    system_config.Camera7 = Cambox7.SelectedIndex;
                    Program_Configuration.UpdateSystem_Config("Camera7", Cambox7.SelectedIndex.ToString());
                    system_config = Program_Configuration.GetSystem_Config();
                    videoCaptureDevice7 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera7].MonikerString);
                    if (videoCaptureDevice7.VideoCapabilities.Length > 0)
                    {
                        foreach (VideoCapabilities videoCapabilities in videoCaptureDevice7.VideoCapabilities)
                        {
                            comboBox7.Items.Add(videoCapabilities.FrameSize.Width.ToString() + "x" + videoCapabilities.FrameSize.Height.ToString());

                        }
                    }
                    Program_Configuration.UpdateSystem_Config("pixel_cam7", comboBox7.SelectedIndex.ToString());
                    load6 = false;
                    break;
                }

            }
        }

        bool load = false;
        bool load1 = false;
        bool load2 = false;
        bool load3 = false;
        bool load4 = false;
        bool load5 = false;
        bool load6 = false;
        private void Cambox1_MouseDown(object sender, MouseEventArgs e)
        {

            load = true;
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            Cambox1.Items.Clear();
           
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {

                Cambox1.Items.Add(filterInfo.Name);
               
            }
        }

        private void Cambox2_MouseDown(object sender, MouseEventArgs e)
        {
            load1 = true;
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
           
            Cambox2.Items.Clear();
           
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
               
                Cambox2.Items.Add(filterInfo.Name);
            }
        }

        private void Cambox3_MouseDown(object sender, MouseEventArgs e)
        {
            load2 = true;
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        
            Cambox3.Items.Clear();
                   
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                
                Cambox3.Items.Add(filterInfo.Name);
            }
        }

        private void Cambox4_MouseDown(object sender, MouseEventArgs e)
        {
            load3 = true;
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
   
            Cambox4.Items.Clear();
       

            if (system_config.add_cam.ToString() == "true")
            {
                Cambox7.Items.Clear();
            }
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
              
                Cambox4.Items.Add(filterInfo.Name);
             

            }
        }

        private void Cambox5_MouseDown(object sender, MouseEventArgs e)
        {
            load4 = true;
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
         
            Cambox5.Items.Clear();
       
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
            
                Cambox5.Items.Add(filterInfo.Name);
           

            }
        }

        private void Cambox6_MouseDown(object sender, MouseEventArgs e)
        {
            load5 = true;
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
         
            Cambox6.Items.Clear();

            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
               
                Cambox6.Items.Add(filterInfo.Name);           
            }
        }
        private void Cambox7_MouseDown(object sender, MouseEventArgs e)
        {
            system_config = Program_Configuration.GetSystem_Config();
            if(system_config.add_cam == "true") 
            {
                load6 = true;
                filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                
                if (system_config.add_cam.ToString() == "true")
                {
                    Cambox7.Items.Clear();
                }
                foreach (FilterInfo filterInfo in filterInfoCollection)
                {
                  
                    if (system_config.add_cam.ToString() == "true")
                    {
                        Cambox7.Items.Add(filterInfo.Name);
                    }

                }
            }
           
        }
        private void Add_cam_Click(object sender, EventArgs e)
        {
            system_config = Program_Configuration.GetSystem_Config();
            if (system_config.add_cam == "false") 
            {              
                Program_Configuration.UpdateSystem_Config("add_cam", "true");
                
                Camera7.Visible = true;
                Cambox7.Visible = true;
                Cam7.Visible = true;
                comboBox7.Visible = true;
                Add_cam.Text = "Disable Explore Camera";
                Cambox7.Items.Clear();
                foreach (FilterInfo filterInfo in filterInfoCollection)
                {
                    Cambox7.Items.Add(filterInfo.Name);
                }
                videoCaptureDevice7 = new VideoCaptureDevice(filterInfoCollection[system_config.Camera7].MonikerString);
            }
            if (system_config.add_cam == "true") 
            {
                            
                Program_Configuration.UpdateSystem_Config("add_cam", "false");
                Cambox7.Items.Clear();
                comboBox7.Items.Clear();
                Camera7.Visible = false;
                Cambox7.Visible = false;
                Cam7.Visible = false;
                comboBox7.Visible = false;
                Add_cam.Text = "Add Camera";
            }
           
        }      
    }
}

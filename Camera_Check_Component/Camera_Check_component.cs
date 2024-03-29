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
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;

using System.IO.Ports;
using System.IO;
using AForge;
using System.Drawing.Imaging;
using S7.Net;
using System.Drawing.Drawing2D;
using System.Threading;

namespace Camera_Check_Component
{
    public partial class Camera_Check_component : Form
    { 
        #region/////////////////////////////////////////////////////// DECLARE
        private FilterInfoCollection filterInfoCollection;
        private VideoCaptureDevice Cam1VIDEO_Device;
        private VideoCaptureDevice Cam2VIDEO_Device;
        private VideoCaptureDevice Cam3VIDEO_Device;
        private VideoCaptureDevice Cam4VIDEO_Device;
        private VideoCaptureDevice Cam5VIDEO_Device;
        private VideoCaptureDevice Cam6VIDEO_Device;
        private VideoCaptureDevice Cam7VIDEO_Device;
        private FilterInfo Cam1_Device;
        private FilterInfo Cam2_Device;
        private FilterInfo Cam3_Device;
        private FilterInfo Cam4_Device;
        private FilterInfo Cam5_Device;
        private FilterInfo Cam6_Device;
        private FilterInfo Cam7_Device;
        private System_config system_config;

        Bitmap Live_Cam_1;
        Bitmap Live_Cam_2;
        Bitmap Live_Cam_3;
        Bitmap Live_Cam_4;
        Bitmap Live_Cam_5;
        Bitmap Live_Cam_6;
        Bitmap Live_Cam_7;
        private BackgroundWorker backgroundWorker_1 = new BackgroundWorker();
        private BackgroundWorker backgroundWorker_2 = new BackgroundWorker();
        private BackgroundWorker backgroundWorker_3 = new BackgroundWorker();
        private BackgroundWorker backgroundWorker_4 = new BackgroundWorker();
        private BackgroundWorker backgroundWorker_5 = new BackgroundWorker();
        private BackgroundWorker backgroundWorker_6 = new BackgroundWorker();
        private BackgroundWorker backgroundWorker_7 = new BackgroundWorker();
        private BackgroundWorker ledinf = new BackgroundWorker();
        BackgroundWorker shot_pic = new BackgroundWorker();
        BackgroundWorker wdata = new BackgroundWorker();
        private SQL_action sql_action = new SQL_action();
        bool order_1 = false;
        bool order_2 = false;
        bool order_3 = false;
        bool order_4 = false;
        bool order_5 = false;
        bool order_6 = false;
        bool order_7 = false;

        bool start_check = false;
        bool allow_check = false;
        bool loadform = false;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        //private System.Windows.Forms.Timer cam_call_back = new System.Windows.Forms.Timer();
       
        private double startPR_Count = 0;
        private double timer_sum = 0;
        private double timer_star = 0;
        private Int64 count_1 = 0;
        private Int64 count_2 = 0;
        private Int64 count_3 = 0;
        private Int64 count_4 = 0;
        private Int64 count_5 = 0;
        private Int64 count_6 = 0;
        private Int64 count_7 = 0;
        private Int64 folderIndex = 0;
        private Int64 load1 = 0;
        private Int64 load2 = 0;
        bool started = false;
        double ratio;
        int stt = 0;
        string DMY = "";
        #endregion
        public Camera_Check_component()
        {
            InitializeComponent();
        }
        
        #region////////////////////////////////////////////////////////////////////////////////////////////////SET UP
        private void Camera_Check_component_Load(object sender, EventArgs e)
        {
            if (loadform) return;
            loadform = true;
            this.Location = new System.Drawing.Point(0, 0);
            system_config = Program_Configuration.GetSystem_Config();
            if (Screen.AllScreens.Length > 1) 
            {
                //this.Size = Screen.PrimaryScreen.WorkingArea.Size;
                this.Width = 3840;
                this.Height = 1080;
            }
            else 
            {
                this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            }
            //unable();
            listviewInit();
            system_config = Program_Configuration.GetSystem_Config();
            DateTime dt = DateTime.Now;
            string daytime = dt.Day.ToString() + "-" + dt.Month.ToString() + "-" + dt.Year.ToString();
            DMY = daytime;
           
            
            if (system_config.PN_Selector != "" || system_config.PN_Selector != null)
            {
                tb_PN.Text = system_config.PN_Selector;

            }
            else
            {
                Program_Configuration.UpdateSystem_Config("PN_Selector", "xxx");
            }

            //if (system_config.new_Day != dt.Day || system_config.new_Month != dt.Month)

            //{
            //    count_1 = 0; count_2 = 0; count_3 = 0; count_4 = 0; count_5 = 0; count_6 = 0; count_7 = 0; folderIndex = 0;load1 = 0;load2 = 1;
            //    Program_Configuration.UpdateSystem_Config("Folder_index_tranfer", load1.ToString());
            //    Program_Configuration.UpdateSystem_Config("Folder_load_check", load2.ToString());
            //    Program_Configuration.UpdateSystem_Config("same_folder_1", folderIndex.ToString());

            //    //Program_Configuration.UpdateSystem_Config("Folder_index_tranfer", folderIndex.ToString());
            //    //Program_Configuration.UpdateSystem_Config("Folder_load_check", folderIndex.ToString());
            //    Program_Configuration.UpdateSystem_Config("Location_cam1_folder", count_1.ToString());
            //    Program_Configuration.UpdateSystem_Config("Location_cam2_folder", count_2.ToString());
            //    Program_Configuration.UpdateSystem_Config("Location_cam3_folder", count_3.ToString());
            //    Program_Configuration.UpdateSystem_Config("Location_cam4_folder", count_4.ToString());
            //    Program_Configuration.UpdateSystem_Config("Location_cam5_folder", count_5.ToString());
            //    Program_Configuration.UpdateSystem_Config("Location_cam6_folder", count_6.ToString());
            //    Program_Configuration.UpdateSystem_Config("Location_cam7_folder", count_7.ToString());
            //    Program_Configuration.UpdateSystem_Config("new_Day", dt.Day.ToString());
            //    Program_Configuration.UpdateSystem_Config("new_Month", dt.Month.ToString());
            //    Program_Configuration.UpdateSystem_Config("new_Year", dt.Year.ToString());
            //    _OKnum = 0;
            //    _NGnum = 0;
            //    _sum = 0;
            //    using (StreamWriter sw =new StreamWriter("Output.txt")) 
            //    {
            //        sw.WriteLine("" + _OKnum.ToString() + "");
            //        sw.WriteLine("" + _NGnum.ToString() + "");
            //    }
            //}
            if (File.Exists("Output.txt")) 
            {
                using (StreamReader sr = new StreamReader("Output.txt"))
                {
                    int a = 0;
                    string[] read = new string[2];
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                      
                        read[a] = line;
                        a++;

                    }
                    if (a > 1) 
                    {
                        _OKnum = Convert.ToInt16(read[0]);
                        _NGnum = Convert.ToInt16(read[1]);
                        _sum = (short)(_NGnum + _OKnum);
                    }
                    else 
                    {
                        _OKnum = 0;
                        _NGnum = 0;
                        _sum = (short)(_NGnum + _OKnum);
                    }
                    
                }
            }
            if (count_1 != system_config.Location_cam1_folder || count_2 != system_config.Location_cam2_folder || count_3 != system_config.Location_cam3_folder || count_4 != system_config.Location_cam4_folder || count_5 != system_config.Location_cam5_folder || count_6 != system_config.Location_cam6_folder || count_7 != system_config.Location_cam7_folder || load1 != system_config.Folder_index_tranfer || load2 != system_config.Folder_load_check || folderIndex != system_config.same_folder_1)
            {
                folderIndex = system_config.same_folder_1;
                load1 = system_config.Folder_index_tranfer;
                load2 = system_config.Folder_load_check;
                count_1 = system_config.Location_cam1_folder;
                count_2 = system_config.Location_cam2_folder;
                count_3 = system_config.Location_cam3_folder;
                count_4 = system_config.Location_cam4_folder;
                count_5 = system_config.Location_cam5_folder;
                count_6 = system_config.Location_cam6_folder;
                count_7 = system_config.Location_cam7_folder;
            }
            Start_btn.Enabled = true;
            Stop_btn.Enabled = false;
          
            Pic_Cam1.SizeMode = PictureBoxSizeMode.StretchImage;
            Pic_Cam2.SizeMode = PictureBoxSizeMode.StretchImage;
            Pic_Cam3.SizeMode = PictureBoxSizeMode.StretchImage;
            Pic_Cam4.SizeMode = PictureBoxSizeMode.StretchImage;
            Pic_Cam5.SizeMode = PictureBoxSizeMode.StretchImage;
            Pic_Cam6.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox15.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox16.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox17.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox18.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox19.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox20.SizeMode = PictureBoxSizeMode.StretchImage;

            pic_full1.SizeMode = PictureBoxSizeMode.StretchImage;
            picfull_2.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_full1.Hide();
            picfull_2.Hide();

            c1h1_1.Visible = false; c1h2_1.Visible = false;
            c2h1_1.Visible = false; c2h2_1.Visible = false;
            c3h1_1.Visible = false; c1h3_1.Visible = false;
            c4h1_1.Visible = false; c2h3_1.Visible = false;
            c1h6_1.Visible = false; c1h4_1.Visible = false;
            c2h6_1.Visible = false; c2h4_1.Visible = false;
            c3h6_1.Visible = false; c1h5_1.Visible = false;
            c4h6_1.Visible = false; c2h5_1.Visible = false;

            c1h1_2.Visible = false; c1h2_2.Visible = false;
            c2h1_2.Visible = false; c2h2_2.Visible = false;
            c3h1_2.Visible = false; c1h3_2.Visible = false;
            c4h1_2.Visible = false; c2h3_2.Visible = false;
            c1h6_2.Visible = false; c1h4_2.Visible = false;
            c2h6_2.Visible = false; c2h4_2.Visible = false;
            c3h6_2.Visible = false; c1h5_2.Visible = false;
            c4h6_2.Visible = false; c2h5_2.Visible = false;

            pinf411.Visible = false;
            pinf421.Visible = false;
            pinf431.Visible = false;
            pinf441.Visible = false;
            pinf211.Visible = false;
            pinf221.Visible = false;

            pinf412.Visible = false;
            pinf422.Visible = false;
            pinf432.Visible = false;
            pinf442.Visible = false;
            pinf212.Visible = false;
            pinf222.Visible = false;

           
          
            PB_active1.SizeMode = PictureBoxSizeMode.StretchImage;
            PB_active1.Hide();
            PB_active2.SizeMode = PictureBoxSizeMode.StretchImage;
            PB_active2.Hide();
            PB_active3.SizeMode = PictureBoxSizeMode.StretchImage;
            PB_active3.Hide();
            PB_active4.SizeMode = PictureBoxSizeMode.StretchImage;
            PB_active4.Hide();
            PB_active5.SizeMode = PictureBoxSizeMode.StretchImage;
            PB_active5.Hide();
            PB_active6.SizeMode = PictureBoxSizeMode.StretchImage;
            PB_active6.Hide();
            pic_loading1.Hide();
            pic_loading2.Hide();
            picload_in.Visible = false;
            #region ///////////////////////////////////////////////////////////// khai báo background worker
            backgroundWorker_1.DoWork += backgroundWorker_1_DoWork;
            backgroundWorker_1.RunWorkerCompleted += backgroundWorker_1_RunWorkerCompleted;
            backgroundWorker_1.WorkerSupportsCancellation = true;

            backgroundWorker_2.DoWork += backgroundWorker_2_DoWork;
            backgroundWorker_2.RunWorkerCompleted += backgroundWorker_2_RunWorkerCompleted;
            backgroundWorker_2.WorkerSupportsCancellation = true;

            backgroundWorker_3.DoWork += backgroundWorker_3_DoWork;
            backgroundWorker_3.RunWorkerCompleted += backgroundWorker_3_RunWorkerCompleted;
            backgroundWorker_3.WorkerSupportsCancellation = true;

            backgroundWorker_4.DoWork += backgroundWorker_4_DoWork;
            backgroundWorker_4.RunWorkerCompleted += backgroundWorker_4_RunWorkerCompleted;
            backgroundWorker_4.WorkerSupportsCancellation = true;

            backgroundWorker_5.DoWork += backgroundWorker_5_DoWork;
            backgroundWorker_5.RunWorkerCompleted += backgroundWorker_5_RunWorkerCompleted;
            backgroundWorker_5.WorkerSupportsCancellation = true;

            backgroundWorker_6.DoWork += backgroundWorker_6_DoWork;
            backgroundWorker_6.RunWorkerCompleted += backgroundWorker_6_RunWorkerCompleted;
            backgroundWorker_6.WorkerSupportsCancellation = true;

            backgroundWorker_7.DoWork += BackgroundWorker_7_DoWork;
            backgroundWorker_7.RunWorkerCompleted += BackgroundWorker_7_RunWorkerCompleted;
            backgroundWorker_7.WorkerSupportsCancellation = true;

            ledinf.DoWork += Ledinf_DoWork;
            ledinf.RunWorkerCompleted += Ledinf_RunWorkerCompleted;
            ledinf.WorkerSupportsCancellation = true;
            shot_pic.DoWork += Shot_pic_DoWork;
            shot_pic.RunWorkerCompleted += Shot_pic_RunWorkerCompleted;
            shot_pic.WorkerSupportsCancellation = true;

            wdata.DoWork += Wdata_DoWork;
            wdata.RunWorkerCompleted += Wdata_RunWorkerCompleted;
            wdata.WorkerSupportsCancellation = true;
            #endregion
            Program_Configuration.UpdateSystem_Config("Location_cam1_folder", count_1.ToString());
            system_config.Location_cam1_folder = Convert.ToInt32(Program_Configuration.GetSystem_Config_Value("Location_cam1_folder"));

            //Parameter_app.OK_TEMP(daytime, system_config.Location_cam1_folder.ToString());
            //Parameter_app.ERROR_TEMP(daytime, system_config.Location_cam1_folder.ToString());
            //label_time.Text = DateTime.Now.ToString();
            if (system_config.inf_process == null)
            {
                TB_LTdate.Text = "";
            }
            else
            {
                TB_LTdate.Text = system_config.inf_process.ToString();
            }
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Enabled = true;
            call_back_cam.Enabled = false;
           // cam_call_back.Interval = 1000;
           // cam_call_back.Tick += Cam_call_back_Tick;
            //cam_call_back.Enabled = true;
            timer.Start();
            //TB_idworker.Enabled = false;
            //TB_wker2.Enabled = false;
            //textBox_stt.Enabled = false;           
            //listView1.Enabled = false;
            listView1.Scrollable = true;
            OKnum.Text = _OKnum.ToString();
            NGnum.Text = _NGnum.ToString();
            totalPN.Text = _sum.ToString();
            
        }

        private void Wdata_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!wdata.IsBusy) wdata.RunWorkerAsync();
        }

        private void Wdata_DoWork(object sender, DoWorkEventArgs e)
        {
            if (w1) 
            {
                string Addr = "DB5.DBX26.0";
                PLCS7_1200.Write(Addr, true);
                w1 = false;
            }
            if (w2)
            {
                string Addr = "DB5.DBX26.1";
                PLCS7_1200.Write(Addr, true);
                w2 = false;
            }
            if (w3)
            {
                string Addr = "DB5.DBX26.2";
                PLCS7_1200.Write(Addr, true);
                w3 = false;
            }
            if (w4)
            {
                string Addr = "DB5.DBX26.3";
                PLCS7_1200.Write(Addr, true);
                w4 = false;
                Thread.Sleep(5);
            }
            if (w5)
            {
                string Addr = "DB5.DBX26.4";
                PLCS7_1200.Write(Addr, true);
                w5 = false;
            }
            if (w6)
            {

                string Addr = "DB5.DBX26.5";
                PLCS7_1200.Write(Addr, true);
                w6 = false;
            }

        }

        private void Ledinf_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (!ledinf.IsBusy) ledinf.RunWorkerAsync();
        }

        private void Ledinf_DoWork(object sender, DoWorkEventArgs e)
        {

            if (started) 
            {
                panel11.BackColor = Color.Green;
            }
            else 
            {
                panel11.BackColor = Color.Red;
            }
        }

        private void unable()
        {
           
            foreach (Control ctl in General_tab.Controls)
            {
                if (ctl.Name == "tabPage3")
                {
                    ctl.Enabled = true;
                    foreach (Control ctrl in tabPage3.Controls)
                    {
                        if (ctrl.Name == "login_btn")
                        {
                            ctrl.Enabled = true;

                        }
                        else
                        {
                            ctrl.Enabled = false;
                        }
                    }
                }
                else
                {
                    ctl.Enabled = false;
                }
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {

            timer_sum++;
            if (started)
            {
                startPR_Count++;
                TimeSpan time = TimeSpan.FromSeconds(startPR_Count);
                LB_TIMER.Text = time.ToString(@"hh\:mm\:ss");
                timer_star++;
              //  if (serialPort_communicate.IsOpen) serialPort_communicate.Close();
                if (!serialPort_communicate.IsOpen) serialPort_communicate.Open();
                if (General_tab.SelectedIndex == 2 && run_out1 && folderIndex < count_6) 
                {
                    upload_image();
                    run_out1 = false;
                }
                if (General_tab.SelectedIndex == 2 && run_out2 && folderIndex < count_6)
                {
                    update_image2();
                    run_out2 = false;
                }            
            }
            else
            {
                ratio = (timer_star / timer_sum) * 100;
            }

            ratio = (timer_star / timer_sum) * 100;
            progressBar1.Value = Convert.ToInt32(ratio);
            LB_oee.Text = Convert.ToInt32(ratio).ToString() + "%";

        }

        private void listviewInit()
        {
            listView1.View = View.Details;
            listView1.Columns.Add("PN Selector");
            listView1.Columns.Add("Status");
            listView1.Columns.Add("Error Type");
            listView1.Columns[1].Width = 42;
            listView1.Columns[0].Width = 140;
            listView1.Columns[2].Width = 140;
        }
        private void set_up()
        {

            if (!Directory.Exists(Parameter_app.IMAGE_FOLDER_PATH))
            {
                Directory.CreateDirectory(Parameter_app.IMAGE_FOLDER_PATH);
            }
            //if (!Directory.Exists(Parameter_app.OK_IMAGE_FOLDER_PATH) && allow_check)
            //{
            //    //Parameter_app.OK_TEMP(daytime, system_config.Location_cam1_folder.ToString());
            //    Directory.CreateDirectory(Parameter_app.OK_IMAGE_FOLDER_PATH);
            //}
            //if (!Directory.Exists(Parameter_app.ERROR_IMAGE_FOLDER_PATH) && allow_check)
            //{
            //    Directory.CreateDirectory(Parameter_app.ERROR_IMAGE_FOLDER_PATH);
            //}
        }
        #endregion

        #region ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////CHỤP ẢNH
        private void BackgroundWorker_7_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            shot7 = "";
            status(" [SYSTEM]" + " CAM 7 Save image" + " " + count_7.ToString());
            n7 = (int)(count_7 / 6);
            count_7++;

        }
        private void BackgroundWorker_7_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker_7.CancellationPending)
            {
                e.Cancel = true;
            }

            if (system_config.add_cam == "true")
            {

                DateTime date = DateTime.Now;
                date.ToString("HH:MM:ss");
                set_up();
                MethodInvoker inv = delegate
                {
                    string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-7" + "-" + count_7.ToString() + ".jpeg";                  
                    string outputFileName = system_config.Map_Path_File + @"\" + str + "";
                    if (count_7 / 6 == 0)
                    {
                        if (count_7 % 6 == 0) lan1_vt7 = outputFileName;
                        if (count_7 % 6 == 1) lan2_vt7 = outputFileName;
                        if (count_7 % 6 == 2) lan3_vt7 = outputFileName;
                        if (count_7 % 6 == 3) lan4_vt7 = outputFileName;
                        if (count_7 % 6 == 4) lan5_vt7 = outputFileName;
                        if (count_7 % 6 == 5) lan6_vt7 = outputFileName;
                    }
                    if (count_7 / 6 == 1)
                    {
                        if (count_7 % 6 == 0) lan12_vt7 = outputFileName;
                        if (count_7 % 6 == 1) lan22_vt7 = outputFileName;
                        if (count_7 % 6 == 2) lan32_vt7 = outputFileName;
                        if (count_7 % 6 == 3) lan42_vt7 = outputFileName;
                        if (count_7 % 6 == 4) lan52_vt7 = outputFileName;
                        if (count_7 % 6 == 5) lan62_vt7 = outputFileName;
                    }
                    if (count_7 / 6 > 1 && count_7 / 6 > n7)
                    {
                        lan1_vt7 = lan12_vt7;
                        lan2_vt7 = lan22_vt7;
                        lan3_vt7 = lan32_vt7;
                        lan4_vt7 = lan42_vt7;
                        lan5_vt7 = lan52_vt7;
                        lan6_vt7 = lan62_vt7;
                        if (count_7 % 6 == 0) lan12_vt7 = outputFileName;
                        if (count_7 % 6 == 1) lan22_vt7 = outputFileName;
                        if (count_7 % 6 == 2) lan32_vt7 = outputFileName;
                        if (count_7 % 6 == 3) lan42_vt7 = outputFileName;
                        if (count_7 % 6 == 4) lan52_vt7 = outputFileName;
                        if (count_7 % 6 == 5) lan62_vt7 = outputFileName;
                    }
                    using (MemoryStream memory = new MemoryStream())
                    {
                        using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                        {
                            //RotateImage(Live_Cam_7, 45).Save(memory, ImageFormat.Jpeg);
                            Live_Cam_7.Save(memory, ImageFormat.Jpeg);
                            byte[] bytes = memory.ToArray();
                            fs.Write(bytes, 0, bytes.Length);
                            fs.Dispose();
                        }
                    }
                    //RotateImage(Live_Cam_7, 45).Dispose();
                    Live_Cam_7.Dispose();
                    order_7 = false;
                   
                }; this.Invoke(inv);

            }
        }
        void backgroundWorker_6_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            w6 = true;
            shot6 = "";
            taked6 = false;
            panel6.BackColor = Color.Black;
            n6 = count_6 / 6;
            count_6++;
            //MethodInvoker inv = delegate
            //{
            Cam6VIDEO_Device.SignalToStop();
            status(" [SYSTEM]" + " CAM 6 Save image" + " " + count_6.ToString());
               
            MethodInvoker inv = delegate
            {
                string Addr = "M295.2";
                PLCS7_1200.Write(Addr, true);
            }; this.Invoke(inv);
         
        }
        void backgroundWorker_6_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker_6.CancellationPending)
            {
                e.Cancel = true;
            }

            DateTime date = DateTime.Now;
            date.ToString("HH:MM:ss");
            panel6.BackColor = Color.GreenYellow;
            set_up();
            MethodInvoker inv = delegate
            {
                string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-6" + "-" + count_6.ToString() + ".jpeg";
                string outputFileName = system_config.Map_Path_File + @"\" + str + "";
                if (count_6 / 6 == 0)
                {
                    if (count_6 % 6 == 0) lan1_vt6 = outputFileName;
                    if (count_6 % 6 == 1) lan2_vt6 = outputFileName;
                    if (count_6 % 6 == 2) lan3_vt6 = outputFileName;
                    if (count_6 % 6 == 3) lan4_vt6 = outputFileName;
                    if (count_6 % 6 == 4) lan5_vt6 = outputFileName;
                    if (count_6 % 6 == 5) lan6_vt6 = outputFileName;
                }
                if (count_6 / 6 == 1)
                {
                    if (count_6 % 6 == 0) lan12_vt6 = outputFileName;
                    if (count_6 % 6 == 1) lan22_vt6 = outputFileName;
                    if (count_6 % 6 == 2) lan32_vt6 = outputFileName;
                    if (count_6 % 6 == 3) lan42_vt6 = outputFileName;
                    if (count_6 % 6 == 4) lan52_vt6 = outputFileName;
                    if (count_6 % 6 == 5) lan62_vt6 = outputFileName;
                }
                if (count_6 / 6 > 1 && count_6 / 6 > n6)
                {
                    lan1_vt6 = lan12_vt6;
                    lan2_vt6 = lan22_vt6;
                    lan3_vt6 = lan32_vt6;
                    lan4_vt6 = lan42_vt6;
                    lan5_vt6 = lan52_vt6;
                    lan6_vt6 = lan62_vt6;
                    if (count_6 % 6 == 0) lan12_vt6 = outputFileName;
                    if (count_6 % 6 == 1) lan22_vt6 = outputFileName;
                    if (count_6 % 6 == 2) lan32_vt6 = outputFileName;
                    if (count_6 % 6 == 3) lan42_vt6 = outputFileName;
                    if (count_6 % 6 == 4) lan52_vt6 = outputFileName;
                    if (count_6 % 6 == 5) lan62_vt6 = outputFileName;
                }
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        RotateImage(Live_Cam_6, 45).Save(memory, ImageFormat.Jpeg);
                        //Live_Cam_6.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                
                Live_Cam_6.Dispose();

                order_6 = false;

            }; this.Invoke(inv);
        }


        void backgroundWorker_5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            w5 = true;
            taked5 = false;
            shot5 = "";
            panel5.BackColor = Color.Black;
            n5 = count_5 / 6;
            count_5++;
            Cam5VIDEO_Device.SignalToStop();
            status(" [SYSTEM]" + " CAM 5 Save image" + " " + count_5.ToString());
            MethodInvoker inv = delegate
            {
                string Addr = "M295.1";
                PLCS7_1200.Write(Addr, true);
            }; this.Invoke(inv);
  
            //}; this.Invoke(inv);


        }

        void backgroundWorker_5_DoWork(object sender, DoWorkEventArgs e)
        {

            if (backgroundWorker_5.CancellationPending)
            {
                e.Cancel = true;
            }

            DateTime date = DateTime.Now;
            date.ToString("HH:MM:ss");
            panel5.BackColor = Color.GreenYellow;
            set_up();
            MethodInvoker inv = delegate
            {
                string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-5" + "-" + count_5.ToString() + ".jpeg";
                string outputFileName = system_config.Map_Path_File + @"\" + str + "";
                if (count_5 / 6 == 0)
                {
                    if (count_5 % 6 == 0) lan1_vt5 = outputFileName;
                    if (count_5 % 6 == 1) lan2_vt5 = outputFileName;
                    if (count_5 % 6 == 2) lan3_vt5 = outputFileName;
                    if (count_5 % 6 == 3) lan4_vt5 = outputFileName;
                    if (count_5 % 6 == 4) lan5_vt5 = outputFileName;
                    if (count_5 % 6 == 5) lan6_vt5 = outputFileName;
                }
                if (count_5 / 6 == 1)
                {
                    if (count_5 % 6 == 0) lan12_vt5 = outputFileName;
                    if (count_5 % 6 == 1) lan22_vt5 = outputFileName;
                    if (count_5 % 6 == 2) lan32_vt5 = outputFileName;
                    if (count_5 % 6 == 3) lan42_vt5 = outputFileName;
                    if (count_5 % 6 == 4) lan52_vt5 = outputFileName;
                    if (count_5 % 6 == 5) lan62_vt5 = outputFileName;
                }
                if (count_5 / 6 > 1 && count_5 / 6 > n5)
                {
                    lan1_vt5 = lan12_vt5;
                    lan2_vt5 = lan22_vt5;
                    lan3_vt5 = lan32_vt5;
                    lan4_vt5 = lan42_vt5;
                    lan5_vt5 = lan52_vt5;
                    lan6_vt5 = lan62_vt5;
                    if (count_5 % 6 == 0) lan12_vt5 = outputFileName;
                    if (count_5 % 6 == 1) lan22_vt5 = outputFileName;
                    if (count_5 % 6 == 2) lan32_vt5 = outputFileName;
                    if (count_5 % 6 == 3) lan42_vt5 = outputFileName;
                    if (count_5 % 6 == 4) lan52_vt5 = outputFileName;
                    if (count_5 % 6 == 5) lan62_vt5 = outputFileName;
                }
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        //RotateImage(Live_Cam_5, 90).Save(memory, ImageFormat.Jpeg);
                        Live_Cam_5.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                //RotateImage(Live_Cam_5, 90).Dispose();
                Live_Cam_5.Dispose();
                order_5 = false;

            }; this.Invoke(inv);
        }
        void backgroundWorker_4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            w4 = true;
            taked4 = false;
            shot4 = "";
            panel4.BackColor = Color.Black;
            n4 = count_4 / 6;
            count_4++;
            Cam4VIDEO_Device.SignalToStop();
            status(" [SYSTEM]" + " CAM 4 Save image" + " " + count_4.ToString());
            MethodInvoker inv = delegate
            {
                string Addr = "M295.6";
                PLCS7_1200.Write(Addr, true);
            }; this.Invoke(inv);
          
            
        }

        void backgroundWorker_4_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker_4.CancellationPending)
            {
                e.Cancel = true;
            }
            DateTime date = DateTime.Now;
            date.ToString("HH:MM:ss");

            panel4.BackColor = Color.GreenYellow;
            set_up();
            MethodInvoker inv = delegate
            {
                string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-4" + "-" + count_4.ToString() + ".jpeg";
                string outputFileName = system_config.Map_Path_File + @"\" + str + "";
                if (count_4 / 6 == 0)
                {
                    if (count_4 % 6 == 0) lan1_vt4 = outputFileName;
                    if (count_4 % 6 == 1) lan2_vt4 = outputFileName;
                    if (count_4 % 6 == 2) lan3_vt4 = outputFileName;
                    if (count_4 % 6 == 3) lan4_vt4 = outputFileName;
                    if (count_4 % 6 == 4) lan5_vt4 = outputFileName;
                    if (count_4 % 6 == 5) lan6_vt4 = outputFileName;
                }
                if (count_4 / 6 == 1)
                {
                    if (count_4 % 6 == 0) lan12_vt4 = outputFileName;
                    if (count_4 % 6 == 1) lan22_vt4 = outputFileName;
                    if (count_4 % 6 == 2) lan32_vt4 = outputFileName;
                    if (count_4 % 6 == 3) lan42_vt4 = outputFileName;
                    if (count_4 % 6 == 4) lan52_vt4 = outputFileName;
                    if (count_4 % 6 == 5) lan62_vt4 = outputFileName;
                }
                if (count_4 / 6 > 1 && count_4 / 6 > n4)
                {
                    lan1_vt4 = lan12_vt4;
                    lan2_vt4 = lan22_vt4;
                    lan3_vt4 = lan32_vt4;
                    lan4_vt4 = lan42_vt4;
                    lan5_vt4 = lan52_vt4;
                    lan6_vt4 = lan62_vt4;
                    if (count_4 % 6 == 0) lan12_vt4 = outputFileName;
                    if (count_4 % 6 == 1) lan22_vt4 = outputFileName;
                    if (count_4 % 6 == 2) lan32_vt4 = outputFileName;
                    if (count_4 % 6 == 3) lan42_vt4 = outputFileName;
                    if (count_4 % 6 == 4) lan52_vt4 = outputFileName;
                    if (count_4 % 6 == 5) lan62_vt4 = outputFileName;
                }
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        //RotateImage(Live_Cam_4, 90).Save(memory, ImageFormat.Jpeg);
                        Live_Cam_4.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                //RotateImage(Live_Cam_4, 90).Dispose();
                Live_Cam_4.Dispose();
                order_4 = false;

            }; this.Invoke(inv);
        }

        void backgroundWorker_3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            w3 = true;
            taked3 = false;
            shot3 = "";
            panel3.BackColor = Color.Black;
            n3 = count_3 / 6;
            count_3++;
            Cam3VIDEO_Device.SignalToStop();
            status(" [SYSTEM]" + " CAM 3 Save image" + " " + count_3.ToString());
            MethodInvoker inv = delegate
            {
                string Addr = "M295.5";
                PLCS7_1200.Write(Addr, true);
            }; this.Invoke(inv);
        }
        void backgroundWorker_3_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker_3.CancellationPending)
            {
                e.Cancel = true;
            }
            DateTime date = DateTime.Now;
            date.ToString("HH:MM:ss");

            panel3.BackColor = Color.GreenYellow;
            set_up();
            MethodInvoker inv = delegate
            {
                string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-3" + "-" + count_3.ToString() + ".jpeg";
                string outputFileName = system_config.Map_Path_File + @"\" + str + "";
                if (count_3 / 6 == 0)
                {
                    if (count_3 % 6 == 0) lan1_vt3 = outputFileName;
                    if (count_3 % 6 == 1) lan2_vt3 = outputFileName;
                    if (count_3 % 6 == 2) lan3_vt3 = outputFileName;
                    if (count_3 % 6 == 3) lan4_vt3 = outputFileName;
                    if (count_3 % 6 == 4) lan5_vt3 = outputFileName;
                    if (count_3 % 6 == 5) lan6_vt3 = outputFileName;
                }
                if (count_3 / 6 == 1)
                {
                    if (count_3 % 6 == 0) lan12_vt3 = outputFileName;
                    if (count_3 % 6 == 1) lan22_vt3 = outputFileName;
                    if (count_3 % 6 == 2) lan32_vt3 = outputFileName;
                    if (count_3 % 6 == 3) lan42_vt3 = outputFileName;
                    if (count_3 % 6 == 4) lan52_vt3 = outputFileName;
                    if (count_3 % 6 == 5) lan62_vt3 = outputFileName;
                }
                if (count_3 / 6 > 1 && count_3 / 6 > n3)
                {
                    lan1_vt3 = lan12_vt3;
                    lan2_vt3 = lan22_vt3;
                    lan3_vt3 = lan32_vt3;
                    lan4_vt3 = lan42_vt3;
                    lan5_vt3 = lan52_vt3;
                    lan6_vt3 = lan62_vt3;
                    if (count_3 % 6 == 0) lan12_vt3 = outputFileName;
                    if (count_3 % 6 == 1) lan22_vt3 = outputFileName;
                    if (count_3 % 6 == 2) lan32_vt3 = outputFileName;
                    if (count_3 % 6 == 3) lan42_vt3 = outputFileName;
                    if (count_3 % 6 == 4) lan52_vt3 = outputFileName;
                    if (count_3 % 6 == 5) lan62_vt3 = outputFileName;
                }
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        //RotateImage(Live_Cam_3, -90).Save(memory, ImageFormat.Jpeg);
                        Live_Cam_3.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                //RotateImage(Live_Cam_3,-90).Dispose();
                Live_Cam_3.Dispose();
                order_3 = false;
            }; this.Invoke(inv);
        }

        void backgroundWorker_2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            w2 = true;
            taked2 = false;
            shot2 = "";
            panel2.BackColor = Color.Black;
            n2 = count_2 / 6;
            count_2++;
            Cam2VIDEO_Device.SignalToStop();
            status(" [SYSTEM]" + " CAM 2 Save image" + " " + count_2.ToString());
            MethodInvoker inv = delegate 
            {
                string Addr = "M295.4";
                PLCS7_1200.Write(Addr, true);
            };this.Invoke(inv);
           

        }
        void backgroundWorker_2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker_2.CancellationPending)
            {
                e.Cancel = true;
            }
            panel2.BackColor = Color.GreenYellow;
            DateTime date = DateTime.Now;
            date.ToString("HH:MM:ss");

            set_up();
            MethodInvoker inv = delegate
            {
                string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-2" + "-" + count_2.ToString() + ".jpeg";
                string outputFileName = system_config.Map_Path_File + @"\" + str + "";
                if (count_2 / 6 == 0)
                {
                    if (count_2 % 6 == 0) lan1_vt2 = outputFileName;
                    if (count_2 % 6 == 1) lan2_vt2 = outputFileName;
                    if (count_2 % 6 == 2) lan3_vt2 = outputFileName;
                    if (count_2 % 6 == 3) lan4_vt2 = outputFileName;
                    if (count_2 % 6 == 4) lan5_vt2 = outputFileName;
                    if (count_2 % 6 == 5) lan6_vt2 = outputFileName;
                }
                if (count_2 / 6 == 1)
                {
                    if (count_2 % 6 == 0) lan12_vt2 = outputFileName;
                    if (count_2 % 6 == 1) lan22_vt2 = outputFileName;
                    if (count_2 % 6 == 2) lan32_vt2 = outputFileName;
                    if (count_2 % 6 == 3) lan42_vt2 = outputFileName;
                    if (count_2 % 6 == 4) lan52_vt2 = outputFileName;
                    if (count_2 % 6 == 5) lan62_vt2 = outputFileName;
                }
                if (count_2 / 6 > 1 && count_2 / 6 > n2)
                {
                    lan1_vt2 = lan12_vt2;
                    lan2_vt2 = lan22_vt2;
                    lan3_vt2 = lan32_vt2;
                    lan4_vt2 = lan42_vt2;
                    lan5_vt2 = lan52_vt2;
                    lan6_vt2 = lan62_vt2;
                    if (count_2 % 6 == 0) lan12_vt2 = outputFileName;
                    if (count_2 % 6 == 1) lan22_vt2 = outputFileName;
                    if (count_2 % 6 == 2) lan32_vt2 = outputFileName;
                    if (count_2 % 6 == 3) lan42_vt2 = outputFileName;
                    if (count_2 % 6 == 4) lan52_vt2 = outputFileName;
                    if (count_2 % 6 == 5) lan62_vt2 = outputFileName;
                }
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        //RotateImage(Live_Cam_2, -90).Save(memory, ImageFormat.Jpeg);
                        Live_Cam_2.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
               //RotateImage(Live_Cam_2, -90).Dispose();
                Live_Cam_2.Dispose();
                order_2 = false;

            }; this.Invoke(inv);
        }
        int cc = 0;
        bool w1 = false;
        bool w2 = false;
        bool w3 = false;
        bool w4 = false;
        bool w5 = false;
        bool w6 = false;
        void backgroundWorker_1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            w1 = true;
            taked1 = false;
            cc++;
            Int64 n = count_1 / 6;
            count_1++;
            panel1.BackColor = Color.Black;
            shot1 = "";
            Cam1VIDEO_Device.SignalToStop();
            status(" [SYSTEM]" + " CAM 1 Save image" + " " + count_1.ToString());
            MethodInvoker inv = delegate
            {
                string Addr = "M295.3";
                PLCS7_1200.Write(Addr, true);
            };this.Invoke(inv);
            
        }
        Int64 n1 = 0;
        Int64 n2 = 0;
        Int64 n3 = 0;
        Int64 n4 = 0;
        Int64 n5 = 0;
        Int64 n6 = 0;
        Int64 n7 = 0;
        void backgroundWorker_1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker_1.CancellationPending)
            {
                e.Cancel = true;
            }

            panel1.BackColor = Color.GreenYellow;
            DateTime date = DateTime.Now;
            date.ToString("HH:MM:ss");
          
            set_up();
            MethodInvoker inv = delegate
            {
                string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-1" + "-" + count_1.ToString() + ".jpeg";
                string outputFileName = system_config.Map_Path_File + @"\" + str + "";
               
                if(count_1/6 == 0) 
                {
                    if (count_1 % 6 == 0) lan1_vt1 = outputFileName;
                    if (count_1 % 6 == 1) lan2_vt1 = outputFileName;
                    if (count_1 % 6 == 2) lan3_vt1 = outputFileName;
                    if (count_1 % 6 == 3) lan4_vt1 = outputFileName;
                    if (count_1 % 6 == 4) lan5_vt1 = outputFileName;
                    if (count_1 % 6 == 5) lan6_vt1 = outputFileName;                               
                }
                if (count_1 / 6 == 1) 
                {
                    if (count_1 % 6 == 0) lan12_vt1 = outputFileName;
                    if (count_1 % 6 == 1) lan22_vt1 = outputFileName;
                    if (count_1 % 6 == 2) lan32_vt1 = outputFileName;
                    if (count_1 % 6 == 3) lan42_vt1 = outputFileName;
                    if (count_1 % 6 == 4) lan52_vt1 = outputFileName;
                    if (count_1 % 6 == 5) lan62_vt1 = outputFileName;
                }
                if(count_1/6 >1 && count_1 / 6 > n1) 
                {
                    lan1_vt1 = lan12_vt1;
                    lan2_vt1 = lan22_vt1;
                    lan3_vt1 = lan32_vt1;
                    lan4_vt1 = lan42_vt1;
                    lan5_vt1 = lan52_vt1;
                    lan6_vt1 = lan62_vt1;
                    if (count_1 % 6 == 0) lan12_vt1 = outputFileName;
                    if (count_1 % 6 == 1) lan22_vt1 = outputFileName;
                    if (count_1 % 6 == 2) lan32_vt1 = outputFileName;
                    if (count_1 % 6 == 3) lan42_vt1 = outputFileName;
                    if (count_1 % 6 == 4) lan52_vt1 = outputFileName;
                    if (count_1 % 6 == 5) lan62_vt1 = outputFileName;
                }

                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        //RotateImage(Live_Cam_1,-90).Save(memory, ImageFormat.Jpeg);
                        //bmp1.Save(memory, ImageFormat.Jpeg);
                        Live_Cam_1.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                //RotateImage(Live_Cam_1, -90).Dispose();
                Live_Cam_1.Dispose();
                order_1 = false;

            }; this.Invoke(inv);

        }
        void Cam7VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (order_7 && system_config.add_cam == "true")
            {
                Live_Cam_7 = (Bitmap)eventArgs.Frame.Clone();
                if (!backgroundWorker_7.IsBusy) backgroundWorker_7.RunWorkerAsync();

            }
            else if (Live_Cam_7 != null)
            {
                Live_Cam_7.Dispose();
            }
            //Cam7VIDEO_Device.SignalToStop();
        }
        void Cam6VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (order_6)
            {
                if (!taked6)
                {
                    Live_Cam_6 = (Bitmap)eventArgs.Frame.Clone();
                    taked6 = true;
                    if (!backgroundWorker_6.IsBusy) backgroundWorker_6.RunWorkerAsync();
                }
                

            }
            else if (Live_Cam_6 != null)
            {
                w6 = false;
                taked6 = false;
                Live_Cam_6.Dispose();
             
            }
            Cam6VIDEO_Device.SignalToStop();

        }
        bool taked1 = false;
        bool taked2 = false;
        bool taked3 = false;
        bool taked4 = false;
        bool taked5 = false;
        bool taked6 = false;

        void Cam5VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            if (order_5)
            {
                if (!taked5) 
                {
                    Live_Cam_5 = (Bitmap)eventArgs.Frame.Clone();
                    taked5 = true;
                    if (!backgroundWorker_5.IsBusy) backgroundWorker_5.RunWorkerAsync();
                }
                

            }
            else if (Live_Cam_5 != null)
            {
                w5 = false;
                taked5 = false;
                Live_Cam_5.Dispose();
               
            }
            Cam5VIDEO_Device.SignalToStop();
        }

        void Cam4VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (order_4)
            {
                if (!taked4) 
                {
                    Live_Cam_4 = (Bitmap)eventArgs.Frame.Clone();
                    taked4 = true;
                    if (!backgroundWorker_4.IsBusy) backgroundWorker_4.RunWorkerAsync();
                }
               

            }
            else if (Live_Cam_4 != null)
            {
                w4 = false;
                taked4 = false;
                Live_Cam_4.Dispose();
               
            }
            Cam4VIDEO_Device.SignalToStop();
        }

        void Cam3VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            if (order_3)
            {
                if (!taked3) 
                {
                    taked3 = true;
                    Live_Cam_3 = (Bitmap)eventArgs.Frame.Clone();
                    if (!backgroundWorker_3.IsBusy) backgroundWorker_3.RunWorkerAsync();
                }
               
            }
            else if (Live_Cam_3 != null)
            {
                w3 = false;
                taked3 = false;
                Live_Cam_3.Dispose();
                
            }
            Cam3VIDEO_Device.SignalToStop();
        }

        void Cam2VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (order_2)
            {
                if (!taked2) 
                {
                    taked2 = true;
                    Live_Cam_2 = (Bitmap)eventArgs.Frame.Clone();
                    if (!backgroundWorker_2.IsBusy) backgroundWorker_2.RunWorkerAsync();
                }
               
               
            }
            else if (Live_Cam_2 != null)
            {
                w2 = false;
                taked2 = false;
                Live_Cam_2.Dispose();
     
            }
            Cam2VIDEO_Device.SignalToStop();
        }
        void Cam1VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            if (order_1)
            {
                if (!taked1) 
                {
                    taked1 = true;
                    Live_Cam_1 = (Bitmap)eventArgs.Frame.Clone();
                    if (!backgroundWorker_1.IsBusy) backgroundWorker_1.RunWorkerAsync();
                }
                

            }
            else if (Live_Cam_1 != null ) 
            {
                taked1 = false;
                Live_Cam_1.Dispose();
                w1 = false;
                
            }
            Cam1VIDEO_Device.SignalToStop();
        }
        #endregion

        #region ///////////////////////////////////////////////////////////////////////////////////////////////////////////////program process
        private void Start_btn_Click(object sender, EventArgs e)
        {
           // btnConnect.PerformClick();
            system_config = Program_Configuration.GetSystem_Config();
            Start_program();
            if (File.Exists("Output.txt"))
            {
                using (StreamReader sr = new StreamReader("Output.txt"))
                {
                    int a = 0;
                    string[] read = new string[2];
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {

                        read[a] = line;
                        a++;

                    }
                    if (a > 1)
                    {
                        _OKnum = Convert.ToInt16(read[0]);
                        _NGnum = Convert.ToInt16(read[1]);
                        _sum = (short)(_NGnum + _OKnum);
                    }
                    else
                    {
                        _OKnum = 0;
                        _NGnum = 0;
                        _sum = (short)(_NGnum + _OKnum);
                    }

                }
            }
        }

        string ID_Operator1 = "";
        string ID_Operator2 = "";
        string PN_Selector = "";
        private void Start_program()
        {
            if (!PLC_con) 
            {
                MessageBox.Show("PLC is not connect, Please Connect PLC first");
                return;
            }
            ID_Operator1 = TB_idworker.Text;
            ID_Operator2 = TB_wker2.Text;
            PN_Selector = tb_PN.Text;
            
            Program_Configuration.UpdateSystem_Config("PN_Selector", PN_Selector);
            system_config = Program_Configuration.GetSystem_Config();
            if (tb_PN.Text == "" || TB_idworker.Text == "" || TB_wker2.Text == "")
            {
                MessageBox.Show("DO NOT HAVE PN Selector or ID Operator");
                return;
            }
            if (!Directory.Exists(system_config.Map_Path_File))
            {
                MessageBox.Show("Could not find Map Path File, please check setting again");
                status("[START] Could not find Map Path File");
                Start_btn.Enabled = true;
                Stop_btn.Enabled = false;
                return;
            }
            tb_PN.Enabled = false;
            TB_idworker.Enabled = false;
            TB_wker2.Enabled = false;
            system_config = Program_Configuration.GetSystem_Config();
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (system_config.Camera1 < filterInfoCollection.Count) Cam1_Device = filterInfoCollection[system_config.Camera1];
            if (system_config.Camera2 < filterInfoCollection.Count) Cam2_Device = filterInfoCollection[system_config.Camera2];
            if (system_config.Camera3 < filterInfoCollection.Count) Cam3_Device = filterInfoCollection[system_config.Camera3];
            if (system_config.Camera4 < filterInfoCollection.Count) Cam4_Device = filterInfoCollection[system_config.Camera4];
            if (system_config.Camera5 < filterInfoCollection.Count) Cam5_Device = filterInfoCollection[system_config.Camera5];
            if (system_config.Camera6 < filterInfoCollection.Count) Cam6_Device = filterInfoCollection[system_config.Camera6];
            if (system_config.add_cam == "true")
            {
                if (system_config.Camera7 < filterInfoCollection.Count) Cam7_Device = filterInfoCollection[system_config.Camera7];
            }
            if (Cam1_Device == null)
            {
                MessageBox.Show("Camera 1 is not available, please check connection setting of device and preview");
                status("[START Camera 1 is not availble]");
                return;
            }
            if (Cam2_Device == null)
            {
                MessageBox.Show("Camera 2 is not available, please check connection setting of device and preview");
                status("[START Camera 2 is not availble]");
                return;
            }
            if (Cam3_Device == null)
            {
                MessageBox.Show("Camera 3 is not available, please check connection setting of device and preview");
                status("[START Camera 3 is not availble]");
                return;
            }
            if (Cam4_Device == null)
            {
                MessageBox.Show("Camera 4 is not available, please check connection setting of device and preview");
                status("[START Camera 4 is not availble]");
                return;
            }
            if (Cam5_Device == null)
            {
                MessageBox.Show("Camera 5 is not available, please check connection setting of device and preview");
                status("[START Camera 5 is not availble]");
                return;
            }
            if (Cam6_Device == null)
            {
                MessageBox.Show("Camera 6 is not available, please check connection setting of device and preview");
                status("[START Camera 6 is not availble]");
                return;
            }
            if (Cam7_Device == null && system_config.add_cam == "true")
            {
                MessageBox.Show("Camera 7 is not available, please check connection setting of device and preview");
                status("[START Camera 7 is not availble]");
                return;
            }
            try
            {
                if (serialPort_communicate.IsOpen) serialPort_communicate.Close();
                serialPort_communicate.PortName = system_config.DefaultComport;
                serialPort_communicate.BaudRate = Convert.ToInt32(system_config.DefaultCOMBaudrate);
                
                serialPort_communicate.Open();
                status("[COMPORT] Comport " + serialPort_communicate.PortName + " Connected");

            }
            catch (Exception)
            {
                MessageBox.Show(system_config.DefaultComport + " Not Existing, please try another one");
                status(" [COMPORT] Comport " + serialPort_communicate.PortName + " Not found");
                RESET();
                return;
            }
           
            if (Cam1VIDEO_Device == null || !Cam1VIDEO_Device.IsRunning)
            {
                Cam1VIDEO_Device = new VideoCaptureDevice(Cam1_Device.MonikerString);
                Cam1VIDEO_Device.VideoResolution = Cam1VIDEO_Device.VideoCapabilities[system_config.pixel_cam1];
                Cam1VIDEO_Device.NewFrame += Cam1VIDEO_Device_NewFrame;
                Cam1VIDEO_Device.Start();
                PB_active1.Show();
            }

            if (Cam2VIDEO_Device == null || !Cam2VIDEO_Device.IsRunning)
            {
                Cam2VIDEO_Device = new VideoCaptureDevice(Cam2_Device.MonikerString);
                Cam2VIDEO_Device.VideoResolution = Cam2VIDEO_Device.VideoCapabilities[system_config.pixel_cam2];
                Cam2VIDEO_Device.NewFrame += Cam2VIDEO_Device_NewFrame;
                Cam2VIDEO_Device.Start();
                PB_active2.Show();
            }
            if (Cam3VIDEO_Device == null || !Cam3VIDEO_Device.IsRunning)
            {
                Cam3VIDEO_Device = new VideoCaptureDevice(Cam3_Device.MonikerString);
                Cam3VIDEO_Device.VideoResolution = Cam3VIDEO_Device.VideoCapabilities[system_config.pixel_cam3];
                Cam3VIDEO_Device.NewFrame += Cam3VIDEO_Device_NewFrame;
                Cam3VIDEO_Device.Start();
                PB_active3.Show();

            }

            if (Cam4VIDEO_Device == null || !Cam4VIDEO_Device.IsRunning)
            {
                Cam4VIDEO_Device = new VideoCaptureDevice(Cam4_Device.MonikerString);
                Cam4VIDEO_Device.VideoResolution = Cam4VIDEO_Device.VideoCapabilities[system_config.pixel_cam4];
                Cam4VIDEO_Device.NewFrame += Cam4VIDEO_Device_NewFrame;
                Cam4VIDEO_Device.Start();
                PB_active4.Show();
            }
            if (Cam5VIDEO_Device == null || !Cam5VIDEO_Device.IsRunning)
            {
                Cam5VIDEO_Device = new VideoCaptureDevice(Cam5_Device.MonikerString);
                Cam5VIDEO_Device.VideoResolution = Cam5VIDEO_Device.VideoCapabilities[system_config.pixel_cam5];
                Cam5VIDEO_Device.NewFrame += Cam5VIDEO_Device_NewFrame;
                Cam5VIDEO_Device.Start();
                PB_active5.Show();
            }

            if (Cam6VIDEO_Device == null || !Cam6VIDEO_Device.IsRunning)
            {
                Cam6VIDEO_Device = new VideoCaptureDevice(Cam6_Device.MonikerString);
                Cam6VIDEO_Device.VideoResolution = Cam6VIDEO_Device.VideoCapabilities[system_config.pixel_cam6];
                Cam6VIDEO_Device.NewFrame += Cam6VIDEO_Device_NewFrame;
                Cam6VIDEO_Device.Start();
                PB_active6.Show();
            }
            if (system_config.add_cam == "true")
            {
                if (Cam7VIDEO_Device == null || !Cam7VIDEO_Device.IsRunning)
                {
                    Cam7VIDEO_Device = new VideoCaptureDevice(Cam7_Device.MonikerString);
                    Cam7VIDEO_Device.VideoResolution = Cam7VIDEO_Device.VideoCapabilities[system_config.pixel_cam7];

                    Cam7VIDEO_Device.NewFrame += Cam7VIDEO_Device_NewFrame;
                    Cam7VIDEO_Device.NewFrame += Cam7VIDEO_Device_NewFrame;
                    Cam7VIDEO_Device.Start();
                }
            }
           
            timer.Start();
            Start_btn.Enabled = false;
            Stop_btn.Enabled = true;     
            start_check = true;
            started = true;
           
            status("[START]" + "Program has been started");


        }

       
        string loi_tam1 = "";
        string loi_tam2 = "";
        bool nhapid = false;
        string shot1 = "";
        string shot2 = "";
        string shot3 = "";
        string shot4 = "";
        string shot5 = "";
        string shot6 = "";
        string shot7 = "";
        private void serialPort_communicate_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string[] shot = new string[7];
            string[] NG_code = new string[3];
            Thread.Sleep(10);
            string cap_order = serialPort_communicate.ReadExisting();
            if (nhapid)
            {
                if (cap_order.Contains("-"))
                {
                    MethodInvoker inv = delegate
                    {
                        string[] id = new string[2];
                        id = cap_order.Split('-');
                        if (id[0] == "ID1")
                        {
                           
                            TB_idworker.Text = id[1];

                        }
                        if (id[0] == "ID2")
                        {
                            TB_wker2.Text = id[1];
                          
                        }


                    }; this.Invoke(inv);
                    if (TB_idworker.Text != "" && TB_wker2.Text != "")
                    {
                        if (serialPort_communicate.IsOpen)
                        {
                            //serialPort_communicate.PortName = system_config.DefaultComport;
                            //serialPort_communicate.BaudRate = Convert.ToInt32(system_config.DefaultCOMBaudrate);
                            serialPort_communicate.Close();
                        }
                        nhapid = false;
                    }
                }
            }
            
            status("[RS232] " + cap_order + "");
            //if (cap_order == "B2") 
            //{
            //    MethodInvoker inv = delegate
            //    {
            //        pic_loading1.Hide();
            //        pic_loading2.Show();
            //    };this.Invoke(inv);
            //}
            //if (cap_order == "B1")
            //{
            //    MethodInvoker inv = delegate
            //    {
            //        pic_loading2.Hide();
            //        pic_loading1.Show();
            //    }; this.Invoke(inv);
            //}
            if (!started) return;                       
            x = cap_order;
            if (x != null)
            {
                t = x;
                m = x;
                doAction();
                
            }
                if (cap_order.Contains("."))
                {
                if (cap_order.Length == 13)
                {
                    //shot_pic.RunWorkerAsync();
                    shot = cap_order.Split('.');
                    shot1 = shot[0];
                    shot2 = shot[1];
                    shot3 = shot[2];
                    shot4 = shot[3];
                    shot5 = shot[4];
                    shot6 = shot[5];
                    shot7 = shot[6];
                    if (shot6 == "1")
                    {
                        order_6 = true;
                        Take_Photo();
                    }
                    if (shot5 == "1")
                    {
                        order_5 = true;
                        Take_Photo();
                    }
                    if (shot4 == "1")
                    {
                        order_4 = true;
                        Take_Photo();
                    }
                    if (shot1 == "1")
                    {
                        order_1 = true;
                        Take_Photo();
                    }
                    if (shot2 == "1")
                    {
                        order_2 = true;
                        Take_Photo();
                    }
                    if (shot3 == "1")
                    {
                        order_3 = true;
                        Take_Photo();
                    }
                    if (shot7 == "1")
                    {
                        order_7 = true;
                        Take_Photo();
                    }
                    MethodInvoker inv = delegate
                    {
                        if (!call_back_cam.Enabled)
                        {
                            call_back_cam.Enabled = true;
                        }
                    }; this.Invoke(inv);
                                
                }
                else
                {
                    PLCS7_1200.Write("M156.0", true);
                }
            }
            if (cap_order == "OK1"  && allow_check && !run_out1)
            {
                if (on1 != 1)                                                                  
                {
                    if (En_chek1.Checked)
                    {
                       
                            string Addr = "DB33.DBX0.0";
                            PLCS7_1200.Write(Addr, int.Parse("1"));
                        
                        OK1_check();
                        MethodInvoker inv = delegate
                        {
                            pic_loading2.Hide();
                            pic_loading1.Show();
                        }; this.Invoke(inv);
                    }
                    //else 
                    //{
                    //    //if (!last1)
                    //    //{
                    //        last1 = true;
                    //        Tranfer("OK");
                    //        DateTime dt = DateTime.Now;
                    //        Program_Configuration.UpdateSystem_Config("inf_process", dt.ToString());
                    //        inf_process();
                    //    //}
                    //}
                }
                else
                {
                    MessageBox.Show("The Zoom processing is running, please turn off first");
                }
            }
            if (cap_order == "OK2" && allow_check && !run_out2) 
            {
                if (on2 != 1)
                {
                    if (En_chek2.Checked) 
                    {                       
                            string Addr = "DB33.DBX0.1";
                            PLCS7_1200.Write(Addr, int.Parse("1"));                 
                        OK2_check();
                        MethodInvoker inv = delegate
                        {
                            pic_loading1.Hide();
                            pic_loading2.Show();
                        }; this.Invoke(inv);
                    }
                    //else 
                    //{
                    //    if (!last2) 
                    //    {
                    //        last2 = true;
                    //        Tranfer1("OK");
                    //        DateTime dt = DateTime.Now;
                    //        Program_Configuration.UpdateSystem_Config("inf_process", dt.ToString());
                    //        inf_process();
                    //    }
                    //}
                   
                }
                else
                {
                    MessageBox.Show(" Zoom processing is running, please turn off first");
                }
            }
            if (cap_order.Contains("#")) 
            {
                NG_code = cap_order.Split('#');
                if (NG_code[0] == "NG1" && allow_check && !run_out1) 
                {
                    if (on1 != 1) 
                    {
                        if (En_chek1.Checked) 
                        {
                            string Addr = "DB33.DBX0.0";
                            PLCS7_1200.Write(Addr, int.Parse("1"));
                            
                            loi_tam1 = NG_code[1];
                            vitri_Erpic(NG_code[2]);
                            err_pic1 = (NG_code[2]);
                            NG1_check();
                            MethodInvoker inv = delegate
                            {
                                pic_loading2.Hide();
                                pic_loading1.Show();
                            }; this.Invoke(inv);
                        }
                        //else 
                        //{
                        //    if (!last1)
                        //    {
                        //        last1 = true;
                        //        Tranfer1("ERROR");
                        //        DateTime dt = DateTime.Now;
                        //        Program_Configuration.UpdateSystem_Config("inf_process", dt.ToString());
                        //        inf_process();
                        //    }
                        //}
                    }
                    else
                    {
                        MessageBox.Show(" Zoom processing is running, please turn off first");
                    }
                }
                if (NG_code[0] == "NG2" && allow_check && !run_out2)  
                {
                    if (on2!=1) 
                    {
                        if (En_chek2.Checked) 
                        {
                            string Addr = "DB33.DBX0.1";
                            PLCS7_1200.Write(Addr, int.Parse("1"));
                            m = "";
                            loi_tam2 = NG_code[1];
                            vitri_Erpic(NG_code[2]);
                            err_pic2 = (NG_code[2]);
                            NG2_check();
                            MethodInvoker inv = delegate
                            {
                                pic_loading1.Hide();
                                pic_loading2.Show();
                            }; this.Invoke(inv);
                        }
                        //if (!last2)
                        //{
                        //    last2 = true;
                        //    Tranfer1("ERROR");
                        //    DateTime dt = DateTime.Now;
                        //    Program_Configuration.UpdateSystem_Config("inf_process", dt.ToString());
                        //    inf_process();
                        //}

                    }
                    else
                    {
                        MessageBox.Show("The Zoom processing is running, please turn off first");
                    }
                }
            }
            if(cap_order.Length < 4) 
            {
                if (cap_order == "Z11" && allow_check)
                {
                    zoom1(1);
                }
                if (cap_order == "Z12" && allow_check)
                {
                    zoom1(2);
                }
                if (cap_order == "Z13" && allow_check)
                {
                    zoom1(3);
                }
                if (cap_order == "Z14" && allow_check)
                {
                    zoom1(4);
                }
                if (cap_order == "Z15" && allow_check)
                {
                    zoom1(5);
                }
                if (cap_order == "Z16" && allow_check)
                {
                    zoom1(6);
                }
                if (cap_order == "Z21" && allow_check)
                {
                    zoom2(1);
                }
                if (cap_order == "Z22" && allow_check)
                {
                    zoom2(2);
                }
                if (cap_order == "Z23" && allow_check)
                {
                    zoom2(3);
                }
                if (cap_order == "Z24" && allow_check)
                {
                    zoom2(4);
                }
                if (cap_order == "Z25" && allow_check)
                {
                    zoom2(5);
                }
                if (cap_order == "Z26" && allow_check)
                {
                    zoom2(6);
                }
            }
            if (cap_order == "F")
            {
                MethodInvoker inv = delegate
                {
                    TB_idworker.Text = "";
                    TB_wker2.Text = "";
                }; this.Invoke(inv);
            }
           
        }
        bool last1 = false;
        bool last2 = false;
        private void Shot_pic_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
          //  if (!shot_pic.IsBusy) shot_pic.RunWorkerAsync();
        }

        private void Shot_pic_DoWork(object sender, DoWorkEventArgs e)
        {
            if(shot_pic.CancellationPending ) 
            {
                e.Cancel = true;
            }
            //Take_Photo();
        }
        
        double time_to_reset_cam = 0;

        private void call_back_cam_Tick(object sender, EventArgs e)
        {
            time_to_reset_cam++;
            if (time_to_reset_cam == 2)
            {
                if (order_1)
                {
                    Cam1VIDEO_Device.Start();
                    Thread.Sleep(5);
                }
                if (order_2)
                {
                    Cam2VIDEO_Device.Start();
                    Thread.Sleep(5);
                }
                if (order_3)
                {
                    Cam3VIDEO_Device.Start();
                    Thread.Sleep(5);
                }
                if (order_4)
                {
                    Cam4VIDEO_Device.Start();
                    Thread.Sleep(5);
                }
                if (order_5)
                {
                    Cam5VIDEO_Device.Start();
                    Thread.Sleep(5);
                }
                if (order_6)
                {
                    Cam6VIDEO_Device.Start();
                    Thread.Sleep(5);
                }
                time_to_reset_cam = 0;
                call_back_cam.Enabled = false;
            }
        }
        private void Take_Photo()
        {
            MethodInvoker inv = delegate
            {              
                if (order_1)
                {
                    Cam1VIDEO_Device.Start();
                System.Threading.Thread.Sleep(20);
            }
                if (order_2)
                {
                    Cam2VIDEO_Device.Start();
                System.Threading.Thread.Sleep(20);
            }
                if (order_3)
                {
                    Cam3VIDEO_Device.Start();
                System.Threading.Thread.Sleep(20);
            }
                if (order_4)
                {
                    Cam4VIDEO_Device.Start();
                System.Threading.Thread.Sleep(20);
            }
                if (order_5)
                {
                    Cam5VIDEO_Device.Start();
                System.Threading.Thread.Sleep(20);
            }
                if (order_6)
                {
                    Cam6VIDEO_Device.Start();
                System.Threading.Thread.Sleep(20);
            }
                if (order_7 && system_config.add_cam == "true")
                {
                    Cam7VIDEO_Device.Start();
                    //System.Threading.Thread.Sleep(20);
                }
        };
            this.Invoke(inv);
    }
      
        private void status(string text)
        {
            MethodInvoker inv = delegate
            {
                textBox_stt.AppendText("[" + DateTime.Now.ToString() + "]" + text + Environment.NewLine);
            };
            this.Invoke(inv);
        }
        private void inf_process()
        {
            MethodInvoker inv = delegate
            {
                TB_LTdate.Text = system_config.inf_process.ToString();
                TB_testpart.Text = (folderIndex-1).ToString();
            };
            this.Invoke(inv);
        }
        #endregion
        #region SETTING
        private void cameraSettingToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (started)
            {
                MessageBox.Show("Please stop program first!");
                return;
            }
            Form cameraform = new Camera_setting_Form();
            cameraform.FormClosed += (object sender2, FormClosedEventArgs e2) =>
            {
                this.Show();
            };
            this.Hide();
            cameraform.Show();
        }

        private void pathFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (started)
            {
                MessageBox.Show("Please stop program first!");
                return;
            }
            Form Path_form = new Path_File_Component();
            Path_form.FormClosed += (object sender3, FormClosedEventArgs e3) =>
            {
                this.Show();
            };
            this.Hide();
            Path_form.Show();
        }
        private void cOMConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (started)
            {
                MessageBox.Show("Please stop program first!");
                return;
            }
            Form Com_setting_form = new Com_setting();
            Com_setting_form.FormClosed += (object sender4, FormClosedEventArgs e4) =>
            {
                this.Show();
            };
            this.Hide();
            Com_setting_form.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mesage = "Do you want to exit the program";
            string cap = "Close the program";
            var result = MessageBox.Show(mesage, cap, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) 
            {
                update_system();
                RESET();
                Environment.Exit(0);
            } 
           
        }
        private void RESET()
        {
            if (backgroundWorker_1.IsBusy) backgroundWorker_1.CancelAsync();
            if (backgroundWorker_2.IsBusy) backgroundWorker_2.CancelAsync();
            if (backgroundWorker_3.IsBusy) backgroundWorker_3.CancelAsync();
            if (backgroundWorker_4.IsBusy) backgroundWorker_4.CancelAsync();
            if (backgroundWorker_5.IsBusy) backgroundWorker_5.CancelAsync();
            if (backgroundWorker_6.IsBusy) backgroundWorker_6.CancelAsync();
            if (backgroundWorker_7.IsBusy) backgroundWorker_7.CancelAsync();
            if (shot_pic.IsBusy) shot_pic.CancelAsync();
           
            PB_active1.Hide();
            if (Cam1VIDEO_Device != null && Cam1VIDEO_Device.IsRunning)
            {
                Cam1VIDEO_Device.Stop();
               
            }
            if (Cam2VIDEO_Device != null && Cam2VIDEO_Device.IsRunning)
            {

                Cam2VIDEO_Device.Stop();
               
            }
            PB_active2.Hide();
            if (Cam3VIDEO_Device != null && Cam3VIDEO_Device.IsRunning)
            {
                Cam3VIDEO_Device.Stop();
            }
            if (Cam4VIDEO_Device != null && Cam4VIDEO_Device.IsRunning)
            {
                Cam4VIDEO_Device.Stop();
               
            }
            PB_active4.Hide();
            PB_active3.Hide();
            if (Cam5VIDEO_Device != null && Cam5VIDEO_Device.IsRunning)
            {
                Cam5VIDEO_Device.Stop();
              
            }
            PB_active5.Hide();
            if (Cam6VIDEO_Device != null && Cam6VIDEO_Device.IsRunning)
            {
                Cam6VIDEO_Device.Stop();              
            }
            PB_active6.Hide();
            if (system_config.add_cam == "true")
            {
                if (Cam7VIDEO_Device != null && Cam7VIDEO_Device.IsRunning) Cam7VIDEO_Device.Stop();
            }
            update_system();
        }

        private void update_system()
        {
            Program_Configuration.UpdateSystem_Config("Folder_index_tranfer", load1.ToString());
            Program_Configuration.UpdateSystem_Config("Folder_load_check", load2.ToString());
            Program_Configuration.UpdateSystem_Config("same_folder_1", folderIndex.ToString());       
            Program_Configuration.UpdateSystem_Config("Location_cam1_folder", count_1.ToString());
            Program_Configuration.UpdateSystem_Config("Location_cam2_folder", count_2.ToString());
            Program_Configuration.UpdateSystem_Config("Location_cam3_folder", count_3.ToString());
            Program_Configuration.UpdateSystem_Config("Location_cam4_folder", count_4.ToString());
            Program_Configuration.UpdateSystem_Config("Location_cam5_folder", count_5.ToString());
            Program_Configuration.UpdateSystem_Config("Location_cam6_folder", count_6.ToString());
            Program_Configuration.UpdateSystem_Config("Location_cam7_folder", count_7.ToString());
            using (StreamWriter sw = new StreamWriter("Output.txt"))
            {
                sw.Write("");            
                sw.WriteLine("" + _OKnum.ToString() + "");
                sw.WriteLine("" + _NGnum.ToString() + "");
                
            }
           
        }
        private void Camera_Check_component_FormClosing(object sender, FormClosingEventArgs e)
        {
            update_system();
            RESET();
            timer.Stop();
            if (serialPort_communicate.IsOpen) serialPort_communicate.Close();
            if (PLC_con) 
            {
                string Addr = "M170.0";
                PLCS7_1200.Write(Addr, int.Parse("1"));
                string Addr1 = "M170.2";
                PLCS7_1200.Write(Addr1, int.Parse("1"));
                loadform = false;
                PLC_con = false;
                PLCS7_1200.Close();
            }
            else 
            {
                PLCS7_1200 = new Plc(CpuType.S71200, "192.168.0.7", 0, 0);
                if (PLCS7_1200.IsAvailable)
                {
                    PLCS7_1200.Open();
                    if (PLCS7_1200.IsConnected == true)
                    {
                        string Addr = "M170.0";
                        PLCS7_1200.Write(Addr, int.Parse("1"));
                        string Addr1 = "M170.2";
                        PLCS7_1200.Write(Addr1, int.Parse("1"));
                        loadform = false;
                        PLC_con = false;
                    }
                    PLCS7_1200.Close();
                }
            }        
            if (ledinf.IsBusy) ledinf.CancelAsync();
        }

        private void Stop_btn_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Do you want to Stop the Program", "Confirm", MessageBoxButtons.OKCancel);
            if (timer.Enabled && result == DialogResult.OK)
            {
                update_system();
                LB_TIMER.Text = "00:00:00";
                start_check = false;
                started = false;
                //TB_idworker.Text = "";
                //TB_wker2.Text = "";
                tb_PN.Enabled = true;
                TB_idworker.Enabled = true;
                TB_wker2.Enabled = true;
                _OKnum = 0;
                _NGnum = 0;
                _sum = 0;
                RESET();
                pic_loading1.Hide();
                pic_loading2.Hide();
                Start_btn.Enabled = true;
                Stop_btn.Enabled = false;
                //ledinf.RunWorkerAsync();
                Pic_Cam1.Image = null;
                Pic_Cam2.Image = null;
                Pic_Cam3.Image = null;
                Pic_Cam4.Image = null;
                Pic_Cam5.Image = null;
                Pic_Cam6.Image = null;
                status("[SYSTEM] Program STOPPED");
                startPR_Count = 0;
            }

        }
        #endregion
        #region///////////////////////////////////////////////////////////////////////////////////////////////////////////////////// XỬ LÝ ẢNH
        private static Image RotateImage(Bitmap img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new System.Drawing.Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }

        bool run_out1 = false;
        bool run_out2 = false;
        private void upload_image()
        {
             DirectoryInfo d = new DirectoryInfo(system_config.Map_Path_File);
             system_config = Program_Configuration.GetSystem_Config();
            if (!d.Exists)
            {
                MessageBox.Show("Folder do not exists ");
                return;
            }
            FileInfo[] fileInfor = d.GetFiles("*.jpeg");
            string[] into_pic = new string[7];

            string[] getpath = new string[14];
            int i = 0;
            int j = 0;
            
            foreach (FileInfo file in fileInfor)
            {
                into_pic = file.Name.Split('-');
                if (into_pic[6] == "" + load1.ToString() + ".jpeg")
                {
                    getpath[i] = file.Name;
                    i++;
                }
            }
    
            bool a1 = false; bool a2 = false; bool a3 = false; bool a4 = false; bool a5 = false; bool a6 = false; bool a7 = false;
            foreach (string file in getpath)
            {

                if (file == null) 
                {
                    if (!a1) path_1_1 = "";
                    if (!a2) path_1_2 = "";
                    if (!a3) path_1_3 = "";
                    if (!a4) path_1_4 = "";
                    if (!a5) path_1_5 = "";
                    if (!a6) path_1_6 = "";
                    
                    break;
                }
                //if (file == null && a1 && a2 && a3 && a4 && a5 && a6)
                //{
                //    break;
                //}
                into_pic = file.Split('-');

                if (into_pic[5] == "1")
                {
                    a1 = true;
                    path_1_1 = file;
                }
                else if (!a1 && into_pic[5] != "1")
                {
                    path_1_1 = "";
                }
                if (into_pic[5] == "2")
                {
                    a2 = true;
                    path_1_2 = file;
                }
                else if (!a2 && into_pic[5] != "2")
                {
                    path_1_2 = "";
                }
                if (into_pic[5] == "3")
                {
                    a3 = true;
                    path_1_3 = file;
                }
                else if (!a3 && into_pic[5] != "3")
                {
                    path_1_3 = "";
                }
                if (into_pic[5] == "4")
                {
                    a4 = true;
                    path_1_4 = file;
                }
                else if (!a4 && into_pic[5] != "4")
                {
                    path_1_4 = "";
                }
                if (into_pic[5] == "5")
                {
                    a5 = true;
                    path_1_5 = file;
                }
                else if (!a5 && into_pic[5] != "5")
                {
                    path_1_5 = "";
                }
                if (into_pic[5] == "6")
                {
                    a6 = true;
                    path_1_6 = file;
                }
                else if (!a6 && into_pic[5] != "6")
                {
                    path_1_6 = "";
                }
                if (into_pic[5] == "7")
                {
                    a7 = true;
                    path_1_7 = file;
                }
                else if (!a7 && into_pic[5] != "7")
                {
                    path_1_7 = "";
                }

            }
            if (path_1_1 == "" || path_1_2 == "" || path_1_3 == "" || path_1_4 == "" || path_1_5 == "" || path_1_6 == "")
            {
                pictureBox1.Image = null;
                pictureBox2.Image = null;
                pictureBox3.Image = null;
                pictureBox4.Image = null;
                pictureBox5.Image = null;
                pictureBox6.Image = null;
                Hname1.Text = "";
                Hname2.Text = "";
                Hname3.Text = "";
                Hname4.Text = "";
                Hname5.Text = "";
                Hname6.Text = "";
                
                run_out1 = true;
                return;
            }
            if (path_1_1 != "")
            {
                pictureBox1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_1_1 + "");
                Hname1.Text = path_1_1;
                c1h1_1.Visible = true; 
                c2h1_1.Visible = true; 
                c3h1_1.Visible = true;
                c4h1_1.Visible = true; 
            }
            else
            {
                pictureBox1.Image = null;
                Hname1.Text = "";
                c1h1_1.Visible = false;
                c2h1_1.Visible = false;
                c3h1_1.Visible = false;
                c4h1_1.Visible = false;
            }
            if (path_1_2 != "")
            {
                pictureBox2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_1_2 + "");
                Hname2.Text = path_1_2;

                c1h2_1.Visible = true; c2h2_1.Visible = true;
            }
            else
            {
                pictureBox2.Image = null;
                Hname2.Text = ""; 
                c1h2_1.Visible = false; c2h2_1.Visible = false;
            }
            if (path_1_3 != "")
            {
                pictureBox3.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_1_3 + "");
                Hname3.Text = path_1_3;
                c1h3_1.Visible = true; c2h3_1.Visible = true;
            }
            else
            {
                pictureBox3.Image = null;
                Hname3.Text = "";
                c1h3_1.Visible = false; c2h3_1.Visible = false;
            }
            if (path_1_4 != "")
            {
                pictureBox4.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_1_4 + "");
                Hname4.Text = path_1_4;
                c1h4_1.Visible = true; c2h4_1.Visible = true;
            }
            else
            {
                pictureBox4.Image = null;
                Hname4.Text = "";
                c1h4_1.Visible = false; c2h4_1.Visible = false;
            }
            if (path_1_5 != "")
            {
                pictureBox5.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_1_5 + "");
                Hname5.Text = path_1_5;
                c1h5_1.Visible = true; c2h5_1.Visible = true;
            }
            else
            {
                pictureBox5.Image = null;
                Hname5.Text = "";
                c1h5_1.Visible = false; c2h5_1.Visible = false;
            }
            if (path_1_6 != "")
            {
                pictureBox6.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_1_6 + "");
                Hname6.Text = path_1_6;
                c1h6_1.Visible = true; 
                c2h6_1.Visible = true; 
                c3h6_1.Visible = true; 
                c4h6_1.Visible = true;
            }
            else
            {
                pictureBox6.Image = null;
                Hname6.Text = "";
                c1h6_1.Visible = false;
                c2h6_1.Visible = false;
                c3h6_1.Visible = false;
                c4h6_1.Visible = false;
            }

        }

        string path_1_1 = "";
        string path_1_2 = "";
        string path_1_3 = "";
        string path_1_4 = "";
        string path_1_5 = "";
        string path_1_6 = "";
        string path_1_7 = "";

        private void update_image2()
        {
            DirectoryInfo d = new DirectoryInfo(system_config.Map_Path_File);
            system_config = Program_Configuration.GetSystem_Config();
            if (!d.Exists)
            {
                MessageBox.Show("Folder do not exists ");
                return;
            }
            FileInfo[] fileInfor = d.GetFiles("*.jpeg");

            string[] into_pic = new string[7];

            string[] getpath = new string[14];
            int i = 0;
            int j = 0;
            bool a1 = false; bool a2 = false; bool a3 = false; bool a4 = false; bool a5 = false; bool a6 = false; bool a7 = false;
            foreach(FileInfo file in fileInfor)
            {
                into_pic = file.Name.Split('-');
                if (into_pic[6] == "" + load2.ToString() + ".jpeg")
                {
                    getpath[i] = file.Name;
                    i++;
                }
            }
   
           
            foreach (string file in getpath)
            {

                if (file == null )
                {
                    if (!a1) path_2_1 = "";
                    if (!a2) path_2_2 = "";
                    if (!a3) path_2_3 = "";
                    if (!a4) path_2_4 = "";
                    if (!a5) path_2_5 = "";
                    if (!a6) path_2_6 = "";
                   
                    break;
                }
                //if (file == null && a1 && a2 && a3 && a4 && a5 && a6)
                //{
                //    break;
                //}
                into_pic = file.Split('-');

                if (into_pic[5] == "1")
                {
                    a1 = true;
                    path_2_1 = file;
                }
                else if (!a1 && into_pic[5] != "1")
                {
                    path_2_1 = "";
                }
                if (into_pic[5] == "2")
                {
                    a2 = true;
                    path_2_2 = file;
                }
                else if (!a2 && into_pic[5] != "2")
                {
                    path_2_2 = "";
                }
                if (into_pic[5] == "3")
                {
                    a3 = true;
                    path_2_3 = file;
                }
                else if (!a3 && into_pic[5] != "3")
                {
                    path_2_3 = "";
                }
                if (into_pic[5] == "4")
                {
                    a4 = true;
                    path_2_4 = file;
                }
                else if (!a4 && into_pic[5] != "4")
                {
                    path_2_4 = "";
                }
                if (into_pic[5] == "5")
                {
                    a5 = true;
                    path_2_5 = file;
                }
                else if (!a5 && into_pic[5] != "5")
                {
                    path_2_5 = "";
                }
                if (into_pic[5] == "6")
                {
                    a6 = true;
                    path_2_6 = file;
                }
                else if (!a6 && into_pic[5] != "6")
                {
                    path_2_6 = "";
                }
                if (into_pic[5] == "7")
                {
                    a7 = true;
                    path_2_7 = file;
                }
                else if (!a7 && into_pic[5] != "7")
                {
                    path_2_7 = "";
                }
      
            }
            if (path_2_1 == "" || path_2_2 == "" || path_2_3 == "" || path_2_4 == "" || path_2_5 == "" || path_2_6 == "") 
            {
                pictureBox15.Image = null;
                pictureBox16.Image = null;
                pictureBox17.Image = null;
                pictureBox18.Image = null;
                pictureBox19.Image = null;
                pictureBox20.Image = null;
                Hname_7.Text = "";
                Hname_8.Text = "";
                Hname_9.Text = "";
                Hname_10.Text = "";
                Hname_11.Text = "";
                Hname_12.Text = "";
             
                run_out2 = true;
                return;
            }
            if (path_2_1 != "")
            {
                pictureBox15.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_2_1 + "");
                Hname_7.Text = path_2_1;
                c1h1_2.Visible = true; 
                c2h1_2.Visible = true; 
                c3h1_2.Visible = true; 
                c4h1_2.Visible = true;
            }
            else
            {
                pictureBox15.Image = null;
                Hname_7.Text = "";
                c1h1_2.Visible = false;
                c2h1_2.Visible = false;
                c3h1_2.Visible = false;
                c4h1_2.Visible = false;
            }
            if (path_2_2 != "")
            {
                pictureBox16.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_2_2 + "");
                Hname_8.Text = path_2_2;

                c1h2_2.Visible = true; c2h2_2.Visible = true;
            }
            else
            {
                pictureBox16.Image = null;
                Hname_8.Text = "";
                c1h2_2.Visible = false; c2h2_2.Visible = false;
            }
            if (path_2_3 != "")
            {
                pictureBox17.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_2_3 + "");
                Hname_9.Text = path_2_3;
                c1h3_2.Visible = true; c2h3_2.Visible = true;
            }
            else
            {
                pictureBox17.Image = null;
                Hname_9.Text = "";
                c1h3_2.Visible = false; c2h3_2.Visible = false;
            }
            if (path_2_4 != "")
            {
                pictureBox18.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_2_4 + "");
                Hname_10.Text = path_2_4;
                c1h4_2.Visible = true; c2h4_2.Visible = true;
            }
            else
            {
                pictureBox18.Image = null;
                Hname_10.Text = "";
                c1h4_2.Visible = false; c2h4_2.Visible = false;
            }
            if (path_2_5 != "")
            {
                pictureBox19.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_2_5 + "");
                Hname_11.Text = path_2_5;
                c1h5_2.Visible = true; c2h5_2.Visible = true;
            }
            else
            {
                pictureBox19.Image = null;
                Hname_11.Text = "";
                c1h5_2.Visible = false; c2h5_2.Visible = false;
            }
            if (path_2_6 != "")
            {
                pictureBox20.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_2_6 + "");
                Hname_12.Text = path_2_6;
                c1h6_2.Visible = true; 
                c2h6_2.Visible = true; 
                c3h6_2.Visible = true; 
                c4h6_2.Visible = true;
            }
            else
            {
                pictureBox20.Image = null;
                Hname_12.Text = "";
                c1h6_2.Visible = false;
                c2h6_2.Visible = false;
                c3h6_2.Visible = false;
                c4h6_2.Visible = false;
            }
            
        }

        string path_2_1 = "";
        string path_2_2 = "";
        string path_2_3 = "";
        string path_2_4 = "";
        string path_2_5 = "";
        string path_2_6 = "";
        string path_2_7 = "";
        private void En_chek1_CheckedChanged(object sender, EventArgs e)
        {
            if (En_chek1.Checked) 
            {
                OK1_check_pause();
                last1 = false;
            }
        }

        private void En_chek2_CheckedChanged(object sender, EventArgs e)
        {
            MethodInvoker inv = delegate
            {
                if (En_chek2.Checked) 
            {
                OK2_check_pause();
                last2 = false;
            }
            }; this.Invoke(inv);
        }
        private void OK1_check_pause() 
        {
            MethodInvoker inv = delegate 
            {
                load1 = folderIndex;
                update_image2();
            };this.Invoke(inv);
            
        }
        private void OK2_check_pause()
        {
            MethodInvoker inv =delegate
            {
                load2 = folderIndex;
                upload_image();
            };           
            this.Invoke(inv);
          
        }
        private void null_pic1() 
        {     
            MethodInvoker inv = delegate
            {
                pictureBox1.Image.Dispose();
                pictureBox2.Image.Dispose();
                pictureBox3.Image.Dispose();
                pictureBox4.Image.Dispose();
                pictureBox5.Image.Dispose();
                pictureBox6.Image.Dispose();
                if (pic_full1.Image != null) { pic_full1.Image.Dispose(); }


                File.Delete(system_config.Map_Path_File + @"/" + path_1_1);
                File.Delete(system_config.Map_Path_File + @"/" + path_1_2);
                File.Delete(system_config.Map_Path_File + @"/" + path_1_3);
                File.Delete(system_config.Map_Path_File + @"/" + path_1_4);
                File.Delete(system_config.Map_Path_File + @"/" + path_1_5);
                File.Delete(system_config.Map_Path_File + @"/" + path_1_6);
                upload_image();
                folderIndex++;
                if (En_chek1.Checked)
                {
                    load1 = folderIndex;
                }
            }; this.Invoke(inv);
        }
        private void null_pic2()
        {
            MethodInvoker inv = delegate
            {
                pictureBox15.Image.Dispose();
                pictureBox16.Image.Dispose();
                pictureBox17.Image.Dispose();
                pictureBox18.Image.Dispose();
                pictureBox19.Image.Dispose();
                pictureBox20.Image.Dispose();
                if (picfull_2.Image != null) { picfull_2.Image.Dispose(); }

                File.Delete(system_config.Map_Path_File + @"/" + path_2_1);
                File.Delete(system_config.Map_Path_File + @"/" + path_2_2);
                File.Delete(system_config.Map_Path_File + @"/" + path_2_3);
                File.Delete(system_config.Map_Path_File + @"/" + path_2_4);
                File.Delete(system_config.Map_Path_File + @"/" + path_2_5);
                File.Delete(system_config.Map_Path_File + @"/" + path_2_6);
                update_image2();
                folderIndex++;
                if (En_chek1.Checked)
                {
                    load1 = folderIndex;
                }
            }; this.Invoke(inv);         
        
        }
        private void Tranfer(string OPTION)
        {
            if (OPTION == "OK" && allow_check)
            {
                Parameter_app.OK_TEMP(DMY ,load1.ToString());
                string[] getpath = new string[7];
                int i = 0;
                getpath = path_1_1.Split('-');

                if (!Directory.Exists(Parameter_app.OK_IMAGE_FOLDER_PATH))
                {
                    Directory.CreateDirectory(Parameter_app.OK_IMAGE_FOLDER_PATH);
                }

                MethodInvoker inv = delegate
                {
                    pictureBox1.Image.Dispose();
                    pictureBox2.Image.Dispose();
                    pictureBox3.Image.Dispose();
                    pictureBox4.Image.Dispose();
                    pictureBox5.Image.Dispose();
                    pictureBox6.Image.Dispose();
                    if (pic_full1.Image != null) { pic_full1.Image.Dispose(); }


                    File.Move(system_config.Map_Path_File + @"/" + path_1_1, Parameter_app.OK_IMAGE_FOLDER_PATH + @"/" + "OK-" + ID_Operator1 + "-" + path_1_1);
                    File.Move(system_config.Map_Path_File + @"/" + path_1_2, Parameter_app.OK_IMAGE_FOLDER_PATH + @"/" + "OK-" + ID_Operator1 + "-" + path_1_2);
                    File.Move(system_config.Map_Path_File + @"/" + path_1_3, Parameter_app.OK_IMAGE_FOLDER_PATH + @"/" + "OK-" + ID_Operator1 + "-" + path_1_3);
                    File.Move(system_config.Map_Path_File + @"/" + path_1_4, Parameter_app.OK_IMAGE_FOLDER_PATH + @"/" + "OK-" + ID_Operator1 + "-" + path_1_4);
                    File.Move(system_config.Map_Path_File + @"/" + path_1_5, Parameter_app.OK_IMAGE_FOLDER_PATH + @"/" + "OK-" + ID_Operator1 + "-" + path_1_5);
                    File.Move(system_config.Map_Path_File + @"/" + path_1_6, Parameter_app.OK_IMAGE_FOLDER_PATH + @"/" + "OK-" + ID_Operator1 + "-" + path_1_6);
                    if (system_config.add_cam == "true")
                    {
                        File.Move(system_config.Map_Path_File + @"/" + path_1_7, Parameter_app.OK_IMAGE_FOLDER_PATH + @"/" + "OK-" + ID_Operator1 + "-" + path_1_7);
                    }
                };this.Invoke(inv);

                string[] tach = new string[2];
                tach = getpath[6].Split('.');
                Boolean check = sql_action.excute_data("INSERT INTO component_status (PN_Selector,Date,Time,Trace,ID,Status,Picture1,Picture2,Picture3,Picture4,Picture5,Picture6) VALUES (N'" + PN_Selector + "','" + getpath[1] + "','" + getpath[2] + "-" + getpath[3] + "-" + getpath[4] + "','" + tach[0] + "','" + ID_Operator1 + "','OK','OK','OK','OK','OK','OK','OK')");
                Boolean orderby = sql_action.excute_data("SELECT Time FROM component_status ORDER BY Time DESC");
                var item = new ListViewItem(new[] { ""+PN_Selector +"-"+folderIndex.ToString()+"", "OK", "" });
                listView1.Items.Add(item);

                status(" [SYSTEM] " + " [OK]" + " SAVED IMAGE[" +load1.ToString() + "]");
            }

            if (OPTION == "ERROR" && allow_check)
            {
                string[] getpath = new string[7];
                int i = 0;
                getpath = path_1_1.Split('-');
                Parameter_app.ERROR_TEMP(DMY, load1.ToString());
                if (!Directory.Exists(Parameter_app.ERROR_IMAGE_FOLDER_PATH))
                {
                    Directory.CreateDirectory(Parameter_app.ERROR_IMAGE_FOLDER_PATH);
                }
                MethodInvoker inv = delegate
                {
                    pictureBox1.Image.Dispose();
                    pictureBox2.Image.Dispose();
                    pictureBox3.Image.Dispose();
                    pictureBox4.Image.Dispose();
                    pictureBox5.Image.Dispose();
                    pictureBox6.Image.Dispose();
                    if (pic_full1.Image != null) { pic_full1.Image.Dispose(); }

                    File.Move(system_config.Map_Path_File + @"/" + path_1_1, Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"/" + error_Type(loi_tam1) + ID_Operator1 + "-" + path_1_1);
                    File.Move(system_config.Map_Path_File + @"/" + path_1_2, Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"/" + error_Type(loi_tam1) + ID_Operator1 + "-" + path_1_2);
                    File.Move(system_config.Map_Path_File + @"/" + path_1_3, Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"/" + error_Type(loi_tam1) + ID_Operator1 + "-" + path_1_3);
                    File.Move(system_config.Map_Path_File + @"/" + path_1_4, Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"/" + error_Type(loi_tam1) + ID_Operator1 + "-" + path_1_4);
                    File.Move(system_config.Map_Path_File + @"/" + path_1_5, Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"/" + error_Type(loi_tam1) + ID_Operator1 + "-" + path_1_5);
                    File.Move(system_config.Map_Path_File + @"/" + path_1_6, Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"/" + error_Type(loi_tam1) + ID_Operator1 + "-" + path_1_6);
                    if (system_config.add_cam == "true")
                    {
                        File.Move(system_config.Map_Path_File + @"/" + path_1_7, Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"/" + error_Type(loi_tam1) + ID_Operator1 + "-" + path_1_7);
                    }
                };this.Invoke(inv);
               
                string[] tach = new string[2];
                tach = getpath[6].Split('.');
                Boolean check = sql_action.excute_data("INSERT INTO component_status (PN_Selector,Date,Time,Trace,ID,Status,Picture1,Picture2,Picture3,Picture4,Picture5,Picture6,NG_Type) VALUES (N'" + PN_Selector + "','" + getpath[1] + "','" + getpath[2] + "-" + getpath[3] + "-" + getpath[4] + "','" + tach[0] + "','" + ID_Operator1 + "','NG','" + h1 + "','" + h2 + "','" + h3 + "','" + h4 + "','" + h5 + "','" + h6 + "','" + error_Type(loi_tam1) + "')");
                Boolean insert = sql_action.excute_data("INSERT INTO NG_detail ([PN_Selector],[Date],[Time],[Trace]) VALUES (N'" + PN_Selector + "','" + getpath[1] + "','" + getpath[2] + "-" + getpath[3] + "-" + getpath[4] + "','" + tach[0] + "_" + err_pic1 + "')");
                Boolean orderby = sql_action.excute_data("SELECT Time FROM component_status ORDER BY (Time) DESC");
                var item = new ListViewItem(new[] { ""+PN_Selector+"-" + folderIndex.ToString() + "", "NG", error_Type(loi_tam1) });
                listView1.Items.Add(item);
                err_pic1 = "";
                status(" [SYSTEM]" + " [ERROR]" + " SAVED IMAGE[" + load1.ToString() + "]");
            }
            folderIndex++;
            if (En_chek1.Checked) 
            {
                load1 = folderIndex;
            }
           
        }

        string h1 = "OK";
        string h2 = "OK";
        string h3 = "OK";
        string h4 = "OK";
        string h5 = "OK";
        string h6 = "OK";

        string err_pic1 = "";
        string err_pic2 = "";
        Int16 _OKnum = 0;
        Int16 _NGnum = 0;
        Int16 _sum = 0;
        private string vitri_Erpic(string so)
        {
            if (so == "1")
            {
                h1 = "NG";
              
            }
            else

            {
                h1 = "OK";
               
            }
            if (so == "2")
            {
                h2 = "NG";
             
            }
            else
            {
                h2 = "OK";
               
            }
            if (so == "3")
            {
                h3 = "NG";
               
            }
            else
            {
                h3 = "OK";
               
            }
            if (so == "4")
            {
                h4 = "NG";
              
            }
            else
            {
                h4 = "OK";
               
            }
            if (so == "5")
            {
                h5 = "NG";
               
            }
            else
            {
                h5 = "OK";
               
            }
            if (so == "6")
            {
                h6 = "NG";
              
            }
            else
            {
                h6 = "OK";
              
            }
            return err_pic;
        }
        string err_pic;
        private string error_Type(string get_error)
        {
            string error_type = "";
            if (get_error == "1")
            {
                error_type = "Incompleted Soldering";
            }
            if (get_error == "2")
            {
                error_type = "Flux";
            }
            if (get_error == "3")
            {
                error_type = "Tinned Winding";
            }
            if (get_error == "4")
            {
                error_type = "Tin on Base(Tin ball)";
            }
            if (get_error == "5")
            {
                error_type = "Damaged(Scratched)";
            }
            if (get_error == "6")
            {
                error_type = "Others";
            }
            return error_type;
        }
        private void Tranfer1(string OPTION)
        {
            if (OPTION == "OK" && allow_check)
            {

                Parameter_app.OK_TEMP(DMY, load2.ToString());

                string[] getpath = new string[7];
                int i = 0;
                getpath = path_2_1.Split('-');

                if (!Directory.Exists(Parameter_app.OK_IMAGE_FOLDER_PATH))
                {
                    Directory.CreateDirectory(Parameter_app.OK_IMAGE_FOLDER_PATH);
                }

                MethodInvoker inv = delegate
                {
                    pictureBox15.Image.Dispose();
                    pictureBox16.Image.Dispose();
                    pictureBox17.Image.Dispose();
                    pictureBox18.Image.Dispose();
                    pictureBox19.Image.Dispose();
                    pictureBox20.Image.Dispose();
                    if (picfull_2.Image != null) { picfull_2.Image.Dispose(); }

                    File.Move(system_config.Map_Path_File + @"/" + path_2_1, Parameter_app.OK_IMAGE_FOLDER_PATH + @"/" + "OK-" + ID_Operator2 + "-" + path_2_1);
                    File.Move(system_config.Map_Path_File + @"/" + path_2_2, Parameter_app.OK_IMAGE_FOLDER_PATH + @"/" + "OK-" + ID_Operator2 + "-" + path_2_2);
                    File.Move(system_config.Map_Path_File + @"/" + path_2_3, Parameter_app.OK_IMAGE_FOLDER_PATH + @"/" + "OK-" + ID_Operator2 + "-" + path_2_3);
                    File.Move(system_config.Map_Path_File + @"/" + path_2_4, Parameter_app.OK_IMAGE_FOLDER_PATH + @"/" + "OK-" + ID_Operator2 + "-" + path_2_4);
                    File.Move(system_config.Map_Path_File + @"/" + path_2_5, Parameter_app.OK_IMAGE_FOLDER_PATH + @"/" + "OK-" + ID_Operator2 + "-" + path_2_5);
                    File.Move(system_config.Map_Path_File + @"/" + path_2_6, Parameter_app.OK_IMAGE_FOLDER_PATH + @"/" + "OK-" + ID_Operator2 + "-" + path_2_6);
                    if (system_config.add_cam == "true")
                    {
                        File.Move(system_config.Map_Path_File + @"/" + path_2_7, Parameter_app.OK_IMAGE_FOLDER_PATH + @"/" + "OK-" + ID_Operator2 + "-" + path_2_7);
                    }
                };this.Invoke(inv);
              
                string[] tach = new string[2];
                tach = getpath[6].Split('.');
                Boolean check = sql_action.excute_data("INSERT INTO component_status (PN_Selector,Date,Time,Trace,ID,Status,Picture1,Picture2,Picture3,Picture4,Picture5,Picture6) VALUES (N'" + PN_Selector + "','" + getpath[1] + "','" + getpath[2] + "-" + getpath[3] + "-" + getpath[4] + "','" + tach[0] + "','" + ID_Operator2 + "','OK','OK','OK','OK','OK','OK','OK')");
                Boolean orderby = sql_action.excute_data("SELECT Time FROM component_status ORDER BY Time DESC");
                var item = new ListViewItem(new[] { "" + PN_Selector + "-" + folderIndex.ToString() + "", "OK", "" });
                listView1.Items.Add(item);

                status(" [SYSTEM] " + " [OK]" + " SAVED IMAGE[" + load2.ToString() + "]");
            }

            if (OPTION == "ERROR" && allow_check)
            {
                string[] getpath = new string[7];
                int i = 0;
                getpath = path_2_1.Split('-');

                Parameter_app.ERROR_TEMP(DMY, load2.ToString());
                if (!Directory.Exists(Parameter_app.ERROR_IMAGE_FOLDER_PATH))
                {
                    Directory.CreateDirectory(Parameter_app.ERROR_IMAGE_FOLDER_PATH);
                }
                MethodInvoker inv = delegate
                {
                    pictureBox15.Image.Dispose();
                    pictureBox16.Image.Dispose();
                    pictureBox17.Image.Dispose();
                    pictureBox18.Image.Dispose();
                    pictureBox19.Image.Dispose();
                    pictureBox20.Image.Dispose();
                    if (picfull_2.Image != null) { picfull_2.Image.Dispose(); }

                    File.Move(system_config.Map_Path_File + @"/" + path_2_1, Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"/" + error_Type(loi_tam2) + ID_Operator2 + "-" + path_2_1);
                    File.Move(system_config.Map_Path_File + @"/" + path_2_2, Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"/" + error_Type(loi_tam2) + ID_Operator2 + "-" + path_2_2);
                    File.Move(system_config.Map_Path_File + @"/" + path_2_3, Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"/" + error_Type(loi_tam2) + ID_Operator2 + "-" + path_2_3);
                    File.Move(system_config.Map_Path_File + @"/" + path_2_4, Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"/" + error_Type(loi_tam2) + ID_Operator2 + "-" + path_2_4);
                    File.Move(system_config.Map_Path_File + @"/" + path_2_5, Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"/" + error_Type(loi_tam2) + ID_Operator2 + "-" + path_2_5);
                    File.Move(system_config.Map_Path_File + @"/" + path_2_6, Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"/" + error_Type(loi_tam2) + ID_Operator2 + "-" + path_2_6);
                    if (system_config.add_cam == "true")
                    {
                        File.Move(system_config.Map_Path_File + @"/" + path_2_7, Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"/" + error_Type(loi_tam2) + ID_Operator2 + "-" + path_2_7);
                    }
                };this.Invoke(inv);
                
                string[] tach = new string[2];
                tach = getpath[6].Split('.');
                Boolean check = sql_action.excute_data("INSERT INTO component_status (PN_Selector,Date,Time,Trace,ID,Status,Picture1,Picture2,Picture3,Picture4,Picture5,Picture6,NG_Type) VALUES (N'" + PN_Selector + "','" + getpath[1] + "','" + getpath[2] + "-" + getpath[3] + "-" + getpath[4] + "','" + tach[0] + "','" + ID_Operator1 + "','NG','" + h1 + "','" + h2 + "','" + h3 + "','" + h4 + "','" + h5 + "','" + h6 + "','" + error_Type(loi_tam2) + "')");
                Boolean insert = sql_action.excute_data("INSERT INTO NG_detail ([PN_Selector],[Date],[Time],[Trace]) VALUES (N'" + PN_Selector + "','" + getpath[1] + "','" + getpath[2] + "-" + getpath[3] + "-" + getpath[4] + "','" + tach[0] + "_" + err_pic2 + "')");
                Boolean orderby = sql_action.excute_data("SELECT Time FROM component_status ORDER BY Time DESC");
                var item = new ListViewItem(new[] { "" + PN_Selector + "-" + folderIndex.ToString() + "", "NG", error_Type(loi_tam2) });
                listView1.Items.Add(item);
                err_pic2 = "";
                status(" [SYSTEM]" + " [ERROR]" + " SAVED IMAGE[" + system_config.Folder_index_tranfer.ToString() + "]");
            }
            folderIndex++;
            if (En_chek2.Checked) 
            {
                load2 = folderIndex;
            }
           
        }
        Int16 num1 = 0;
        Int16 num2 = 0;
        private void OK1_check()
        {

            MethodInvoker inv = delegate
            {
                num1++;
                Tranfer("OK");
                upload_image();
                Program_Configuration.UpdateSystem_Config("inf_process", DateTime.Now.ToString());
                inf_process();
                _OKnum++;
                _sum = (short)(_NGnum + _OKnum);
                OKnum.Text = _OKnum.ToString();
                totalPN.Text = _sum.ToString();
                ck_num1.Text = num1.ToString();
                dung_may();
            };
            this.Invoke(inv);
        }
        private void NG1_check()
        {
            MethodInvoker inv = delegate
            {
                num1++;
                DateTime dt = DateTime.Now;
                Tranfer("ERROR");
                upload_image();
                Program_Configuration.UpdateSystem_Config("inf_process", DateTime.Now.ToString());
                inf_process();
                _NGnum++;
                _sum = (short)(_NGnum + _OKnum);
                NGnum.Text = _NGnum.ToString();
                totalPN.Text = _sum.ToString();
                ck_num1.Text = num1.ToString();
                dung_may();
            };
            this.Invoke(inv);
        }
        private void OK2_check()
        {
            MethodInvoker inv = delegate
            {
                num2++;
                DateTime dt = DateTime.Now;
                Tranfer1("OK");
                update_image2();
                Program_Configuration.UpdateSystem_Config("inf_process", dt.ToString());
                inf_process();
                _OKnum++;
                _sum = (short)(_NGnum + _OKnum);
                OKnum.Text = _OKnum.ToString();
                totalPN.Text = _sum.ToString();
                ck_num2.Text = num2.ToString();
                dung_may();
            };
            this.Invoke(inv);
        }
        private void NG2_check()
        {
            MethodInvoker inv = delegate
            {
                num2++;
                DateTime dt = DateTime.Now;
                Tranfer1("ERROR");
                update_image2();
                Program_Configuration.UpdateSystem_Config("inf_process", dt.ToString());
                inf_process();
                _NGnum++;
                _sum = (short)(_NGnum + _OKnum);
                NGnum.Text = _NGnum.ToString();
                totalPN.Text = _sum.ToString();
                ck_num2.Text = num2.ToString();
                dung_may();
            };
            this.Invoke(inv);
        }
        private int on1 = 0;

        private void zoom1(int pic)
        {
            MethodInvoker inv = delegate
            {
                on1++;
                if (on1 == 1 && allow_check)
                {
                    pictureBox1.Hide();
                    pictureBox2.Hide();
                    pictureBox3.Hide();
                    pictureBox4.Hide();
                    pictureBox5.Hide();
                    pictureBox6.Hide();
                    capture1.Visible = false;
                    capture2.Visible = false;
                    capture3.Visible = false;
                    capture4.Visible = false;
                    capture5.Visible = false;
                    capture6.Visible = false;
                    Hname1.Visible = false;
                    Hname2.Visible = false;
                    Hname3.Visible = false;
                    Hname4.Visible = false;
                    Hname5.Visible = false;
                    Hname6.Visible = false;

                    c1h1_1.Visible = false; c1h2_1.Visible = false;
                    c2h1_1.Visible = false; c2h2_1.Visible = false;
                    c3h1_1.Visible = false; c1h3_1.Visible = false;
                    c4h1_1.Visible = false; c2h3_1.Visible = false;
                    c1h6_1.Visible = false; c1h4_1.Visible = false;
                    c2h6_1.Visible = false; c2h4_1.Visible = false;
                    c3h6_1.Visible = false; c1h5_1.Visible = false;
                    c4h6_1.Visible = false; c2h5_1.Visible = false;
                    switch (pic)
                    {
                        case 1:
                            pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_1_1);
                            pic_full1.Show();
                            pinf411.Text = c1h1_1.Text;
                            pinf421.Text = c2h1_1.Text;
                            pinf431.Text = c3h1_1.Text;
                            pinf441.Text = c4h1_1.Text;
                            pinf211.Visible = false;
                            pinf221.Visible = false;
                            pinf411.Visible = true;
                            pinf421.Visible = true;
                            pinf431.Visible = true;
                            pinf441.Visible = true;
                            break;
                        case 2:
                            pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_1_2);
                            pic_full1.Show();
                            pinf211.Text = c1h2_1.Text;
                            pinf221.Text = c2h2_1.Text;

                            pinf211.Visible = true;
                            pinf221.Visible = true;
                            pinf411.Visible = false;
                            pinf421.Visible = false;
                            pinf431.Visible = false;
                            pinf441.Visible = false;
                            break;
                        case 3:
                            pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_1_3);
                            pic_full1.Show();
                            pinf211.Text = c1h3_1.Text;
                            pinf221.Text = c2h3_1.Text;

                            pinf211.Visible = true;
                            pinf221.Visible = true;
                            pinf411.Visible = false;
                            pinf421.Visible = false;
                            pinf431.Visible = false;
                            pinf441.Visible = false;
                            break;
                        case 4:
                            pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_1_4);
                            pic_full1.Show();
                            pinf211.Text = c1h4_1.Text;
                            pinf221.Text = c2h4_1.Text;

                            pinf211.Visible = true;
                            pinf221.Visible = true;
                            pinf411.Visible = false;
                            pinf421.Visible = false;
                            pinf431.Visible = false;
                            pinf441.Visible = false;
                            break;
                        case 5:
                            pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_1_5);
                            pic_full1.Show();
                            pinf211.Text = c1h5_1.Text;
                            pinf221.Text = c2h5_1.Text;

                            pinf211.Visible = true;
                            pinf221.Visible = true;
                            pinf411.Visible = false;
                            pinf421.Visible = false;
                            pinf431.Visible = false;
                            pinf441.Visible = false;
                            break;
                        case 6:
                            pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_1_6);
                            pic_full1.Show();
                            pinf411.Text = c1h6_1.Text;
                            pinf421.Text = c2h6_1.Text;
                            pinf431.Text = c3h6_1.Text;
                            pinf441.Text = c4h6_1.Text;
                            pinf211.Visible = false;
                            pinf221.Visible = false;
                            pinf411.Visible = true;
                            pinf421.Visible = true;
                            pinf431.Visible = true;
                            pinf441.Visible = true;
                            break;

                    }
                }
                if (on1 == 2 && allow_check)
                {

                    pic_full1.Image.Dispose();
                    pic_full1.Hide();
                    pictureBox1.Show();
                    pictureBox2.Show();
                    pictureBox3.Show();
                    pictureBox4.Show();
                    pictureBox5.Show();
                    pictureBox6.Show();
                    capture1.Visible = true;
                    capture2.Visible = true;
                    capture3.Visible = true;
                    capture4.Visible = true;
                    capture5.Visible = true;
                    capture6.Visible = true;
                    Hname1.Visible = true;
                    Hname2.Visible = true;
                    Hname3.Visible = true;
                    Hname4.Visible = true;
                    Hname5.Visible = true;
                    Hname6.Visible = true;
                    pinf411.Visible = false;
                    pinf421.Visible = false;
                    pinf431.Visible = false;
                    pinf441.Visible = false;
                    pinf211.Visible = false;
                    pinf221.Visible = false;

                    c1h1_1.Visible = true; c1h2_1.Visible = true;
                    c2h1_1.Visible = true; c2h2_1.Visible = true;
                    c3h1_1.Visible = true; c1h3_1.Visible = true;
                    c4h1_1.Visible = true; c2h3_1.Visible = true;
                    c1h6_1.Visible = true; c1h4_1.Visible = true;
                    c2h6_2.Visible = true; c2h4_1.Visible = true;
                    c3h6_1.Visible = true; c1h5_1.Visible = true;
                    c4h6_1.Visible = true; c2h5_1.Visible = true;
                    on1 = 0;
                }
            };
            this.Invoke(inv);
        }
        private int on2 = 0;

        private void zoom2(int pic)
        {
            MethodInvoker inv = delegate
            {
                on2++;
                if (on2 == 1 && allow_check)
                {
                    pictureBox15.Hide();
                    pictureBox16.Hide();
                    pictureBox17.Hide();
                    pictureBox18.Hide();
                    pictureBox19.Hide();
                    pictureBox20.Hide();
                    capture_7.Visible = false;
                    capture_8.Visible = false;
                    capture_9.Visible = false;
                    capture_10.Visible = false;
                    capture_11.Visible = false;
                    capture_12.Visible = false;
                    Hname_7.Visible = false;
                    Hname_8.Visible = false;
                    Hname_9.Visible = false;
                    Hname_10.Visible = false;
                    Hname_11.Visible = false;
                    Hname_12.Visible = false;

                    c1h1_2.Visible = false; c1h2_2.Visible = false;
                    c2h1_2.Visible = false; c2h2_2.Visible = false;
                    c3h1_2.Visible = false; c1h3_2.Visible = false;
                    c4h1_2.Visible = false; c2h3_2.Visible = false;
                    c1h6_2.Visible = false; c1h4_2.Visible = false;
                    c2h6_2.Visible = false; c2h4_2.Visible = false;
                    c3h6_2.Visible = false; c1h5_2.Visible = false;
                    c4h6_2.Visible = false; c2h5_2.Visible = false;
                    switch (pic)
                    {
                        case 1:
                            picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_2_1);
                            picfull_2.Show();
                            pinf412.Text = c1h1_2.Text;
                            pinf422.Text = c2h1_2.Text;
                            pinf432.Text = c3h1_2.Text;
                            pinf442.Text = c4h1_2.Text;
                            pinf212.Visible = false;
                            pinf222.Visible = false;
                            pinf412.Visible = true;
                            pinf422.Visible = true;
                            pinf432.Visible = true;
                            pinf442.Visible = true;
                            break;
                        case 2:
                            picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_2_2);
                            picfull_2.Show();
                            pinf212.Text = c1h2_2.Text;
                            pinf222.Text = c2h2_2.Text;

                            pinf212.Visible = true;
                            pinf222.Visible = true;
                            pinf412.Visible = false;
                            pinf422.Visible = false;
                            pinf432.Visible = false;
                            pinf442.Visible = false;
                            break;
                        case 3:
                            picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_2_3);
                            picfull_2.Show();
                            pinf212.Text = c1h3_2.Text;
                            pinf222.Text = c2h3_2.Text;

                            pinf212.Visible = true;
                            pinf222.Visible = true;
                            pinf412.Visible = false;
                            pinf422.Visible = false;
                            pinf432.Visible = false;
                            pinf442.Visible = false;
                            break;
                        case 4:
                            picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_2_4);
                            picfull_2.Show();
                            pinf212.Text = c1h4_2.Text;
                            pinf222.Text = c2h4_2.Text;

                            pinf212.Visible = true;
                            pinf222.Visible = true;
                            pinf412.Visible = false;
                            pinf422.Visible = false;
                            pinf432.Visible = false;
                            pinf442.Visible = false;
                            break;
                        case 5:
                            picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_2_5);
                            picfull_2.Show();
                            pinf212.Text = c1h5_2.Text;
                            pinf222.Text = c2h5_2.Text;

                            pinf212.Visible = true;
                            pinf222.Visible = true;
                            pinf412.Visible = false;
                            pinf422.Visible = false;
                            pinf432.Visible = false;
                            pinf442.Visible = false;
                            break;
                        case 6:
                            picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + path_2_6);
                            picfull_2.Show();
                            pinf412.Text = c1h6_2.Text;
                            pinf422.Text = c2h6_2.Text;
                            pinf432.Text = c3h6_2.Text;
                            pinf442.Text = c4h6_2.Text;
                            pinf212.Visible = false;
                            pinf222.Visible = false;
                            pinf412.Visible = true;
                            pinf422.Visible = true;
                            pinf432.Visible = true;
                            pinf442.Visible = true;
                            break;

                    }
                }
                if (on2 == 2 && allow_check)
                {
                    picfull_2.Image.Dispose();
                    picfull_2.Hide();
                    pictureBox15.Show();
                    pictureBox16.Show();
                    pictureBox17.Show();
                    pictureBox18.Show();
                    pictureBox19.Show();
                    pictureBox20.Show();
                    capture_7.Visible = true;
                    capture_8.Visible = true;
                    capture_9.Visible = true;
                    capture_10.Visible = true;
                    capture_11.Visible = true;
                    capture_12.Visible = true;
                    Hname_7.Visible = true;
                    Hname_8.Visible = true;
                    Hname_9.Visible = true;
                    Hname_10.Visible = true;
                    Hname_11.Visible = true;
                    Hname_12.Visible = true;

                    pinf412.Visible = false;
                    pinf422.Visible = false;
                    pinf432.Visible = false;
                    pinf442.Visible = false;
                    pinf212.Visible = false;
                    pinf222.Visible = false;

                    c1h1_2.Visible = true; c1h2_2.Visible = true;
                    c2h1_2.Visible = true; c2h2_2.Visible = true;
                    c3h1_2.Visible = true; c1h3_2.Visible = true;
                    c4h1_2.Visible = true; c2h3_2.Visible = true;
                    c1h6_2.Visible = true; c1h4_2.Visible = true;
                    c2h6_2.Visible = true; c2h4_2.Visible = true;
                    c3h6_2.Visible = true; c1h5_2.Visible = true;
                    c4h6_2.Visible = true; c2h5_2.Visible = true;
                    on2 = 0;
                }
            };
            this.Invoke(inv);
        }
        #endregion
        private void General_tab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (General_tab.SelectedIndex == 2 && start_check && count_6 > 1)
            {
                stt++;
                allow_check = true;
                if (stt == 1)
                {
                    DirectoryInfo d = new DirectoryInfo(system_config.Map_Path_File);
                    upload_image();
                    if (folderIndex == 0)
                    {
                        folderIndex++;
                        load2 = folderIndex;
                    }
                    update_image2();
                   
                }

                if(stt > 1) 
                {
                    if (run_out1 && folderIndex < count_6)
                    {
                        upload_image();
                        run_out1 = false;
                    }
                    if (run_out2 && folderIndex < count_6)
                    {
                        update_image2();
                        run_out2 = false;
                    }
                }
            }
            else allow_check = false;
            string per = sql_action.getID_per_group(UserID);
            if (General_tab.SelectedIndex == 3) 
            {
                //if (!serialPort_communicate.IsOpen) serialPort_communicate.Open();
                if ((per == "3" || per == "1")) 
                {
                    groupBox13.Enabled = false;
                  
                    groupBox12.Enabled = false;
                    groupBox15.Enabled = false;
                    groupBox14.Enabled = false;
                    groupBox2.Enabled = false;
                    groupBox6.Enabled = false;
                    groupBox7.Enabled = false;
                    groupBox8.Enabled = false;
                    groupBox9.Enabled = false;
                    groupBox10.Enabled = false;
                    groupBox11.Enabled = false;
                    txtIPAddress.Enabled = false;
                    btnAutoHome.Enabled = false;
                }                
            }
            if (General_tab.SelectedIndex == 1 && !started)
            {
                system_config = Program_Configuration.GetSystem_Config();

                if (!serialPort_communicate.IsOpen)
                {
                    serialPort_communicate.PortName = system_config.DefaultComport;
                    serialPort_communicate.BaudRate = Convert.ToInt32(system_config.DefaultCOMBaudrate);
                    serialPort_communicate.Open();
                    nhapid = true;
                }
                if (TB_idworker.Text != "" && TB_wker2.Text != "")
                {
                    if (serialPort_communicate.IsOpen) { serialPort_communicate.Close(); nhapid = false; }
                }
                if (TB_idworker.Text == "" || TB_wker2.Text == "")
                {
                    nhapid = true;
                }
            }
            else nhapid = false;
        }
        
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (started)
            {
                MessageBox.Show("Please stop program first!");
                return;
            }
            var result = MessageBox.Show("Do you want to reset Program to default setting", "RESET", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                count_1 = 0;
                count_2 = 0;
                count_3 = 0;
                count_4 = 0;
                count_5 = 0;
                count_6 = 0;
                count_7 = 0;
                folderIndex = 0;
                load1 = 0;
                load2 = 1;
                _OKnum = 0;
                _NGnum = 0;
                _sum = 0;
                Program_Configuration.UpdateSystem_Config("Folder_index_tranfer", load1.ToString());
                Program_Configuration.UpdateSystem_Config("same_folder_1", folderIndex.ToString());
                Program_Configuration.UpdateSystem_Config("Folder_load_check", load2.ToString());
                Program_Configuration.UpdateSystem_Config("Location_cam1_folder", "0");
                Program_Configuration.UpdateSystem_Config("Location_cam2_folder", "0");
                Program_Configuration.UpdateSystem_Config("Location_cam3_folder", "0");
                Program_Configuration.UpdateSystem_Config("Location_cam4_folder", "0");
                Program_Configuration.UpdateSystem_Config("Location_cam5_folder", "0");
                Program_Configuration.UpdateSystem_Config("Location_cam6_folder", "0");
                Program_Configuration.UpdateSystem_Config("Location_cam7_folder", "0");
                
                using (StreamWriter sw = new StreamWriter("Output.txt"))
                {
                    sw.Write("");
                    sw.WriteLine("" + _OKnum.ToString() + "");
                    sw.WriteLine("" + _NGnum.ToString() + "");
                }

            }
        }
       

        #region /////////////////////////////////////////////////////////////////////////////////////////////////////////////LOGIN____LOGOUT
        private string UserID = "";
        bool load = false;

        private void login_btn_Click(object sender, EventArgs e)
        {
            if (load) return;
            load = true;
        
            System.Threading.Thread.Sleep(10);    
            //picload_in.Visible = true;
            Login loginfrm = new Login();
            loginfrm.Show();
          
            loginfrm.FormClosed += (object sender1, FormClosedEventArgs e1) =>
                {
                    UserID = loginfrm.ID_user;
                    if (UserID != "")
                    {
                        login(UserID);
                    }
                    load = false;
                    picload_in.Visible = false;
                };             
              
         
        }
        private void permiss_1() // master
        {
            foreach (Control ctrl in General_tab.Controls)
            {
                if (ctrl.Name == "tabPage3")
                {
                    foreach (Control ctl in tabPage3.Controls)
                    {
                        if (ctl.Name == "login_btn")
                        {
                            ctl.Enabled = false;
                        }
                        else
                        {
                            ctl.Enabled = true;
                        }
                    }
                }
                ctrl.Enabled = true;
            }
        }
        private void permiss_2() //operator
        {
            foreach (Control ctrl in General_tab.Controls)
            {
                if (ctrl.Name == "tabPage3")
                {
                    foreach (Control ctl in tabPage3.Controls)
                    {

                        if (ctl.Name == "Logout_btn")
                        {
                            ctl.Enabled = true;
                        }
                        else if (ctl.Name == "TB_idworker")
                        {
                            ctl.Enabled = true;
                        }
                        else if (ctl.Name == "TB_wker2")
                        {
                            ctl.Enabled = true;
                        }
                        else if (ctl.Name == "label8")
                        {
                            ctl.Enabled = true;
                        }
                        else if (ctl.Name == "label13")
                        {
                            ctl.Enabled = true;
                        }
                        else if (ctl.Name == "view_btn")
                        {
                            ctl.Enabled = false;
                        }
                        else
                        {
                            ctl.Enabled = false;
                        }
                    }
                }
                if (ctrl.Name == "tabPage4")
                {
                    foreach (Control ctl in tabPage4.Controls)
                    {
                        if (ctl.Name == "btnConnect")
                        {
                            ctl.Enabled = true;
                        }
                        else if (ctl.Name == "btnAutoHome")
                        {
                            ctl.Enabled = true;
                        }
                        else
                        {
                            ctl.Enabled = false;
                        }
                    }
                }
                else
                {
                    ctrl.Enabled = true;
                }
            }
        }
        private void permiss_3() // machanic
        {
            foreach (Control ctrl in General_tab.Controls)
            {
                if (ctrl.Name == "tabPage3")
                {
                    foreach (Control ctl in tabPage3.Controls)
                    {
                        if (ctl.Name == "delete_btn")
                        {
                            ctl.Enabled = false;
                        }
                        else if (ctl.Name == "login_btn")
                        {
                            ctl.Enabled = false;
                        }
                        else if (ctl.Name == "view_btn")
                        {
                            ctl.Enabled = false;
                        }
                        else if (ctl.Name == "comboBox1")
                        {
                            ctl.Enabled = false;
                        }
                        else if (ctl.Name == "sign_up")
                        {
                            ctl.Enabled = false;
                        }
                        else
                        {
                            ctl.Enabled = true;
                        }
                    }
                }
     
                else
                {
                    ctrl.Enabled = true;
                }
            }
        }
        private void permiss_4() //production engineer
        {
            foreach (Control ctrl in General_tab.Controls)
            {
                if (ctrl.Name == "tabPage3")
                {
                    foreach (Control ctl in tabPage3.Controls)
                    {
                        if (ctl.Name == "login_btn")
                        {
                            ctl.Enabled = false;
                        }
                        else
                        {
                            ctl.Enabled = true;
                        }
                    }
                }
                ctrl.Enabled = true;
            }
        }
        private void login(string id)
        {
            string per = "";
            per = sql_action.getID_per_group(id);
            if (per == "1")
            {
                permiss_1();
            }
            if (per == "2")
            {
                permiss_2();
            }
            if (per == "3")
            {
                permiss_3();
            }
            if (per == "4")
            {
                permiss_4();
            }
        }

        private void Logout_btn_Click(object sender, EventArgs e)
        {
            if (started) 
            {
                MessageBox.Show("Please Stop Program Fisrt");
                return;
            }
            else 
            {               
                var result = MessageBox.Show("Do you want to Log out?", "Confirm", MessageBoxButtons.OKCancel);
                if (timer.Enabled && result == DialogResult.OK)
                {
                    unable();
                    UserID = "";
                    // if (timer.Enabled) timer.Stop();
                    LB_TIMER.Text = "00:00:00";
                    start_check = false;
                    started = false;
                    RESET();
                    Start_btn.Enabled = true;
                    Stop_btn.Enabled = false;                 
                    Pic_Cam1.Image = null;
                    Pic_Cam2.Image = null;
                    Pic_Cam3.Image = null;
                    Pic_Cam4.Image = null;
                    Pic_Cam5.Image = null;
                    Pic_Cam6.Image = null;
                    status("[SYSTEM] Program STOPPED");
                    startPR_Count = 0;
                   
                        Login loginfrm = new Login();
                        loginfrm.FormClosed += (object sender1, FormClosedEventArgs e1) =>
                        {
                            UserID = loginfrm.ID_user;
                            if (UserID != "")
                            {
                                login(UserID);
                            }
                            load = false;
                        };
                        loginfrm.Show();
                }
            }
        }
        private void sign_up_Click(object sender, EventArgs e)
        {
            if (started)
            {
                MessageBox.Show("Please stop the program first");
                return;
            }
            Sign_up sign_form = new Sign_up();
            sign_form.FormClosed += (object sender2, FormClosedEventArgs e2) =>
            {
                this.Show();
            };
            this.Hide();
            sign_form.Show();
        }

        #endregion
        private void view_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.DataBindings.Clear();
            dataGridView2.DataBindings.Clear();
           
                DataTable dt = sql_action.result_tbl("component_status");
                dataGridView1.DataSource = dt;
           
                DataTable dt2 = sql_action.result_tbl("NG_Detail");
                dataGridView2.DataSource = dt2;                            
        }
        #region ////////////////////////////////////////////////////////////////////////////////////////////////////////////manual s7net PLC

        Plc PLCS7_1200 = null;
        ReadPLC ReadData = new ReadPLC();

        Class1 cl = new Class1();
        Class2 cl2 = new Class2();
        Class3 cl3 = new Class3();
        bool PLC_con = false;
        private void btnConnect_Click(object sender, EventArgs e)
        {
            PLCS7_1200 = new Plc(CpuType.S71200, "192.168.0.7", 0, 0);
            if (PLCS7_1200.IsAvailable)
            {
                PLCS7_1200.Open();
                if (PLCS7_1200.IsConnected == true)
                {
                    MessageBox.Show("Connect to PLC Successful", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnAutoHome.Enabled = true;
                    PLC_con = true;
                    btnReadAll.PerformClick();
                }
                else
                {
                    MessageBox.Show("Error");
                    PLC_con = false;
                }

            }
            else 
            {
                MessageBox.Show("Error");
                PLC_con = false;
            }

        }
        string t = "";
       

        public void doAction()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(doAction));
            else
            {
                //label16.Text = x;
                if (x == "11")
                {
                    pic1.Visible = true;
                    pic2.Visible = false;
                    lblRightC.Text = "1";
                    lblLeftC.Text = "0";
                }
                if (x == "21")
                {
                    pic1.Visible = false;
                    pic2.Visible = true;
                    lblRightC.Text = "0";
                    lblLeftC.Text = "1";
                }
                if (x == "12")
                {
                    pic3.Visible = true;
                    pic4.Visible = false;
                    lblUpPUI.Text = "1";
                    lblDownPUI.Text = "0";
                }
                if (x == "22")
                {
                    pic3.Visible = false;
                    pic4.Visible = true;
                    lblUpPUI.Text = "0";
                    lblDownPUI.Text = "1";
                }
                if (x == "13")
                {
                    pic5.Visible = true;
                    pic6.Visible = false;
                    lblUpXL3.Text = "1";
                    lblDownXL3.Text = "0";
                }
                if (x == "23")
                {
                    pic5.Visible = false;
                    pic6.Visible = true;
                    lblUpXL3.Text = "0";
                    lblDownXL3.Text = "1";
                }
                if (x == "14")
                {
                    pic7.Visible = true;
                    pic8.Visible = false;
                    lblUpXL4.Text = "1";
                    lblDownXL4.Text = "0";
                }
                if (x == "24")
                {
                    pic7.Visible = false;
                    pic8.Visible = true;
                    lblUpXL4.Text = "0";
                    lblDownXL4.Text = "1";
                }
                if (x == "15")
                {
                    pic9.Visible = true;
                    pic10.Visible = false;
                    lblUpPUC.Text = "1";
                    lblDownPUC.Text = "0";
                }
                if (x == "25")
                {
                    pic9.Visible = false;
                    pic10.Visible = true;
                    lblUpPUC.Text = "0";
                    lblDownPUC.Text = "1";
                }
                if (x == "16")
                {
                    pic11.Visible = true;
                    pic12.Visible = false;
                    lblRightMC.Text = "1";
                    lblLeftMC.Text = "0";
                }
                if (x == "26")
                {
                    pic11.Visible = false;
                    pic12.Visible = true;
                    lblRightMC.Text = "0";
                    lblLeftMC.Text = "1";
                }
                if (x == "17")
                {
                    pic13.Visible = true;
                    pic14.Visible = false;
                    lblUpPP.Text = "1";
                    lblDownPP.Text = "0";
                }
                if (x == "27")
                {
                    pic13.Visible = false;
                    pic14.Visible = true;
                    lblUpPP.Text = "0";
                    lblDownPP.Text = "1";
                }
                if (x == "18")
                {
                    pic15.Visible = true;
                    pic16.Visible = false;
                    lblUpOut.Text = "1";
                    lblDownOut.Text = "0";
                }
                if (x == "28")
                {
                    pic15.Visible = false;
                    pic16.Visible = true;
                    lblUpOut.Text = "0";
                    lblDownOut.Text = "1";
                }
                string[] arr = t.Split('#');
                if (arr[0] == "NG1")
                {
                    if (!run_out1) 
                    {
                        string Addr = "DB33.DBX0.0";
                        PLCS7_1200.Write(Addr, int.Parse("1"));
                        m = "";
                    }                   
                }
                if (arr[0] == "NG2")
                {
                    if (!run_out2) 
                    {
                        string Addr = "DB33.DBX0.1";
                        PLCS7_1200.Write(Addr, int.Parse("1"));
                        m = "";
                    }                   
                }
            }
        }
        string x = "";
        string m = "";
        private void ShowData(object sender, EventArgs e)
        {
            string[] arr = x.Split('#');
            if (arr[0] == "NG1"||m=="OK1")
            {
                string Addr = "DB33.DBX0.0";
                PLCS7_1200.Write(Addr, int.Parse("1"));
            }
            if (arr[0] == "NG2")
            {
                string Addr = "DB33.DBX0.1";
                PLCS7_1200.Write(Addr, int.Parse("1"));
            }
        }
      
        private void btnJogForInput_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB1.DBX16.2";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void btnJogForInput_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB1.DBX16.2";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void btnJogBackInput_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB1.DBX16.3";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void btnJogBackInput_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB1.DBX16.3";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }
        private void btnMovePositionInput_Click(object sender, EventArgs e)
        {
            DataType dt = DataType.DataBlock;
            int DB1 = 1;
            int DB5 = 5;
            int StartByte1 = 46;
            int StartByte2 = 2;
            PLCS7_1200.Write(dt, DB1, StartByte1, Convert.ToInt32(txtPositionInput.Text));
            PLCS7_1200.Write(dt, DB5, StartByte2, Convert.ToInt16(txtSpeedInput.Text));
            string Addr = "M101.6";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }
        private void btnMovePositionInput_MouseUp(object sender, MouseEventArgs e)
        {
            //      string Addr = "M500.4";
            //    PLCS7_1200.Write(Addr, int.Parse("0"));

        }
        private void btnMovePositionInput_MouseDown(object sender, MouseEventArgs e)
        {
            //string Addr = "M500.4";
            //PLCS7_1200.Write(Addr, int.Parse("1"));
            //      string Addr1 = "DB1.DBD32";
            //     PLCS7_1200.Write(Addr1, short.Parse(txtPositionInput.Text));
            //  string Addr2 = "DB1.DBW36";
            //   PLCS7_1200.Write(Addr2, int.Parse(txtSpeedInput.Text));
        }

        private void txtPositionInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtSpeedInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        private void ShowDataRead(Label lb, double db)
        {
            double c = Math.Round(db);
            lb.Text = c.ToString();
        }
        private void ShowDataReadtxt(TextBox tx, double db)
        {
            double c = Math.Round(db);
            tx.Text = c.ToString();
        }
        private void btnJogForPG_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB2.DBX56.0";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void btnJogForPG_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB2.DBX56.0";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void btnJogBackPG_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB2.DBX56.1";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }
        private void btnJogBackPG_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB2.DBX56.1";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void btnReadPositionIP_Click(object sender, EventArgs e)
        {
            PLCS7_1200.ReadClass(ReadData, 5, 8);
            ShowDataRead(lblReadPositionInput, ReadData.current_Position_SV1);
        }

        private void btnReadPositionPG_Click(object sender, EventArgs e)
        {
            PLCS7_1200.ReadClass(cl, 2, 52);
            ShowDataRead(lblReadPositionPG, cl.Distance_Start);
        }

        private void txtPositionPG_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtSpeedPG_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }



        private void picCentRight_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX0.3";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void picCentRight_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX0.3";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void picCentLeft_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX1.4";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void picCentLeft_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX1.4";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void btnSetHomeInput_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "M500.3";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void btnSetHomeInput_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "M500.3";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void picUpPUI_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX0.4";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void picDownPUI_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX1.5";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void picUpPUI_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX0.4";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void picDownPUI_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX1.5";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void picUpXL3_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX0.5";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void picUpXL3_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX0.5";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void picDownXL3_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX1.6";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void picDownXL3_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX1.6";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void picUpXL4_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX0.6";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void picUpXL4_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX0.6";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void picDownXL4_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX1.7";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void picDownXL4_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX1.7";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void picUpPUC_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX0.7";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void picUpPUC_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX0.7";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void picDownPUC_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX2.0";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void picDownPUC_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX2.0";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void picRightMC_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX1.0";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void picRightMC_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX1.0";
            PLCS7_1200.Write(Addr, int.Parse("0"));

        }

        private void picLeftMC_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX2.1";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void picLeftMC_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX2.1";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void picUpPP_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX1.1";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void picUpPP_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX1.1";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void PicDownPP_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX2.2";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void PicDownPP_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX2.2";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string Addr = "M100.2";
            PLCS7_1200.Write(Addr, int.Parse("1"));
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;
            groupBox5.Enabled = true;
            groupBox6.Enabled = true;
            groupBox7.Enabled = true;
            groupBox8.Enabled = true;
            groupBox9.Enabled = true;
            groupBox10.Enabled = true;
            groupBox11.Enabled = true;
            ShowPosition();

        }
       
        private void lblSetSpeedJogSV1_Click(object sender, EventArgs e)
        {
            DataType dt = DataType.DataBlock;
            int DB1 = 5;
            int StartByte = 1570;
            PLCS7_1200.Write(dt, DB1, StartByte, Convert.ToInt16(txtSpeedJogSV1.Text));
            MessageBox.Show("Change Speed Success", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void picUpOut_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX1.2";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void picUpOut_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX1.2";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void picDownOut_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX2.3";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void picDownOut_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "DB33.DBX2.3";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void lblSetSpeedJogSV2_Click(object sender, EventArgs e)
        {
            DataType dt = DataType.DataBlock;
            int DB1 = 2;
            int StartByte = 60;
            PLCS7_1200.Write(dt, DB1, StartByte, Convert.ToInt16(txtSpeedJogSV2.Text));
            MessageBox.Show("Change Speed Success", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void btnMovePositionPG_Click(object sender, EventArgs e)
        {
            DataType dt = DataType.DataBlock;
            int DB1 = 2;
            int StartByte1 = 6;
            int StartByte2 = 64;
            PLCS7_1200.Write(dt, DB1, StartByte1, Convert.ToInt32(txtPositionPG.Text));
            PLCS7_1200.Write(dt, DB1, StartByte2, Convert.ToInt16(txtSpeedPG.Text));
            string Addr = "M104.0";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void btnSetHomePG_MouseUp(object sender, MouseEventArgs e)
        {
            string Addr = "M104.2";
            PLCS7_1200.Write(Addr, int.Parse("0"));
        }

        private void btnSetHomePG_MouseDown(object sender, MouseEventArgs e)
        {
            string Addr = "M104.2";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void txtPositionInput_TextChanged(object sender, EventArgs e)
        {
            if (txtPositionInput.Text != null)
            {
                Int32 t = Int32.Parse(txtPositionInput.Text);
                if (t > 86000)
                {
                    MessageBox.Show("journey limit exceeded", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnMovePositionInput.Enabled = false;
                }
                else
                {
                    btnMovePositionInput.Enabled = true;
                }
            }

        }

        private void txtPositionPG_TextChanged(object sender, EventArgs e)
        {
            if (txtPositionPG.Text != "")
            {
                Int32 t = Int32.Parse(txtPositionPG.Text);
                if (t > 86000)
                {
                    MessageBox.Show("journey limit exceeded", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnMovePositionPG.Enabled = false;
                }
                else
                {
                    btnMovePositionPG.Enabled = true;
                }
            }
        }
        private void ShowPosition()
        {
            PLCS7_1200.ReadClass(cl2, 2, 18);
            ShowDataReadtxt(txtPosNG1, cl2.Distance1);
            ShowDataReadtxt(txtPosNG2, cl2.Distance2);
            ShowDataReadtxt(txtPosNG3, cl2.Distance3);
            ShowDataReadtxt(txtPosNG4, cl2.Distance4);
            ShowDataReadtxt(txtPosNG5, cl2.Distance5);
            ShowDataReadtxt(txtPosNG6, cl2.Distance6);
            ShowDataReadtxt(txtPosOK, cl2.DistanceOK);
            PLCS7_1200.ReadClass(cl2, 2, 22);
            ShowDataReadtxt(txtPosOutput, cl2.Distance_Start);
            PLCS7_1200.ReadClass(cl3, 1, 18);
            ShowDataReadtxt(txtPosInput, cl3.Distance_Centering);
            ShowDataReadtxt(txtPosStart, cl3.Distance_Start);
            PLCS7_1200.ReadClass(cl3, 1, 24);
            ShowDataReadtxt(txtPS1, cl3.Distance1);
            ShowDataReadtxt(txtPS2, cl3.Distance2);
            ShowDataReadtxt(txtPS3, cl3.Distance3);
            ShowDataReadtxt(txtPS4, cl3.Distance4);
            ShowDataReadtxt(txtPS5, cl3.Distance5);
            ShowDataReadtxt(txtPS6, cl3.Distance6);
            ShowDataReadtxt(txtPS7, cl3.Distance7);
            ShowDataReadtxt(txtPS8, cl3.Distance8);
            ShowDataReadtxt(txtPS9, cl3.Distance9);
            PLCS7_1200.ReadClass(cl3, 1, 0);
            ShowDataReadtxt(txtVP1, cl3.Velocity_Point);
            PLCS7_1200.ReadClass(cl3, 1, 4);
            ShowDataReadtxt(txtVB1, cl3.Velocity_Back);
            PLCS7_1200.ReadClass(cl2, 2, 18);
            ShowDataReadtxt(txtVP2, cl2.Velocity_Point);
            PLCS7_1200.ReadClass(cl2, 2, 14);
            ShowDataReadtxt(txtVB2, cl2.Velocity_Back);
        }

        private void btnPositionNG1_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 2, 18, Convert.ToInt32(txtPosNG1.Text));
            PLCS7_1200.ReadClass(cl2, 2, 18);
            ShowDataReadtxt(txtPosNG1, cl2.Distance1);
        }

        private void btnPositionNG2_Click(object sender, EventArgs e)
        {

            PLCS7_1200.Write(DataType.DataBlock, 2, 22, Convert.ToInt32(txtPosNG2.Text));
            PLCS7_1200.ReadClass(cl2, 2, 18);
            ShowDataReadtxt(txtPosNG2, cl2.Distance2);
        }

        private void btnPositionNG3_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 2, 26, Convert.ToInt32(txtPosNG3.Text));
            PLCS7_1200.ReadClass(cl2, 2, 18);
            ShowDataReadtxt(txtPosNG3, cl2.Distance3);
        }

        private void btnPositionNG4_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 2, 30, Convert.ToInt32(txtPosNG4.Text));
            PLCS7_1200.ReadClass(cl2, 2, 18);
            ShowDataReadtxt(txtPosNG4, cl2.Distance4);
        }

        private void btnPositionNG5_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 2, 34, Convert.ToInt32(txtPosNG5.Text));
            PLCS7_1200.ReadClass(cl2, 2, 18);
            ShowDataReadtxt(txtPosNG5, cl2.Distance5);
        }

        private void btnPositionNG6_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 2, 38, Convert.ToInt32(txtPosNG6.Text));
            PLCS7_1200.ReadClass(cl2, 2, 18);
            ShowDataReadtxt(txtPosNG6, cl2.Distance6);
        }

        private void btnPositionOK_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 2, 42, Convert.ToInt32(txtPosOK.Text));
            PLCS7_1200.ReadClass(cl2, 2, 18);
            ShowDataReadtxt(txtPosOK, cl2.DistanceOK);
        }


        private void btnPositionOutPut_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 2, 50, Convert.ToInt16(txtPosOutput.Text));
            PLCS7_1200.ReadClass(cl2, 2, 22);
            ShowDataReadtxt(txtPosOutput, cl2.Distance_Start);
        }

        private void btnPositionInput_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 1, 18, Convert.ToInt16(txtPosInput.Text));
            PLCS7_1200.ReadClass(cl3, 1, 18);
            ShowDataReadtxt(txtPosInput, cl3.Distance_Centering);
        }

        private void btnPositionStart_Click(object sender, EventArgs e)
        {

            PLCS7_1200.Write(DataType.DataBlock, 1, 20, Convert.ToInt16(txtPosStart.Text));
            PLCS7_1200.ReadClass(cl3, 1, 18);
            ShowDataReadtxt(txtPosStart, cl3.Distance_Start);
        }

        private void btnReadAll_Click(object sender, EventArgs e)
        {
            ShowPosition();
            MessageBox.Show("Read Success", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnWriteAll_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 2, 18, Convert.ToInt32(txtPosNG1.Text));
            PLCS7_1200.Write(DataType.DataBlock, 2, 22, Convert.ToInt32(txtPosNG2.Text));
            PLCS7_1200.Write(DataType.DataBlock, 2, 26, Convert.ToInt32(txtPosNG3.Text));
            PLCS7_1200.Write(DataType.DataBlock, 2, 30, Convert.ToInt32(txtPosNG4.Text));
            PLCS7_1200.Write(DataType.DataBlock, 2, 34, Convert.ToInt32(txtPosNG5.Text));
            PLCS7_1200.Write(DataType.DataBlock, 2, 38, Convert.ToInt32(txtPosNG6.Text));
            PLCS7_1200.Write(DataType.DataBlock, 2, 42, Convert.ToInt32(txtPosOK.Text));
            PLCS7_1200.Write(DataType.DataBlock, 2, 50, Convert.ToInt16(txtPosOutput.Text));
            PLCS7_1200.Write(DataType.DataBlock, 1, 18, Convert.ToInt16(txtPosInput.Text));
            PLCS7_1200.Write(DataType.DataBlock, 1, 20, Convert.ToInt16(txtPosStart.Text));
            PLCS7_1200.Write(DataType.DataBlock, 2, 48, Convert.ToInt16(txtVP2.Text));
            PLCS7_1200.Write(DataType.DataBlock, 2, 46, Convert.ToInt16(txtVB2.Text));
            PLCS7_1200.Write(DataType.DataBlock, 1, 28, Convert.ToInt16(txtPS1.Text));
            PLCS7_1200.Write(DataType.DataBlock, 1, 30, Convert.ToInt16(txtPS2.Text));
            PLCS7_1200.Write(DataType.DataBlock, 1, 32, Convert.ToInt16(txtPS3.Text));
            PLCS7_1200.Write(DataType.DataBlock, 1, 34, Convert.ToInt16(txtPS4.Text));
            PLCS7_1200.Write(DataType.DataBlock, 1, 36, Convert.ToInt16(txtPS5.Text));
            PLCS7_1200.Write(DataType.DataBlock, 1, 38, Convert.ToInt16(txtPS6.Text));
            PLCS7_1200.Write(DataType.DataBlock, 1, 40, Convert.ToInt16(txtPS7.Text));
            PLCS7_1200.Write(DataType.DataBlock, 1, 42, Convert.ToInt16(txtPS8.Text));
            PLCS7_1200.Write(DataType.DataBlock, 1, 44, Convert.ToInt16(txtPS9.Text));
            PLCS7_1200.Write(DataType.DataBlock, 1, 24, Convert.ToInt16(txtVP1.Text));
            PLCS7_1200.Write(DataType.DataBlock, 1, 26, Convert.ToInt16(txtVB1.Text));
            MessageBox.Show("Write Success", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAutoHome_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do you sure want to set home for Auto mode", "Notice", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (dialog == DialogResult.OK)
            {
                string Addr = "M137.7";
                PLCS7_1200.Write(Addr, int.Parse("1"));
            }
        }

        private void btnSP1_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 1, 28, Convert.ToInt16(txtPS1.Text));
            PLCS7_1200.ReadClass(cl3, 1, 24);
            ShowDataReadtxt(txtPS1, cl3.Distance1);
        }

        private void btnPS2_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 1, 30, Convert.ToInt16(txtPS2.Text));
            PLCS7_1200.ReadClass(cl3, 1, 24);
            ShowDataReadtxt(txtPS2, cl3.Distance2);
        }

        private void btnPS3_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 1, 32, Convert.ToInt16(txtPS3.Text));
            PLCS7_1200.ReadClass(cl3, 1, 24);
            ShowDataReadtxt(txtPS3, cl3.Distance3);
        }

        private void btnPS4_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 1, 34, Convert.ToInt16(txtPS4.Text));
            PLCS7_1200.ReadClass(cl3, 1, 24);
            ShowDataReadtxt(txtPS4, cl3.Distance4);
        }

        private void btnPS5_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 1, 36, Convert.ToInt16(txtPS5.Text));
            PLCS7_1200.ReadClass(cl3, 1, 24);
            ShowDataReadtxt(txtPS5, cl3.Distance5);
        }

        private void btnPS6_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 1, 38, Convert.ToInt16(txtPS6.Text));
            PLCS7_1200.ReadClass(cl3, 1, 24);
            ShowDataReadtxt(txtPS6, cl3.Distance6);
        }

        private void btnPS7_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 1, 40, Convert.ToInt16(txtPS7.Text));
            PLCS7_1200.ReadClass(cl3, 1, 24);
            ShowDataReadtxt(txtPS7, cl3.Distance7);
        }

        private void btnPS8_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 1, 42, Convert.ToInt16(txtPS8.Text));
            PLCS7_1200.ReadClass(cl3, 1, 24);
            ShowDataReadtxt(txtPS8, cl3.Distance8);
        }

        private void btnPS9_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 1, 44, Convert.ToInt16(txtPS9.Text));
            PLCS7_1200.ReadClass(cl3, 1, 24);
            ShowDataReadtxt(txtPS9, cl3.Distance9);
        }

        private void btnVP1_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 1, 24, Convert.ToInt16(txtVP1.Text));
            PLCS7_1200.ReadClass(cl3, 1, 0);
            ShowDataReadtxt(txtVP1, cl3.Velocity_Point);
        }

        private void btnVB1_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 1, 26, Convert.ToInt16(txtVB1.Text));
            PLCS7_1200.ReadClass(cl3, 1, 4);
            ShowDataReadtxt(txtVB1, cl3.Velocity_Back);
        }

        private void btnVP2_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write(DataType.DataBlock, 2, 48, Convert.ToInt16(txtVP2.Text));
            PLCS7_1200.ReadClass(cl2, 2, 18);
            ShowDataReadtxt(txtVP2, cl2.Velocity_Point);
        }

        private void btnVB2_Click(object sender, EventArgs e)
        {

            PLCS7_1200.Write(DataType.DataBlock, 2, 46, Convert.ToInt16(txtVB2.Text));
            PLCS7_1200.ReadClass(cl2, 2, 14);
            ShowDataReadtxt(txtVB2, cl2.Velocity_Back);
        }

        private void btnC1_Click(object sender, EventArgs e)
        {
            string Addr = "DB5.DBX26.0";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void btnC2_Click(object sender, EventArgs e)
        {
            string Addr = "DB5.DBX26.1";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void btnC3_Click(object sender, EventArgs e)
        {
            string Addr = "DB5.DBX26.2";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void btnC4_Click(object sender, EventArgs e)
        {
            string Addr = "DB5.DBX26.3";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void btnC5_Click(object sender, EventArgs e)
        {
            string Addr = "DB5.DBX26.4";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        private void btnC6_Click(object sender, EventArgs e)
        {
            string Addr = "DB5.DBX26.5";
            PLCS7_1200.Write(Addr, int.Parse("1"));
        }

        #endregion

        #region////////////////////////////////////////////////////////////////////////////////////////////// Recover 
        string lan1_vt1 = ""; string lan2_vt1 = ""; string lan3_vt1 = ""; string lan4_vt1 = ""; string lan5_vt1 = ""; string lan6_vt1 = "";
        string lan12_vt1 = ""; string lan22_vt1 = ""; string lan32_vt1 = ""; string lan42_vt1 = ""; string lan52_vt1 = ""; string lan62_vt1 = "";
        string lan1_vt2 = ""; string lan2_vt2 = ""; string lan3_vt2 = ""; string lan4_vt2 = ""; string lan5_vt2 = ""; string lan6_vt2 = "";
        string lan12_vt2 = ""; string lan22_vt2 = ""; string lan32_vt2 = ""; string lan42_vt2 = ""; string lan52_vt2 = ""; string lan62_vt2 = "";
        string lan1_vt3 = ""; string lan2_vt3 = ""; string lan3_vt3 = ""; string lan4_vt3 = ""; string lan5_vt3 = ""; string lan6_vt3 = "";
        string lan12_vt3 = ""; string lan22_vt3 = ""; string lan32_vt3 = ""; string lan42_vt3 = ""; string lan52_vt3 = ""; string lan62_vt3 = "";
        string lan1_vt4 = ""; string lan2_vt4 = ""; string lan3_vt4 = ""; string lan4_vt4 = ""; string lan5_vt4 = ""; string lan6_vt4 = "";
        string lan12_vt4 = ""; string lan22_vt4 = ""; string lan32_vt4 = ""; string lan42_vt4 = ""; string lan52_vt4 = ""; string lan62_vt4 = "";
        string lan1_vt5 = ""; string lan2_vt5 = ""; string lan3_vt5 = ""; string lan4_vt5 = ""; string lan5_vt5 = ""; string lan6_vt5 = "";
        string lan12_vt5 = ""; string lan22_vt5 = ""; string lan32_vt5 = ""; string lan42_vt5 = ""; string lan52_vt5 = ""; string lan62_vt5 = "";
        string lan1_vt6 = ""; string lan2_vt6 = ""; string lan3_vt6 = ""; string lan4_vt6 = ""; string lan5_vt6 = ""; string lan6_vt6 = "";
        string lan12_vt6 = ""; string lan22_vt6 = ""; string lan32_vt6 = ""; string lan42_vt6 = ""; string lan52_vt6 = ""; string lan62_vt6 = "";
        string lan1_vt7 = ""; string lan2_vt7 = ""; string lan3_vt7 = ""; string lan4_vt7 = ""; string lan5_vt7 = ""; string lan6_vt7 = "";
        string lan12_vt7 = ""; string lan22_vt7 = ""; string lan32_vt7 = ""; string lan42_vt7 = ""; string lan52_vt7 = ""; string lan62_vt7 = "";
        /*
         chu trình 1        1      2    3     4     5     6     7     8     9     10    11
        vi trí -------------1           2           3           4           5           6           7           8       Cam1    Cam2    Cam3    Cam4    Cam5    Cam6
        no1                Cam1         x           x           x           x           x           x           x        cp1
        no2                Cam1   cp1   x           x           x           x           x           x           x        cp2    
        no3                Cam1   cp2   Cam2        x           x           x           x           x           x        cp3    cp1
        no4                Cam1   cp3   Cam2  cp1   x           X           x           x           x           x        cp4    cp2
        no5                Cam1   cp4   Cam2  cp2   Cam3        x           x           x           x           x        cp5    cp3     cp1
        no6                Cam1   cp5   Cam2  cp3   Cam3       Cam4         x           x           x           x        cp6    cp4     cp2     cp1
        no7                x      cp6   Cam2  cp4   Cam3       Cam4         x           x           x           x               cp5     cp3     cp2
        no8                x            Cam2  cp5   Cam3       Cam4         x           x           x           x               cp6     cp4     cp3     
        no9                x            x     cp6   Cam3       Cam4        Cam5         x           x           x                       cp5     cp4     cp1
        no10               x            x           Cam3       Cam4        Cam5        Cam6         x           x                       cp6     cp5     cp2     cp1       grip(1)   
        bonus              x            x            x          x           x           x         grip          x                                                                     

        chu trình 2
        no13              Cam1          x           x           x           x           x           x           x       cp11                           
        no14              Cam1  cp11    x           x           x           x           x           x           x       cp21
        no15              Cam1  cp21   Cam2         x           x           x           x           x           x       cp31    cp11
        no16              Cam1  cp31   Cam2  cp11   x           x           x           x           x           x       cp41    cp21        
        no17              cam1  cp41   Cam2  cp21   Cam3        Cam4        Cam5        Cam6        x           x       cp51    cp31    cp11    cp6     cp3     cp2     grip(2)
        no18              Cam1  cp51   Cam2  cp31   Cam3        Cam4        Cam5        Cam6     grip()         x       cp61    cp41    cp21    cp11    cp4     cp3     grip(3)
        no19               x    cp61   Cam2  cp41   Cam3        Cam4        Cam5        Cam6     grip()         x               cp51    cp31    cp21    cp5     cp4     grip(4)
        no20               x           Cam2  cp51   Cam3        Cam4        Cam5        Cam6     grip()         x               cp61    cp41    cp31    cp6     cp5     grip(5)
        no21               x            x    cp61   Cam3        Cam4        Cam5        Cam6     grip()         x                       cp51    cp41    cp11    cp6     grip(6)
        no22               x            x           Cam3        Cam4        Cam5        Cam6     grip()         x                       cp61    cp51    cp21    cp11    grip(11) 
        bonus              x            x           x           x           x           x        grip()         x                                                                                                                                                                                                                       
        
        chu trình 3
        no23              Cam1          x           x           x           x           x           x           x       cp12
        no24              Cam1  cp12    x           x           x           x           x           x           x       cp22
        no25              Cam1  cp22    Cam2        x           x           x           x           x           x       cp32    cp12
        no26              Cam1  cp32    Cam2 cp12   x           x           x           x           x           x       cp42    cp22
        no27              Cam1  cp42    Cam2 cp22   Cam3        Cam4        Cam5        Cam6        grip()      x       cp52    cp32    cp12    cp61    cp31    cp21    grip(21)
        no28              Cam1  cp52    Cam2 cp31   Cam3        Cam4        Cam5        Cam6        grip()      x       cp62    cp42    cp22    cp12    cp41    cp31    grip(31)
        no29               x    cp62    Cam2 cp41   Cam3        Cam4        Cam5        Cam6        grip()      x               cp52    cp32    cp22    cp51    cp41    grip(41)
        no30               x            Cam2 cp51   Cam3        Cam4        Cam5        Cam6        grip()      x               cp62    cp42    cp32    cp61    cp51    grip(51)
        no31               x            x    cp61   cam3        Cam4        Cam5        Cam6        grip()      x                       cp52    cp42    cp12    cp61    grip(61)
        no21               x            x           Cam3        Cam4        Cam5        Cam6        grip()      x                       cp62    cp52    cp22    cp12    grip(12)
        */
        private void revcover(string vitriloi,string chuTrinh) 
        {
            if (chuTrinh == "1") 
            {
                if (vitriloi == "1") 
                {
                    if (File.Exists(lan1_vt1)) { File.Delete(lan1_vt1); count_1 = count_1 - 1; }
                    
                                                 
                }
                if (vitriloi == "2") 
                {
                    if (File.Exists(lan1_vt1)) { File.Delete(lan1_vt1); count_1 = count_1 - 1; }                                     
                    if (File.Exists(lan2_vt1)) { File.Delete(lan2_vt1); count_1 = count_1 - 1; }
                  
                }
                if (vitriloi == "3") 
                {
                    if (File.Exists(lan1_vt1)) { File.Delete(lan1_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan2_vt1)) { File.Delete(lan2_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan3_vt1)) { File.Delete(lan3_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan1_vt2)) { File.Delete(lan1_vt2); count_2 = count_2 - 1; }
                }
                if (vitriloi == "4") 
                {
                    if (File.Exists(lan1_vt1)) { File.Delete(lan1_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan2_vt1)) { File.Delete(lan2_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan3_vt1)) { File.Delete(lan3_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan4_vt1)) { File.Delete(lan4_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan1_vt2)) { File.Delete(lan1_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan2_vt2)) { File.Delete(lan2_vt2); count_2 = count_2 - 1; }
                }
                if (vitriloi == "5") 
                {
                    if (File.Exists(lan1_vt1)) { File.Delete(lan1_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan2_vt1)) { File.Delete(lan2_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan3_vt1)) { File.Delete(lan3_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan4_vt1)) { File.Delete(lan4_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan5_vt1)) { File.Delete(lan5_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan1_vt2)) { File.Delete(lan1_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan2_vt2)) { File.Delete(lan2_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan3_vt2)) { File.Delete(lan3_vt2); count_2 = count_2 - 1; }

                    if (File.Exists(lan1_vt3)) { File.Delete(lan1_vt3); count_3 = count_3 - 1; }
                   

                }
                if (vitriloi == "6")
                {
                    if (File.Exists(lan1_vt1)) { File.Delete(lan1_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan2_vt1)) { File.Delete(lan2_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan3_vt1)) { File.Delete(lan3_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan4_vt1)) { File.Delete(lan4_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan5_vt1)) { File.Delete(lan5_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan6_vt1)) { File.Delete(lan6_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan1_vt2)) { File.Delete(lan1_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan2_vt2)) { File.Delete(lan2_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan3_vt2)) { File.Delete(lan3_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan4_vt2)) { File.Delete(lan4_vt2); count_2 = count_2 - 1; }

                    if (File.Exists(lan1_vt3)) { File.Delete(lan1_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan2_vt3)) { File.Delete(lan1_vt3); count_3 = count_3 - 1; }

                    if (File.Exists(lan1_vt4)) { File.Delete(lan1_vt4); count_4 = count_4 - 1; }
                }
                if (vitriloi == "7") 
                {
                    if (File.Exists(lan1_vt1)) { File.Delete(lan1_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan2_vt1)) { File.Delete(lan2_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan3_vt1)) { File.Delete(lan3_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan4_vt1)) { File.Delete(lan4_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan5_vt1)) { File.Delete(lan5_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan6_vt1)) { File.Delete(lan6_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan1_vt2)) { File.Delete(lan1_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan2_vt2)) { File.Delete(lan2_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan3_vt2)) { File.Delete(lan3_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan4_vt2)) { File.Delete(lan4_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan5_vt2)) { File.Delete(lan5_vt2); count_2 = count_2 - 1; }

                    if (File.Exists(lan1_vt3)) { File.Delete(lan1_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan2_vt3)) { File.Delete(lan2_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan3_vt3)) { File.Delete(lan3_vt3); count_3 = count_3 - 1; }

                    if (File.Exists(lan1_vt4)) { File.Delete(lan1_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan2_vt4)) { File.Delete(lan2_vt4); count_4 = count_4 - 1; }

                }
                if (vitriloi == "8") 
                {
                    if (File.Exists(lan1_vt1)) { File.Delete(lan1_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan2_vt1)) { File.Delete(lan2_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan3_vt1)) { File.Delete(lan3_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan4_vt1)) { File.Delete(lan4_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan5_vt1)) { File.Delete(lan5_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan6_vt1)) { File.Delete(lan6_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan1_vt2)) { File.Delete(lan1_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan2_vt2)) { File.Delete(lan2_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan3_vt2)) { File.Delete(lan3_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan4_vt2)) { File.Delete(lan4_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan5_vt2)) { File.Delete(lan5_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan6_vt2)) { File.Delete(lan6_vt2); count_2 = count_2 - 1; }


                    if (File.Exists(lan1_vt3)) { File.Delete(lan1_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan2_vt3)) { File.Delete(lan2_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan3_vt3)) { File.Delete(lan3_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan4_vt3)) { File.Delete(lan4_vt3); count_3 = count_3 - 1; }


                    if (File.Exists(lan1_vt4)) { File.Delete(lan1_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan2_vt4)) { File.Delete(lan2_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan3_vt4)) { File.Delete(lan3_vt4); count_4 = count_4 - 1; }
                }
                if (vitriloi == "9") 
                {
                    if (File.Exists(lan1_vt1)) { File.Delete(lan1_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan2_vt1)) { File.Delete(lan2_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan3_vt1)) { File.Delete(lan3_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan4_vt1)) { File.Delete(lan4_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan5_vt1)) { File.Delete(lan5_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan6_vt1)) { File.Delete(lan6_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan1_vt2)) { File.Delete(lan1_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan2_vt2)) { File.Delete(lan2_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan3_vt2)) { File.Delete(lan3_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan4_vt2)) { File.Delete(lan4_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan5_vt2)) { File.Delete(lan5_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan6_vt2)) { File.Delete(lan6_vt2); count_2 = count_2 - 1; }


                    if (File.Exists(lan1_vt3)) { File.Delete(lan1_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan2_vt3)) { File.Delete(lan2_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan3_vt3)) { File.Delete(lan3_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan4_vt3)) { File.Delete(lan4_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan5_vt3)) { File.Delete(lan5_vt3); count_3 = count_3 - 1; }

                    if (File.Exists(lan1_vt4)) { File.Delete(lan1_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan2_vt4)) { File.Delete(lan2_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan3_vt4)) { File.Delete(lan3_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan4_vt4)) { File.Delete(lan4_vt4); count_4 = count_4 - 1; }

                    if (File.Exists(lan1_vt5)) { File.Delete(lan1_vt5); count_5 = count_5 - 1; }
                }
                if (vitriloi == "10") 
                {
                    if (File.Exists(lan1_vt1)) { File.Delete(lan1_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan2_vt1)) { File.Delete(lan2_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan3_vt1)) { File.Delete(lan3_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan4_vt1)) { File.Delete(lan4_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan5_vt1)) { File.Delete(lan5_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan6_vt1)) { File.Delete(lan6_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan1_vt2)) { File.Delete(lan1_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan2_vt2)) { File.Delete(lan2_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan3_vt2)) { File.Delete(lan3_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan4_vt2)) { File.Delete(lan4_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan5_vt2)) { File.Delete(lan5_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan6_vt2)) { File.Delete(lan6_vt2); count_2 = count_2 - 1; }


                    if (File.Exists(lan1_vt3)) { File.Delete(lan1_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan2_vt3)) { File.Delete(lan2_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan3_vt3)) { File.Delete(lan3_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan4_vt3)) { File.Delete(lan4_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan5_vt3)) { File.Delete(lan5_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan6_vt3)) { File.Delete(lan6_vt3); count_3 = count_3 - 1; }

                    if (File.Exists(lan1_vt4)) { File.Delete(lan1_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan2_vt4)) { File.Delete(lan2_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan3_vt4)) { File.Delete(lan3_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan4_vt4)) { File.Delete(lan4_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan5_vt4)) { File.Delete(lan5_vt4); count_4 = count_4 - 1; }

                    if (File.Exists(lan1_vt5)) { File.Delete(lan1_vt5); count_5 = count_5 - 1; }
                    if (File.Exists(lan2_vt5)) { File.Delete(lan2_vt5); count_5 = count_5 - 1; }

                    if (File.Exists(lan1_vt6)) { File.Delete(lan1_vt6); count_6 = count_6 - 1; }
                }                     
            }
            if(chuTrinh == "2") 
            {      
                if(vitriloi == "1") 
                {
                    if (File.Exists(lan12_vt1)) { File.Delete(lan12_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan2_vt1)) { File.Delete(lan2_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan3_vt1)) { File.Delete(lan3_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan4_vt1)) { File.Delete(lan4_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan5_vt1)) { File.Delete(lan5_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan6_vt1)) { File.Delete(lan6_vt1); count_1 = count_1 - 1; }
                 
                    if (File.Exists(lan2_vt2)) { File.Delete(lan2_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan3_vt2)) { File.Delete(lan3_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan4_vt2)) { File.Delete(lan4_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan5_vt2)) { File.Delete(lan5_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan6_vt2)) { File.Delete(lan6_vt2); count_2 = count_2 - 1; }

                    if (File.Exists(lan2_vt3)) { File.Delete(lan2_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan3_vt3)) { File.Delete(lan3_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan4_vt3)) { File.Delete(lan4_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan5_vt3)) { File.Delete(lan5_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan6_vt3)) { File.Delete(lan6_vt3); count_3 = count_3 - 1; }

                    if (File.Exists(lan2_vt4)) { File.Delete(lan2_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan3_vt4)) { File.Delete(lan3_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan4_vt4)) { File.Delete(lan4_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan5_vt4)) { File.Delete(lan5_vt4); count_4 = count_4 - 1; }

                    if (File.Exists(lan2_vt5)) { File.Delete(lan2_vt5); count_5 = count_5 - 1; }
                }
                if(vitriloi == "2") 
                {
                    if (File.Exists(lan12_vt1)) { File.Delete(lan12_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan22_vt1)) { File.Delete(lan22_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan2_vt1)) { File.Delete(lan2_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan3_vt1)) { File.Delete(lan3_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan4_vt1)) { File.Delete(lan4_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan5_vt1)) { File.Delete(lan5_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan6_vt1)) { File.Delete(lan6_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan2_vt2)) { File.Delete(lan2_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan3_vt2)) { File.Delete(lan3_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan4_vt2)) { File.Delete(lan4_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan5_vt2)) { File.Delete(lan5_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan6_vt2)) { File.Delete(lan6_vt2); count_2 = count_2 - 1; }

                    if (File.Exists(lan2_vt3)) { File.Delete(lan2_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan3_vt3)) { File.Delete(lan3_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan4_vt3)) { File.Delete(lan4_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan5_vt3)) { File.Delete(lan5_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan6_vt3)) { File.Delete(lan6_vt3); count_3 = count_3 - 1; }

                    if (File.Exists(lan2_vt4)) { File.Delete(lan2_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan3_vt4)) { File.Delete(lan3_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan4_vt4)) { File.Delete(lan4_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan5_vt4)) { File.Delete(lan5_vt4); count_4 = count_4 - 1; }

                    if (File.Exists(lan2_vt5)) { File.Delete(lan2_vt5); count_5 = count_5 - 1; }
                }
                if(vitriloi == "3") 
                {
                    if (File.Exists(lan12_vt1)) { File.Delete(lan12_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan22_vt1)) { File.Delete(lan22_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan32_vt1)) { File.Delete(lan32_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan12_vt2)) { File.Delete(lan12_vt2); count_2 = count_2 - 1; }

                    if (File.Exists(lan2_vt1)) { File.Delete(lan2_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan3_vt1)) { File.Delete(lan3_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan4_vt1)) { File.Delete(lan4_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan5_vt1)) { File.Delete(lan5_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan6_vt1)) { File.Delete(lan6_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan2_vt2)) { File.Delete(lan2_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan3_vt2)) { File.Delete(lan3_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan4_vt2)) { File.Delete(lan4_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan5_vt2)) { File.Delete(lan5_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan6_vt2)) { File.Delete(lan6_vt2); count_2 = count_2 - 1; }

                    if (File.Exists(lan2_vt3)) { File.Delete(lan2_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan3_vt3)) { File.Delete(lan3_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan4_vt3)) { File.Delete(lan4_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan5_vt3)) { File.Delete(lan5_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan6_vt3)) { File.Delete(lan6_vt3); count_3 = count_3 - 1; }

                    if (File.Exists(lan2_vt4)) { File.Delete(lan2_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan3_vt4)) { File.Delete(lan3_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan4_vt4)) { File.Delete(lan4_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan5_vt4)) { File.Delete(lan5_vt4); count_4 = count_4 - 1; }

                    if (File.Exists(lan2_vt5)) { File.Delete(lan2_vt5); count_5 = count_5 - 1; }
                }
                if(vitriloi == "4")
                {
                    if (File.Exists(lan12_vt1)) { File.Delete(lan12_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan22_vt1)) { File.Delete(lan22_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan32_vt1)) { File.Delete(lan32_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan42_vt1)) { File.Delete(lan42_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan12_vt2)) { File.Delete(lan12_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan22_vt2)) { File.Delete(lan22_vt2); count_2 = count_2 - 1; }

                    if (File.Exists(lan2_vt1)) { File.Delete(lan2_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan3_vt1)) { File.Delete(lan3_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan4_vt1)) { File.Delete(lan4_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan5_vt1)) { File.Delete(lan5_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan6_vt1)) { File.Delete(lan6_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan2_vt2)) { File.Delete(lan2_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan3_vt2)) { File.Delete(lan3_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan4_vt2)) { File.Delete(lan4_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan5_vt2)) { File.Delete(lan5_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan6_vt2)) { File.Delete(lan6_vt2); count_2 = count_2 - 1; }

                    if (File.Exists(lan2_vt3)) { File.Delete(lan2_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan3_vt3)) { File.Delete(lan3_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan4_vt3)) { File.Delete(lan4_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan5_vt3)) { File.Delete(lan5_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan6_vt3)) { File.Delete(lan6_vt3); count_3 = count_3 - 1; }

                    if (File.Exists(lan2_vt4)) { File.Delete(lan2_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan3_vt4)) { File.Delete(lan3_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan4_vt4)) { File.Delete(lan4_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan5_vt4)) { File.Delete(lan5_vt4); count_4 = count_4 - 1; }

                    if (File.Exists(lan2_vt5)) { File.Delete(lan2_vt5); count_5 = count_5 - 1; }
                }
                if (vitriloi == "5")
                {
                    if (File.Exists(lan12_vt1)) { File.Delete(lan12_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan22_vt1)) { File.Delete(lan22_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan32_vt1)) { File.Delete(lan32_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan42_vt1)) { File.Delete(lan42_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan52_vt1)) { File.Delete(lan52_vt1); count_1 = count_1 - 1; }
                 

                    if (File.Exists(lan12_vt2)) { File.Delete(lan12_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan22_vt2)) { File.Delete(lan22_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan32_vt2)) { File.Delete(lan32_vt2); count_2 = count_2 - 1; }


                    if (File.Exists(lan3_vt1)) { File.Delete(lan3_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan4_vt1)) { File.Delete(lan4_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan5_vt1)) { File.Delete(lan5_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan6_vt1)) { File.Delete(lan6_vt1); count_1 = count_1 - 1; }


                    if (File.Exists(lan3_vt2)) { File.Delete(lan3_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan4_vt2)) { File.Delete(lan4_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan5_vt2)) { File.Delete(lan5_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan6_vt2)) { File.Delete(lan6_vt2); count_2 = count_2 - 1; }

                    if (File.Exists(lan12_vt3)) { File.Delete(lan12_vt3); count_3 = count_3 - 1; }
                  

                    if (File.Exists(lan3_vt3)) { File.Delete(lan3_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan4_vt3)) { File.Delete(lan4_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan5_vt3)) { File.Delete(lan5_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan6_vt3)) { File.Delete(lan6_vt3); count_3 = count_3 - 1; }


                    if (File.Exists(lan3_vt4)) { File.Delete(lan3_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan4_vt4)) { File.Delete(lan4_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan5_vt4)) { File.Delete(lan5_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan6_vt4)) { File.Delete(lan6_vt4); count_4 = count_4 - 1; }

                    if (File.Exists(lan3_vt5)) { File.Delete(lan3_vt5); count_5 = count_5 - 1; }
                   
                }
                if (vitriloi == "6")
                {
                    if (File.Exists(lan12_vt1)) { File.Delete(lan12_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan22_vt1)) { File.Delete(lan22_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan32_vt1)) { File.Delete(lan32_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan42_vt1)) { File.Delete(lan42_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan52_vt1)) { File.Delete(lan52_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan62_vt1)) { File.Delete(lan62_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan12_vt2)) { File.Delete(lan12_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan22_vt2)) { File.Delete(lan22_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan32_vt2)) { File.Delete(lan32_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan42_vt2)) { File.Delete(lan42_vt2); count_2 = count_2 - 1; }

                   
                    if (File.Exists(lan4_vt1)) { File.Delete(lan4_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan5_vt1)) { File.Delete(lan5_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan6_vt1)) { File.Delete(lan6_vt1); count_1 = count_1 - 1; }


                   
                    if (File.Exists(lan4_vt2)) { File.Delete(lan4_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan5_vt2)) { File.Delete(lan5_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan6_vt2)) { File.Delete(lan6_vt2); count_2 = count_2 - 1; }

                    if (File.Exists(lan12_vt3)) { File.Delete(lan12_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan22_vt3)) { File.Delete(lan22_vt3); count_3 = count_3 - 1; }

                  
                    if (File.Exists(lan4_vt3)) { File.Delete(lan4_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan5_vt3)) { File.Delete(lan5_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan6_vt3)) { File.Delete(lan6_vt3); count_3 = count_3 - 1; }

                    if (File.Exists(lan12_vt4)) { File.Delete(lan12_vt4); count_4 = count_4 - 1; }
                 
               
                    if (File.Exists(lan4_vt4)) { File.Delete(lan4_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan5_vt4)) { File.Delete(lan5_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan6_vt4)) { File.Delete(lan6_vt4); count_4 = count_4 - 1; }

                    if (File.Exists(lan4_vt5)) { File.Delete(lan4_vt5); count_5 = count_5 - 1; }
                }
                if (vitriloi == "7")
                {
                    if (File.Exists(lan12_vt1)) { File.Delete(lan12_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan22_vt1)) { File.Delete(lan22_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan32_vt1)) { File.Delete(lan32_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan42_vt1)) { File.Delete(lan42_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan52_vt1)) { File.Delete(lan52_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan62_vt1)) { File.Delete(lan62_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan12_vt2)) { File.Delete(lan12_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan22_vt2)) { File.Delete(lan22_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan32_vt2)) { File.Delete(lan32_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan42_vt2)) { File.Delete(lan42_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan52_vt2)) { File.Delete(lan52_vt2); count_2 = count_2 - 1; }

                 
                    if (File.Exists(lan5_vt1)) { File.Delete(lan5_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan6_vt1)) { File.Delete(lan6_vt1); count_1 = count_1 - 1; }


                   
                    if (File.Exists(lan5_vt2)) { File.Delete(lan5_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan6_vt2)) { File.Delete(lan6_vt2); count_2 = count_2 - 1; }

                    if (File.Exists(lan12_vt3)) { File.Delete(lan12_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan22_vt3)) { File.Delete(lan22_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan32_vt3)) { File.Delete(lan32_vt3); count_3 = count_3 - 1; }

                 
                    if (File.Exists(lan5_vt3)) { File.Delete(lan5_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan6_vt3)) { File.Delete(lan6_vt3); count_3 = count_3 - 1; }

                    if (File.Exists(lan12_vt4)) { File.Delete(lan12_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan22_vt4)) { File.Delete(lan22_vt4); count_4 = count_4 - 1; }


                    if (File.Exists(lan5_vt4)) { File.Delete(lan5_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan6_vt4)) { File.Delete(lan6_vt4); count_4 = count_4 - 1; }

                    if (File.Exists(lan5_vt5)) { File.Delete(lan5_vt5); count_5 = count_5 - 1; }
                }
                if (vitriloi == "8")
                {
                    if (File.Exists(lan12_vt1)) { File.Delete(lan12_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan22_vt1)) { File.Delete(lan22_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan32_vt1)) { File.Delete(lan32_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan42_vt1)) { File.Delete(lan42_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan52_vt1)) { File.Delete(lan52_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan62_vt1)) { File.Delete(lan62_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan12_vt2)) { File.Delete(lan12_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan22_vt2)) { File.Delete(lan22_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan32_vt2)) { File.Delete(lan32_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan42_vt2)) { File.Delete(lan42_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan52_vt2)) { File.Delete(lan52_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan62_vt2)) { File.Delete(lan62_vt2); count_2 = count_2 - 1; }

                 
                    if (File.Exists(lan6_vt1)) { File.Delete(lan6_vt1); count_1 = count_1 - 1; }

                 
                    if (File.Exists(lan6_vt2)) { File.Delete(lan6_vt2); count_2 = count_2 - 1; }

                    if (File.Exists(lan12_vt3)) { File.Delete(lan12_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan22_vt3)) { File.Delete(lan22_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan32_vt3)) { File.Delete(lan32_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan42_vt3)) { File.Delete(lan42_vt3); count_3 = count_3 - 1; }


                    if (File.Exists(lan6_vt3)) { File.Delete(lan6_vt3); count_3 = count_3 - 1; }

                    if (File.Exists(lan12_vt4)) { File.Delete(lan12_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan22_vt4)) { File.Delete(lan22_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan32_vt4)) { File.Delete(lan32_vt4); count_4 = count_4 - 1; }


                    if (File.Exists(lan6_vt4)) { File.Delete(lan6_vt4); count_4 = count_4 - 1; }

                    if (File.Exists(lan6_vt5)) { File.Delete(lan6_vt5); count_5 = count_5 - 1; }
                }
                if (vitriloi == "9")
                {
                    if (File.Exists(lan12_vt1)) { File.Delete(lan12_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan22_vt1)) { File.Delete(lan22_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan32_vt1)) { File.Delete(lan32_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan42_vt1)) { File.Delete(lan42_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan52_vt1)) { File.Delete(lan52_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan62_vt1)) { File.Delete(lan62_vt1); count_1 = count_1 - 1; }

                    if (File.Exists(lan12_vt2)) { File.Delete(lan12_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan22_vt2)) { File.Delete(lan22_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan32_vt2)) { File.Delete(lan32_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan42_vt2)) { File.Delete(lan42_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan52_vt2)) { File.Delete(lan52_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan62_vt2)) { File.Delete(lan62_vt2); count_2 = count_2 - 1; }




                    if (File.Exists(lan12_vt3)) { File.Delete(lan12_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan22_vt3)) { File.Delete(lan22_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan32_vt3)) { File.Delete(lan32_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan42_vt3)) { File.Delete(lan42_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan52_vt3)) { File.Delete(lan52_vt3); count_3 = count_3 - 1; }


                    if (File.Exists(lan12_vt4)) { File.Delete(lan12_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan22_vt4)) { File.Delete(lan22_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan32_vt4)) { File.Delete(lan32_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan42_vt4)) { File.Delete(lan42_vt4); count_4 = count_4 - 1; }

                    if (File.Exists(lan12_vt5)) { File.Delete(lan12_vt5); count_5 = count_5 - 1; }
                }
                if (vitriloi == "10")
                {
                    
                    if (File.Exists(lan22_vt1)) { File.Delete(lan22_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan32_vt1)) { File.Delete(lan32_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan42_vt1)) { File.Delete(lan42_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan52_vt1)) { File.Delete(lan52_vt1); count_1 = count_1 - 1; }
                    if (File.Exists(lan62_vt1)) { File.Delete(lan62_vt1); count_1 = count_1 - 1; }

                   
                    if (File.Exists(lan22_vt2)) { File.Delete(lan22_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan32_vt2)) { File.Delete(lan32_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan42_vt2)) { File.Delete(lan42_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan52_vt2)) { File.Delete(lan52_vt2); count_2 = count_2 - 1; }
                    if (File.Exists(lan62_vt2)) { File.Delete(lan62_vt2); count_2 = count_2 - 1; }




                  
                    if (File.Exists(lan22_vt3)) { File.Delete(lan22_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan32_vt3)) { File.Delete(lan32_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan42_vt3)) { File.Delete(lan42_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan52_vt3)) { File.Delete(lan52_vt3); count_3 = count_3 - 1; }
                    if (File.Exists(lan62_vt3)) { File.Delete(lan62_vt3); count_3 = count_3 - 1; }

                   
                    if (File.Exists(lan22_vt4)) { File.Delete(lan22_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan32_vt4)) { File.Delete(lan32_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan42_vt4)) { File.Delete(lan42_vt4); count_4 = count_4 - 1; }
                    if (File.Exists(lan52_vt4)) { File.Delete(lan52_vt4); count_4 = count_4 - 1; }

                    if (File.Exists(lan22_vt5)) { File.Delete(lan22_vt5); count_5 = count_5 - 1; }
                }
            }
        }


        #endregion
        private void dung_may() 
        {
            MethodInvoker inv = delegate
            {
                if (_sum % 5 == 0)
                {
                    PLCS7_1200.Write("M232.3", true);
                }
            };
            this.Invoke(inv);
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            null_pic1();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            null_pic2();
        }
    }
    
}

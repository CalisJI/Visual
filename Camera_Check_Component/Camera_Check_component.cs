using System;
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Data.SqlClient;

namespace Camera_Check_Component
{
    public partial class Camera_Check_component : Form
    {
        
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
        private Timer timer = new Timer();
        private double startPR_Count = 0;
        private double timer_sum = 0;
        private double timer_star = 0;
        private int count_1 = 0;
        private int count_2 = 0;
        private int count_3 = 0;
        private int count_4 = 0;
        private int count_5 = 0;
        private int count_6 = 0;
        private int count_7 = 0;
        private int folderIndex = 0;
        private int load1 = 0;
        private int load2 = 0;
        bool started = false;
        double ratio;
        int stt = 0;
        string DMY = "";
        public Camera_Check_component()
        {
            InitializeComponent();
        }
        private void Camera_Check_component_Load(object sender, EventArgs e)
        {
            this.Location = new System.Drawing.Point(0, 0);
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;

            //unable();
            listviewInit();

            DateTime dt = DateTime.Now;
            string daytime = dt.Day.ToString() + "-" + dt.Month.ToString() + "-" + dt.Year.ToString();
            DMY = daytime;
            system_config = Program_Configuration.GetSystem_Config();




            if (system_config.new_Day != dt.Day || system_config.new_Month != dt.Month)

            {
                count_1 = 0; count_2 = 0; count_3 = 0; count_4 = 0; count_5 = 0; count_6 = 0; count_7 = 0; folderIndex = 0;load1 = 0;load2 = 1;
                Program_Configuration.UpdateSystem_Config("Folder_index_tranfer", load1.ToString());
                Program_Configuration.UpdateSystem_Config("Folder_load_check", load2.ToString());
                Program_Configuration.UpdateSystem_Config("same_folder_1", folderIndex.ToString());

                //Program_Configuration.UpdateSystem_Config("Folder_index_tranfer", folderIndex.ToString());
                //Program_Configuration.UpdateSystem_Config("Folder_load_check", folderIndex.ToString());
                Program_Configuration.UpdateSystem_Config("Location_cam1_folder", count_1.ToString());
                Program_Configuration.UpdateSystem_Config("Location_cam2_folder", count_2.ToString());
                Program_Configuration.UpdateSystem_Config("Location_cam3_folder", count_3.ToString());
                Program_Configuration.UpdateSystem_Config("Location_cam4_folder", count_4.ToString());
                Program_Configuration.UpdateSystem_Config("Location_cam5_folder", count_5.ToString());
                Program_Configuration.UpdateSystem_Config("Location_cam6_folder", count_6.ToString());
                Program_Configuration.UpdateSystem_Config("Location_cam7_folder", count_7.ToString());
                Program_Configuration.UpdateSystem_Config("new_Day", dt.Day.ToString());
                Program_Configuration.UpdateSystem_Config("new_Month", dt.Month.ToString());
                Program_Configuration.UpdateSystem_Config("new_Year", dt.Year.ToString());
            }
            else if (count_1 != system_config.Location_cam1_folder || count_2 != system_config.Location_cam2_folder || count_3 != system_config.Location_cam3_folder || count_4 != system_config.Location_cam4_folder || count_5 != system_config.Location_cam5_folder || count_6 != system_config.Location_cam6_folder || count_7 != system_config.Location_cam7_folder || load1 != system_config.Folder_index_tranfer || load2 != system_config.Folder_load_check || folderIndex != system_config.same_folder_1) 
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

            //this.AutoSize = true;
            //this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Start_btn.Enabled = true;
            Stop_btn.Enabled = false;
            //Manual_btn.Enabled = false;
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
            #region // khai báo background worker
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
            #endregion
            Program_Configuration.UpdateSystem_Config("Location_cam1_folder", count_1.ToString());
            system_config.Location_cam1_folder = Convert.ToInt32(Program_Configuration.GetSystem_Config_Value("Location_cam1_folder"));

            Parameter_app.OK_TEMP(daytime, system_config.Location_cam1_folder.ToString());
            Parameter_app.ERROR_TEMP(daytime, system_config.Location_cam1_folder.ToString());
            //label_time.Text = DateTime.Now.ToString();
            if (system_config.inf_process == null)
            {
                TB_LTdate.Text = "";
            }
            else
            {
                TB_LTdate.Text = system_config.inf_process.ToString();
            }
            //set_up();
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Enabled = true;
            timer.Start();
            tb_PN.SelectedIndex = 0;
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
        bool mem7 = false;
        private void BackgroundWorker_7_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!backgroundWorker_7.IsBusy && system_config.add_cam == "true")
            {
                set_up();
            }
            status(" [SYSTEM]" + " CAM 7 Save image" + " " + count_7.ToString());
            count_7++;

        }
        private void BackgroundWorker_7_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker_7.CancellationPending)
            {
                e.Cancel = true;
            }

            if (mem7 && system_config.add_cam == "true")
            {

                DateTime date = DateTime.Now;
                date.ToString("HH:MM:ss");
                set_up();
                MethodInvoker inv = delegate
                {
                    string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-7" + "-" + count_7.ToString() + ".jpeg";                  
                    string outputFileName = system_config.Map_Path_File + @"\" + str + "";
                    using (MemoryStream memory = new MemoryStream())
                    {
                        using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                        {
                            Live_Cam_7.Save(memory, ImageFormat.Jpeg);
                            byte[] bytes = memory.ToArray();
                            fs.Write(bytes, 0, bytes.Length);
                            fs.Dispose();
                        }
                    }
                    Live_Cam_7.Dispose();
                    order_7 = false;
                   
                }; this.Invoke(inv);

            }
        }
        void backgroundWorker_6_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!backgroundWorker_6.IsBusy)
            {
                set_up();
            }
            status(" [SYSTEM]" + " CAM 6 Save image" + " " + count_6.ToString());

            panel6.BackColor = Color.Black;

            count_6++;
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
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        //bmp1.Save(memory, ImageFormat.Jpeg);
                        Live_Cam_6.Save(memory, ImageFormat.Jpeg);
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
            if (!backgroundWorker_5.IsBusy)
            {
                set_up();
            }
            status(" [SYSTEM]" + " CAM 5 Save image" + " " + count_5.ToString());

            panel5.BackColor = Color.Black;

            count_5++;
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
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {

                        Live_Cam_5.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
               Live_Cam_5.Dispose();
                order_5 = false;

            }; this.Invoke(inv);
        }
        void backgroundWorker_4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!backgroundWorker_4.IsBusy)
            {
                set_up();
            }
            status(" [SYSTEM]" + " CAM 4 Save image" + " " + count_4.ToString());

            panel4.BackColor = Color.Black;

            count_4++;
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

                //string outputFileName = Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\" + str + "";
                string outputFileName = system_config.Map_Path_File + @"\" + str + "";
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        //bmp1.Save(memory, ImageFormat.Jpeg);
                        Live_Cam_4.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                Live_Cam_4.Dispose();
                order_4 = false;

            }; this.Invoke(inv);
        }

        void backgroundWorker_3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!backgroundWorker_3.IsBusy)
            {
                set_up();
            }
            status(" [SYSTEM]" + " CAM 3 Save image" + " " + count_3.ToString());

            panel3.BackColor = Color.Black;

            count_3++;
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
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        Live_Cam_3.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                Live_Cam_3.Dispose();
                order_3 = false;
            }; this.Invoke(inv);
        }

        void backgroundWorker_2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            status(" [SYSTEM]" + " CAM 2 Save image" + " " + count_2.ToString());
            panel2.BackColor = Color.Black;
            count_2++;
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
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        //bmp1.Save(memory, ImageFormat.Jpeg);
                        Live_Cam_2.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                //bmp1.Dispose();
                Live_Cam_2.Dispose();
                order_2 = false;

            }; this.Invoke(inv);
        }
        void backgroundWorker_1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            status(" [SYSTEM]" + " CAM 1 Save image" + " " + count_1.ToString());
            count_1++;
            panel1.BackColor = Color.Black;

        }
        void backgroundWorker_1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker_1.CancellationPending)
            {
                e.Cancel = true;
            }

            panel1.BackColor = Color.GreenYellow;
            DateTime date = DateTime.Now;
            date.ToString("HH:MM:ss");
            // Parameter_app.TEMP(system_config.new_Day, system_config.new_Month, system_config.new_Year, system_config.Location_cam1_folder.ToString());
            set_up();
            MethodInvoker inv = delegate
            {

                string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-1" + "-" + count_1.ToString() + ".jpeg";
                //string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-1" + "-" + system_config.Location_cam1_folder.ToString() + ".jpeg";
                string outputFileName = system_config.Map_Path_File + @"\" + str + "";
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        //bmp1.Save(memory, ImageFormat.Jpeg);
                        Live_Cam_1.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }

                Live_Cam_1.Dispose();
                order_1 = false;

            }; this.Invoke(inv);

        }
        private void set_up()
        {

            if (!Directory.Exists(Parameter_app.IMAGE_FOLDER_PATH))
            {
                Directory.CreateDirectory(Parameter_app.IMAGE_FOLDER_PATH);
            }
            if (!Directory.Exists(Parameter_app.OK_IMAGE_FOLDER_PATH) && allow_check)
            {
                Directory.CreateDirectory(Parameter_app.OK_IMAGE_FOLDER_PATH);
            }
            if (!Directory.Exists(Parameter_app.ERROR_IMAGE_FOLDER_PATH) && allow_check)
            {
                Directory.CreateDirectory(Parameter_app.ERROR_IMAGE_FOLDER_PATH);
            }
        }

        private void Start_btn_Click(object sender, EventArgs e)
        {
            system_config = Program_Configuration.GetSystem_Config();
            Star_program();
        }

        string ID_Operator1 = "";
        string ID_Operator2 = "";
        string PN_Selector = "";
        private void Star_program()
        {
            ID_Operator1 = TB_idworker.Text;
            ID_Operator2 = TB_wker2.Text;
            PN_Selector = tb_PN.Text;
            tb_PN.Enabled = false;
            TB_idworker.Enabled = false;
            TB_wker2.Enabled = false;
            
            system_config = Program_Configuration.GetSystem_Config();
            if (tb_PN.Text == "" || TB_idworker.Text == "" || TB_wker2.Text == "")
            {
                MessageBox.Show("DO NOT HAVE PN Selector or ID Worker");
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
                // serialPort_communicate.DataReceived += serialPort_communicate_DataReceived;
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
            //Manual_btn.Enabled = true;
            start_check = true;
            started = true;
            status("[START]" + "Program has been started");


        }

       
        string loi_tam1 = "";
        string loi_tam2 = "";
 
        private void serialPort_communicate_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string shot1 = "";
            string shot2 = "";
            string shot3 = "";
            string shot4 = "";
            string shot5 = "";
            string shot6 = "";
            string shot7 = "";
            string[] shot = new string[7];
            string[] NG_code = new string[3];
            string cap_order = serialPort_communicate.ReadExisting();

            if (cap_order.Contains("."))
            {
                shot = cap_order.Split('.');
                shot1 = shot[0];
                shot2 = shot[1];
                shot3 = shot[2];
                shot4 = shot[3];
                shot5 = shot[4];
                shot6 = shot[5];
                shot7 = shot[6];
                if (shot1 == "1") Take_Photo("1");
                if (shot2 == "1") Take_Photo("2");
                if (shot3 == "1") Take_Photo("3");
                if (shot4 == "1") Take_Photo("4");
                if (shot5 == "1") Take_Photo("5");
                if (shot6 == "1") Take_Photo("6");
                if (shot7 == "1") Take_Photo("7");
            }
            if (cap_order == "OK1"  && allow_check)
            {
                if (on1 != 1)
                {
                    OK1_check();
                }
                else
                {
                    MessageBox.Show("The Zoom processing is running, please turn off first");
                }
            }
            if (cap_order == "OK2" && allow_check)
            {
                if (on2 != 1)
                {
                    OK2_check();
                }
                else
                {
                    MessageBox.Show(" Zoom processing is running, please turn off first");
                }
            }
            if (cap_order.Contains("#")) 
            {
                NG_code = cap_order.Split('#');
                if (NG_code[0] == "NG1" && allow_check ) 
                {
                    if (on1 != 1) 
                    {
                        //error_Type(NG_code[1]);
                        loi_tam1 = NG_code[1];
                        vitri_Erpic(NG_code[2]);
                        NG1_check();
                    }
                    else
                    {
                        MessageBox.Show(" Zoom processing is running, please turn off first");
                    }
                }
                if (NG_code[0] == "NG2" && allow_check) 
                {
                    if (on2!=1) 
                    {
                        //error_Type(NG_code[1]);
                        loi_tam2 = NG_code[1];
                        vitri_Erpic(NG_code[2]);
                        NG2_check();
                    }
                    else
                    {
                        MessageBox.Show("The Zoom processing is running, please turn off first");
                    }
                }
            }
            if (cap_order == "Z11" && allow_check)
            {
                zoom1(0);
            }
            if (cap_order == "Z12" && allow_check)
            {
                zoom1(1);
            }
            if (cap_order == "Z13" && allow_check)
            {
                zoom1(2);
            }
            if (cap_order == "Z14" && allow_check)
            {
                zoom1(3);
            }
            if (cap_order == "Z15" && allow_check)
            {
                zoom1(4);
            }
            if (cap_order == "Z16" && allow_check)
            {
                zoom1(5);
            }
            if (cap_order == "Z21" && allow_check)
            {
                zoom2(0);
            }
            if (cap_order == "Z22" && allow_check)
            {
                zoom2(1);
            }
            if (cap_order == "Z23" && allow_check)
            {
                zoom2(2);
            }
            if (cap_order == "Z24" && allow_check)
            {
                zoom2(3);
            }
            if (cap_order == "Z25" && allow_check)
            {
                zoom2(4);
            }
            if (cap_order == "Z26" && allow_check)
            {
                zoom2(5);
            }        
        }

        private void Take_Photo(string order)
        {
            MethodInvoker inv = delegate
            {
                if (order == "1") order_1 = true;
                if (order == "2") order_2 = true;
                if (order == "3") order_3 = true;
                if (order == "4") order_4 = true;
                if (order == "5") order_5 = true;
                if (order == "6") order_6 = true;
                if (order == "7") order_7 = true;

                if (order_1)
                {
                    Cam1VIDEO_Device.Start();
                    System.Threading.Thread.Sleep(10);
                }              
                if (order_2)
                {
                    Cam2VIDEO_Device.Start();
                    System.Threading.Thread.Sleep(10);
                }
                if (order_3)
                {
                    Cam3VIDEO_Device.Start();
                    System.Threading.Thread.Sleep(10);
                }
                if (order_4)
                {
                    Cam4VIDEO_Device.Start();
                    System.Threading.Thread.Sleep(10);
                }
                if (order_5)
                {
                    Cam5VIDEO_Device.Start();
                    System.Threading.Thread.Sleep(10);
                }
                if (order_6)
                {
                    Cam6VIDEO_Device.Start();
                    System.Threading.Thread.Sleep(10);
                }
                if(order_7 && system_config.add_cam == "true")
                {
                    Cam7VIDEO_Device.Start();
                    System.Threading.Thread.Sleep(10);
                }
            };
            this.Invoke(inv);
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
            Cam7VIDEO_Device.SignalToStop();
        }
        void Cam6VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (order_6)
            {
                Live_Cam_6 = (Bitmap)eventArgs.Frame.Clone();
                if (!backgroundWorker_6.IsBusy) backgroundWorker_6.RunWorkerAsync();

            }
            else if (Live_Cam_6 != null)
            {
                Live_Cam_6.Dispose();
            }
            Cam6VIDEO_Device.SignalToStop();
        }

        void Cam5VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            if (order_5)
            {
                Live_Cam_5 = (Bitmap)eventArgs.Frame.Clone();
                if (!backgroundWorker_5.IsBusy) backgroundWorker_5.RunWorkerAsync();

            }
            else if (Live_Cam_5 != null)
            {
                Live_Cam_5.Dispose();
            }
            Cam5VIDEO_Device.SignalToStop();
        }

        void Cam4VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (order_4)
            {
                Live_Cam_4 = (Bitmap)eventArgs.Frame.Clone();
                if (!backgroundWorker_4.IsBusy) backgroundWorker_4.RunWorkerAsync();

            }
            else if (Live_Cam_4 != null)
            {
                Live_Cam_4.Dispose();
            }
            Cam4VIDEO_Device.SignalToStop();
        }

        void Cam3VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            if (order_3)
            {
                Live_Cam_3 = (Bitmap)eventArgs.Frame.Clone();
                if (!backgroundWorker_3.IsBusy) backgroundWorker_3.RunWorkerAsync();

            }
            else if (Live_Cam_3 != null)
            {
                Live_Cam_3.Dispose();
            }
            Cam3VIDEO_Device.SignalToStop();
        }

        void Cam2VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            if (order_2)
            {
                Live_Cam_2 = (Bitmap)eventArgs.Frame.Clone();
                if (!backgroundWorker_2.CancellationPending)
                {
                    if (!backgroundWorker_2.IsBusy) backgroundWorker_2.RunWorkerAsync();
                }
            }
            else if (Live_Cam_2 != null)
            {
                Live_Cam_2.Dispose();
            }
            Cam2VIDEO_Device.SignalToStop();
        }
        void Cam1VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            if (order_1)
            {
                Live_Cam_1 = (Bitmap)eventArgs.Frame.Clone();
                if (!backgroundWorker_1.CancellationPending)
                {
                    if (!backgroundWorker_1.IsBusy) backgroundWorker_1.RunWorkerAsync();
                }
            }
            else if (Live_Cam_1 != null)
            {
                Live_Cam_1.Dispose();
            }
            Cam1VIDEO_Device.SignalToStop();

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
            if (result == DialogResult.Yes) Environment.Exit(0);
            RESET();
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
            if (serialPort_communicate.IsOpen) serialPort_communicate.Close();
            if (Cam1VIDEO_Device != null && Cam1VIDEO_Device.IsRunning)
            {
                Cam1VIDEO_Device.Stop();
                PB_active1.Hide();
            }
            if (Cam2VIDEO_Device != null && Cam2VIDEO_Device.IsRunning)
            {

                Cam2VIDEO_Device.Stop();
                PB_active2.Hide();
            }
            if (Cam3VIDEO_Device != null && Cam3VIDEO_Device.IsRunning)
            {
                Cam3VIDEO_Device.Stop();
                PB_active3.Hide();
            }
            if (Cam4VIDEO_Device != null && Cam4VIDEO_Device.IsRunning)
            {
                Cam4VIDEO_Device.Stop();
                PB_active4.Hide();
            }
            if (Cam5VIDEO_Device != null && Cam5VIDEO_Device.IsRunning)
            {
                Cam5VIDEO_Device.Stop();
                PB_active5.Hide();
            }
            if (Cam6VIDEO_Device != null && Cam6VIDEO_Device.IsRunning)
            {
                Cam6VIDEO_Device.Stop();
                PB_active6.Hide();
            }
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
        }
        private void Camera_Check_component_FormClosing(object sender, FormClosingEventArgs e)
        {
            update_system();
            RESET();
            timer.Stop();

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

                tb_PN.Enabled = true;
                TB_idworker.Enabled = true;
                TB_wker2.Enabled = true;

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
            }

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

                if (file == null && !a1 && !a2 && !a3 && !a4 && !a5 && !a6) 
                {
                    path_1_1 = "";
                    path_1_2 = "";
                    path_1_3 = "";
                    path_1_4 = "";
                    path_1_5 = "";
                    path_1_6 = "";
                    path_1_7 = "";
                    break;
                }
                if(file == null && a1 && a2 && a3 && a4 && a5 && a6) 
                {
                    break;
                }
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
            if (path_1_1 == "" && path_1_2 == "" && path_1_3 == "" && path_1_4 == "" && path_1_5 == "" && path_1_6 == "")
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
                MessageBox.Show("Out of picture at screen 1 ");
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

                if (file == null && !a1 && !a2 && !a3 && !a4 && !a5 && !a6)
                {
                    path_2_1 = "";
                    path_2_2 = "";
                    path_2_3 = "";
                    path_2_4 = "";
                    path_2_5 = "";
                    path_2_6 = "";
                    path_2_7 = "";
                    break;
                }
                if (file == null && a1 && a2 && a3 && a4 && a5 && a6)
                {
                    break;
                }
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
            if (path_2_1 == "" && path_2_2 == "" && path_2_3 == "" && path_2_4 == "" && path_2_5 == "" && path_2_6 == "") 
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
                MessageBox.Show("Out of picture at screen 2 ");
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
                var item = new ListViewItem(new[] { PN_Selector, "OK", "" });
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
                Boolean insert = sql_action.excute_data("INSERT INTO NG_detail ([PN_Selector],[Date],[Time],[Trace]) VALUES (N'" + PN_Selector + "','" + getpath[1] + "','" + getpath[2] + "-" + getpath[3] + "-" + getpath[4] + "','" + tach[0] + "_" + err_pic + "')");
                Boolean orderby = sql_action.excute_data("SELECT Time FROM component_status ORDER BY (Time) DESC");
                var item = new ListViewItem(new[] { PN_Selector, "NG", error_Type(loi_tam1) });
                listView1.Items.Add(item);

                status(" [SYSTEM]" + " [ERROR]" + " SAVED IMAGE[" + load1.ToString() + "]");
            }
            folderIndex++;
            load1 = folderIndex;
        }

        string h1 = "OK";
        string h2 = "OK";
        string h3 = "OK";
        string h4 = "OK";
        string h5 = "OK";
        string h6 = "OK";

        string err_pic = "";
        private void vitri_Erpic(string so)
        {
            if (so == "1")
            {
                h1 = "NG";
                err_pic = "1";
            }
            else

            {
                h1 = "OK";
                err_pic = "";
            }
            if (so == "2")
            {
                h2 = "NG";
                err_pic = "2";
            }
            else
            {
                h2 = "OK";
                err_pic = "";
            }
            if (so == "3")
            {
                h3 = "NG";
                err_pic = "3";
            }
            else
            {
                h3 = "OK";
                err_pic = "";
            }
            if (so == "4")
            {
                h4 = "NG";
                err_pic = "4";
            }
            else
            {
                h4 = "OK";
                err_pic = "";
            }
            if (so == "5")
            {
                h5 = "NG";
                err_pic = "5";
            }
            else
            {
                h5 = "OK";
                err_pic = "";
            }
            if (so == "6")
            {
                h6 = "NG";
                err_pic = "6";
            }
            else
            {
                h6 = "OK";
                err_pic = "";
            }
        }
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
                var item = new ListViewItem(new[] { PN_Selector, "OK", "" });
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
                Boolean insert = sql_action.excute_data("INSERT INTO NG_detail ([PN_Selector],[Date],[Time],[Trace]) VALUES (N'" + PN_Selector + "','" + getpath[1] + "','" + getpath[2] + "-" + getpath[3] + "-" + getpath[4] + "','" + tach[0] + "_" + err_pic + "')");
                Boolean orderby = sql_action.excute_data("SELECT Time FROM component_status ORDER BY Time DESC");
                var item = new ListViewItem(new[] { PN_Selector, "NG", error_Type(loi_tam2) });
                listView1.Items.Add(item);

                status(" [SYSTEM]" + " [ERROR]" + " SAVED IMAGE[" + system_config.Folder_index_tranfer.ToString() + "]");
            }
            folderIndex++;
            load2 = folderIndex;
        }

        private void OK1_check()
        {

            MethodInvoker inv = delegate
            {
                Tranfer("OK");
                upload_image();
                Program_Configuration.UpdateSystem_Config("inf_process", DateTime.Now.ToString());
                inf_process();
            };
            this.Invoke(inv);
        }
        private void NG1_check()
        {
            MethodInvoker inv = delegate
            {
                DateTime dt = DateTime.Now;
                Tranfer("ERROR");
                upload_image();
                Program_Configuration.UpdateSystem_Config("inf_process", DateTime.Now.ToString());
                inf_process();
            };
            this.Invoke(inv);
        }
        private void OK2_check()
        {
            MethodInvoker inv = delegate
            {
                DateTime dt = DateTime.Now;
                Tranfer1("OK");
                update_image2();
                Program_Configuration.UpdateSystem_Config("inf_process", dt.ToString());
                inf_process();
            };
            this.Invoke(inv);
        }
        private void NG2_check()
        {
            MethodInvoker inv = delegate
            {
                DateTime dt = DateTime.Now;
                Tranfer1("ERROR");
                update_image2();
                Program_Configuration.UpdateSystem_Config("inf_process", dt.ToString());
                inf_process();
            };
            this.Invoke(inv);
        }
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
                        run_out2 = true;
                    }
                }
            }
            else allow_check = false;
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
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

            }
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
        private string UserID = "";
        private void login_btn_Click(object sender, EventArgs e)
        {
            Login loginfrm = new Login();
            loginfrm.FormClosed += (object sender1, FormClosedEventArgs e1) =>
            {
                UserID = loginfrm.ID_user;
                if (UserID != "")
                {
                    login(UserID);
                }
            };
            loginfrm.Show();
        }
        private void permiss_1()
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
        private void permiss_2()
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
                        else
                        {
                            ctl.Enabled = false;
                        }
                    }
                }
                else if (ctrl.Name == "tabPage4")
                {
                    ctrl.Enabled = false;
                }
                else
                {
                    ctrl.Enabled = true;
                }
            }
        }
        private void permiss_3()
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
                        else
                        {
                            ctl.Enabled = true;
                        }
                    }
                }
                if (ctrl.Name == "tabPage4")
                {
                    ctrl.Enabled = false;
                }
                else
                {
                    ctrl.Enabled = true;
                }
            }
        }
        private void permiss_4()
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
                        if (ctl.Name == "Logout_btn")
                        {
                            ctl.Enabled = true;
                        }
                    }
                }
                if (ctrl.Name == "tabPage4")
                {
                    ctrl.Enabled = false;
                }
                else
                {
                    ctrl.Enabled = true;
                }
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

        private void button1_Click(object sender, EventArgs e)
        {
            order_1 = true;
            order_2 = true;
            order_3 = true;
            order_4 = true;
            order_5 = true;
            order_6 = true;
            if (order_6)
            {
                Cam6VIDEO_Device.Start();
                System.Threading.Thread.Sleep(10);
            }
            if (order_1)
            {
                Cam1VIDEO_Device.Start();
                System.Threading.Thread.Sleep(10);
            }
            if (order_2)
            {
                Cam2VIDEO_Device.Start();
                System.Threading.Thread.Sleep(10);
            }
            if (order_3)
            {
                Cam3VIDEO_Device.Start();
                System.Threading.Thread.Sleep(10);
            }
            if (order_4)
            {
                Cam4VIDEO_Device.Start();
                System.Threading.Thread.Sleep(10);
            }
            if (order_5)
            {
                Cam5VIDEO_Device.Start();
                System.Threading.Thread.Sleep(10);
            }           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "1") 
            {
                OK1_check();
            }
            if (textBox1.Text == "2")
            {
                OK2_check();
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            int hinh = int.Parse(textBox1.Text);
            if (hinh < 10) 
            {
                zoom1(hinh);
            }
            if (hinh > 10) 
            {
                hinh = hinh % 10;
                zoom2(hinh);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MethodInvoker inv = delegate
            {
                vitri_Erpic("5");
                loi_tam1 = textBox1.Text;
                NG1_check();
            };this.Invoke(inv);           
        }
        private void view_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.DataBindings.Clear();
            if (comboBox1.Text == "PN Selector Status") 
            {
                DataTable dt = sql_action.result_tbl("component_status");
                dataGridView1.DataSource = dt;
            }
            if(comboBox1.Text == "NG Detail") 
            {
                DataTable dt = sql_action.result_tbl("NG_Detail");
                dataGridView1.DataSource = dt;
            }
            else if(comboBox1.Text == "") 
            {
                return;
            }
           
        }
    }
}

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

using System.Drawing.Imaging;


namespace Camera_Check_Component
{
    public partial class Checking_Form_2 : Form
    {
        System_config system_config;
        private FileSystemWatcher watcher;
        int folderIndex = 0;
        public Checking_Form_2()
        {
            FilterInfoCollection filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            InitializeComponent();
        }

        private void Checking_Form_2_Load(object sender, EventArgs e)
        {
            system_config = Program_Configuration.GetSystem_Config();
            folderIndex = system_config.Folder_index_tranfer;
            Program_Configuration.UpdateSystem_Config("Folder_index_tranfer", folderIndex.ToString());
            //Parameter_app.TEMP(system_config.Folder_index_tranfer.ToString());
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox9.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox10.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox11.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox12.SizeMode = PictureBoxSizeMode.StretchImage;

            textBox1.Multiline = true;
            upload_image();
            folderIndex++;
            Program_Configuration.UpdateSystem_Config("Folder_load_check", folderIndex.ToString());
            //Parameter_app.TEMP(system_config.Folder_load_check.ToString());
            update_image2();
            status("[CHECK] " + " Checking Process started");
        }
        private void status(string text)
        {
            MethodInvoker inv = delegate
            {
                textBox1.AppendText("[" + DateTime.Now.ToString() + "]" + text + Environment.NewLine);
            };
            this.Invoke(inv);
        }
        private void start() 
        {
            system_config = Program_Configuration.GetSystem_Config();
            watcher = new FileSystemWatcher(Path.GetDirectoryName(system_config.Map_Path_File));
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.txt";
            watcher.Changed += watcher_Changed;
            watcher.EnableRaisingEvents = true;
        }

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            system_config.Folder_load_check = Convert.ToInt32(Program_Configuration.GetSystem_Config_Value("Folder_index_tranfer"));
            system_config.Folder_index_tranfer = Convert.ToInt32(Program_Configuration.GetSystem_Config_Value("Folder_index_tranfer"));
            system_config.Folder_index_tranfer = system_config.Folder_index_tranfer+1;
        }
        private void Start_btn_Click(object sender, EventArgs e)
        {
           
            
        }
        private void upload_image() 
        {

            if (!File.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img1" + system_config.Folder_index_tranfer.ToString() + ".jpeg"))
            {
                status("[CHECKING] " + " IMAGE_1" + system_config.Folder_index_tranfer.ToString() + " NOT EXIST");

            }
            else pictureBox1.Load(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img1" + system_config.Folder_index_tranfer.ToString() + ".jpeg");

            if (!File.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img2" + system_config.Folder_index_tranfer.ToString() + ".jpeg"))
            {
                status("[CHECKING] " + " IMAGE_2" + system_config.Folder_index_tranfer.ToString() + " NOT EXIST");

            }
            else pictureBox2.Load(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img2" + system_config.Folder_index_tranfer.ToString() + ".jpeg");


            if (!File.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img3" + system_config.Folder_index_tranfer.ToString() + ".jpeg"))
            {
                status("[CHECKING] " + " IMAGE_3" + system_config.Folder_index_tranfer.ToString() + " NOT EXIST");

            }
            else pictureBox3.Load(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img3" + system_config.Folder_index_tranfer.ToString() + ".jpeg");

            if (!File.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img4" + system_config.Folder_index_tranfer.ToString() + ".jpeg"))
            {
                status("[CHECKING] " + " IMAGE_4" + system_config.Folder_index_tranfer.ToString() + " NOT EXIST");

            }
            else pictureBox4.Load(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img4" + system_config.Folder_index_tranfer.ToString() + ".jpeg");

            if (!File.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img5" + system_config.Folder_index_tranfer.ToString() + ".jpeg"))
            {
                status("[CHECKING] " + " IMAGE_5" + system_config.Folder_index_tranfer.ToString() + " NOT EXIST");

            }
            else pictureBox5.Load(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img5" + system_config.Folder_index_tranfer.ToString() + ".jpeg");

            if (!File.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img6" + system_config.Folder_index_tranfer.ToString() + ".jpeg"))
            {
                status("[CHECKING] " + " IMAGE_6" + system_config.Folder_index_tranfer.ToString() + " NOT EXIST");
                return;
            }
            else pictureBox6.Load(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img6" + system_config.Folder_index_tranfer.ToString() + ".jpeg");


            //====================================================== IMAGE 2x ===========================================================================

          
        }
        private void update_image2() 
        {
            if (!File.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img1" + (system_config.Folder_load_check).ToString() + ".jpeg"))
            {
                status("[CHECKING] " + " IMAGE_1" + system_config.Folder_load_check.ToString() + " NOT EXIST");

            }
            else pictureBox7.Load(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img1" + (system_config.Folder_load_check).ToString() + ".jpeg");

            if (!File.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img2" + (system_config.Folder_load_check).ToString() + ".jpeg"))
            {
                status("[CHECKING] " + " IMAGE_2" + system_config.Folder_load_check.ToString() + " NOT EXIST");

            }
            else pictureBox8.Load(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img2" + (system_config.Folder_load_check).ToString() + ".jpeg");


            if (!File.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img3" + (system_config.Folder_load_check).ToString() + ".jpeg"))
            {
                status("[CHECKING] " + " IMAGE_3" + system_config.Folder_load_check.ToString() + " NOT EXIST");

            }
            else pictureBox9.Load(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img3" + (system_config.Folder_load_check).ToString() + ".jpeg");

            if (!File.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img4" + (system_config.Folder_load_check).ToString() + ".jpeg"))
            {
                status("[CHECKING] " + " IMAGE_4" + system_config.Folder_load_check.ToString() + " NOT EXIST");

            }
            else pictureBox10.Load(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img4" + (system_config.Folder_load_check).ToString() + ".jpeg");

            if (!File.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img5" + (system_config.Folder_load_check).ToString() + ".jpeg"))
            {
                status("[CHECKING] " + " IMAGE_5" + system_config.Folder_load_check.ToString() + " NOT EXIST");

            }
            else pictureBox11.Load(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img5" + (system_config.Folder_load_check).ToString() + ".jpeg");

            if (!File.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img6" + (system_config.Folder_load_check).ToString() + ".jpeg"))
            {
                status("[CHECKING] " + " IMAGE_6" + system_config.Folder_load_check.ToString() + " NOT EXIST");
                return;
            }
            else pictureBox12.Load(Parameter_app.TEMP_IMAGE_FOLDER_PATH + @"\img6" + (system_config.Folder_load_check).ToString() + ".jpeg");
        }
        private void Tranfer(string OPTION) 
        {
            if (OPTION == "OK") 
            {
                
                Parameter_app.OK_TEMP(system_config.Folder_index_tranfer.ToString());
                if (!Directory.Exists(Parameter_app.OK_IMAGE_FOLDER_PATH))
                {
                    Directory.CreateDirectory(Parameter_app.OK_IMAGE_FOLDER_PATH);
                }
                string outputFileName1 = Parameter_app.OK_IMAGE_FOLDER_PATH + @"\img1" + system_config.Folder_index_tranfer.ToString() + ".jpeg";
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName1, FileMode.Create, FileAccess.ReadWrite))
                    {
                        pictureBox1.Image.Save(memory,ImageFormat.Jpeg);
                        //Live_Cam_1.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                status(" [SYSTEM] " + " [OK]" + " SAVED IMAGE[1_" + system_config.Folder_index_tranfer.ToString() + "]");
                string outputFileName2 = Parameter_app.OK_IMAGE_FOLDER_PATH + @"\img2" + system_config.Folder_index_tranfer.ToString() + ".jpeg";
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName2, FileMode.Create, FileAccess.ReadWrite))
                    {
                        pictureBox2.Image.Save(memory, ImageFormat.Jpeg);
                        //Live_Cam_1.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                status(" [SYSTEM] " + " [OK]" + " SAVED IMAGE[2_" + system_config.Folder_index_tranfer.ToString() + "]");
                //pictureBox3.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img3" + system_config.Folder_load_check.ToString() + ".jpeg");
                //pictureBox4.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img4" + system_config.Folder_load_check.ToString() + ".jpeg");
                //pictureBox5.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img5" + system_config.Folder_load_check.ToString() + ".jpeg");
                //pictureBox6.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img6" + system_config.Folder_load_check.ToString() + ".jpeg");
            }
            if(OPTION=="ERROR")
            {
                //Program_Configuration.UpdateSystem_Config("Folder_index_tranfer", folderIndex.ToString());
               // system_config = Program_Configuration.GetSystem_Config();
               // system_config.Folder_load_check = Convert.ToInt32(Program_Configuration.GetSystem_Config_Value("Folder_index_tranfer"));
               // system_config.Folder_index_tranfer = Convert.ToInt32(Program_Configuration.GetSystem_Config_Value("Folder_index_tranfer"));
                Parameter_app.ERROR_TEMP(system_config.Folder_index_tranfer.ToString());
                if (!Directory.Exists(Parameter_app.ERROR_IMAGE_FOLDER_PATH))
                {
                    Directory.CreateDirectory(Parameter_app.ERROR_IMAGE_FOLDER_PATH);
                }
                string outputFileName1 = Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"\img1" + system_config.Folder_index_tranfer.ToString() + ".jpeg";
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName1, FileMode.Create, FileAccess.ReadWrite))
                    {
                        pictureBox1.Image.Save(memory, ImageFormat.Jpeg);
                        //Live_Cam_1.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                status(" [SYSTEM]" + " [ERROR]" + " SAVED IMAGE[1_" + system_config.Folder_index_tranfer.ToString() + "]");
                string outputFileName2 = Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"\img2" + system_config.Folder_index_tranfer.ToString() + ".jpeg";
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName2, FileMode.Create, FileAccess.ReadWrite))
                    {
                        pictureBox2.Image.Save(memory, ImageFormat.Jpeg);
                        //Live_Cam_1.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                status(" [SYSTEM]" + " [ERROR]" + " SAVED IMAGE[2_" + system_config.Folder_index_tranfer.ToString() + "]");
                //pictureBox3.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img3" + system_config.Folder_load_check.ToString() + ".jpeg");
                //pictureBox4.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img4" + system_config.Folder_load_check.ToString() + ".jpeg");
                //pictureBox5.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img5" + system_config.Folder_load_check.ToString() + ".jpeg");
                //pictureBox6.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img6" + system_config.Folder_load_check.ToString() + ".jpeg");
            }
           

        }
        private void Tranfer1(string OPTION)
        {
            if (OPTION == "OK")
            {

                Parameter_app.OK_TEMP(system_config.Folder_load_check.ToString());
                if (!Directory.Exists(Parameter_app.OK_IMAGE_FOLDER_PATH))
                {
                    Directory.CreateDirectory(Parameter_app.OK_IMAGE_FOLDER_PATH);
                }
                string outputFileName1 = Parameter_app.OK_IMAGE_FOLDER_PATH + @"\img1" + system_config.Folder_load_check.ToString() + ".jpeg";
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName1, FileMode.Create, FileAccess.ReadWrite))
                    {
                        pictureBox7.Image.Save(memory, ImageFormat.Jpeg);
                        //Live_Cam_1.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                status(" [SYSTEM] " + " [OK]" + " SAVED IMAGE[1_" + system_config.Folder_load_check.ToString() + "]");
                string outputFileName2 = Parameter_app.OK_IMAGE_FOLDER_PATH + @"\img2" + system_config.Folder_load_check.ToString() + ".jpeg";
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName2, FileMode.Create, FileAccess.ReadWrite))
                    {
                        pictureBox8.Image.Save(memory, ImageFormat.Jpeg);
                        //Live_Cam_1.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                status(" [SYSTEM] " + " [OK]" + " SAVED IMAGE[2_" + system_config.Folder_load_check.ToString() + "]");
                //pictureBox3.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img3" + system_config.Folder_load_check.ToString() + ".jpeg");
                //pictureBox4.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img4" + system_config.Folder_load_check.ToString() + ".jpeg");
                //pictureBox5.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img5" + system_config.Folder_load_check.ToString() + ".jpeg");
                //pictureBox6.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img6" + system_config.Folder_load_check.ToString() + ".jpeg");
            }
            if (OPTION == "ERROR")
            {
               
                Parameter_app.ERROR_TEMP(system_config.Folder_load_check.ToString());
                if (!Directory.Exists(Parameter_app.ERROR_IMAGE_FOLDER_PATH))
                {
                    Directory.CreateDirectory(Parameter_app.ERROR_IMAGE_FOLDER_PATH);
                }
                string outputFileName1 = Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"\img1" + system_config.Folder_load_check.ToString() + ".jpeg";
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName1, FileMode.Create, FileAccess.ReadWrite))
                    {
                        pictureBox7.Image.Save(memory, ImageFormat.Jpeg);
                        //Live_Cam_1.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                status(" [SYSTEM]" + " [ERROR]" + " SAVED IMAGE[1_" + system_config.Folder_load_check.ToString() + "]");
                string outputFileName2 = Parameter_app.ERROR_IMAGE_FOLDER_PATH + @"\img2" + system_config.Folder_load_check.ToString() + ".jpeg";
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName2, FileMode.Create, FileAccess.ReadWrite))
                    {
                        pictureBox8.Image.Save(memory, ImageFormat.Jpeg);
                        //Live_Cam_1.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                    }
                }
                status(" [SYSTEM]" + " [ERROR]" + " SAVED IMAGE[2_" + system_config.Folder_load_check.ToString() + "]");
                //pictureBox3.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img3" + system_config.Folder_load_check.ToString() + ".jpeg");
                //pictureBox4.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img4" + system_config.Folder_load_check.ToString() + ".jpeg");
                //pictureBox5.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img5" + system_config.Folder_load_check.ToString() + ".jpeg");
                //pictureBox6.Image.Save(Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + Parameter_app.TEMP_IMAGE_FOLDER_NAME + @"\img6" + system_config.Folder_load_check.ToString() + ".jpeg");
            }


        }
        private void OK_btn_Click(object sender, EventArgs e)
        {
           
            Tranfer("OK");
            if (folderIndex < 10)
            {
               
                folderIndex++;
                  
                    if (folderIndex == 10)
                    {
                        folderIndex = 0;
                    }           
            }
            Program_Configuration.UpdateSystem_Config("Folder_index_tranfer", folderIndex.ToString());
            system_config = Program_Configuration.GetSystem_Config();
         
            system_config.Folder_index_tranfer = Convert.ToInt32(Program_Configuration.GetSystem_Config_Value("Folder_index_tranfer"));
            //Parameter_app.TEMP(system_config.Folder_index_tranfer.ToString());
            if (!Directory.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH)) 
            {
                status("[CHECKING] "+" Foler:"+Parameter_app.TEMP_IMAGE_FOLDER_PATH+"does not exist");
                return;
            }
            upload_image();

            
        }

        private void ERROR_btn_Click(object sender, EventArgs e)
        {
           
            Tranfer("ERROR");
          
            if (folderIndex < 10)
            {
                
                folderIndex++;             
                if (folderIndex == 10)
                {
                    folderIndex = 0;
                }
               
            }
            Program_Configuration.UpdateSystem_Config("Folder_index_tranfer", folderIndex.ToString());
            system_config = Program_Configuration.GetSystem_Config();
            system_config.Folder_index_tranfer = Convert.ToInt32(Program_Configuration.GetSystem_Config_Value("Folder_index_tranfer"));
            //Parameter_app.TEMP(system_config.Folder_index_tranfer.ToString());
            if (!Directory.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH))
            {
                status("[CHECKING] " + " Foler:" + Parameter_app.TEMP_IMAGE_FOLDER_PATH + "does not exist");
                return;
            }
            upload_image();

        }

        private void OK2_btn_Click(object sender, EventArgs e)
        {
            Tranfer1("OK");
            if (folderIndex < 10)
            {
                //if (folderIndex != system_config.Folder_index_tranfer)
                //{
                folderIndex++;
             
                if (folderIndex == 10)
                {
                    folderIndex = 0;
                }
                //}

            }
            Program_Configuration.UpdateSystem_Config("Folder_load_check", folderIndex.ToString());
            system_config = Program_Configuration.GetSystem_Config();
            system_config.Folder_load_check = Convert.ToInt32(Program_Configuration.GetSystem_Config_Value("Folder_load_check"));
            //Parameter_app.TEMP(system_config.Folder_load_check.ToString());
            if (!Directory.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH))
            {
                status("[CHECKING] " + " Foler:" + Parameter_app.TEMP_IMAGE_FOLDER_PATH + "does not exist");
                return;
            }
            update_image2(); 
        }

        private void ERROR2_btn_Click(object sender, EventArgs e)
        {
            Tranfer1("ERROR");

            if (folderIndex < 10)
            {
                //if (folderIndex != system_config.Folder_index_tranfer)
                //{
                folderIndex++;
                if (folderIndex == 10)
                {
                    folderIndex = 0;
                }
                //}
            }
            Program_Configuration.UpdateSystem_Config("Folder_load_check", folderIndex.ToString());
            system_config = Program_Configuration.GetSystem_Config();
            system_config.Folder_load_check = Convert.ToInt32(Program_Configuration.GetSystem_Config_Value("Folder_load_check"));
            //Parameter_app.TEMP(system_config.Folder_load_check.ToString());
            if (!Directory.Exists(Parameter_app.TEMP_IMAGE_FOLDER_PATH))
            {
                status("[CHECKING] " + " Foler:" + Parameter_app.TEMP_IMAGE_FOLDER_PATH + "does not exist");
                return;
            }
            update_image2();
        }

       
  
    }
}

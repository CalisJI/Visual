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
using AForge.Imaging;
using Emgu.CV.UI;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;    

namespace Camera_Check_Component
{
    public partial class Cam_Review : Form
    {
        FilterInfoCollection filterinfocollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        VideoCaptureDevice videoCaptureDevice = new VideoCaptureDevice();
        private int Cam_Index;
        private string Cam_name;
        private int pixel;
      
      
        public Cam_Review(int Cam_Index, string Cam_name, int pixel)
        {
            InitializeComponent();
            this.Cam_Index = Cam_Index;
            this.Cam_name = Cam_name;
            this.pixel = pixel;
        }
        private void Cam_Review_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            ShowIcon = false;
            MinimizeBox = false;
            int w = pictureBox1.Width;
            
            label1.Text = Cam_name +" : "+ filterinfocollection[this.Cam_Index].Name;
            if(filterinfocollection.Count >0 && this.Cam_Index < filterinfocollection.Count)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                videoCaptureDevice = new VideoCaptureDevice(filterinfocollection[this.Cam_Index].MonikerString);
                if (pixel < 0) 
                {
                    MessageBox.Show("Please select your resolution first");                    
                    return;
                }
                //this.Close();
                videoCaptureDevice.VideoResolution = videoCaptureDevice.VideoCapabilities[pixel];
                videoCaptureDevice.NewFrame += HandleCaptureDeviceStreamNewFrame;
                videoCaptureDevice.Start();
            }
            else
            {
                MessageBox.Show("Camera can not found");
            }
        }
        private void HandleCaptureDeviceStreamNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap video;
           
            video = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = video;
            if (video != null) 
            {
                video.Dispose();
            }
        }
        private void Cam_Review_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoCaptureDevice.IsRunning) 
            {
                videoCaptureDevice.Stop();
            }
        }
        
        private void Take_photo_btn_Click(object sender, EventArgs e)
        {
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;       
            pictureBox2.Image = (Bitmap)pictureBox1.Image.Clone();
        }      
    }
}

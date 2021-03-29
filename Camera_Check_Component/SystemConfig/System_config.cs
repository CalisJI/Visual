using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camera_Check_Component;

namespace Camera_Check_Component
{
    public class System_config
    {
        public string Map_Path_File {get; set; }
        public string Output_File { get; set; }
        public string DefaultComport { get; set; }
        public string DefaultCOMBaudrate { get; set; }
        public System_config()
        {
        }
        public int Camera1 { get; set; }
        public int Camera2 { get; set; }
        public int Camera3 { get; set; }
        public int Camera4 { get; set; }
        public int Camera5 { get; set; }
        public int Camera6 { get; set; }
        public int Camera7 { get; set; }
        public string add_cam { get; set; }
       
        public int Location_cam1_folder { get; set; }
        public int Location_cam2_folder { get; set; }
        public int Location_cam3_folder { get; set; }

        public int Location_cam4_folder { get; set; }
        public int Location_cam5_folder { get; set; }
        public int Location_cam6_folder { get; set; }
        public int Location_cam7_folder { get; set; }

        public int Folder_load_check { get; set; }
        public int Folder_index_tranfer { get; set; }
        public int same_folder_1 { get; set; }
        public int same_folder_2 { get; set; }
        public int old_Day1 { get; set; }
        public int old_Month1 { get; set; }
        public int old_Year1 { get;set; }
        public int old_DAY { get; set; }
        public int old_MONTH { get; set; }
        public int old_YEAR { get; set; }
        public int new_Day { get; set; }
        public int new_Month { get; set; }
        public int new_Year { get; set; }
        public int pixel_cam1 { get; set; }
        public int pixel_cam2 { get; set; }
        public int pixel_cam3 { get; set; }
        public int pixel_cam4 { get; set; }
        public int pixel_cam5 { get; set; }
        public int pixel_cam6 { get; set; }
        public int pixel_cam7 { get; set; }
        public string SQL_server { get; set; }
        public string Database { get; set; }
        public string inf_process { get; set; }
        public string PN_Selector { get; set; }
    }
}

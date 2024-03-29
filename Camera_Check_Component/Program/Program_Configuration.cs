﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camera_Check_Component;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;

namespace Camera_Check_Component
{
   public class Program_Configuration
    {
       
       private static string System_File_Config_Path = Parameter_app.App_Folder + @"\" + Parameter_app.System_Config_File_Name;
       private static string Default_Code_Path = Parameter_app.App_Folder + @"\" + Parameter_app.Code_Flie_Name;
       private static string Output_File_Path = Parameter_app.App_Folder + @"\" + Parameter_app.Output_File_Name;
       
       public static System_config GetSystem_Config() 
       {
           try 
           { 
               if (!File.Exists(Default_Code_Path)) 
               {
                   File.WriteAllText(Default_Code_Path, "0");
               }
               if (!File.Exists(Output_File_Path)) 
               {
                   File.WriteAllText(Output_File_Path, "");
               }
           }
           catch (System.Exception) 
           {
            
           }
           if (File.Exists(System_File_Config_Path)) 
           {
               XmlSerializer serializer = new XmlSerializer(typeof(System_config));
               Stream stream = new FileStream(System_File_Config_Path,FileMode.Open);
               System_config systemConfig = (System_config)serializer.Deserialize(stream);
               stream.Close();
               return systemConfig;
           }
           else 
           {
               System_config system_Config = new System_config();
               system_Config.add_cam = "false";
               system_Config.Camera1 = 0;
               system_Config.Camera2 = 1;
               system_Config.Camera3 = 2;
               system_Config.Camera4 = 3;
               system_Config.Camera5 = 4;
               system_Config.Camera6 = 5;

               system_Config.DefaultComport = "COM1";
               system_Config.DefaultCOMBaudrate = "9600";
               system_Config.SQL_server = @"DESKTOP-CDO0SQ2\SQLEXPRESS";
               system_Config.Database = "ComponentState";
               system_Config.Map_Path_File = Default_Code_Path;
               system_Config.Output_File = Output_File_Path;
                system_Config.PN_Selector = "3DC";
                system_Config.inf_process = "";
               XmlSerializer serializer = new XmlSerializer(typeof(System_config));
               Stream stream = new FileStream(System_File_Config_Path, FileMode.Create);

               XmlWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
               serializer.Serialize(writer, system_Config);
               writer.Close();
               stream.Close();
               return system_Config;
           }
       }
       public static string GetSystem_Config_Value(string nodeName) 
       {
           if (File.Exists(System_File_Config_Path)) 
           {
               XmlDocument xmlDoc = new XmlDocument();
               xmlDoc.Load(System_File_Config_Path);
               XmlElement xml_elm = xmlDoc.DocumentElement;
               foreach (XmlNode node in xml_elm.ChildNodes) 
               {
                   if (node.Name == nodeName) 
                   {
                       return node.InnerText;
                   }
               }
           }
           return null;
       }
       public static void UpdateSystem_Config(string nodeName, string value)
       {
           XmlDocument xmlDoc = new XmlDocument();
           xmlDoc.Load(System_File_Config_Path);
           XmlElement xml_elm = xmlDoc.DocumentElement;
           foreach (XmlNode node in xml_elm.ChildNodes) 
           {
               if (node.Name == nodeName) node.InnerText = value;
           }
           xmlDoc.Save(System_File_Config_Path);

       }

    }
}


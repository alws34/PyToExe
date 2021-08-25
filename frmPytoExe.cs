using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace PythonToExe
{
    public partial class frmPyToExe : Form
    {
        public frmPyToExe()
        {
            InitializeComponent();
        }

        private void compile()
        {
            try
            {
                string full_file_path = textBoxPath.Text;

                if (!String.IsNullOrWhiteSpace(full_file_path))
                {
                    if (full_file_path.Contains("\""))//remove parentheses
                        full_file_path = full_file_path.Replace("\"", "");

                    string filename_WO_Ext = Path.GetFileNameWithoutExtension(full_file_path);//get file name wihtout extension
                    string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);//get current user desktop folder
                    string work_folder = desktop + @"\" + filename_WO_Ext; // put the new folder on the desktop
                    string bat_file = work_folder + @"\Pycompile.bat"; //bat file path

                    check_dir(work_folder);
                    check_file(work_folder);
                    
                    using (StreamWriter sw = new StreamWriter(bat_file))//write commands to file
                    {
                        sw.WriteLine("cd /d " + "\"" + work_folder); // cd into work folder 
                        sw.WriteLine("pyinstaller --onefile " + "\"" + full_file_path + "\""); // run the python compiler
                        sw.WriteLine("explorer " + work_folder + @"\dist"); //open the folder where the exe is
                    }

                    Process.Start(bat_file);//run batch file
                }
            }
            catch (IOException ioe)
            {
                MessageBox.Show(ioe.ToString());
            }
        }

        private void check_dir(string path)//check if directory exists
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private void check_file(string path) //check if path exists
        {
            if (!File.Exists(path))
                File.Create(path);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            compile();
        }

        private void textBoxPath_TextChanged(object sender, EventArgs e)
        {
            string path = textBoxPath.Text;//validate the file path (the file) exists
            path = path.Replace("\"", "");
            if (!File.Exists(path))
            {
                MessageBox.Show("file does not exists or path is invalid");
                textBoxPath.Text = "";
            }
        }
    }
}

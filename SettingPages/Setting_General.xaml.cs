using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Light_IDE.Other;

namespace Light_IDE.SettingPages
{
    /// <summary>
    /// Setting_General.xaml 的交互逻辑
    /// </summary>
    public partial class Setting_General : Page
    {
        public Setting_General()
        {
            InitializeComponent();
            edi_Cache.Text = Global.CachePath;
            edi_py.Text = Global.PythonPath;
            edi_java.Text = Global.JavaPath;
            edi_c.Text = Global.CPath;
            switch (Global.NowProgramMode)
            {
                case ProgramMode.Java:
                    combo_lang.SelectedIndex = 0;
                    break;
                case ProgramMode.Python:
                    combo_lang.SelectedIndex = 1;
                    break;
                case ProgramMode.C:
                    combo_lang.SelectedIndex = 2;
                    break;
                default:
                    break;
            }
        }
        private void btn_CachePath_Click(object sender, RoutedEventArgs e)
        {
            string path = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "folders|*.sado;fjsdal;fj";
            ofd.FileName = "选择文件夹";
            ofd.CheckFileExists = false; ofd.CheckPathExists = true;
            //ofd.ValidateNames = false; 
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ofd.FileName = ofd.FileName.TrimEnd('\r');
                int lastSeparatorIndex = ofd.FileName.LastIndexOf('\\');
                ofd.FileName = ofd.FileName.Remove(lastSeparatorIndex);
                path = ofd.FileName;
            }
            else
            {
                return;
            }
            edi_Cache.Text = path;

        }

        private void btn_py_Click(object sender, RoutedEventArgs e)
        {
            string path = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "folders|*.sado;fjsdal;fj";
            ofd.FileName = "选择文件夹";
            ofd.CheckFileExists = false; ofd.CheckPathExists = true;
            //ofd.ValidateNames = false; 
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ofd.FileName = ofd.FileName.TrimEnd('\r');
                int lastSeparatorIndex = ofd.FileName.LastIndexOf('\\');
                ofd.FileName = ofd.FileName.Remove(lastSeparatorIndex);
                path = ofd.FileName;
            }
            else
            {
                return;
            }
            edi_py.Text = path;

        }

        private void btn_java_Click(object sender, RoutedEventArgs e)
        {
            string path = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "folders|*.sado;fjsdal;fj";
            ofd.FileName = "选择文件夹";
            ofd.CheckFileExists = false; ofd.CheckPathExists = true;
            //ofd.ValidateNames = false; 
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ofd.FileName = ofd.FileName.TrimEnd('\r');
                int lastSeparatorIndex = ofd.FileName.LastIndexOf('\\');
                ofd.FileName = ofd.FileName.Remove(lastSeparatorIndex);
                path = ofd.FileName;
            }
            else
            {
                return;
            }
            edi_java.Text = path;

        }

        private void btn_c_Click(object sender, RoutedEventArgs e)
        {
            string path = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "folders|*.sado;fjsdal;fj";
            ofd.FileName = "选择文件夹";
            ofd.CheckFileExists = false; ofd.CheckPathExists = true;
            //ofd.ValidateNames = false; 
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ofd.FileName = ofd.FileName.TrimEnd('\r');
                int lastSeparatorIndex = ofd.FileName.LastIndexOf('\\');
                ofd.FileName = ofd.FileName.Remove(lastSeparatorIndex);
                path = ofd.FileName;
            }
            else
            {
                return;
            }
            edi_c.Text = path;

        }

    }
}

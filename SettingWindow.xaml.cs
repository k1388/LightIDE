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
using System.Windows.Shapes;
using Light_IDE.SettingPages;
using Light_IDE.Other;

namespace Light_IDE
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        static public Setting_General gPage = new Setting_General();
        static public Setting_Advanced aPage = new Setting_Advanced();

        static public Frame gF = new Frame() { Content = gPage };
        static public Frame aF = new Frame() { Content = aPage };
        public SettingWindow()
        {
            InitializeComponent();
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            //配置更新
            //更新代码缓存
            Global.CachePath = gPage.edi_Cache.Text;
            Global.PythonPath = gPage.edi_py.Text;
            Global.JavaPath = gPage.edi_java.Text;
            Global.CPath = gPage.edi_c.Text;
            //更新编译语言
            switch (gPage.combo_lang.SelectedIndex)
            {
                case 0:
                    Global.NowProgramMode = ProgramMode.Java;
                    break;
                case 1:
                    Global.NowProgramMode = ProgramMode.Python;
                    break;
                case 2:
                    Global.NowProgramMode = ProgramMode.C;
                    break;
                default:
                    Global.NowProgramMode = ProgramMode.Python;
                    break;
            }
            //更新GPT参数
            Global.GPT_tem = aPage.sld_tem.Value / 100;
            Global.GPT_tp = aPage.sld_tp.Value / 100;
            //更新GPTkey
            if (aPage.edi_key.Text == string.Empty) {
                Global.CustomKey = string.Empty;
            }
            else
            {
                Global.CustomKey = aPage.edi_key.Text;
            }


            //将设置信息写入配置文件
            try
            {
                Global.Ini.IniWriteValue("Options", "ProLang", Global.NowProgramMode == ProgramMode.Java ? "Java" :
                    (Global.NowProgramMode == ProgramMode.Python ? "Python" : "C"));

                Global.Ini.IniWriteValue("Options", "CachePath", Global.CachePath);
                Global.Ini.IniWriteValue("Options", "PyPath", Global.PythonPath);
                Global.Ini.IniWriteValue("Options", "JavaPath", Global.JavaPath);
                Global.Ini.IniWriteValue("Options", "CPath", Global.CPath);

                Global.Ini.IniWriteValue("Options", "GPT_tem", Global.GPT_tem.ToString());
                Global.Ini.IniWriteValue("Options", "GPT_tp", Global.GPT_tp.ToString());
                Global.Ini.IniWriteValue("Options", "GPT_Key", Global.CustomKey);
            }
            catch (Exception)
            {
            }
            if (gPage.edi_Cache.Text == string.Empty)
            {
                Global.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); 
                Global.Ini.IniWriteValue("Options", "CachePath", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                gPage.edi_Cache.Text = Global.CachePath;
            }
            if (gPage.edi_py.Text == string.Empty)
            {
                gPage.edi_py.Text = Global.PythonPath;
            }
            if (gPage.edi_java.Text == string.Empty)
            {
                gPage.edi_java.Text = Global.JavaPath;
            }
            if (gPage.edi_c.Text == string.Empty)
            {
                gPage.edi_c.Text = Global.CPath;
            }

            this.Close();
        }

        private void bar_Title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void tab_top_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (tab_top.SelectedIndex)
            {

                case 0:
                    ctrl_page.Content = gF;
                    break;
                case 1:
                    ctrl_page.Content = aF;
                    break;
                default:
                    ctrl_page.Content = gF;
                    break;
            }
        }
    }
}

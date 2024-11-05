using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using Light_IDE.Pages;
using Light_IDE.Other;
using static Light_IDE.Other.Tools;

namespace Light_IDE
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            tab_Left.SelectedIndex = 1;
            //连接到服务器
            //Dispatcher.BeginInvoke((Action)delegate ()
            //{
               OnlineManeger.Connect();
            //});
            Global.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); ;
            UpdateIni();
            AutoLogIn();
            if (OnlineManeger.isLoged)
            {
                Global.accountPage.Content = Global.account_SignedFrame;
            }
        }
        public void AutoLogIn() {
            if (
                Global.Ini.IniReadValue("Account", "UserName")==""|| Global.Ini.IniReadValue("Account", "UserName") == null || 
                Global.Ini.IniReadValue("Account", "Password")=="" ||Global.Ini.IniReadValue("Account", "Password") == null
                )
            {
                return;
            }
            OnlineManeger.Login(Global.Ini.IniReadValue("Account", "UserName"), Global.Ini.IniReadValue("Account", "Password"));
        }
        public void UpdateIni() {
            //读取设置配置
            try
            {
                //定向配置文件
                Global.Ini = new IniFiles(System.Environment.CurrentDirectory + @"\Options.INI");
                //prolang键设置
                switch (Global.Ini.IniReadValue("Options", "ProLang"))
                {
                    case "Java":
                        Global.NowProgramMode = ProgramMode.Java;
                        break;
                    case "Python":
                        Global.NowProgramMode = ProgramMode.Python;
                        break;
                    case "C":
                        Global.NowProgramMode = ProgramMode.C;
                        break;
                    default:
                        break;
                }
                //cache键设置
                Global.CachePath = Global.Ini.IniReadValue("Options", "CachePath");
                Global.PythonPath = Global.Ini.IniReadValue("Options", "PyPath");
                Global.JavaPath = Global.Ini.IniReadValue("Options", "JavaPath");
                Global.CPath = Global.Ini.IniReadValue("Options", "CPath");
                //GPT设置
                Global.GPT_tem =double.Parse(Global.Ini.IniReadValue("Options", "GPT_tem"));
                Global.GPT_tp = double.Parse(Global.Ini.IniReadValue("Options", "GPT_tp"));
                Global.CustomKey = Global.Ini.IniReadValue("Options", "GPT_Key");
            }
            //
            catch (Exception)
            {
                //System.Windows.MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 窗口拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bar_Title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        /// <summary>
        /// 窗口最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MinSize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        /// <summary>
        /// 退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }
        /// <summary>
        /// 设置按钮被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Setting_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.ShowDialog();
        }
        /// <summary>
        /// 导航栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (OnlineManeger.isLoged)
            {
                Global.accountPage.Content = Global.account_SignedFrame;
                Global.cloudPage.Button_Click(null, null);
            }
            switch (tab_Left.SelectedIndex)
            {

                case 0:
                    contentContorl_ContentPage.Content = Global.accountFrame;
                    break;
                case 1:
                    contentContorl_ContentPage.Content = Global.editFrame;
                    break;
                case 2:
                    contentContorl_ContentPage.Content = Global.cloudFrame;
                    break;
                case 3:
                    contentContorl_ContentPage.Content = Global.gptFrame;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 外部刷新导航页
        /// </summary>
        /// <param name="index"></param>
        public void RefreshIndex() {
            tab_Left.SelectedIndex = Global.MainTabelSelectedIndex;
            TabControl_SelectionChanged(null, null);
        }

        /// <summary>
        /// 窗口大小设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {

            if (e.WidthChanged)
                this.Height = this.Width * 0.75;
            if (e.HeightChanged)
                this.Width = this.Height * 4 / 3;
        }
        /// <summary>
        /// 检测快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
       {
            //KeyCommon.Key = e.Key;
        }


    }
}


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
using Light_IDE.Other;

namespace Light_IDE.Pages.SubPages
{
    /// <summary>
    /// Account_SignUpPage.xaml 的交互逻辑
    /// </summary>
    public partial class Account_SignUpPage : Page
    {
        /// <summary>
        /// 验证码图片
        /// </summary>
        public VerificationCode Verification { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        public Account_SignUpPage()
        {
            InitializeComponent();
            Verification = VerificationCodeMaker.CreatCode();
            img_VerificaionCode.Source = Verification.img;
            Code = Verification.code;
        }
        /// <summary>
        /// 刷新验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void img_VerificaionCode_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string code = "";

            Verification = VerificationCodeMaker.CreatCode();
            img_VerificaionCode.Source = Verification.img;
            Code = Verification.code;
        }
        private void btn_logIn_Click(object sender, RoutedEventArgs e)
        {
            Global.IsInLoginPage = true;
            Global.accountPage.Switch();
        }

        private void btn_signUp_Click(object sender, RoutedEventArgs e)
        {
            var Vcode = edi_Verification.Text;
            var userName = edi_login_userName.Text;
            var psw = edi_login_psw.Password;
            if (Vcode != Code)
            {
                MessageBox.Show("验证码错误！", "Attention");
                return;
            }
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                try
                {
                    OnlineManeger.SQLDefend(userName);
                    OnlineManeger.SQLDefend(psw);

                    switch (OnlineManeger.SignUp(userName, psw))
                    {
                        case 0:
                            MessageBox.Show("注册成功", "Attention");
                            break;
                        case 1:
                            MessageBox.Show("用户已存在！", "Attention");
                            break;
                        case -1:
                            MessageBox.Show("连接不到服务器,请检查您的网络设置！", "Attention");
                            break;
                        default:

                            break;
                    }

                }
                catch (StringHasSymbolException)
                {
                    MessageBox.Show("用户名和密码中不能含有下划线以外的符号！", "Attention");
                }

            });
        }
    }
}

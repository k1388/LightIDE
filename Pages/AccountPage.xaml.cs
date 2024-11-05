using Light_IDE.Other;
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
using Light_IDE.Pages.SubPages;

namespace Light_IDE.Pages
{
    /// <summary>
    /// AccountPage.xaml 的交互逻辑
    /// </summary>
    public partial class AccountPage : Page
    {
        public Frame loginframe = new Frame() { Content = new Account_LoginPage() };
        public Frame signUpframe = new Frame() { Content = new Account_SignUpPage() };
        /// <summary>
        /// 页面构造方法
        /// </summary>
        public AccountPage()
        {
            InitializeComponent();
            this.Content = loginframe;
           // loginframe.Visibility = Visibility.Visible;
            //signUpframe.Visibility = Visibility.Hidden;
            Global.IsInLoginPage = true;
        }

        public void Switch()
        {
            if (Global.IsInLoginPage)
            {
                this.Content = loginframe;
                //  loginframe.Visibility = Visibility.Hidden;
                // signUpframe.Visibility = Visibility.Visible;
            }
            else
            {
                this.Content = signUpframe;
                // loginframe.Visibility = Visibility.Visible;
                //  signUpframe.Visibility = Visibility.Hidden;
            }
        }
    }
}

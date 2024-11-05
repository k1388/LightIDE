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
    /// Account_Signed.xaml 的交互逻辑
    /// </summary>
    public partial class Account_Signed : Page
    {
        public Account_Signed()
        {
            InitializeComponent();
        }

        private void btn_py_Click(object sender, RoutedEventArgs e)
        {
            Global.MainTabelSelectedIndex = 1;
            Global.editPage.edi_Main.Text = "print(\"Hello, World\")";
            Global.NowProgramMode = ProgramMode.Python;
            Global.editPage.edi_ClassName.Text = "HelloWorld";
            Tools.GetParentObject<MainWindow>(Tools.GetParentObject<AccountPage>(this)).RefreshIndex();

        }

        private void btn_java_Click(object sender, RoutedEventArgs e)
        {
            Global.MainTabelSelectedIndex = 1;
            Global.editPage.edi_Main.Text =
                "class HelloWorld{\n" +
                "\tpublic static void main(String[] args){\n" +
                "\t\tSystem.out.println(\"Hello World!\");\n" +
                "\t}\n" +
                "}";
            Global.NowProgramMode = ProgramMode.Java;
            Global.editPage.edi_ClassName.Text = "HelloWorld";
            Tools.GetParentObject<MainWindow>(Tools.GetParentObject<AccountPage>(this)).RefreshIndex();

        }

        private void btn_c_Click(object sender, RoutedEventArgs e)
        {
            Global.MainTabelSelectedIndex = 1;
            Global.editPage.edi_Main.Text =
                "#include <stdio.h>\n\n" +
                "int main()\n" +
                "\t{\n" +
                "\tprintf(\"Hello World!\");\n" +
                "\treturn 0;\n" +
                "}";
            Global.NowProgramMode = ProgramMode.C;
            Global.editPage.edi_ClassName.Text = "HelloWorld";
            Tools.GetParentObject<MainWindow>(Tools.GetParentObject<AccountPage>(this)).RefreshIndex();

          
        }

        private void btn_logoff_Click(object sender, RoutedEventArgs e)
        {
            OnlineManeger.LogOff();
            Tools.GetParentObject<AccountPage>(this).Content = Tools.GetParentObject<AccountPage>(this).loginframe;
        }
    }
}

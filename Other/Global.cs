using Light_IDE.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Light_IDE.Pages.SubPages;
using static Light_IDE.Other.Tools;
using System.Diagnostics;

namespace Light_IDE.Other
{
    public class Global
    {
        static public bool IsInLoginPage { get; set; }
        static public string CachePath { get; set; }//缓存路径
        static public ProgramMode NowProgramMode { get; set; }//当前编程语言枚举属性
        static public IniFiles Ini { get; set; }//配置文件
        static public CloudCodeStructure[] CloudCodes { get; set; }
        static public string CodeChche { get; set; }
        static public string CustomKey { get; set; }
        static public double GPT_tem { get; set; }
        static public double GPT_tp { get; set; }

        static public string PythonPath { get; set; }//进行编译使用的Python.exe路径，下面类似
        static public string CPath { get; set; }
        static public string JavaPath { get; set; }

        static public EditPage editPage = new EditPage();
        static public AccountPage accountPage = new AccountPage();
        static public CloudPage cloudPage = new CloudPage();
        static public GPTPage gptPage = new GPTPage();
        static public  Account_Signed account_Signed = new Account_Signed();
        static public Frame editFrame = new Frame() { Content = editPage };
        static public Frame accountFrame = new Frame() { Content = accountPage };
        static public Frame cloudFrame = new Frame() { Content = cloudPage };
        static public Frame gptFrame = new Frame() { Content = gptPage };
        static public Frame account_SignedFrame = new Frame() { Content = account_Signed };

        static public int MainTabelSelectedIndex { get; set; }
    }
}

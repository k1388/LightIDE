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

namespace Light_IDE.SettingPages
{
    /// <summary>
    /// Setting_Advanced.xaml 的交互逻辑
    /// </summary>
    public partial class Setting_Advanced : Page
    {
        public Setting_Advanced()
        {
            InitializeComponent();
            sld_tem.Value = Global.GPT_tem*100;
            sld_tp.Value = Global.GPT_tp*100;
            edi_key.Text = Global.CustomKey;
        }
    }
}

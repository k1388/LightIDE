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

namespace Light_IDE.Pages
{

    /// <summary>
    /// Listbox的数据模型类
    /// </summary>
    public class TodoItem
    {
        public string T_CloudCodeTitle { get; set; }
        public string T_CloudCodeDate { get; set; }
        public string T_CloudCodeType { get; set; }

    }

    /// <summary>
    /// CloudPage.xaml 的交互逻辑
    /// </summary>
    public partial class CloudPage : Page
    {
        public CloudPage()
        {
            InitializeComponent();
        }


        private void list_onlineCodes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Global.CodeChche = Global.CloudCodes[list_onlineCodes.SelectedIndex].Code;
            Global.editPage.Refresh();
            Global.MainTabelSelectedIndex = 1;
            Tools.GetParentObject<MainWindow>(this).RefreshIndex();
        }

        private void Btn_refreshOnlineCode_Click(object sender, RoutedEventArgs e)

        {
            ItemsControl itemsControl = new ItemsControl();
            itemsControl.Items.Clear();
            CloudCodeStructure[] cloudCodes = OnlineManeger.GetCodesFromCloud();
            list_onlineCodes.Items.Clear();
            Global.CloudCodes = cloudCodes;
            try
            {
                foreach (var item in cloudCodes)
                {
                    list_onlineCodes.Items.Add(new TodoItem() { T_CloudCodeTitle = item.CodeTitle, T_CloudCodeDate = item.CodeDate, T_CloudCodeType = item.CodeType });

                }

            }
            catch (Exception)
            {

            }

        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!OnlineManeger.isLoged)
            {
                MessageBox.Show("未登录");
                return;
            }
            ItemsControl itemsControl = new ItemsControl();
            itemsControl.Items.Clear();
            CloudCodeStructure[] cloudCodes = OnlineManeger.GetCodesFromCloud();
            list_onlineCodes.Items.Clear();
            Global.CloudCodes = cloudCodes;
            try
            {
                foreach (var item in cloudCodes)
                {
                    list_onlineCodes.Items.Add(new TodoItem() { T_CloudCodeTitle = item.CodeTitle, T_CloudCodeDate = item.CodeDate, T_CloudCodeType = item.CodeType });

                }

            }
            catch (Exception)
            {

            }


        }

        private void btn_deleteSelected_Click(object sender, RoutedEventArgs e)
        {
            // 获取当前选中的ListBoxItem
            var selectedItem = list_onlineCodes.SelectedItem as TodoItem;

            // 获取ListBoxItem的内容
            var text = selectedItem?.T_CloudCodeDate;

            OnlineManeger.DeleteCode(text);
            Button_Click(null,null);
        }
    }
}

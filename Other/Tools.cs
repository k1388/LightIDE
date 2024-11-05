using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace Light_IDE.Other
{
    public class Tools
    {
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UnBase64String(string value)
        {
            if (value == null || value == "")
            {
                return "";
            }
            byte[] bytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 通过名称查找父控件
        /// </summary>
        /// <example>Grid layoutGrid = VTHelper.GetParentObject<Grid>(this.spDemoPanel, "LayoutRoot")</example>
        /// <typeparam name="T">父控件类型</typeparam>
        /// <param name="obj">子控件</param>
        /// <param name="name">父控件名称，默认为空</param>
        /// <returns></returns>
        public static T GetParentObject<T>(DependencyObject obj, string name = null) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T && (((T)parent).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }


        /// <summary>
        /// INI读写工具类
        /// </summary>
        public class IniFiles
        {
            public string inipath;

            //声明API函数

            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
            /// <summary> 
            /// 构造方法 
            /// </summary> 
            /// <param name="INIPath">文件路径</param> 
            public IniFiles(string INIPath)
            {
                inipath = INIPath;
            }

            public IniFiles() { }

            /// <summary> 
            /// 写入INI文件 
            /// </summary> 
            /// <param name="Section">项目名称(如 [TypeName] )</param> 
            /// <param name="Key">键</param> 
            /// <param name="Value">值</param> 
            public void IniWriteValue(string Section, string Key, string Value)
            {
                WritePrivateProfileString(Section, Key, Value, this.inipath);
            }
            /// <summary> 
            /// 读出INI文件 
            /// </summary> 
            /// <param name="Section">项目名称(如 [TypeName] )</param> 
            /// <param name="Key">键</param> 
            public string IniReadValue(string Section, string Key)
            {
                StringBuilder temp = new StringBuilder(500);
                int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
                return temp.ToString();
            }
            /// <summary> 
            /// 验证文件是否存在 
            /// </summary> 
            /// <returns>布尔值</returns> 
            public bool ExistINIFile()
            {
                return File.Exists(inipath);
            }
        }
    }
}

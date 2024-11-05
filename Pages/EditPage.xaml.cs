using Light_IDE.Other;
using System;
using System.Collections.Generic;
using System.IO;
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
using static Light_IDE.Other.Tools;
using MessageBox = System.Windows.Forms.MessageBox;
using Microsoft.Win32;
using System.Security.Permissions;
using System.Windows.Threading;
using System.Reflection;
using System.Runtime.InteropServices;
using ICSharpCode.AvalonEdit;
using System.Diagnostics;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Rendering;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit.Document;

namespace Light_IDE.Pages
{
    /// <summary>
    /// EditPage.xaml 的交互逻辑
    /// </summary>
    public partial class EditPage : Page
    {

        static Process Cmd = new Process();//cmd的进程属性（等等不是属性）
        public bool Saved { get; set; }//编辑框内代码是否已保存
        static public string NowPath { get; set; }//现在编辑框代码的路径
        public bool HasChanged { get; set; }//编辑框内容是否已被改变


        private Process cmdProcess;
        private StreamReader outputReader;
        private StreamReader errorReader;
        private DispatcherTimer outputTimer;
        private StreamWriter inputWriter;




        CodeSuggestion CodeSuggestion = new CodeSuggestion();

        List<string> codeSuggestions;

        /// <summary>
        /// 页面构造方法
        /// </summary>
        public EditPage()
        {

            
            InitializeComponent();
            edi_Main.ShowLineNumbers = true;
            edi_Main.Padding = new Thickness(10);
            edi_Main.FontFamily = new FontFamily("Console");
            edi_Main.FontSize = 18;
            edi_Main.SyntaxHighlighting =
                ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("Java");
            Global.NowProgramMode = ProgramMode.Python;

            // 创建 cmd 进程
            cmdProcess = new Process();
            cmdProcess.StartInfo.FileName = "cmd.exe";
            cmdProcess.StartInfo.RedirectStandardOutput = true;
            cmdProcess.StartInfo.RedirectStandardError = true;
            cmdProcess.StartInfo.RedirectStandardInput = true;
            cmdProcess.StartInfo.UseShellExecute = false;
            cmdProcess.StartInfo.CreateNoWindow = true;
            cmdProcess.Start();

            // 输出输入 StreamReader
            outputReader = cmdProcess.StandardOutput;
            errorReader = cmdProcess.StandardError;
            inputWriter = cmdProcess.StandardInput;

            // 定期读取进程输出并更新的内容
            outputTimer = new DispatcherTimer();
            outputTimer.Interval = TimeSpan.FromMilliseconds(100);
            outputTimer.Tick += OutputTimer_Tick;
            //outputTimer.Tick += ErrorTimer_Tick;


            outputTimer.Start();


            codeSuggestions = CodeSuggestion.pyCodeSug;
        }
        private bool isReadingOutput = false;



        private async void OutputTimer_Tick(object sender, EventArgs e)
        {
            if (isReadingOutput)
                return;
            

            isReadingOutput = true;

            while (!outputReader.EndOfStream)
            {
                string output = await outputReader.ReadLineAsync();

                await Dispatcher.InvokeAsync(() =>
                {

                    edi_cmd.Text += output + Environment.NewLine;

                    edi_cmd.ScrollToEnd();
                });
            }


            isReadingOutput = false;
        }
        private async void ErrorTimer_Tick(object sender, EventArgs e)
        {
            if (isReadingOutput)
                return;


            isReadingOutput = true;

            while (!errorReader.EndOfStream)
            {
                string error = await errorReader.ReadLineAsync();
                await Dispatcher.InvokeAsync(() =>
                {

                    edi_cmd.Text += error + Environment.NewLine;

                    edi_cmd.ScrollToEnd();
                });
            }


            isReadingOutput = false;
        }
        public void Send(string command)
        {
            outputTimer.Stop();
            inputWriter.WriteLine(command);
            inputWriter.Flush();
            outputTimer.Start();
        }


        #region 代码补全相关
        private string previousText = string.Empty;

        private void edi_Main_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (completionPopup.IsOpen)
            {
                if (completionList != null)
                {
                    if (e.Key == Key.Tab)
                    {
                        e.Handled = true; // 阻止 Tab 键传递到编辑器

                        if (completionList.SelectedIndex < completionList.Items.Count - 1)
                        {
                            // 选择下一个项
                            completionList.SelectedIndex++;
                            completionList.ScrollIntoView(completionList.SelectedItem);
                        }
                        else
                        {
                            // 循环选择第一个项
                            completionList.SelectedIndex = 0;
                            completionList.ScrollIntoView(completionList.SelectedItem);
                        }
                    }
                    else if (e.Key == Key.Enter)
                    {
                        e.Handled = true; // 阻止 Enter 键传递到编辑器

                        // 按下 Enter 键时将选中的项输入到编辑器中
                        var selectedItem = completionList.SelectedItem as string;
                        if (!string.IsNullOrEmpty(selectedItem))
                        {
                            // 获取当前光标位置和输入的文本
                            int caretOffset = edi_Main.CaretOffset;
                            string inputText = edi_Main.Text.Substring(0, caretOffset);

                            // 获取选中的建议代码
                            string selectedSuggestion = completionList.SelectedItem.ToString();

                            // 计算要替换的文本范围
                            int startIndex = inputText.LastIndexOfAny(new char[] { ' ', '\t', '\n', '\r' }) + 1;
                            int length = caretOffset - startIndex;

                            // 替换文本范围为选中的建议代码
                            edi_Main.Document.Replace(startIndex, length, selectedSuggestion);
                        }


                        // 关闭弹出窗口
                        completionPopup.IsOpen = false;


                    }
                }
            }
        }

        private void OnEditorTextChanged(object sender, EventArgs e)
        {
            string currentText = edi_Main.Text;

            // 检查用户输入的文本是否满足触发条件（例如以"."结尾）
            if (IsTriggerCodeCompletion(previousText, currentText))
            {
                // 获取代码补全建议
                List<string> suggestions = GetCodeSuggestions(currentText);

                // 如果有建议，显示建议窗口
                
                if (suggestions.Count > 0)
                {
                    var completionListBox = new ListBox();
                    completionListBox.ItemsSource = suggestions;

                    // 设置代码补全弹出窗口的位置
                    var caretLocation = edi_Main.TextArea.Caret.Position.Location;
                    var caretLine = edi_Main.TextArea.Document.GetLineByNumber(caretLocation.Line);
                    var caretPosition = new TextViewPosition(caretLine.LineNumber,caretLocation.Column);
                    var caretVisualPosition = edi_Main.TextArea.TextView.GetVisualPosition(caretPosition, VisualYPosition.LineBottom);

                    double popupX = caretVisualPosition.X;
                    double popupY = edi_Main.FontSize - edi_Main.ActualHeight+edi_Main.LineCount * 6d ;
                    completionPopup.HorizontalOffset = popupX;
                    completionPopup.VerticalOffset = popupY;

                    completionList.ItemsSource = suggestions;
                    completionPopup.IsOpen = true;
                }
                else
                {
                    // 如果没有建议，关闭建议窗口
                    completionPopup.IsOpen = false;
                }
            }
            else
            {
                // 如果不满足触发条件，关闭建议窗口
                completionPopup.IsOpen = false;
            }
            
            
            previousText = currentText;
        }

        private bool IsTriggerCodeCompletion(string previousText, string currentText)
        {
            bool isTriggered = true;
            // 在这里添加你判断是否需要触发代码补全的逻辑
            // 例如，你可以检查当前文本是否以"."结尾且不是由删除操作引起的变化
            
            if (currentText.Length < previousText.Length) {
                isTriggered = false;
            }
            if (currentText.EndsWith(" ")) {
                isTriggered = false;
            }
            DocumentLine line = edi_Main.Document.GetLineByNumber(edi_Main.TextArea.Caret.Line);
            string lineText = edi_Main.Document.GetText(line.Offset, line.Length);
            if (lineText == "")
            {
                isTriggered = false;
            }
            if (lineText == " ")
            {
                isTriggered = false;
            }
            return isTriggered;
        }

        private List<string> GetCodeSuggestions(string inputText)
        {
            // 从输入文本中获取当前光标所在的单词
            int caretOffset = edi_Main.CaretOffset;
            string textBeforeCaret = inputText.Substring(0, caretOffset);

            string[] words = textBeforeCaret.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            string currentWord = words.LastOrDefault();

            switch (Global.NowProgramMode) {
                case ProgramMode.Python:
                    codeSuggestions = CodeSuggestion.pyCodeSug;
                    break;
                case ProgramMode.C:
                    codeSuggestions = CodeSuggestion.CCodeSug;
                    break;
                case ProgramMode.Java:
                    codeSuggestions = CodeSuggestion.javaCodeSug;
                    break;
                default:
                    break;


            }
            // 使用正则表达式匹配所有的单词
            string pattern = @"\b\w+\b";
            MatchCollection matches = Regex.Matches(edi_Main.Text, pattern);

            List<string> contextWords = new List<string>();
             //将匹配到的单词添加到列表中
            foreach (Match match in matches)
            {
                contextWords.Add(match.Value);
            }
            if (contextWords.Count > 0)
            {
                List<string> filteredSuggestions = codeSuggestions.FindAll(s => s.StartsWith(currentWord));
                filteredSuggestions.AddRange(contextWords.FindAll(s => s.StartsWith(currentWord)));

                //去重算法
                bool endAble = false;
                while (true)
                {
                    bool breakAble = false;
                    if (filteredSuggestions.Count != 0)
                    {
                        foreach (var item in filteredSuggestions)
                        {
                            int count = 0;
                            for (int i = 0; i < filteredSuggestions.Count; i++)
                            {
                                if (filteredSuggestions[i] == item && count > 1)
                                {
                                    filteredSuggestions.RemoveAt(i);
                                    breakAble = true;
                                }
                                else
                                {
                                    count++;
                                }
                            }
                            if (breakAble)
                            {
                                count = 0;
                                break;
                            }
                            endAble = true;
                        }
                        if (endAble) { break; }
                    }
                    else { break; }

                }
                try
                {
                    filteredSuggestions.Remove(matches[matches.Count - 1].Value);

                }
                catch (Exception)
                {
                    return filteredSuggestions;
                }
                return filteredSuggestions;
            }
            else {
                return new List<string>();
            }
          

        }
        #endregion


        /// <summary>
        /// 运行python脚本的方法
        /// </summary>
        private void RunPython()
        {
            //Send("ping 8.8.8.8");
            
            string name = edi_ClassName.Text;
            //将代码写入本地
            using (FileStream fsWrite = new FileStream(Global.CachePath + @"\" + name + ".py", FileMode.OpenOrCreate, FileAccess.Write))
            {
                fsWrite.SetLength(0);
                byte[] buffer = Encoding.Default.GetBytes(edi_Main.Text);
                fsWrite.Write(buffer, 0, buffer.Length);
                //edi_cmd.Text += "\n -------------------编译代码中--------------------\n";
            }
            Send(
                //Global.PythonPath + "\\python.exe " + name + ".py " + Global.CachePath

            "cd " +Global.CachePath + " &"+ Global.PythonPath +"\\python.exe " + Global.CachePath+ "\\" +  name + ".py"
            ) ;
            edi_cmd.ScrollToEnd();
            
        }

        private void RunJava()
        {
            try
            {
                //-------------对java代码进行编译运行，字节码文件和java源码存储至缓存文件夹
                //获取编译的文件名
                string name = edi_ClassName.Text;
                //写操作
                using (FileStream fsWrite = new FileStream(Global.CachePath + @"\" + name + ".java", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fsWrite.SetLength(0);
                    byte[] buffer = Encoding.Default.GetBytes(edi_Main.Text);
                    fsWrite.Write(buffer, 0, buffer.Length);
                    edi_cmd.Text +="\n -------------------编译代码中--------------------\n";
                }
                //调用cmd命令进行编译，提示输出
                    Send(
                        "cd " + Global.CachePath + " &" + Global.JavaPath + "\\javac.exe " + name + ".java"
                        );

                //edi_cmd.Text += "\n\n-------以下为程序输出-------";
                //调用cmd命令运行，并输出

                    Send(
                        "cd " + Global.CachePath + " &" + Global.JavaPath + "\\java.exe " + name
                    );
            }
            //捕捉异常，啥都抓了，到时候会改进//
            //不改了（7.17注）
            catch (Exception e)
            {
                Directory.CreateDirectory(Global.CachePath);
                MessageBox.Show(e.ToString());
            }
            edi_cmd.ScrollToEnd();

        }
        private void RunC()
        {
            try
            {
                string name = edi_ClassName.Text;
                //将代码写入本地
                using (FileStream fsWrite = new FileStream(Global.CachePath + @"\" + name + ".c", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fsWrite.SetLength(0);
                    byte[] buffer = Encoding.Default.GetBytes(edi_Main.Text);
                    fsWrite.Write(buffer, 0, buffer.Length);
                    //edi_cmd.Text += "\n -------------------编译代码中--------------------\n";
                }
                //调用cmd命令运行脚本(tiny c compiler)
                Send(
                        @"cd " + Global.CachePath + " &"+ Global.CPath +
                        "\\tcc -run " +Global.CachePath+ "\\"+  name + ".c"
                    ); 
                //Send(
                //     Global.CachePath + "\\" + name + ".exe"
                //);
            }
            catch (Exception) {
            
            }
            edi_cmd.ScrollToEnd();
        }

        private void btn_new_Click(object sender, RoutedEventArgs e)
        {
            {

                if (!Saved)
                {
                    switch (System.Windows.MessageBox.Show( "当前文件未保存，是否保存？", "注意", MessageBoxButton.YesNoCancel))
                    {
                        //用户选择取消
                        case MessageBoxResult.Cancel:
                            return;

                        //用户选择否
                        case MessageBoxResult.No:
                            NowPath = "";
                            edi_Main.Text = "";
                            edi_cmd.Text = "已创建新文件\n";
                            return;

                        //用户选择是
                        case MessageBoxResult.Yes:
                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.Title = "保存";
                            sfd.InitialDirectory = @"C:\";
                            sfd.Filter = "Java文件| *.java";
                            sfd.ShowDialog();
                            string path = sfd.FileName;
                            if (path == "")
                            {
                                return;
                            }
                            using (FileStream fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                byte[] buffer = Encoding.Default.GetBytes(edi_Main.Text);
                                fsWrite.Write(buffer, 0, buffer.Length);
                                //MessageBox.Show("保存成功");

                            }
                            //NowPath = "";
                            edi_Main.Text = "";
                            edi_cmd.Text =  "已创建新文件\n";
                            return;

                        default:
                            break;
                    }

                }
            }
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            //文件未被保存且路径为空
            if (NowPath == "" || Saved == false)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title ="另存为";
                sfd.InitialDirectory = @"C:\";
                sfd.Filter = "Java文件| *.java";
                sfd.ShowDialog();
                string path = sfd.FileName;

                if (path == "")
                {
                    return;
                }

                using (FileStream fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    byte[] buffer = Encoding.Default.GetBytes(edi_Main.Text);
                    fsWrite.Write(buffer, 0, buffer.Length);
                    MessageBox.Show("保存成功");

                }
                NowPath = path;
                Saved = true;
            }
            else
            {
                using (FileStream fsWrite = new FileStream(NowPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    byte[] buffer = Encoding.Default.GetBytes(edi_Main.Text);
                    fsWrite.Write(buffer, 0, buffer.Length);
                    MessageBox.Show("保存成功");

                }
            }
        }

        private void edi_Main_DocumentChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        /// <summary>
        /// 拖入文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edi_Main_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = false;
        }

        /// <summary>
        /// 拖入文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edi_Main_PreviewDrop(object sender, DragEventArgs e)
        {

            string msg = "Drop";
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                msg = ((System.Array)e.Data.GetData(System.Windows.DataFormats.FileDrop)).GetValue(0).ToString();
                string path = msg;
                using (FileStream fsRead = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    byte[] buffer = new byte[1024 * 1024 * 5];
                    int r = fsRead.Read(buffer, 0, buffer.Length);
                    edi_Main.Text = Encoding.Default.GetString(buffer, 0, r);
                }
            }


        }

        /// <summary>
        /// 执行代码按钮被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            /*
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Global.CachePath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("未知错误");
            }*/


            //如果还未对运行代码的文件名进行设置，则提示后直接返回
            if (edi_ClassName.Text == ("输入调试启动的类名") || edi_ClassName.Text == "")
            {
                MessageBox.Show("输入一个正确的类名");
                return;
            }

            edi_cmd.Text = "";
            //Global.NowProgramMode = ProgramMode.Java;
            //Global.NowProgramMode = ProgramMode.Python;
            //Global.NowProgramMode = ProgramMode.C;
            //判断当前使用的语言
            switch (Global.NowProgramMode)

            {
                
                //如果为java，运行RJ（）方法
                case ProgramMode.Java:
                    RunJava();
                    break;
                //如果为python，运行RP（）方法
                case ProgramMode.Python:
                    RunPython();
                    return;
                //如果为C，运行RC（）方法
                case ProgramMode.C:
                    RunC();
                    break;
                default:
                    break;
            }
            //输出框滚动至最底部
            edi_cmd.ScrollToEnd();
        }

        private void btn_upload_Click(object sender, RoutedEventArgs e)
        {
            if (!OnlineManeger.isLoged)
            {
                MessageBox.Show("未登录");
                return ;
            }
            try
            {
                if (OnlineManeger.SaveCodeInCloud(edi_ClassName.Text, edi_Main.Text, Global.NowProgramMode))
                {
                    MessageBox.Show("已上传");
                }

            }
            catch (Exception)
            {
                //MessageBox.Show("未知错误");
            }
        }

        public void Refresh() {
            edi_Main.Text = Global.CodeChche;
            edi_ClassName.Text = string.Empty;
        }


    }
}

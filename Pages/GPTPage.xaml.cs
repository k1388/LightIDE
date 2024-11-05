
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Light_IDE.Other;
using static OpenAI.GPT3.ObjectModels.SharedModels.IOpenAiModels;

namespace Light_IDE.Pages
{
    /// <summary>
    /// GPTPage.xaml 的交互逻辑
    /// </summary>
    public partial class GPTPage : Page
    {
        public GPTPage()
        {
            InitializeComponent();
            //Chat();

        }


        string key = Tools.UnBase64String("c2stTG9zeUIyTkxvcXdqYnhIYUdPenBUM0JsYmtGSjdSTnBIVkUzRU55RWlwMWZpd0lz");


        private async void Chat(string q)
        {
            
            Dispatcher.Invoke(() => {
                pro_load.Visibility = Visibility.Visible;
            });
            try
            {
                // ChatGPT API 相关信息
                string apiKey = Global.CustomKey==null||Global.CustomKey==""? key :Global.CustomKey ;
                string endpoint = "https://api.openai.com/v1/chat/completions";


                // 要发送的聊天消息
                string message = q;

                // 模型参数
                string model = "gpt-3.5-turbo";

                // 创建 HttpClient 实例
                using (HttpClient client = new HttpClient())
                {
                    // 设置请求头
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                    // 构建请求数据
                    var requestData = new
                    {
                        model,
                        messages = new[] { new { role = "system", content = message } },
                        temperature = Global.GPT_tem,
                        top_p = Global.GPT_tp

                    };

                    var json = JsonSerializer.Serialize(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // 创建 HttpRequestMessage
                    var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
                    request.Content = content;

                    // 发送请求并获取响应
                    var response = await client.SendAsync(request);

                    // 处理响应
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var document = JsonDocument.Parse(responseContent);
                    var root = document.RootElement;
                    dynamic jsonResponse = JsonSerializer.Deserialize<dynamic>(responseContent);

                    // 提取回复消息
                    string reply = root.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                    Dispatcher.Invoke(() =>
                    {
                        edi_a.Text = reply;
                    });

                }
            }
            catch (Exception)
            {
                Dispatcher.Invoke(() =>
                {
                    edi_a.Text = "网络连接错误！（由于政策原因，您需要处于能连接到OpenAI官网的网络条件下才能使用此功能）";
                });
            }
            finally {
                Dispatcher.Invoke(() => {
                    pro_load.Visibility = Visibility.Hidden;
                });
            }


        }
        private void btn_send_Click(object sender, RoutedEventArgs e)
        {
            edi_a.Clear();
            Chat(edi_q.Text);

        }

    }


}


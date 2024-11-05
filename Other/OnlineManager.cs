using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySqlConnector;

namespace Light_IDE.Other
{


    /// <summary>
    /// SQL注入异常
    /// </summary>
    class StringHasSymbolException : Exception
    {
        public StringHasSymbolException(string message) : base(message)
        {
        }
    }


    /// <summary>
    /// 进行所有云操作的类
    /// </summary>
    public class OnlineManeger
    {

        /// <summary>
        /// 数据库连接
        /// </summary>
        public static MySqlConnection connection;
        /// <summary>
        /// 用户名
        /// </summary>
        public static string Uname;
        /// <summary>
        /// 用户的唯一主键
        /// </summary>
        public static int UID;
        /// <summary>
        /// 在线状态
        /// </summary>
        private static bool isOnline;
        /// <summary>
        /// 登录状态
        /// </summary>
        public static bool isLoged;


        /// <summary>
        /// 连接到服务器
        /// </summary>
        public static void Connect()
        {
            //与数据库连接的信息
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            //用户名
            builder.UserID = Tools.UnBase64String("azEzODg=");
            //密码
            builder.Password = Tools.UnBase64String("WmtfNTE5ODgx");
            //服务器地址
            builder.Server = Tools.UnBase64String("bGlnaHRpZGUucndsYi5yZHMuYWxpeXVuY3MuY29t");
            //连接时的数据库
            builder.Database = Tools.UnBase64String("T1RFRWRpdG9y");
            builder.SslMode = MySqlSslMode.None;

            builder.Port = 3306;


            try
            {
                //定义与数据连接的链接
                MySqlConnection conn = new MySqlConnection(builder.ConnectionString);
                conn.Open();
                connection = conn;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("未能连接到服务器！");
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        public static void LogOff() {
            //清空用户名
            Uname = null;
            //设置登录状态
            isLoged = false;
            //
            UID = -1;
            //清空配置文件登录信息
            Global.Ini.IniWriteValue("Account", "UserName", "");
            Global.Ini.IniWriteValue("Account", "UserName", "");
        }

        /// <summary>
        /// 获取在线状态
        /// </summary>
        /// <returns>在线返回true</returns>
        public static bool GetOnlineState()
        {
            try
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Broken || connection.State == System.Data.ConnectionState.Closed)
                {
                    isOnline = false;
                }
                else
                {
                    isOnline = true;
                }
                return isOnline;
            }
            catch (Exception)
            {
                return false;
            }
            /*
            finally {
                connection.Close();
            }*/

        }

        /// <summary>
        ///  获取用户信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns></returns>
        public static bool GetUserMes(string UserName)
        {
            if (GetOnlineState())
            {
                try
                {
                    string sql = "select * from oteeditor_sign where UNAME = @UNAME";

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    MySqlParameter p1 = new MySqlParameter("@UNAME", MySqlDbType.VarChar);
                    p1.Value = UserName;
                    cmd.Parameters.Add(p1);


                        connection.Open();
                        var result = cmd.ExecuteReader();
                        if (result.HasRows)
                        {
                            UID = result.GetInt32(0);
                            Uname = result.GetString(1);

                        }


                    
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 清除所有云代码
        /// </summary>
        /// <returns></returns>
        public static bool ClearAllCode()
        {
            try
            {
                string sql = "delete from oteeditor_csave where ID = @UID";

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlParameter p1 = new MySqlParameter("@UID", MySqlDbType.Int32);
                p1.Value = UID;
                cmd.Parameters.Add(p1);


                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result >= 1)
                    {
                        return true;
                    }
                    else
                    {

                    }
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// 存储云代码
        /// </summary>
        /// <param name="code">代码文本</param>
        /// <param name="codeName">代码名</param>
        /// <param name="mode">代码类型</param>
        /// <returns></returns>
        public static bool SaveCodeInCloud(string codeName, string code, ProgramMode mode)
        {
            try
            {
                string sql = "insert into oteeditor_csave (ID,CODE,CTYPE,CNAME,CDATE) values (@UID,@CODE,@CTYPE,@CNAME,@CDATE)";

                MySqlCommand cmd = new MySqlCommand(sql, connection);

                MySqlParameter p0 = new MySqlParameter("@CNAME", MySqlDbType.VarChar);
                p0.Value = codeName;
                MySqlParameter p1 = new MySqlParameter("@UID", MySqlDbType.Int32);
                p1.Value = UID;
                MySqlParameter p2 = new MySqlParameter("@CODE", MySqlDbType.MediumText);
                p2.Value = code;
                MySqlParameter p3 = new MySqlParameter("@CTYPE", MySqlDbType.VarChar);
                MySqlParameter p4 = new MySqlParameter("@CDATE", MySqlDbType.VarChar);
                p4.Value = System.DateTime.Now.ToLocalTime().ToString();
                switch (mode)
                {
                    case ProgramMode.Java:
                        p3.Value = "Java";
                        break;
                    case ProgramMode.Python:
                        p3.Value = "Python";
                        break;
                    case ProgramMode.C:
                        p3.Value = "C";
                        break;
                    default:
                        break;
                }

                cmd.Parameters.Add(p0);
                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);
                cmd.Parameters.Add(p3);
                cmd.Parameters.Add(p4);


                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result >= 1)
                    {
                        return true;
                    }
                    else
                    {

                    }
                
            }
            catch (Exception)
            {
                return true;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

        /// <summary>
        /// 删除特定云代码文件
        /// </summary>
        /// <param name="codeName">文件名</param>
        /// <returns></returns>
        public static bool DeleteCode(string date)
        {
            /*      废弃的方案：按标题删除
            try
            {
                string sql = "delete from oteeditor_csave where CNAME = @CNAME";

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlParameter p1 = new MySqlParameter("@CNAME", MySqlDbType.VarChar);
                p1.Value = codeName;
                cmd.Parameters.Add(p1);


                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
            
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            */

            //按日期删除
            try
            {
                string sql = "delete from oteeditor_csave where CDATE = @CDATE";

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlParameter p1 = new MySqlParameter("@CDATE", MySqlDbType.VarChar);
                p1.Value = date;
                cmd.Parameters.Add(p1);


                connection.Open();
                int result = cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// 从云端加载代码
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// 
        public static CloudCodeStructure[] GetCodesFromCloud()
        {
            try
            {
                string sql = "select * from oteeditor_csave WHERE ID = @ID";
                string sqlC = "select count(*) from  oteeditor_csave";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlCommand cmdC = new MySqlCommand(sqlC, connection);
                MySqlParameter p1 = new MySqlParameter("@ID", MySqlDbType.Int32);
                p1.Value = UID;
                cmd.Parameters.Add(p1);


                    connection.Open();
                    int rowCount = int.Parse(cmdC.ExecuteScalar().ToString());
                    //connection.c();
                    //connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        CloudCodeStructure[] cloudCodes = new CloudCodeStructure[rowCount];
                        int count = 0;
                        while (reader.Read())
                        {

                            cloudCodes[count] = new CloudCodeStructure()
                            {
                                Code = reader[1].ToString(),
                                CodeType = reader[2].ToString(),
                                CodeTitle = reader[3].ToString(),
                                CodeDate = reader[4].ToString()

                            };
                            count++;

                        }
                        return cloudCodes;
                    }
                    else
                    {
                        return null;
                    }
                  
                
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                try
                {
                    connection.Close();

                }
                catch { }
                
            }

        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static int Login(string userName, string password)
        {
            try
            {
                string sql = "select * from oteeditor_sign where UNAME = @uname and UPSW = @psw";

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlParameter p1 = new MySqlParameter("@uname", MySqlDbType.VarChar);
                p1.Value = userName;
                cmd.Parameters.Add(p1);
                MySqlParameter p2 = new MySqlParameter("@psw", MySqlDbType.VarChar);
                p2.Value = password;
                cmd.Parameters.Add(p2);

                    connection.Open();

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        UID = int.Parse(reader[0].ToString());
                    isLoged = true;
                    return 0;

                    }
                    else
                    {
                        return 1;
                    }


                

            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                try
                {
                    connection.Close();

                }
                catch { }
                    }
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int SignUp(string userName, string password)
        {
            string sql = "select * from oteeditor_sign where UNAME = @uname ";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlParameter p1 = new MySqlParameter("@uname", MySqlDbType.VarChar);
            p1.Value = userName;
            cmd.Parameters.Add(p1);

            try
            {
                connection.Open();

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    UID = int.Parse(reader[0].ToString());
                    return 1;
                }

            }
            catch (Exception)
            {
                return -1;
            }
            finally {

                try
                {
                    connection.Close();

                }
                catch { }
            }



            string sql2 = "INSERT INTO oteeditor_sign (UNAME,UPSW) VALUES (@userN,@psw)";

            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
            MySqlParameter p2 = new MySqlParameter("@userN", MySqlDbType.VarChar);
            MySqlParameter p3 = new MySqlParameter("@psw", MySqlDbType.VarChar);
            p2.Value = userName;
            p3.Value = password;
            cmd2.Parameters.Add(p2);
            cmd2.Parameters.Add(p3);


                connection.Open();
                if (cmd2.ExecuteNonQuery() >= 1)
                {

                    return 0;
                }


            connection.Open();
            return 1;





        }



        /// <summary>
        /// SQL注入检查
        /// </summary>
        /// <param name="aimString">要检查的字符串</param>
        /// <returns></returns>
        public static void SQLDefend(string aimString)
        {
            Regex regex = new Regex(@"^[\u4E00-\u9FA5A-Za-z0-9_]+$");
            if (regex.IsMatch(aimString))
            {
                return;
            }
            else
            {
                throw new StringHasSymbolException("");
            }

        }
    }
}

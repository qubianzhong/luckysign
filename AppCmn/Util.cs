﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Web.UI;
using System.Data.OleDb;
using System.Collections;
using System.IO;

namespace AppCmn
{
    /// <summary>
    /// Summary description for Util.
    /// </summary>
    public class Util
    {
        private Util()
        {
        }

        private static string[] ChineseNum = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };

        public static bool IsDate(string date)
        {
            try
            {
                DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsZipCode(string zip)
        {
            // 避免全角字符
            try { int.Parse(zip); }
            catch { return false; }

            if (IsNaturalNumber(zip) && zip.Length == 6)
                return true;
            else
                return false;
        }

        public static bool IsMoney(string money)
        {
            try
            {
                Decimal.Parse(money);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool HasMoreRow(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool HasMoreRow(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return false;
            else
                return true;
        }

        //整理字符串到安全格式
        public static string SafeFormat(string strInput)
        {
            if (string.IsNullOrEmpty(strInput) == true)
                return string.Empty;

            return strInput.Trim().Replace("'", "''");
        }

        public static string ToSqlString(string paramStr)
        {
            return "'" + SafeFormat(paramStr) + "'";
        }

        public static string ToSqlLikeString(string paramStr)
        {
            return "'%" + SafeFormat(paramStr) + "%'";
        }
        public static string ToSqlLikeStringR(string paramStr)
        {
            return "'" + SafeFormat(paramStr) + "%'";
        }
        /// <summary>
        /// 传入的参数必须是'yyyy-MM-dd' 格式. 不另外检查
        /// </summary>
        /// <param name="paramDate"></param>
        /// <returns></returns>
        public static string ToSqlEndDate(string paramDate)
        {
            return ToSqlString(paramDate + " 23:59:59");
        }


        public static string TrimNull(Object obj)
        {
            if (obj is System.DBNull)
            {
                return AppConst.StringNull;
            }
            else
            {
                return obj.ToString().Trim();
            }
        }

        public static int TrimIntNull(Object obj)
        {
            if (obj is System.DBNull)
            {
                return AppConst.IntNull;
            }
            else
            {
                return Int32.Parse(obj.ToString());
            }
        }

        public static decimal TrimDecimalNull(Object obj)
        {
            if (obj is System.DBNull)
            {
                return AppConst.DecimalNull;
            }
            else
            {
                return decimal.Parse(obj.ToString());
            }
        }
        public static DateTime TrimDateNull(Object obj)
        {
            if (obj is System.DBNull)
            {
                return AppConst.DateTimeNull;
            }
            else
            {
                return DateTime.Parse(obj.ToString());
            }
        }
        public static string GetMD5(String str)
        {
            ///byjerry  ////暂时取消md5签名
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5").ToLower();
            //////////////////////////////////////////////////////

            //return str;

            //			byte[] data = Encoding.Unicode.GetBytes(str);
            //			MD5 md = new MD5CryptoServiceProvider();
            //			byte[] result = md.ComputeHash(data);
            //			return Encoding.Unicode.GetString(result);
        }
        // Function to test for Positive Integers.  
        public static bool IsNaturalNumber(String strNumber)
        {
            Regex objNotNaturalPattern = new Regex("[^0-9]");
            Regex objNaturalPattern = new Regex("0*[1-9][0-9]*");
            return !objNotNaturalPattern.IsMatch(strNumber) &&
                objNaturalPattern.IsMatch(strNumber);
        }

        // Function to test for Positive Integers with zero inclusive  
        public static bool IsWholeNumber(String strNumber)
        {
            Regex objNotWholePattern = new Regex("[^0-9]");
            return !objNotWholePattern.IsMatch(strNumber);
        }

        // Function to Test for Integers both Positive & Negative  

        public static bool IsInteger(String strNumber)
        {
            Regex objNotIntPattern = new Regex("[^0-9-]");
            Regex objIntPattern = new Regex("^-[0-9]+$|^[0-9]+$");
            return !objNotIntPattern.IsMatch(strNumber) && objIntPattern.IsMatch(strNumber);
        }

        // Function to Test for Positive Number both Integer & Real 

        public static bool IsPositiveNumber(String strNumber)
        {
            Regex objNotPositivePattern = new Regex("[^0-9.]");
            Regex objPositivePattern = new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            return !objNotPositivePattern.IsMatch(strNumber) &&
                objPositivePattern.IsMatch(strNumber) &&
                !objTwoDotPattern.IsMatch(strNumber);
        }

        // Function to test whether the string is valid number or not
        public static bool IsNumber(String strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");
            return !objNotNumberPattern.IsMatch(strNumber) &&
                !objTwoDotPattern.IsMatch(strNumber) &&
                !objTwoMinusPattern.IsMatch(strNumber) &&
                objNumberPattern.IsMatch(strNumber);
        }

        // Function To test for Alphabets. 
        public static bool IsAlpha(String strToCheck)
        {
            Regex objAlphaPattern = new Regex("[^a-zA-Z]");
            return !objAlphaPattern.IsMatch(strToCheck);
        }
        // Function to Check for AlphaNumeric.
        public static bool IsAlphaNumeric(String strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex("[^a-zA-Z0-9]");
            return !objAlphaNumericPattern.IsMatch(strToCheck);
        }
        public static bool IsEmailAddress(string strEmailAddress)
        {
            Regex objNotEmailAddress = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return objNotEmailAddress.IsMatch(strEmailAddress);
        }
        /// <summary>
        ///check for InternetURL 
        ///add by shadow 2007-01-23
        /// </summary>
        /// <param name="strURL"></param>
        /// <returns></returns>
        public static bool IsInternetURL(string strURL)
        {
            Regex objInternetURL = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            Regex objInternetUrl = new Regex(@"([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            Regex objUrl = new Regex("(" + objInternetURL + ")|(" + objInternetUrl + ")");
            return objUrl.IsMatch(strURL);
        }
        // Function Format Money 
        public static decimal ToMoney(string moneyStr)
        {
            return decimal.Round(decimal.Parse(moneyStr), 2);
        }
        public static decimal ToMoney(decimal moneyValue)
        {
            return decimal.Round(moneyValue, 2);
        }
        /// <summary>
        /// 舍去金额的分,直接舍去,非四舍五入
        /// </summary>
        /// <param name="moneyValue"></param>
        /// <returns></returns>
        public static decimal TruncMoney(decimal moneyValue)
        {
            int tempAmt = Convert.ToInt32(moneyValue * 100) % 10;
            moneyValue = (decimal)((moneyValue * 100 - tempAmt) / 100m);
            return moneyValue;
        }
        /// <summary>
        /// 判断是否手机号码
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static bool IsCellNumber(string cell)
        {
            if (string.IsNullOrEmpty(cell) == true)
                return false;

            try
            {
                // 验证为数字，防止全角字符
                Convert.ToInt64(cell);

                return Regex.IsMatch(cell, @"^1[3|5]\d{9}$");
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 简单的身份证验证
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static bool IsIDCard(string idCard)
        {
            if (string.IsNullOrEmpty(idCard) == true)
                return false;

            try
            {

                return Regex.IsMatch(idCard, @"(^\d{15}$)|(\d{17}(?:\d|x|X)$)");
            }
            catch
            {
                return false;
            }
        }

        public static void ToExcel(System.Web.UI.WebControls.DataGrid DataGrid2Excel, string FileName, string Title)
        {
            ToExcel(DataGrid2Excel, FileName, Title, "");
        }

        /// <summary>
        /// Renders the html text before the datagrid.
        /// </summary>
        /// <param name="writer">A HtmlTextWriter to write html to output stream</param>
        private static void FrontDecorator(HtmlTextWriter writer)
        {
            writer.WriteFullBeginTag("HTML");
            writer.WriteFullBeginTag("Head");
            //			writer.RenderBeginTag(HtmlTextWriterTag.Style);
            //			writer.Write("<!--");
            //			
            //			StreamReader sr = File.OpenText(CurrentPage.MapPath(MY_CSS_FILE));
            //			String input;
            //			while ((input=sr.ReadLine())!=null) 
            //			{
            //				writer.WriteLine(input);
            //			}
            //			sr.Close();
            //			writer.Write("-->");
            //			writer.RenderEndTag();
            writer.WriteEndTag("Head");
            writer.WriteFullBeginTag("Body");
        }

        /// <summary>
        /// Renders the html text after the datagrid.
        /// </summary>
        /// <param name="writer">A HtmlTextWriter to write html to output stream</param>
        private static void RearDecorator(HtmlTextWriter writer)
        {
            writer.WriteEndTag("Body");
            writer.WriteEndTag("HTML");
        }

        public static void ToExcel(System.Web.UI.WebControls.DataGrid DataGrid2Excel, string FileName, string Title, string Head)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

            FrontDecorator(hw);
            if (Title != "")
                hw.Write(Title + "<br>");
            if (Head != "")
                hw.Write(Head + "<br>");

            DataGrid2Excel.EnableViewState = false;
            DataGrid2Excel.RenderControl(hw);

            RearDecorator(hw);

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.Buffer = true;
            response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-7");
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ".xls");
            response.Charset = "gb2312";
            response.Write(sw.ToString());
            response.End();
        }

        public static void ToExcel(DataTable dt, string FileName)
        {
            System.Web.UI.WebControls.DataGrid dgTemp = new System.Web.UI.WebControls.DataGrid();
            dgTemp.DataSource = dt;
            dgTemp.DataBind();
            ToExcel(dgTemp, FileName, "", "");
        }

        public static void Swap(ref int x, int y)
        {
            int temp = x;
            x = y;
            y = temp;
        }
        public static void Swap(ref decimal x, decimal y)
        {
            decimal temp = x;
            x = y;
            y = temp;
        }
        public static void Swap(ref string x, string y)
        {
            string temp = x;
            x = y;
            y = temp;
        }

        #region ChineseMoney
        private static string getSmallMoney(string moneyValue)
        {
            int intMoney = Convert.ToInt32(moneyValue);
            if (intMoney == 0)
            {
                return "";
            }
            string strMoney = intMoney.ToString();
            int temp;
            StringBuilder strBuf = new StringBuilder(10);
            if (strMoney.Length == 4)
            {
                temp = Convert.ToInt32(strMoney.Substring(0, 1));
                strMoney = strMoney.Substring(1, strMoney.Length - 1);
                strBuf.Append(ChineseNum[temp]);
                if (temp != 0)
                    strBuf.Append("仟");
            }
            if (strMoney.Length == 3)
            {
                temp = Convert.ToInt32(strMoney.Substring(0, 1));
                strMoney = strMoney.Substring(1, strMoney.Length - 1);
                strBuf.Append(ChineseNum[temp]);
                if (temp != 0)
                    strBuf.Append("佰");
            }
            if (strMoney.Length == 2)
            {
                temp = Convert.ToInt32(strMoney.Substring(0, 1));
                strMoney = strMoney.Substring(1, strMoney.Length - 1);
                strBuf.Append(ChineseNum[temp]);
                if (temp != 0)
                    strBuf.Append("拾");
            }
            if (strMoney.Length == 1)
            {
                temp = Convert.ToInt32(strMoney);
                strBuf.Append(ChineseNum[temp]);
            }
            return strBuf.ToString();
        }

        public static string GetChineseMoney(decimal moneyValue)
        {
            string result = "";
            if (moneyValue == 0)
                return "零";

            if (moneyValue < 0)
            {
                moneyValue *= -1;
                result = "负";
            }
            ///			int intMoney = Convert.ToInt32(Util.TruncMoney(moneyValue)*100); 
            ///			Update By Cindy 2006.07.06发票打印大写金额不截断“分”
            int intMoney = Convert.ToInt32(moneyValue * 100);
            string strMoney = intMoney.ToString();
            int moneyLength = strMoney.Length;
            StringBuilder strBuf = new StringBuilder(100);
            if (moneyLength > 14)
            {
                throw new Exception("Money Value Is Too Large");
            }

            //处理亿部分
            if (moneyLength > 10 && moneyLength <= 14)
            {
                strBuf.Append(getSmallMoney(strMoney.Substring(0, strMoney.Length - 10)));
                strMoney = strMoney.Substring(strMoney.Length - 10, 10);
                strBuf.Append("亿");
            }

            //处理万部分
            if (moneyLength > 6)
            {
                strBuf.Append(getSmallMoney(strMoney.Substring(0, strMoney.Length - 6)));
                strMoney = strMoney.Substring(strMoney.Length - 6, 6);
                strBuf.Append("万");
            }

            //处理元部分
            if (moneyLength > 2)
            {
                strBuf.Append(getSmallMoney(strMoney.Substring(0, strMoney.Length - 2)));
                strMoney = strMoney.Substring(strMoney.Length - 2, 2);
                strBuf.Append("元");
            }

            //处理角、分处理分
            if (Convert.ToInt32(strMoney) == 0)
            {
                strBuf.Append("整");
            }
            else
            {
                if (moneyLength > 1)
                {
                    int intJiao = Convert.ToInt32(strMoney.Substring(0, 1));
                    strBuf.Append(ChineseNum[intJiao]);
                    if (intJiao != 0)
                    {
                        strBuf.Append("角");
                    }
                    strMoney = strMoney.Substring(1, 1);
                }

                int intFen = Convert.ToInt32(strMoney.Substring(0, 1));
                if (intFen != 0)
                {
                    strBuf.Append(ChineseNum[intFen]);
                    strBuf.Append("分");
                }
            }
            string temp = strBuf.ToString();
            while (temp.IndexOf("零零") != -1)
            {
                strBuf.Replace("零零", "零");
                temp = strBuf.ToString();
            }

            strBuf.Replace("零亿", "亿");
            strBuf.Replace("零万", "万");
            strBuf.Replace("亿万", "亿");

            return result + strBuf.ToString();
        }
        #endregion

        public static string DataTableToExcel(DataTable dt, string excelName)
        {
            if (dt == null)
            {
                return "DataTable不能为空";
            }

            int rows = dt.Rows.Count;
            int cols = dt.Columns.Count;
            StringBuilder sb;

            if (rows == 0)
            {
                return "没有数据";
            }

            sb = new StringBuilder();

            string physicPath = AppConfig.ErrorLogFolder;

            if (System.IO.File.Exists(physicPath + excelName))
                System.IO.File.Delete(physicPath + excelName);

            string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + physicPath + excelName + ";Extended Properties=Excel 8.0;";


            //生成创建表的脚本
            sb.Append("CREATE TABLE ");
            sb.Append(dt.TableName + " ( ");

            for (int i = 0; i < cols; i++)
            {
                string oleColumnType = dt.Columns[i].DataType.Name;

                if (dt.Columns[i].DataType == System.Type.GetType("System.Decimal"))
                    oleColumnType = "decimal";
                else if (dt.Columns[i].DataType == System.Type.GetType("System.Int32")
                    || dt.Columns[i].DataType == System.Type.GetType("System.Int")
                    )

                    oleColumnType = "int";

                if (i < cols - 1)
                    sb.Append(string.Format("{0} " + oleColumnType + ",", dt.Columns[i].ColumnName));
                else
                    sb.Append(string.Format("{0} " + oleColumnType + ")", dt.Columns[i].ColumnName));
            }

            using (OleDbConnection objConn = new OleDbConnection(connString))
            {
                OleDbCommand objCmd = new OleDbCommand();
                objCmd.Connection = objConn;

                objCmd.CommandText = sb.ToString();

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    return "在Excel中创建表失败，错误信息：" + e.Message;
                }

                #region 生成插入数据脚本
                sb.Remove(0, sb.Length);
                sb.Append("INSERT INTO ");
                sb.Append(dt.TableName + " ( ");

                for (int i = 0; i < cols; i++)
                {
                    if (i < cols - 1)
                        sb.Append(dt.Columns[i].ColumnName + ",");
                    else
                        sb.Append(dt.Columns[i].ColumnName + ") values (");
                }

                for (int i = 0; i < cols; i++)
                {
                    if (i < cols - 1)
                        sb.Append("@" + dt.Columns[i].ColumnName + ",");
                    else
                        sb.Append("@" + dt.Columns[i].ColumnName + ")");
                }
                #endregion


                //建立插入动作的Command
                objCmd.CommandText = sb.ToString();
                OleDbParameterCollection param = objCmd.Parameters;

                for (int i = 0; i < cols; i++)
                {
                    param.Add(new OleDbParameter("@" + dt.Columns[i].ColumnName, OleDbType.VarChar));
                }

                //遍历DataTable将数据插入新建的Excel文件中
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < param.Count; i++)
                    {
                        param[i].Value = row[i];
                    }

                    objCmd.ExecuteNonQuery();
                }

                return "数据已成功导入Excel";
            }//end using
        }

        #region		Added by SeanQu 2006/05/08

        /// <summary>
        /// 判断是否为合法的数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValidNumber(string value)
        {
            return Regex.IsMatch(value, @"^\d*$");
        }


        /// <summary>
        /// 将一个新的字符串添加到旧字符串的的最前面，并且将旧字符串中含有与新字符串相同字符串的去掉，
        /// 分隔符为自定义
        /// </summary>
        /// <param name="insertString"></param>
        /// <param name="oldString"></param>
        /// <param name="compartChar"></param>
        /// <returns></returns>
        public static string SortWithInsertNewString(string insertString, string oldString, char compartChar)
        {
            string[] oldStringList = oldString.Split(new char[] { compartChar });
            string newString = "";
            ArrayList newStringList = new ArrayList();

            newStringList.Add(insertString);

            for (int i = 0; i < oldStringList.Length; i++)
            {
                if (oldStringList[i] != insertString)
                {
                    newStringList.Add(oldStringList[i]);
                }
            }

            for (int i = 0; i < newStringList.Count; i++)
            {
                if (newString == "")
                {
                    newString = newStringList[i].ToString();
                }
                else
                {
                    newString += compartChar.ToString() + newStringList[i].ToString();
                }
                if (i == 9)
                {
                    break;
                }
            }
            return newString;
        }

        /// <summary>
        /// Added by seanqu 2006/06/07
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetAbsoluteFilePath(string filePath)
        {
            string file = filePath;
            if (!filePath.Substring(1, 1).Equals(":")
                && !filePath.StartsWith("\\"))
            {
                file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            }

            return file.Replace("/", "\\");
        }

        #endregion

        #region HBX 方法  -- David Liu @ 2006-05-31

        public static string GetPageName(string URL)
        {
            int tmpPos1 = 0;
            int tmpPos2 = 0;
            string tmpURL1 = "";
            string tmpURL2 = "";

            if (URL == null || URL.Trim().Length == 0)
                return string.Empty;

            tmpPos1 = URL.LastIndexOf("/");
            tmpURL1 = URL.Substring(tmpPos1);
            tmpPos2 = tmpURL1.IndexOf(".");
            tmpURL2 = tmpURL1.Substring(1, tmpPos2 - 1);

            return tmpURL2;
        }

        #endregion

        public static string RemoveHtmlTag(string str)
        {
            //Regex regex  = new Regex(@"<([\s-\S][^>]*)?>");
            //Regex regex2 = new Regex(@"(\w(\.|,|\/))");

            //return regex2.Replace(regex.Replace(str,""),"");

            Regex reg = new Regex(@"<\/*[^<>]*>");
            return reg.Replace(str, "");
        }
        public static string GetLastName(string str)
        {
            string[] names = str.Split('_');
            string lastName = str;
            if (names.Length <= 1)
                return lastName;
            lastName = "";
            for (int i = 1; i < names.Length; i++)
            {
                if (lastName.Length != 0)
                    lastName = lastName + "_";
                lastName = lastName + names[i];
            }
            return lastName;
        }
    }
}

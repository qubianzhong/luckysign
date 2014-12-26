﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Collections.Generic;
using Com.Alipay;
using AppMod.Order;
using AppBll.Order;
using AppCmn;
using AppMod.QA;
using AppBll.QA;
using WebMonitor;

namespace WebForMain.Order
{
    public partial class PayNotifyForAlipay : PageBase
    {
        private string orderID = "";
        private int ordertype = 0;
        private bool succ = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码


                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    //商户订单号

                    string out_trade_no = Request.Form["out_trade_no"];
                    orderID = out_trade_no;
                    //支付宝交易号

                    string trade_no = Request.Form["trade_no"];

                    //交易状态
                    string trade_status = Request.Form["trade_status"];


                    if (Request.Form["trade_status"] == "TRADE_FINISHED" || Request.Form["trade_status"] == "TRADE_SUCCESS")
                    {
                        succ = true;
                        if (out_trade_no.Contains("C"))
                        {
                            ordertype = 1;
                        }
                        else if (out_trade_no.Contains("P"))
                        {
                            ordertype = 2;
                        }
                        ORD_CashMod m_mod = ORD_CashBll.GetInstance().GetModelByOrderID(out_trade_no);
                        if (m_mod == null)
                        {
                            ShowError("");//订单号错误
                        }
                        if (m_mod.Status == (int)AppEnum.CashOrderStatus.beforepay)
                        {
                            m_mod.CurrentID = trade_no;//记录支付流水号
                            ORD_CashBll.GetInstance().SetPaySucc(m_mod);
                        }
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序

                        //注意：
                        //该种交易状态只在两种情况下出现
                        //1、开通了普通即时到账，买家付款成功后。
                        //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。
                    }
                    //else if (Request.Form["trade_status"] == "TRADE_SUCCESS")
                    //{
                    //    //判断该笔订单是否在商户网站中已经做过处理
                    //    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                    //    //如果有做过处理，不执行商户的业务程序

                    //    //注意：
                    //    //该种交易状态只在一种情况下出现——开通了高级即时到账，买家付款成功后。
                    //}
                    else
                    {
                        LogManagement.getInstance().WriteTrace("订单" + orderID + "支付宝返回" + Request.QueryString["trade_status"], "PayReturnForAlipay", base.Request.UserHostAddress);
                    }

                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    Response.Write("success");  //请不要修改或删除

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    Response.Write("fail");
                }
            }
            else
            {
                Response.Write("无通知参数");
            }
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }
    }
}
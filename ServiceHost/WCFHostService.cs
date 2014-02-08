﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using XMS.Core;
using XMS.Core.WCF;
using XMS.Core.Tasks;

namespace ServiceHost
{
    public partial class WCFHostService : ServiceBase
    {
        public WCFHostService()
        {
            InitializeComponent();

            // 向服务管理器中注册服务
            //ManageableServiceHostManager.Instance.RegisterService(typeof(WCFServiceForApp.CustomerService));

            // 向任务管理器中注册任务
            TaskManager.Instance.DefaultTriggerTaskHost.RegisterTriggerTask(new WCFServiceForApp.Task.RewardTask("RewardTask", "RewardTask"));
        }

        protected override void OnStart(string[] args)
        {
            // 启动服务管理器
            //ManageableServiceHostManager.Instance.Start();

            // 启动任务管理器
            TaskManager.Instance.Start();

            base.OnStart(args);
        }

        protected override void OnStop()
        {
            // 停止服务管理器
            //ManageableServiceHostManager.Instance.Stop();

            // 停止任务管理器
            TaskManager.Instance.Stop();

            base.OnStop();
        }
    }
}

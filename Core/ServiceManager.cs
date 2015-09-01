using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventEditor.JustNetwork
{
    /// <summary>
    /// 服务管理者，管理系统中用到的所有服务
    /// </summary>
    public class ServiceManager : IDisposable
    {
        private static Dictionary<string, ServiceInterface> services = new Dictionary<string, ServiceInterface>();

        static ServiceManager()
        { }

        /// <summary>
        /// 获取一个服务
        /// </summary>
        /// <param name="serviceName">服务的名字</param>
        /// <returns>服务</returns>
        public static ServiceInterface GetService(string serviceName)
        {
            Dictionary<string, ServiceInterface> sv = services;
            if(sv.ContainsKey(serviceName))
            {
                return sv[serviceName];
            }
            return null;
        }

        /// <summary>
        /// 安装一个服务
        /// </summary>
        /// <param name="service">需要安装的服务</param>
        /// <param name="SerivceName">需要安装的服务的名字</param>
        public static void InstallService(ServiceInterface service, string ServiceName)
        {
            Dictionary<string, ServiceInterface> sv = services;
            if (sv.ContainsKey(ServiceName))
            {
                UnInstallSerive(ServiceName);
            }
            

            //启动服务
            service.OnServiceStart();
            sv.Add(ServiceName, service);
        }

        /// <summary>
        /// 取消安装一个服务
        /// </summary>
        /// <param name="ServiceName">服务名称</param>
        public static void UnInstallSerive(string ServiceName)
        {
            Dictionary<string, ServiceInterface> sv = services;
            ServiceInterface service = null;

            if (sv.ContainsKey(ServiceName))
            {
                service = sv[ServiceName];

                //关闭服务
                service.OnServiceStop();
                sv.Remove(ServiceName);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dictionary<string, ServiceInterface> sv = services;

            foreach(string ServiceName in sv.Keys)
            {
                UnInstallSerive(ServiceName);
            }
        }
    }
}

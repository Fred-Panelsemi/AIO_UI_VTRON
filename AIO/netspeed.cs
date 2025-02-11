using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace AIO
{
    class netspeed
    {
        public static void pingResult(ref List<string> lst, string ipStr)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            string data = "test data";
            byte[] buff = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = pingSender.Send(ipStr, timeout, buff, options);
            if(reply.Status == IPStatus.Success)
            {
                lst.Add("答覆的主機地址: " + reply.Address.ToString());
                lst.Add("往返時間: " + reply.RoundtripTime);
                lst.Add("生存時間(TTL): " + reply.Options.Ttl);
                lst.Add("是否控制數據包的分段: " + reply.Options.DontFragment);
                lst.Add("緩衝區大小: " + reply.Buffer.Length);
            }
            else
            {
                lst.Add(reply.Status.ToString());
            }
        }





        #region NetworkInterface.Speed 屬性 from https://learn.microsoft.com/zh-tw/dotnet/api/system.net.networkinformation.networkinterface.speed?view=net-7.0
        public static void ShowInterfaceSpeedAndQueue()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                IPv4InterfaceStatistics stats = adapter.GetIPv4Statistics();
                Console.WriteLine(adapter.Description);
                Console.WriteLine("     Speed .................................: {0}",
                    adapter.Speed);
                Console.WriteLine("     Output queue length....................: {0}",
                    stats.OutputQueueLength);
            }
        }
        #endregion NetworkInterface.Speed 屬性 from https://learn.microsoft.com/zh-tw/dotnet/api/system.net.networkinformation.networkinterface.speed?view=net-7.0



        #region 获取实时网速利用PerformanceCounter以及NetworkInterface, from https://blog.csdn.net/chscomfaner/article/details/82784999
        public static void speed()
        {
            List<PerformanceCounter> pcs = new List<PerformanceCounter>();
            List<PerformanceCounter> pcs2 = new List<PerformanceCounter>();
            string[] names = getAdapter();
            foreach (string name in names)
            {
                try
                {
                    //PerformanceCounter pc = new PerformanceCounter("Network Interface", "Byte Received/sec", name.Replace('(', '[').Replace(')', ']'), ".");
                    //PerformanceCounter pc2 = new PerformanceCounter("Network Interface", "Byte Received/sec", name.Replace('(', '[').Replace(')', ']'), ".");

                    PerformanceCounter pc = new PerformanceCounter("Inter(R) Wireless", "Byte Received/sec", name.Replace('(', '[').Replace(')', ']'), ".");
                    PerformanceCounter pc2 = new PerformanceCounter("Network Interface", "Byte Received/sec", name.Replace('(', '[').Replace(')', ']'), ".");

                    pc.NextValue();
                    pcs.Add(pc);
                    pcs2.Add(pc2);
                }
                catch
                {
                    continue;
                }
            }
            ParameterizedThreadStart ts = new ParameterizedThreadStart(run);
            Thread monitor = new Thread(ts);
            List<PerformanceCounter>[] pcss = new List<PerformanceCounter>[1];
            pcss[0] = pcs;
            pcss[1] = pcs2;
            monitor.Start(pcss);
        }
        public static void run(object obj)
        {
            List<PerformanceCounter>[] pcss = (List<PerformanceCounter>[])obj;
            List<PerformanceCounter> pcs = pcss[0];
            List<PerformanceCounter> pcs2 = pcss[1];
            while (true)
            {
                long recv = 0;
                long sent = 0;
                foreach(PerformanceCounter pc in pcs)
                {
                    recv += Convert.ToInt32(pc.NextValue()) / 1000;
                }
                foreach(PerformanceCounter pc in pcs2)
                {
                    sent += Convert.ToInt32(pc.NextValue()) / 1000;
                }
                Console.Write(recv + "mbps" + ",sent:" + sent + "mbps");
                Thread.Sleep(500);
            }
         }
        public static string[] getAdapter()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            string[] name = new string[adapters.Length];
            int index = 0;
            foreach (NetworkInterface ni in adapters)
            {
                name[index] = ni.Description;
                index++;
            }
            return name;
        }
        #endregion 获取实时网速利用PerformanceCounter以及NetworkInterface


    }
}

using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
/// <summary>
/// 域名相当于IP地址的别名
/// 如果不知道对方的IP地址，想通过域名与对方连接
/// 则用Dns类获得IPHostEntry类(里面有IP地址 别名 对方的DNS主机的名称)信息
/// </summary>
public class Lesson2 : MonoBehaviour
{
    //域名解析，就是域名到IP地址的转换过程，域名www.baidu.com，ip：118.22.33.44（ipv4）
    //域名解析工作由DNS服务器完成（Domain Name System,为了方便记忆，所以由域名）

    private void Start()
    {
        /// <summary>
        /// 能够获取关联IP  AddressList
        /// 获取主机列表别名 Aliases
        /// 获取DNS名称  Hostname
        /// </summary>
        /// <returns></returns>
        IPHostEntry iPHostEntry = new IPHostEntry();//一般不会这么用，一般用Dns类获取了赋值给他
        IPAddress[] iPAddresses = iPHostEntry.AddressList;
        string[] names = iPHostEntry.Aliases;
        string hostName =iPHostEntry.HostName;//主机名称


        //Dns类，可以更具它提供的静态方法获取IP地址
        //获取本机系统的主机名
        Debug.Log(Dns.GetHostName());

        //获取指定域名的IP地址
        //同步获取，获取远程主机，可能会阻塞主线程
        IPHostEntry entry =Dns.GetHostEntry("www.baidu.com");
        //结果www.baidu.com关联了两个IP地址
        for (int i = 0; i < entry.AddressList.Length; i++)
        {
            print("IP地址:"+entry.AddressList[i].ToString());
        }
        //别名是没有的
        for (int i = 0; i < entry.Aliases.Length; i++)
        {
            print("主机别名"+entry.AddressList[i]);
        }
        //www.a.shifen.com为DNS服务器名称
        print("DNS服务器名称"+entry.HostName);

        //这样主线程不会阻塞
        GetHostEntry();
    }
    //异步获取IP地址
    private async void GetHostEntry()
    {
        Task<IPHostEntry> task = Dns.GetHostEntryAsync("www.baidu.com");
        await task;//等task执行完之后执行后面的类容
        //task.Result就是得到的IPHostEntry类
        //结果www.baidu.com关联了两个IP地址
        for (int i = 0; i < task.Result.AddressList.Length; i++)
        {
            print("IP地址:"+task.Result.AddressList[i].ToString());
        }
        //别名是没有的
        for (int i = 0; i < task.Result.Aliases.Length; i++)
        {
            print("主机别名"+task.Result.AddressList[i]);
        }
        //www.a.shifen.com为DNS服务器名称
        print("DNS服务器名称"+task.Result.HostName);
    }

}

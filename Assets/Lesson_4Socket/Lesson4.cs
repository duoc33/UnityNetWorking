using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Lesson4 : MonoBehaviour
{
   private void Start() {
      #region Socket 作用
      //Socket套接字 支持TCP/IP网络通信的基本操作单位
      //一个Socket套接字对象包含的关键信息有：
      //1.本机的IP地址和端口
      //2.对方主机的IP地址和端口
      //3.双方通信的协议信息

      //可以视为一个数据通道，连接服务端与客户端，一般制作长连接游戏，就会使用socket套接字通信方案
      #endregion


      #region Socket 类型  构造参数 通过构造函数声明不同类型的套接字
      //1.流套接字
      //主要用于实现TCP通信，面向连接、可靠的、有序的、数据无差错的且无重复的数据传输服务。

      //2.数据报套接字
      //主要用于实现UDP通信，提供了无连接的，数据包不超过32kb，不提供正确型，可能出现丢失，可能出现重发。

      //3.原始套接字(不常用)
      //主要用于IP数据包通信，用于直接访问协议的较低层，常用于侦听和分析数据包

      //通过构造函数声明不同类型的套接字,构造时一共有3个重要参数AddressFamily、SocketType、ProtocolType
      
      //AddressFamily 网络寻址方案类，枚举类型，解决寻址方案。
      //常用
      //1.IPv4寻址 InterNetwork
      //2.IPv6寻址 InterNetwork6
      //了解的：
      //1.UNIX UNIX本地到主机地址
      //2.ImpLink ARPANETIMP地址
      //3.Ipx
      //4.Iso
      //5.Osi 等协议地址
      //AddressFamily.InterNetwork;   

      //SocketType 套接字枚举类型，决定使用的套接字类型
      //常用
      //1.Dgram 支持数据报，最大长度固定无连接，不可靠消息(用于UDP通信)
      //2.Stream 支持可靠的，双向的，基于连接的字节流(主要用于TCP通信)
      //了解的
      //1.Raw   支持对基础协议的访问
      //2.Rdm   支持无连接、面向消息、以可靠方式发送信息
      //3.Seqpacket 提供排序字节流的面向连接且可靠的双向传输
      
      //ProtocolType 协议类型枚举，决定套接字使用的通讯协议。
      //常用
      //1.TCP  传输控制协议
      //2.UDP  用户数据报协议
      //了解
      //1.IP IP网际协议
      //2.Icmp Icmp网际消息控制协议
      //3.Igmp Igmp网际组管理协议
      //4.Idp 等等等。。。。

      //三个参数的常用搭配
      // SocketType.Dgram  +  ProtocolType.Udp 用于UDP
      // SocketType.Stream  +  ProtocolType.Tcp 用TCP

      //TCP流套接字
      Socket sTcp=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
      //UDP数据报套接字
      Socket sUdp=new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);

      #endregion


      #region Socket 常用属性
      //Socket 
      if(sTcp.Connected){
         //判断是否处于连接状态
      }
      //获取套接字类型
      print(sTcp.SocketType);
      //获取套接字协议类型
      print(sTcp.ProtocolType);
      //获取套接字寻址方式
      print(sTcp.AddressFamily);
      //获取网络中获取准备读取的数据量(字节数)
      int byteNum =sTcp.Available;
      //获取本机EndPoint对象(IPEndPoint对象是IP地址和端口号的信息类,EndPoint是它的父类)
      // EndPoint endPoint = sTcp.LocalEndPoint;
      // IPEndPoint iPEndPoint=sTcp.LocalEndPoint as IPEndPoint;
      //获取远端EndPoint对象
      // EndPoint endPoint1 = sTcp.RemoteEndPoint;
      // IPEndPoint iPEndPoint1=sTcp.RemoteEndPoint as IPEndPoint;
      #endregion
      
      
      #region Socket 常用方法
      //1.用于服务端的方法
      //1.1给指定套接字绑定IP和端口号
      IPEndPoint ipPoint=new IPEndPoint(IPAddress.Parse("127.0.0.1"),8080);//IP和端口号相关信息
      sTcp.Bind(ipPoint);
      //1.2设置客户端最大连接数
      sTcp.Listen(999);
      //1.3等待客户端连入
      sTcp.Accept();

      //2.用于客户端的方法
      //2.1连接远程服务端
      sTcp.Connect(IPAddress.Parse("118.12.132.11"),8080);
      sTcp.Connect(sTcp.RemoteEndPoint);

      //3.C/S都会用的方法
      byte[] bytes=new byte[]{};
      //3.1同步发送和接收 相对应的也有异步方法
      sTcp.Send(bytes);//主要用于TCP ,sUdp.SendTo();主要用于Udp
      sTcp.Receive(bytes);
      //3.2释放连接并关闭Socket，先于Close调用
      sTcp.Shutdown(SocketShutdown.Receive);//停止接收
      sTcp.Shutdown(SocketShutdown.Send);//停止发送
      sTcp.Shutdown(SocketShutdown.Both);//同时停止接收和发送消息
      //3.3关闭连接，释放所有Socket关联资源
      sTcp.Close();
      
      #endregion
   }
}

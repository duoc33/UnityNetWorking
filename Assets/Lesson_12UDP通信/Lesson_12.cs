using System.Text;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Lesson_12 : MonoBehaviour
{
    private void Start()
    {
        #region 实现Udp客户端 通信基本流程
        //1.创建客户端套接字
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
       
        //2.绑定本机地址
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),8080);
        socket.Bind(iPEndPoint);
        
        //3.发送到指定目标 （服务器端口号就得是8081了）
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081);
        //指定要发送的字节 和 远程计算机的端口
        socket.SendTo(Encoding.UTF8.GetBytes("cc,here!"),remoteEndPoint);
        
        //4.接收消息（发送和接收都不能超过548个字节）
        byte[] bytes = new byte[512];
        //这个EndPoint变量主要用来装远端服务器的信息，它自己首次实例本没有含义，而是用它的实例去接收远端服务器信息
        EndPoint remoteEndPoint_1 = new IPEndPoint(IPAddress.Any, 0);
        int receiveNum=socket.ReceiveFrom(bytes,ref remoteEndPoint_1);
        print("IP:" + (remoteEndPoint_1 as IPEndPoint).Address.ToString() +
               "Port" + (remoteEndPoint_1 as IPEndPoint).Port +
              " 发来了 " + Encoding.UTF8.GetString(bytes, 0, receiveNum));

        //5.释放关闭
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
        #endregion
    }
}

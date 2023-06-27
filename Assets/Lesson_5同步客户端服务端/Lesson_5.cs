using System.Text;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Lesson_5 : MonoBehaviour
{
   private void Start() {
    #region 客户端需要做的
    //1.创建套接字
    //2.用Connect方法与服务器连接
    //3.用Send和Receive相关方法收发数据
    //4.用ShutDown方法释放连接
    //5.关闭套接字
    #endregion

    #region 实现逻辑
    Socket socket =new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
    IPEndPoint iPEndPoint=new IPEndPoint(IPAddress.Parse("127.0.0.1"),8080);
    try
    {
        socket.Connect(iPEndPoint);
    }
    //SocketException 套接字错误类
    catch (SocketException e)
    {
        //套接字错误码 属性，==10061时表示服务器拒绝了或者服务器没开
        if(e.ErrorCode==10061)
        print("Server refuse");
        else
        print("Server connet unsuccessfully");
        return;
    }

    byte[] recevieBytes = new byte[1024];
    int recevieBytesNum=socket.Receive(recevieBytes);
    //首先解析消息ID
    int msgID =BitConverter.ToInt32(recevieBytes,0);
    switch (msgID)
    {
        case 101:
        PlayerMsg msg=new PlayerMsg();
        msg.Reading(recevieBytes,4);
        print(msg.playerID);
        print(msg.playerData.name);
        print(msg.playerData.atk);
        print(msg.playerData.lev);
        break;
    }

    socket.Send(Encoding.UTF8.GetBytes("here is Client"));

    socket.Shutdown(SocketShutdown.Both);
    #endregion
    
   }
}

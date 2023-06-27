using System.Diagnostics;
using System;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Lesson13 : MonoBehaviour
{
    private byte[] cacheBytes = new byte[512];
    private void Start() {
        #region UDP异步方法
        Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
        #endregion


        #region Begin
        //BeginSendTo
        byte[] bytes = Encoding.UTF8.GetBytes("2131reafe1413d");
        EndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),8080);
        socket.BeginSendTo(bytes,0,bytes.Length,SocketFlags.None,ipPoint,SendToOver,socket);

        //BeginReceiveFrom
        socket.BeginReceiveFrom(cacheBytes, 0, cacheBytes.Length, SocketFlags.None,ref ipPoint, ReceiverFromOver, (socket,ipPoint));
        #endregion


        #region Async
        //SendToAsync
        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        //设置发送的数据
        args.SetBuffer(bytes,0,bytes.Length);
        //设置完成事件
        args.Completed += SendToCompleted;
        socket.SendToAsync(args);

        //ReceiveFromAsync
        SocketAsyncEventArgs args1 = new SocketAsyncEventArgs();
        //设置接收消息的容器
        args1.SetBuffer(cacheBytes, 0, cacheBytes.Length);
        args1.Completed +=ReceiveFromCompleted;
        socket.ReceiveFromAsync(args1);
        #endregion
    }
    //发送完成执行的
    private void SendToOver(IAsyncResult result) { 
        try
        {
            Socket s = result.AsyncState as Socket;
            s.EndSendTo(result);
            print("发送成功");
        }
        catch (SocketException s)
        {
            print("发送失败:"+s.Message);
        }
    }
    private void ReceiverFromOver(IAsyncResult result) { 
        try
        {
            //得到元组信息
            (Socket s, EndPoint ipPonint) info =((Socket s, EndPoint ipPonint))result.AsyncState;
            int receiveNum = info.s.EndReceiveFrom(result,ref info.ipPonint);
            //处理消息

            //继续接收消息
            info.s.BeginReceiveFrom(cacheBytes, 0, cacheBytes.Length, SocketFlags.None, ref info.ipPonint, ReceiverFromOver, info);
        }
        catch (SocketException s)
        {
            print("接收消息出错:"+s.Message);
        }
    }
    private void SendToCompleted(object sender,SocketAsyncEventArgs args) {
        if (args.SocketError == SocketError.Success) {
            print("发送成功");
            //继续发送
            (sender as Socket).SendToAsync(args);
        }
        else
        {
            print("发送失败");
        }
    }
    private void ReceiveFromCompleted(object sender, SocketAsyncEventArgs args) {
        if (args.SocketError == SocketError.Success)
        {
            print("接收成功");
            //处理消息
            //args.BytesTransferred 具体接收的字节数
            //args.Buffer 这就是接受容器cache的字节数组

            //解析消息

            //继续接收(可以重新设置容器偏移位置，要接收的长度)
            args.SetBuffer(0,cacheBytes.Length);
            (sender as Socket).ReceiveFromAsync(args);
        }
        else
        {
            print("接收失败");
        }
    }
}

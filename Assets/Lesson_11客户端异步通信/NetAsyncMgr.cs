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
public class NetAsyncMgr : MonoBehaviour
{
    private static NetAsyncMgr instance;
    public static NetAsyncMgr Instance  => instance;
    //和服务器进行连接的socket
    private Socket socket;
    //接收消息用的缓存容器
    private byte[] cacheBytes = new byte[1024*1024];
    //缓存索引
    // private int cacheNum = 0;
    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    //连接服务器
    public void Connect(string ip,int port) {
        if(socket!=null&&socket.Connected)return;
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

        //开始异步连接，先给异步通讯的参数赋值
        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        //远端需要连接的服务器赋值
        args.RemoteEndPoint = iPEndPoint;
        //完成之后要做事件(是个object和SocketAsyncEventArgs类型的参数)
        args.Completed += (socket,args) => {
            //这里socket就是完成连接后的类里的socket，但需要转换socket as Socket，又因为在类里，这个参数可以直接用this.socket
            //这里的args就是完成链接后的SocketAsyncEventArgs类型的args，用它判断链接是否成功
            if (args.SocketError == SocketError.Success) {
                print("连接成功");
                //开始异步收消息 又需要新的 SocketAsyncEventArgs 变量
                SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
                //设置接收容量
                receiveArgs.SetBuffer(cacheBytes,0,cacheBytes.Length);
                receiveArgs.Completed += ReceiveCallBack;
                this.socket.ReceiveAsync(receiveArgs);
            }
            else
            {
                print("连接失败: "+args.SocketError);
            }
        };
        //开始异步连接
        socket.ConnectAsync(args);
    }
    //接收消息成功过后需要执行的方法 args就是接收到消息后的args
    private void ReceiveCallBack(object obj,SocketAsyncEventArgs args) {
        if (args.SocketError == SocketError.Success) {
            //解析消息 目前用的字符串规则args.Buffer，就是我们上一个代码设置的字节数组容器
            //args.BytesTransferred 接收到的字节数组
            print(Encoding.UTF8.GetString(args.Buffer,0,args.BytesTransferred));
            //继续异步收消息 则继续设置容器的偏移位置和能收多长的消息，该方法会延用上一次设置的cacheBytes字节数组
            args.SetBuffer(0,args.Buffer.Length);
            if(this.socket!=null&&this.socket.Connected)
                this.socket.ReceiveAsync(args);//连接成功后的args，就相当于接收到消息后的receiveArgs
            else
            {
                Close();
            }
        }
        else {
            print("接收消息出错:" + args.SocketError);
            //关闭连接
            Close();
        }
    }
    public void Send(string str) {
        if (this.socket != null && this.socket.Connected) {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
            sendArgs.SetBuffer(bytes,0,bytes.Length);
            sendArgs.Completed += (socket,args) => {
                if (args.SocketError != SocketError.Success) {
                    print("发送消息失败："+args.SocketError);
                    Close();
                }
            };
            this.socket.SendAsync(sendArgs);
        }
        else
        {
            Close();
        }
    }
    public void Close() {
        if (socket != null) {
            socket.Shutdown(SocketShutdown.Both);
            socket.Disconnect(false);
            socket.Close();
            socket = null;
        }
    }
}

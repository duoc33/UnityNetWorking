using System;
using System.Threading;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
/// <summary>
/// Udp通信客户端
/// 1.区分消息类型
/// 2.发送消息
/// 3.接收消息
/// 4.判断不是服务端消息不接收 
/// </summary>
public class UdpNetMgr : MonoBehaviour
{
    private static UdpNetMgr instance;
    public static UdpNetMgr Instance => instance;
    /// <summary>
    /// 服务器ip、port
    /// </summary>
    private EndPoint serverIpPoint;
    /// <summary>
    /// 客户端socket
    /// </summary>
    private Socket socket;
    /// <summary>
    /// 客户端socket是否关闭
    /// </summary>
    private bool isClose=true;

    //存储发送接收消息的队列,在多线程操作
    private Queue<BaseMsg> sendQueue = new Queue<BaseMsg>();
    private Queue<BaseMsg> receiveQueue = new Queue<BaseMsg>();

    //接收到的缓存字节数组
    private byte[] cacheBytes = new byte[512];

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update() {
        if (receiveQueue.Count > 0) {
            BaseMsg baseMsg = receiveQueue.Dequeue();
            switch (baseMsg)
            {
                //这里的语法表示，直接判断该类是否为下面子类，并强转
                case PlayerMsg msg:
                    print(msg.playerID);
                    print(msg.playerData.name);
                    print(msg.playerData.atk);
                    print(msg.playerData.lev);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 启动客户端(bind receiver)
    /// </summary>
    /// <param name="ip">远端服务器ip</param>
    /// <param name="port">远端服务器port</param>
    public void StartClient(string ip,int port) {
        //如果是开启状态不用再开了
        if(!isClose)return;
        serverIpPoint = new IPEndPoint(IPAddress.Parse(ip),port);
        IPEndPoint clientIpPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),8081);
        try
        {
            socket = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
            socket.Bind(clientIpPoint);
            isClose = false;
            print("客户端网络启动");
            ThreadPool.QueueUserWorkItem(ReceiveMsg);
            ThreadPool.QueueUserWorkItem(SendMsg);
        }
        catch (System.Exception e)
        {
            print("启动socket出问题:"+e.Message);
            throw;
        }
    }
    /// <summary>
    /// 通过线程池开启的接收方法
    /// </summary>
    /// <param name="obj"></param>
    private void ReceiveMsg(object obj) {
        EndPoint tempIpPoint = new IPEndPoint(IPAddress.Any,0);
        int nowIndex;
        int msgID;
        int Length;
        while (socket!=null&&!isClose)
        {
            if (socket.Available > 0) { 
                try
                {
                    //接收消息，并得到远程发送的计算机ip port信息
                    socket.ReceiveFrom(cacheBytes,ref tempIpPoint);
                    //为了避免非服务器的的消息，需要进行与服务器ip，pot的信息进行对比
                    if (!tempIpPoint.Equals(serverIpPoint)) {
                        continue;
                    }
                    //处理服务器发来的消息
                    nowIndex = 0;
                    //解析ID
                    msgID = BitConverter.ToInt32(cacheBytes,nowIndex);
                    nowIndex += 4;
                    //解析length
                    Length = BitConverter.ToInt32(cacheBytes, nowIndex);
                    nowIndex += 4;
                    //解析消息体
                    BaseMsg baseMsg = null;
                    switch (msgID)
                    {
                        case 101:
                            baseMsg = new PlayerMsg();
                            baseMsg.Reading(cacheBytes,nowIndex);
                            break;
                        default:
                            break;
                    }
                    if(baseMsg!=null)
                        //放入主线程的容器进行处理
                        receiveQueue.Enqueue(baseMsg);
                }
                catch (SocketException s)
                {
                    print("接收消息出问题:"+s.Message);
                }
                catch (Exception e) {
                    print("接收消息出非网络问题:" + e.Message);
                }
            }
        }
    }
    //发送消息
    private void SendMsg(object obj) {
        while (socket != null&&!isClose)
        {
            if (sendQueue.Count > 0)
            {
                try
                {
                    socket.SendTo(sendQueue.Dequeue().Writing(), serverIpPoint);
                }
                catch (SocketException s)
                {
                    print("发送消息出错:" + s.Message);
                }
            }
        }
    }
    public void Send(BaseMsg baseMsg) {
        sendQueue.Enqueue(baseMsg);
    }
    //关闭socket
    public void Close() {
        if (socket != null) {
            isClose = true;
            //发送退出消息
            socket.SendTo(new QuitMsg().Writing(),serverIpPoint);
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket = null;
        }
    }
    private void OnDestroy() {
        Close();
    }
}

using System;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 关闭连接的接口 发送消息给Server的接口 内部可处理从Server收到的消息
/// </summary>
public class NetManager : MonoBehaviour
{
   private static NetManager _instance;
   public static NetManager instance=>_instance;
   #region 客户端发送消息的字段
   //客户端socket
   private Socket socket;
   //客户端发送消息的队列容器
   private Queue<BaseMsg> sendMsgQuene=new Queue<BaseMsg>();
   
   //发送消息的线程
   //private Thread sendThread;
   #endregion

   #region 客户端接收消息的字段
   //用于接收消息的队列(子线程往里放，主线程往里取，因为Unity主线程不能直接从子线程里取东西)
   private Queue<BaseMsg> receiveMsgQuene=new Queue<BaseMsg>();

   //用于接收消息的线程
   //private Thread receiveThread;
   #endregion

   //是否连接的标识
   private bool isConnected =false;
   //缓存容器,用于处理分包的字节数组
   private byte[] cacheBytes=new byte[1024*1024];
   //用于缓存的字节数组数量
   private int cacheNum=0;

    //发送心跳消息间隔时间
    private float SEND_HEART_MSG_TIME = 2f;
    private void Awake() {
        _instance = this;
        //这个网络模块即使切换场景也不销毁
        DontDestroyOnLoad(this.gameObject);
        //客户端定时给服务端发送心跳消息
        InvokeRepeating("SendHeartMsg", 0, SEND_HEART_MSG_TIME);
    }
    private void SendHeartMsg() {
        print(10);
        if (isConnected) {
            Send(new HeartMsg());
        }
    }
    private void Update() {
        if(receiveMsgQuene.Count>0){
        BaseMsg msg=receiveMsgQuene.Dequeue();
        if(msg is PlayerMsg){
            PlayerMsg playerMsg = msg as PlayerMsg;
            print(playerMsg.playerID);
            print(playerMsg.playerData.name);
            print(playerMsg.playerData.atk);
            print(playerMsg.playerData.lev);
        }
    }
   }
   private void OnDestroy() {
    //MonoBehavior对象删除时自动断开连接
    Close();
   }
   //连接服务端
   public void Connect(string ip,int port)
   {
        //如果是连接状态 直接返回，避免多次连接报错
        if(isConnected)return;
        if (socket == null)
            socket = new Socket(AddressFamily.InterNetwork, socketType: SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ipString: ip), port);
        try
        {
            //连接并声明和开启收发的子线程
            socket.Connect(iPEndPoint);
            isConnected=true;
            ThreadPool.QueueUserWorkItem(AsyncSendMsg);
            ThreadPool.QueueUserWorkItem(AsyncReceiveMsg);
            // sendThread=new Thread(AsyncSendMsg);
            // sendThread.Start();
            // receiveThread=new Thread(AsyncReceiveMsg);
            // receiveThread.Start();
        }
        catch (SocketException e)
        {
            if (e.ErrorCode == 10061)
                print("server refused");
            else
            {
                print("connect unsuccessfully:" + e.Message);
            }
        }
   }
   //发送消息
   public void Send(BaseMsg msg)
   {
    //主线程只负责在容器里装消息
    sendMsgQuene.Enqueue(msg);
   }
   public void SendTest(byte[] bytes)
   {
    socket.Send(bytes);
   }
   //用来发送消息的线程方法，发送什么样的消息取决于主线程在容器里装的什么。
   private void AsyncSendMsg(object obj)
    {//死循环
        while (isConnected)
        {
            if (sendMsgQuene.Count > 0)
            {
                socket.Send(sendMsgQuene.Dequeue().Writing());
            }
        }
   }
   //不停接收消息（接收线程处理）会将信息装进容器，信息的处理会在主线程(Update等)
   private void AsyncReceiveMsg(object obj)
   {
    while (isConnected)
    {
        //做一次判断节约性能，
        if(socket.Available>0)
        {
            //改成临时变量，消耗性能，但节约内存
            byte[] recevieBytes=new byte[1024*1024];
            int recevieNum = socket.Receive(recevieBytes);
            HandleReceiveMsg(recevieBytes,recevieNum);

            //首先解析消息ID，前4个字节，才能知道那个对象的解析。
            // int msgID=BitConverter.ToInt32(recevieBytes,0);
            // BaseMsg bMsg =  null;
            // switch (msgID)
            // {
            //     case 101:
            //     PlayerMsg msg = new PlayerMsg();
            //     msg.Reading(recevieBytes,4);
            //     bMsg=msg;
            //     break;
            // }
            // 如果消息为空，则continue，不用执行后面的逻辑了
            // if(bMsg== null)continue;
            // receiveMsgQuene.Enqueue(bMsg);
        }
    }
   }
   //处理分包黏包的方法
   private void HandleReceiveMsg(byte[] receiveBytes,int recevieNum){
        //信息ID
        int msgID = 0;
        //有效信息体长度
        int msgLength = 0;
        //解析到的字节数组位置变量
        int nowIndex = 0;
        //收消息前，先处理之前缓存的容器，有缓存的内容则将其拼接到后面
        receiveBytes.CopyTo(cacheBytes,cacheNum);
        //缓存的字节数则要加上现在收到的字节数(cacheNum是全局变量，下次再执行这个逻辑就不是0了)
        cacheNum+=recevieNum;
        //逻辑就变成了从处理receiveBytes到现在处理cacheBytes就可以了
        while (true)
        {
            //每一次循环msgLength需要重新读取，如果上一次的msgLength还在，
            //则可能直接进入cacheNum-nowIndex>=msgLength的代码
            //例如读到再一次循环cacheNum-nowIndex==2，而上一次的msgLength<=2,
            //则代码不执行ID的解析和长度的解析，直接进行信息体的解析，导致报错
            //所以这里msgLength进行重置,长度不可能等于负数，
            msgLength=-1;//也不要置0，因为可能也消息体为0的信息，当然逻辑上也能没问题
            //收到的字节数肯定得大于8
            if(cacheNum-nowIndex>=8)
            {
                //解析ID和Length
                msgID = BitConverter.ToInt32(cacheBytes, nowIndex);
                nowIndex += 4;
                msgLength = BitConverter.ToInt32(cacheBytes, nowIndex);
                nowIndex += 4;
            }
            //如果recevieNum-8刚好等于msgLength，则既不存在分包也不存在黏包
            //如果黏包cacheNum-8>=msgLength，
            //则会循环多次，cacheNum>=8 cacheNum-8>=msgLength这两个条件下的流程，
            //直到nowIndex的数==cacheNum缓存的数(即收到的字节数组)
            if(cacheNum-nowIndex>=msgLength&&msgLength!=-1)
            {
                //解析消息体
                BaseMsg bMsg = null;
                switch (msgID)
                {
                    case 101:
                        PlayerMsg msg = new PlayerMsg();
                        msg.Reading(cacheBytes, nowIndex);
                        bMsg = msg;
                        break;
                }
                if (bMsg != null)
                    receiveMsgQuene.Enqueue(bMsg);
                nowIndex += msgLength;
                //当现在的索引数==cacheNum时结束
                if(nowIndex==cacheNum){
                    cacheNum=0;
                    break;

                }
            }
            //如果出现分包的情况，recevieNum-8 < msgLength，打破循环，接收更多的信息
            else
            {
                //收集当前内容装进缓存容器，记录下来缓存数量(就是从Server接收到的数量)，下次接收消息再处理
                // receiveBytes.CopyTo(cacheBytes,0);
                // cacheNum=recevieNum;
                
                //如果进行了ID和长度的解析，但是没有成功解析消息体，我们需要减去nowIndex移动的位置
                if(msgLength!=-1)nowIndex-=8;
                //把cacheBytes后面的某个位置开始的cacheBytes，copy到了从0开始到某个位置的cacheBytes里
                Array.Copy(cacheBytes,nowIndex,cacheBytes,0,cacheNum-nowIndex);
                //此时缓存的数量变为上一次消息体没被成功解析时的那个总体长度(包括了ID和Length信息)
                cacheNum=cacheNum-nowIndex;
                //这样做就便于下次解析，下次传入消息就直接往这个缓存里加
                break;
            }
        }
    }
    public void Close(){
        if (socket != null){
            print("客户端已断开连接");
            //模拟非正常断开连接可以注释以下代码

            //主动给服务器端发送断开连接消息
            QuitMsg quitMsg = new QuitMsg();
            //阻塞式发送，可以等待该消息发送过去才能继续执行
            socket.Send(quitMsg.Writing());
            socket.Shutdown(SocketShutdown.Both);
            //必须再Shutdown后使用
            //断开该socket，并且不会再用了(这是由于我们连接时每次都会判断socket是否为空，并重新new)
            socket.Disconnect(false);//测试没有起作用(服务端仍然没法准确快速知道客户端已经断开了)
            socket.Close();
            socket = null;//因为不会再用这个socket，所以置空,再用会新new
            //将连接标识置为false，停止收发消息的循环
            isConnected = false;
            //将子线程置空（如果用了ThreadPool.QueueUserWorkItem方法)，则不用将Thread线程置空了，线程池会自动管理
            // sendThread = null;
            // receiveThread=null;
        }
    }
}

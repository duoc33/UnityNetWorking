using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Net.Sockets;
using System.Net;
using System.Text;
/// <summary>
/// 
/// </summary>
public class Lesson10 : MonoBehaviour
{
    private byte[] resultBytes = new byte[1024];
    private void Start() {
        //1.Begin开头的API
        //内部开多线程，通过回调形式返回结果，需要和End方法相配合
        //2.Async结尾的API
        //内部开多线程，通过回调形式返回结果，依赖SocketAsyncEventArgs对象配合使用
        //操作更加方便

        #region 异步方法和同步方法的区别
        //同步方法：方法中逻辑完毕后，再继续执行后面的方法
        //异步方法：方法中逻辑可能还没有执行完毕，就继续执行后面的内容

        //异步方法的本质
        //往往异步方法当中都会使用多线程执行某部分逻辑
        //因为我们不需要等待方法中逻辑执行完毕就可以继续下面的逻辑了

        //注意：Unity中的协程的某些异步方法，有些使用的是多线程，有些是迭代器分布执行
        #endregion

        #region 举例说明异步方法的原理（就是开线程，不影响主线程）
        //1.线程回调
        // CountDownAsync(5, () => { print("倒计时结束"); });
        // print("异步执行");

        //2.async await 会等待线程执行完毕 继续执行后面的逻辑
        //可以让函数分步执行
        // CountDownAsync(5);
        // print("异步执行");
        #endregion

        #region socket TCP通讯中的异步方法(Begin开头方法)
        //回调函数参数IAsyncResult

        //AsyncState 调用异步方法时传入的参数
        //AsyncWaitHandle 用于同步等待

        //服务器相关
        //BeginAccept
        //EndAccept
        Socket socketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socketTcp.BeginAccept(AcceptCallBack,socketTcp);

        //客户端相关
        //BeginConnect
        //EndConnect
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),8080);
        socketTcp.BeginConnect(iPEndPoint,(IAsyncResult result)=>{
            // Socket s = result.AsyncState as Socket;
            // s.EndConnect(result);
            //等于下面这个语法
            try
            {
                socketTcp.EndConnect(result);
            }
            catch (SocketException e)
            {
                print(e.SocketErrorCode);
            }
        },socketTcp);

        //通用的
        //接收消息
        //BeginReceive
        //EndReceive
        socketTcp.BeginReceive(resultBytes,0,resultBytes.Length,SocketFlags.None,RecevieCallBack,socketTcp);

        //发送消息
        //BeginSend
        //EndSend
        byte[] bytes = Encoding.UTF8.GetBytes("124254243531343435");
        socketTcp.BeginSend(bytes,0,bytes.Length,SocketFlags.None,(IAsyncResult result)=>{
            try
            {
                //返回值是发送成功多少个字节
                socketTcp.EndSend(result);
                print("发送成功");
            }
            catch (SocketException e)
            {
                print(e.SocketErrorCode);
            }
        },socketTcp);

        #endregion

        #region socket TCP通信中的异步方法2(Async结尾方法)
        //关键变量类型
        //SocketAsyncEventArgs e
        //e.Completed绑定(object sender, TEventArgs e)参数的方法
        //socket.AccpetAsync(e); 其中sender参数就是代表socket自身，args带表e,可以得到接收或者发送消息的信息或者socket等

        //它会作为Async异步方法的传入值
        //我们需要通过它进行一些关键参数的赋值

        //服务器
        //AcceptAsync
        SocketAsyncEventArgs e = new SocketAsyncEventArgs();
        //这个是个事件，绑定参数为(object sender, TEventArgs e)的方法
        //如果完成了，就会调用这个方法,args其实就是SocketAsyncEventArgs类型的e
        e.Completed += (socket,args) => {
            if (args.SocketError == SocketError.Success) {
                //获取连入的客户端socket
                Socket client = args.AcceptSocket;
                (socket as Socket).AcceptAsync(args);
            }
            else
            {
                print(args.SocketError);
            }
        };
        socketTcp.AcceptAsync(e);

        //客户端
        //ConnectAsync
        SocketAsyncEventArgs e1 = new SocketAsyncEventArgs();
        e1.Completed += (socket,args) => {
            if (args.SocketError == SocketError.Success) { 
                //连接成功
            }
            else
            {
                //连接失败
                print(args.SocketError);
            }
        };
        socketTcp.ConnectAsync(e1);

        //通用
        //args.SetBuffer 既可以用来设置接收消息的容器，也可以设值要发送的信息，
        //如果设置过一次容器，就不用再设置容器了,args.Buffer 就是上次创建的容器
        //发送消息
        //SendAsync
        SocketAsyncEventArgs e2 = new SocketAsyncEventArgs();
        //传要传的字节数组
        byte[] bytes1 = Encoding.UTF8.GetBytes("Sdasdasfadgasdagafsa");
        e2.SetBuffer(bytes1,0,bytes1.Length);
        //监听完成后执行的事件
        e2.Completed += (socket,args) => {
            if (args.SocketError == SocketError.Success) {
                //发送成功
            }
            else
            {
                //发送失败
            }
        };
        socketTcp.SendAsync(e2);

        //接受消息
        //ReceiveAsync
        SocketAsyncEventArgs e3 = new SocketAsyncEventArgs();
        //收数据,用接收的方法去设置接收的容器
        e3.SetBuffer(new byte[1024*1024],0,1024*1024);
        //完成后
        e3.Completed += (socket, args) => {
            if (args.SocketError == SocketError.Success) {
                //收取存储的字节信息

                //args.Buffer就是接收到的字节数组容器，args.BytesTransferred是收取了多少字节
                Encoding.UTF8.GetString(args.Buffer,0,args.BytesTransferred);

                //如果要再次接收消息，则再设置容器可用空间
                args.SetBuffer(0,args.Buffer.Length);
                //参数1的socket就是外部的socketTcp，让他再次调用ReceiveAsync(),并把e传进去，此时是args
                (socket as Socket).ReceiveAsync(args);
            }
            else { 
                //处理失败
            }
        };
        socketTcp.ReceiveAsync(e3);



        #endregion
    }
    #region Begin开头的方法
    private void RecevieCallBack(IAsyncResult result) {
        try
        {
            //将传入的最后个参数socketTcp转化进来
            Socket s = result.AsyncState as Socket;
            //num是收到了多少个字节。下面代码就表示resultBytes收到了信息
            int num = s.EndReceive(result);
            //进行消息处理
            Encoding.UTF8.GetString(resultBytes, 0, num);

            //如果还要继续接收，继续这个表达式
            s.BeginReceive(resultBytes,0,resultBytes.Length,SocketFlags.None,RecevieCallBack,s);
        }
        catch (SocketException e)
        {
            print(e.Message);
        }
    }
    public void CountDownAsync(int second,UnityAction callBack) {
        print("开始倒计时");
        Thread t = new Thread(()=>{
            while (true)
            {
                print(second);
                Thread.Sleep(1000);
                --second;
                if(second==0)
                    break;
            }
            callBack?.Invoke();
        });
        t.Start();
    }
    public async void CountDownAsync(int second) {
        print("倒计时开始");
        await Task.Run(()=>{
            while (true)
            {
                print(second);
                Thread.Sleep(1000);
                --second;
                if (second == 0)
                    break;
            }
        });
        print("倒计时结束");
    }

    private void AcceptCallBack(IAsyncResult result) {
        try
        {
            //获取传入的参数,其实就是获取object state这个我们传的参数(socketTcp)
            //s其实就是socketTcp
            Socket s = result.AsyncState as Socket;
            //通过调用EndAccept就可以得到连入的客户端Socket
            Socket clientSocket = s.EndAccept(result);
            //如果还需要连入找客户端Socket，则继续下面的代码
            s.BeginAccept(AcceptCallBack,s);
        }
        catch (SocketException e)
        {
            print(e.SocketErrorCode);
        }
     }
    #endregion



}

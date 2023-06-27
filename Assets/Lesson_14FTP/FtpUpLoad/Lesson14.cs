using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Lesson14 : MonoBehaviour
{
    private void Start() {
        #region 搭建FTP服务器的方式
        //1 使用别人做好的FTP服务器软件

        //2 自己编写FTP服务器应用程序，基于FTP的工作原理，用socket中的TCP通信进行编程
        //3 将电脑搭建位FTP文件共享服务器
        //2 3 都是后端程序员做的
        #endregion

        #region 使用别人做好的FTP服务器软件搭建FTP服务器
        //下载Serv-U等FTP服务器软件
        //1.创建域
        //2.使用单向加密
        //3.创建用于上传下载的FTP 账号和密码 
        #endregion

        #region NetworkCredential类
        //通信凭证类
        //用于在Ftp文件传输时设置账号和密码
        NetworkCredential n = new NetworkCredential("DuoChen","duoc199945");
        #endregion

        #region FtpWebRequest类
        //Ftp文件传输协议客户端操作类，用于上传下载删除服务器上的文件

        //重要方法
        
        //1.Create 创建新的WebRequest，用于进行Ftp相关操作（即对服务器的请求）
        //new Uri("ftp://127.0.0.1") 表示服务器的地址，这里服务器就是本机,也可继续指定文件该地址下的指定文件
        FtpWebRequest req =FtpWebRequest.Create(new Uri("ftp://127.0.0.1/Test.txt")) as FtpWebRequest;

        //2.Abort 如果正在进行文件传输，用此方法终止
        req.Abort();

        //3.GetRequestStream 获取用于上传的流
        Stream s = req.GetRequestStream();

        //4.GetResponse 返回Ftp服务器响应
        FtpWebResponse res = req.GetResponse() as FtpWebResponse;

        //重要成员
        //1.Credential 通信凭证，NetworkCredential类对象(这样就可以把账号密码传递给Ftp服务器了)
        req.Credentials = n;
       
        //2.KeepAlive bool值，当完成请求时是否关闭到Ftp服务器的控制连接(默认true，不关闭)
        req.KeepAlive = false;

        //3.Method
        // WebRequestMethods.Ftp静态类中的操作命令属性(全是字符串)
        // DeletFile 删除文件
        // DownloadFile 下载文件
        // ListDirectory 获取文件简短列表
        // ListDirectoryDetails 获取文件详细列表
        // MakeDirectory 创建目录
        // RemvoeDirectory 删除目录
        req.Method =WebRequestMethods.Ftp.DeleteFile;
        // req.Method = WebRequestMethods.Ftp.DownloadFile;
        // req.Method = WebRequestMethods.Ftp.ListDirectory;
        // req.Method = WebRequestMethods.Ftp.MakeDirectory;

        //4.UseBinary 是否使用2进制传输
        req.UseBinary = true;

        //5.RenameTo 重命名
        req.RenameTo = "";
        #endregion

        #region FtpWebResponse类
        //用于封装Ftp服务器对请求的响应
        //它提供操作状态以及从服务器下载数据
        //可以通过FtpWebRequest对象中GetResponse()方法获取
        //使用完毕时，要使用Close释放

        FtpWebResponse response = req.GetResponse() as FtpWebResponse;
        //重要方法
        //1.Close 释放所有资源
        response.Close();
        //2.GetResponseStream 返回从Ftp服务器下载的数据流
        Stream stream = response.GetResponseStream();
        //stream.Read();

        //重要成员
        //1.ContentLength：接受到的数据长度
        long num =response.ContentLength;
        //2.ContentType 接受的数据类型
        print(response.ContentType);
        //3.StatusCode Ftp服务器下发的最新状态代码
        print(response.StatusCode);
        //4.StatusDescription Ftp服务器下发的状态代码文本
        print(response.StatusDescription);
        //5.BannerMessage Ftp登录前建立连接时Ftp服务器发送消息
        print(response.BannerMessage);
        //6.ExitMessage Ftp会话结束时服务器发送的消息
        print(response.ExitMessage);
        //7.LastModified Ftp服务器上的文件的上次修改日期和时间
        print(response.LastModified);
        #endregion
    }
}
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.Events;
/// <summary>
/// 
/// </summary>
public class FtpMgr
{
    private static FtpMgr instance = new FtpMgr();
    public static FtpMgr Instance => instance;
    /// <summary>
    /// 远端ftp服务器地址
    /// </summary>
    private string FTP_PATH = "ftp://127.0.0.1/";
    //用户名和密码
    private string User_Name = "DuoChen";
    private string User_PassWord = "duoc199945";
    /// <summary>
    /// 用异步函数上传文件给Ftp服务器
    /// </summary>
    /// <param name="fileName">上传成功后ftp服务器上显示的文件名称</param>
    /// <param name="localPath">需要上传的文件的本地路径</param>
    /// <param name="action">文件上传成功后的行为</param>
    public async void UploadFile(string fileName,string localPath,UnityAction action=null) {
        await Task.Run(()=>{
            try
            {
                //1.创建与FTP服务器的连接请求
                FtpWebRequest req = FtpWebRequest.Create(new Uri(FTP_PATH + fileName)) as FtpWebRequest;
                //2.进行设置
                //凭证
                req.Credentials = new NetworkCredential(User_Name, User_PassWord);
                //是否操作接受后关闭 Tcp
                req.KeepAlive = false;
                //传输类型（使用二进制）
                req.UseBinary = true;
                //操作类型(即上传服务器或者从服务器下载或创建服务器目录等等)
                req.Method = WebRequestMethods.Ftp.UploadFile;
                //代理设置为空
                req.Proxy = null;

                //3.上传 (获取上传流)
                Stream uploadStream = req.GetRequestStream();
                using (FileStream fileStream = File.OpenRead(localPath))
                {
                    //流 类型 是按顺序读取或写入文件的二进制数据的对象，中间的字节数组仅仅是容器，
                    //下一次读或写，依然会从上一次读取的位置继续读或写

                    byte[] bytes = new byte[1024];
                    //把该文件流 读进字节数组
                    int contentLength = fileStream.Read(bytes, 0, bytes.Length);
                    while (contentLength != 0)
                    {
                        //把字节数组写进上传流中
                        uploadStream.Write(bytes, 0, contentLength);
                        //再继续将文件剩下的流读取的二进制放进字节数组，直到contentLength(读取字节长度)为0
                        contentLength = fileStream.Read(bytes, 0, bytes.Length);
                    }
                    //4.关闭流，上传结束
                    fileStream.Close();
                    uploadStream.Close();
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        });
        //await之后就是上传成功之后执行的行为
        action?.Invoke();
    }

    /// <summary>
    /// 用异步函数从Ftp服务器下载文件到本地
    /// </summary>
    /// <param name="fileName">下载成功后本地显示的文件名称</param>
    /// <param name="localPath">需要下载的文件的本地文件路径</param>
    /// <param name="action">文件下载成功后的行为</param>
    public async void DownloadFile(string fileName, string localPath, UnityAction action = null)
    {
        await Task.Run(() =>{
            try
            {
                //1.创建FTP连接
                FtpWebRequest req = FtpWebRequest.Create(new Uri(FTP_PATH + fileName)) as FtpWebRequest;
                //2.进行一些设置
                req.Credentials = new NetworkCredential(User_Name, User_PassWord);
                req.KeepAlive = false;
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.DownloadFile;
                req.Proxy = null;
                //3.下载
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                Stream downLoadStream = res.GetResponseStream();
                using (FileStream fileStream = File.Create(localPath))
                {
                    byte[] bytes = new byte[1024];
                    int contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                    while (contentLength != 0)
                    {
                        //读进去
                        fileStream.Write(bytes, 0, contentLength);
                        //如果还有继续写进去
                        contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                    }
                    downLoadStream.Close();
                    fileStream.Close();
                }
                res.Close();
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        });
        action?.Invoke();
    }

    /// <summary>
    /// 移除指定文件
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="action">移除之后的行为</param>
    public async void DeleteFile(string fileName,UnityAction<bool> action = null) {
        await Task.Run(()=>{
            try
            {
                FtpWebRequest req = FtpWebRequest.Create(new Uri(FTP_PATH + fileName)) as FtpWebRequest;
                req.Credentials = new NetworkCredential(User_Name, User_PassWord);
                req.KeepAlive = false;
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.DeleteFile;
                req.Proxy = null;

                //删除
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                res.Close();

                action?.Invoke(true);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
                action?.Invoke(false);
            }
        });
    }

    /// <summary>
    /// 获取Ftp服务器上某个文件的大小
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="action">获取成功后的大小</param>
    public async void GetFileSize(string fileName, UnityAction<long> action = null) {
        await Task.Run(() =>
        {
            try
            {
                FtpWebRequest req = FtpWebRequest.Create(new Uri(FTP_PATH + fileName)) as FtpWebRequest;
                req.Credentials = new NetworkCredential(User_Name, User_PassWord);
                req.KeepAlive = false;
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.GetFileSize;
                req.Proxy = null;

                //获得到大小(字节数)
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                action?.Invoke(res.ContentLength);
                res.Close();
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
                action?.Invoke(0);
            }
        });
    }

    //创建文件夹
    public async void CreateDirectory(string DirectoryName, UnityAction<bool> action = null)
    {
        await Task.Run(() =>
        {
            try
            {
                FtpWebRequest req = FtpWebRequest.Create(new Uri(FTP_PATH + DirectoryName)) as FtpWebRequest;
                req.Credentials = new NetworkCredential(User_Name, User_PassWord);
                req.KeepAlive = false;
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.MakeDirectory;
                req.Proxy = null;

                //创建文件夹
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                res.Close();
                action?.Invoke(true);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
                action?.Invoke(false);
            }
        });
    }

    //获取所有文件名，DirectoryName为空，则是获取根目录里的文件。不包括文件夹(目录)
    public async void GetFileList(string fileName, UnityAction<List<string>> action = null)
    {
        await Task.Run(() =>
        {
            try
            {
                FtpWebRequest req = FtpWebRequest.Create(new Uri(FTP_PATH + fileName)) as FtpWebRequest;
                req.Credentials = new NetworkCredential(User_Name, User_PassWord);
                req.KeepAlive = false;
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.ListDirectory;
                req.Proxy = null;

                //获取所有文件名
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                //s就是文件名的信息流
                Stream s = res.GetResponseStream();
                //把下载的信息流转成StreamReader 方便一行一行的读取信息.
                StreamReader streamReader = new StreamReader(s);
                //用于存储文件名的列表(相当于)
                List<string> names = new List<string>();
                //一行一行的读取
                string line = streamReader.ReadLine();
                while (line!=null)
                {
                    names.Add(line);
                    line = streamReader.ReadLine();
                }
                res.Close();
                action?.Invoke(names);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
                action?.Invoke(null);
            }
        });
    }

}

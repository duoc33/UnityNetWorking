using System.Net.Mime;
using System.Linq.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetWWWMgr : MonoBehaviour
{
    private static NetWWWMgr instance;

    public static NetWWWMgr Instance => instance;
    private string HTTP_SERVER_PATH = "http://192.168.50.109:8000/Http_Server/";

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// 提供给外部加载资源用的方法
    /// </summary>
    /// <typeparam name="T">资源的类型</typeparam>
    /// <param name="path">资源的路径 http ftp file都支持</param>
    /// <param name="action">加载结束后的回调函数 因为WWW是通过结合协同程序异步加载的 所以不能马上获取结果 需要回调获取</param>
    [Obsolete]
    public void LoadRes<T>(string path, UnityAction<T> action) where T : class
    {
        StartCoroutine(LoadResAsync<T>(path, action));
    }

    [Obsolete]
    private IEnumerator LoadResAsync<T>(string path, UnityAction<T> action) where T : class
    {
        //声明www对象 用于下载或加载
        WWW www = new WWW(path);
        //等待下载或者加载结束（异步）
        yield return www;
        //如果没有错误 证明加载成功
        if (www.error == null)
        {
            //根据T泛型的类型  决定使用哪种类型的资源 传递给外部
            if(typeof(T) == typeof(AssetBundle))
            {
                action?.Invoke(www.assetBundle as T);
            }
            else if (typeof(T) == typeof(Texture))
            {
                action?.Invoke(www.texture as T);
            }
            else if (typeof(T) == typeof(AudioClip))
            {
                action?.Invoke(www.GetAudioClip() as T);
            }
            else if (typeof(T) == typeof(string))
            {
                action?.Invoke(www.text as T);
            }
            else if (typeof(T) == typeof(byte[]))
            {
                action?.Invoke(www.bytes as T);
            }
            //自定义一些类型 可能需要将bytes 转换成对应的类型来使用
        }
        //如果错误 就提示别人
        else
        {
            Debug.LogError("www加载资源出错" + www.error);
        }
    }

    [Obsolete]
    public void SendMsg<T>(BaseMsg msg, UnityAction<T> action) where T : BaseMsg
    {
        StartCoroutine(SendMsgAsync<T>(msg, action));
    }

    [Obsolete]
    private IEnumerator SendMsgAsync<T>(BaseMsg msg, UnityAction<T> action) where T : BaseMsg
    {
        //消息发送
        WWWForm data = new WWWForm();
        //准备要发送的消息数据
        data.AddBinaryData("Msg", msg.Writing());

        WWW www = new WWW(HTTP_SERVER_PATH, data);
        //我们也可以直接传递 2进制字节数组 只要和后端定好规则 怎么传都是可以的
        //WWW www = new WWW("HTTP_SERVER_PATH", msg.Writing());

        //异步等待 发送结束 才会继续执行后面的代码
        yield return www;

        //发送完毕过后 收到响应 
        //认为 后端发回来的内容 也是一个继承自BaseMsg类的一个字节数组对象
        if (www.error == null)
        {
            //先解析 ID和消息长度
            int index = 0;
            int msgID = BitConverter.ToInt32(www.bytes, index);
            index += 4;
            int msgLength = BitConverter.ToInt32(www.bytes, index);
            index += 4;
            //反序列化 BaseMsg
            BaseMsg baseMsg = null;
            switch (msgID)
            {
                case 1001:
                    baseMsg = new PlayerMsg();
                    baseMsg.Reading(www.bytes, index);
                    break;
            }
            if (baseMsg != null)
                action?.Invoke(baseMsg as T);
        }
        else
            Debug.LogError("发消息出问题" + www.error);
    }

    /// <summary>
    /// UnityWebRequset 外部加载资源方法
    /// </summary>
    /// <typeparam name="T">资源类型(byte[]、objcet(File)、AssetBundle、Texture、Audioclip等等)</typeparam>
    /// <param name="path">资源网络路径</param>
    /// <param name="action">加载完成后的回调</param>
    /// <param name="localPath">如果需要存储在本地的路径</param>
    /// <param name="type">如果是音频文件，音频文件的类型</param>
    public void UnityWebRequestLoad<T>(string path,UnityAction<T> action,string localPath="",AudioType type=AudioType.MPEG)where T :class {
        StartCoroutine(UnityWebRequestLoadAsync<T>(path, action,localPath = "",type = AudioType.MPEG));
    }
    private IEnumerator UnityWebRequestLoadAsync<T>(string path, UnityAction<T> action, 
    string localPath = "", AudioType type = AudioType.MPEG) where T : class{
        UnityWebRequest req = new UnityWebRequest(path,UnityWebRequest.kHttpVerbGET);
        if (typeof(T) == typeof(byte[])) 
            req.downloadHandler = new DownloadHandlerBuffer();
        else if (typeof(T) == typeof(Texture))
            req.downloadHandler = new DownloadHandlerTexture();
        else if (typeof(T) == typeof(AssetBundle))
            //第二个参数是AB包的校检码，默认0
            req.downloadHandler = new DownloadHandlerAssetBundle(req.url,0);
        else if (typeof(T) == typeof(object))//如果是object就是文件处理，因为File为静态类，不能作为泛型参数
            req.downloadHandler = new DownloadHandlerFile(localPath);
        else if (typeof(T) == typeof(AudioClip))
            req = UnityWebRequestMultimedia.GetAudioClip(path,type);
        else
        {
            Debug.Log("未知信息");
            yield break;
        }
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success) {
            if (typeof(T) == typeof(byte[]))
                action?.Invoke(req.downloadHandler.data as T);
            else if (typeof(T) == typeof(Texture))
                //action?.Invoke((req.downloadHandler as DownloadHandlerTexture).texture as T);
                action?.Invoke(DownloadHandlerTexture.GetContent(req) as T);
            else if (typeof(T) == typeof(AssetBundle))
                action?.Invoke((req.downloadHandler as DownloadHandlerAssetBundle).assetBundle as T);
            else if (typeof(T) == typeof(object))
                action?.Invoke(null);
            else if (typeof(T) == typeof(AudioClip))
                action?.Invoke(DownloadHandlerAudioClip.GetContent(req) as T);
            else
            {
                Debug.Log("未知信息");
                yield break;
            }
        }
        else
        {
            Debug.Log("获取信息失败");
        }
    }


    /// <summary>
    /// 上传文件的方法
    /// </summary>
    /// <param name="fileName">上传上去的文件名</param>
    /// <param name="localPath">本地想要上传文件的路径</param>
    /// <param name="action">上传完成后的回调函数</param>
    public void UploadFile(string fileName, string localPath, UnityAction<UnityWebRequest.Result> action)
    {
        StartCoroutine(UploadFileAsync(fileName, localPath, action));
    }

    private IEnumerator UploadFileAsync(string fileName, string localPath, UnityAction<UnityWebRequest.Result> action)
    {
        //添加要上传文件的数据
        List<IMultipartFormSection> dataList = new List<IMultipartFormSection>();
        dataList.Add(new MultipartFormFileSection(fileName, File.ReadAllBytes(localPath)));

        UnityWebRequest req = UnityWebRequest.Post(HTTP_SERVER_PATH, dataList);

        yield return req.SendWebRequest();

        action?.Invoke(req.result);
        //如果不成功
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("上传出现问题" + req.error + req.responseCode);
        }
    }

}

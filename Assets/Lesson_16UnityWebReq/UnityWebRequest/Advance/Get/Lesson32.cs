using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Lesson32 : MonoBehaviour
{
    public RawImage image;
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 高级操作指什么？
        //在常用操作中我们使用的是Unity为我们封装好的一些方法
        //我们可以方便的进行一些指定类型的数据获取

        //比如
        //下载数据时：
        //1.文本和2进制
        //2.图片
        //3.AB包
        //如果我们想要获取其它类型的数据应该如何处理呢？

        //上传数据时：
        //1.可以指定参数和值
        //2.可以上传文件
        //如果想要上传一些基于HTTP规则的其它数据应该如何处理呢？

        //高级操作就是用来处理 常用操作不能完成的需求的
        //它的核心思想就是：UnityWebRequest中可以将数据处理分离开
        //比如常规操作中我们用到的
        //DownloadHandlerTexture 和 DownloadHandlerAssetBundle两个类
        //就是用来将2进制字节数组转换成对应类型进行处理的

        //所以高级操作时指 让你按照规则来实现更多的数据获取、上传等功能
        #endregion

        #region 知识点二 UnityWebRequest类的更多内容
        //目前已学的内容
        //UnityWebRequest req = UnityWebRequest.Get("");
        //UnityWebRequest req = UnityWebRequestTexture.GetTexture("");
        //UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle("");
        //UnityWebRequest req = UnityWebRequest.Put()
        //UnityWebRequest req = UnityWebRequest.Post

        //req.isDone
        //req.downloadProgress;
        //req.downloadedBytes;
        //req.uploadProgress;
        //req.uploadedBytes

        //req.SendWebRequest()

        //更多内容
        //1.构造函数
        //UnityWebRequest req = new UnityWebRequest();

        //2.请求地址
        //req.url = "服务器地址";

        //3.请求类型
        //req.method = UnityWebRequest.kHttpVerbPOST;

        //4.进度
        //req.downloadProgress
        //req.uploadProgress

        //5.超时设置
        //req.timeout = 2000;

        //6.上传、下载的字节数
        //req.downloadedBytes
        //req.uploadedBytes

        //7.重定向次数 设置为0表示不进行重定向 可以设置次数
        //req.redirectLimit = 10;

        //8.状态码、结果、错误内容
        //req.result
        //req.error
        //req.responseCode

        //9.下载、上传处理对象
        //req.downloadHandler
        //req.uploadHandler

        //更多内容
        //https://docs.unity.cn/cn/2020.3/ScriptReference/Networking.UnityWebRequest.html
        #endregion

        #region 知识点三 自定义获取数据DownloadHandler相关类
        //关键类：
        //1.DownloadHandlerBuffer 用于简单的数据存储，得到对应的2进制数据。
        //2.DownloadHandlerFile 用于下载文件并将文件保存到磁盘（内存占用少）。
        //3.DownloadHandlerTexture 用于下载图像。
        //4.DownloadHandlerAssetBundle 用于提取 AssetBundle。
        //5.DownloadHandlerAudioClip 用于下载音频文件。

        StartCoroutine(DownLoadTex());

        StartCoroutine(DownLoadAB());

        //以上的这些类，其实就是Unity帮助我们实现好的，用于解析下载下来的数据的类
        //使用对应的类处理下载数据，他们就会在内部将下载的数据处理为对应的类型，方便我们使用

        //DownloadHandlerScript 是一个特殊类。就其本身而言，不会执行任何操作。
        //但是，此类可由用户定义的类继承。此类接收来自 UnityWebRequest 系统的回调，
        //然后可以使用这些回调在数据从网络到达时执行完全自定义的数据处理。

        StartCoroutine(DownLoadCustomHandler());
        #endregion

        #region 总结
        //我们可以自己设置UnityWebRequest当中的下载处理对象
        //当设置后，下载数据后它会使用该对象中对应的函数处理数据
        //让我们更方便的获取我们想要的数据
        //方便我们对数据下载或获取进行拓展
        #endregion
    }

    IEnumerator DownLoadTex()
    {
        UnityWebRequest req = new UnityWebRequest("http://192.168.50.109:8000/Http_Server/实战就业路线.jpg", 
                                                   UnityWebRequest.kHttpVerbGET);
        //req.method = UnityWebRequest.kHttpVerbGET;
        //1.DownloadHandlerBuffer
        //DownloadHandlerBuffer bufferHandler = new DownloadHandlerBuffer();
        //req.downloadHandler = bufferHandler;

        //2.DownloadHandlerFile
        //print(Application.persistentDataPath);
        //req.downloadHandler = new DownloadHandlerFile(Application.persistentDataPath + "/downloadFile.jpg");

        //3.DownloadHandlerTexture
        DownloadHandlerTexture textureHandler = new DownloadHandlerTexture();
        req.downloadHandler = textureHandler;

        yield return req.SendWebRequest();

        if(req.result == UnityWebRequest.Result.Success)
        {
            //获取字节数组
            //bufferHandler.data
            
            //textureHandler.texture
            image.texture = textureHandler.texture;
        }
        else
        {
            print("获取数据失败" + req.result + req.error + req.responseCode);
        }
    }

    IEnumerator DownLoadAB()
    {
        UnityWebRequest req = new UnityWebRequest("http://192.168.50.109:8000/Http_Server/lua", UnityWebRequest.kHttpVerbGET);
        //第二个参数 需要已知校检码 才能进行比较 检查完整性 如果不知道的话 只能传0 不进行完整性的检查
        //所以一般 只有进行AB包热更新时 服务器发送了 对应的 文件列表中 包含了 验证码 才能进行检查
        DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(req.url, 0);
        req.downloadHandler = handler;

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            AssetBundle ab = handler.assetBundle;
            
            print(ab.name);
        }
        else
        {
            print("获取数据失败" + req.result + req.error + req.responseCode);
        }
    }

    IEnumerator DownLoadAudioClip()
    {
        UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip("http://192.168.50.109:8000/Http_Server/音效名.mp3", 
                                                                      AudioType.MPEG);
        yield return req.SendWebRequest();
        
        if (req.result == UnityWebRequest.Result.Success)
        {
            AudioClip a = DownloadHandlerAudioClip.GetContent(req);
        }
        else
        {
            print("获取数据失败" + req.result + req.error + req.responseCode);
        }
    }

    IEnumerator DownLoadCustomHandler()
    {
        UnityWebRequest req = new UnityWebRequest("http://192.168.50.109:8000/Http_Server/21.服务端.mp4", UnityWebRequest.kHttpVerbGET);

        //使用自定义的下载处理对象 来处理获取到的 2进制字节数组
        print(Application.persistentDataPath);
        req.downloadHandler = new CustomDownLoadFileHandler(Application.persistentDataPath + "/CustomHandler.mp4");

        yield return req.SendWebRequest();

        if(req.result == UnityWebRequest.Result.Success)
        {
            print("存储本地成功");
        }
        else
        {
            print("获取数据失败" + req.result + req.error + req.responseCode);
        }
    }
    
}

public class CustomDownLoadFileHandler:DownloadHandlerScript
{
    //用于保存 本地存储时的路径
    private string savePath;

    //用于缓存收到的数据的容器
    private byte[] cacheBytes;
    //这是当前已收到的数据长度
    private int index = 0;

    public CustomDownLoadFileHandler():base()
    {

    }

    public CustomDownLoadFileHandler(byte[] bytes) :base(bytes)
    {

    }

    public CustomDownLoadFileHandler(string path) : base()
    {
        savePath = path;
    }

    protected override byte[] GetData()
    {
        //返回字节数组
        return cacheBytes;
    }

    /// <summary>
    /// 从网络收到数据后 每帧会调用的方法  会自动调用的方法
    /// </summary>
    /// <param name="data"></param>
    /// <param name="dataLength"></param>
    /// <returns></returns>
    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        Debug.Log("收到数据长度：" + data.Length);
        Debug.Log("收到数据长度dataLength：" + dataLength);
        data.CopyTo(cacheBytes, index);
        index += dataLength;
        return true;
    }

    /// <summary>
    /// 从服务器收到 COntent-Length标头时  会自动调用的方法
    /// </summary>
    /// <param name="contentLength"></param>
    protected override void ReceiveContentLengthHeader(ulong contentLength)
    {
        //base.ReceiveContentLengthHeader(contentLength);
        Debug.Log("收到数据长度：" + contentLength);
        //根据收到的标头 决定字节数组容器的大小
        cacheBytes = new byte[contentLength];
    }

    /// <summary>
    /// 当消息收完了 会自动调用的方法
    /// </summary>
    protected override void CompleteContent()
    {
        Debug.Log("消息收完");
        //把收到的字节数组 进行自定义处理 我们在这 处理成 存储到本地
        File.WriteAllBytes(savePath, cacheBytes);
    }

}

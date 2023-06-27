using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Lesson30_Get : MonoBehaviour
{
    public RawImage image;
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 UnityWebRequest是什么?
        //UnityWebRequest是一个Unity提供的一个模块化的系统类
        //用于构成HTTP请求和处理HTTP响应
        //它主要目标是让Unity游戏和Web服务端进行交互
        //它将之前WWW的相关功能都集成在了其中
        //所以新版本中都建议使用UnityWebRequest类来代替WWW类

        //它在使用上和WWW很类似
        //主要的区别就是UnityWebRequest把下载下来的数据处理单独提取出来了
        //我们可以根据自己的需求选择对应的数据处理对象来获取数据

        //注意：
        //1.UnityWebRequest和WWW一样，需要配合协同程序使用
        //2.UnityWebRequest和WWW一样，支持http、ftp、file协议下载或加载资源
        //3.UnityWebRequest能够上传文件到HTTP资源服务器
        #endregion

        #region 知识点二 UnityWebRequest类的常用操作
        //1.使用Get请求获取文本或二进制数据
        //2.使用Get请求获取纹理数据
        //3.使用Get请求获取AB包数据
        //4.使用Post请求发送数据
        //5.使用Put请求上传数据
        #endregion

        #region 知识点三 Get获取操作
        //1.获取文本或2进制
        StartCoroutine(LoadText());
        //2.获取纹理
        StartCoroutine(LoadTexture());
        //3.获取AB包
        StartCoroutine(LoadAB());
        #endregion

        #region 总结 
        //UnityWebRequest使用上和WWW类很类似
        //我们需要注意的是
        //1.获取文本或二进制数据时
        //  使用UnityWebRequest.Get
        //2.获取纹理图片数据时
        //  使用UnityWebRequestTexture.GetTexture
        //  以及DownloadHandlerTexture.GetContent
        //3.获取AB包数据时
        //  使用UnityWebRequestAssetBundle.GetAssetBundle
        //  以及DownloadHandlerAssetBundle.GetContent
        #endregion
    }

    IEnumerator LoadText()
    {
        UnityWebRequest req = UnityWebRequest.Get("http://192.168.50.109:8000/Http_Server/test.txt");
        //就会等待 服务器端响应后 断开连接后 再继续执行后面的内容
        yield return req.SendWebRequest();

        //如果处理成功 结果就是成功枚举
        if(req.result == UnityWebRequest.Result.Success)
        {
            //文本 字符串
            print(req.downloadHandler.text);
            //字节数组
            byte[] bytes = req.downloadHandler.data;
            print("字节数组长度" + bytes.Length);
        }
        else
        {
            print("获取失败:" + req.result + req.error + req.responseCode);
        }
    }

    IEnumerator LoadTexture()
    {
        //UnityWebRequest req = UnityWebRequestTexture.GetTexture("http://192.168.50.109:8000/Http_Server/实战就业路线.jpg");

        //UnityWebRequest req = UnityWebRequestTexture.GetTexture("ftp://127.0.0.1/实战就业路线.jpg");

        UnityWebRequest req = UnityWebRequestTexture.GetTexture("file://" + Application.streamingAssetsPath + "/test.png");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            //(req.downloadHandler as DownloadHandlerTexture).texture
            //DownloadHandlerTexture.GetContent(req)
            //image.texture = (req.downloadHandler as DownloadHandlerTexture).texture;
            image.texture = DownloadHandlerTexture.GetContent(req);
        }
        else
            print("获取失败" + req.error + req.result + req.responseCode);
    }

    IEnumerator LoadAB()
    {
        UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle("http://192.168.50.109:8000/Http_Server/lua");

        req.SendWebRequest();

        while (!req.isDone)
        {
            print(req.downloadProgress);
            print(req.downloadedBytes);
            yield return null;
        }
        //yield return req.SendWebRequest();

        print(req.downloadProgress);
        print(req.downloadedBytes);

        if (req.result == UnityWebRequest.Result.Success)
        {
            //AssetBundle ab = (req.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
            AssetBundle ab = DownloadHandlerAssetBundle.GetContent(req);
            print(ab.name);
        }
        else
            print("获取失败" + req.error + req.result + req.responseCode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

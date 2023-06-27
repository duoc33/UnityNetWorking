using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson28 : MonoBehaviour
{
    public RawImage image;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        #region 知识点一 WWW类的作用
        //WWW是Unity提供给我们简单的访问网页的类
        //我们可以通过该类下载和上传一些数据
        //在使用http协议时，默认的请求类型是Get，如果想要Post上传，需要配合下节课学习的WWWFrom类使用
        //它主要支持的协议
        //1.http://和https:// 超文本传输协议
        //2.ftp:// 文件传输协议（但仅限于匿名下载）
        //3.file:// 本地文件传输协议，可以使用该协议异步加载本地文件（PC、IOS、Android都支持）
        //我们本节课主要学习利用WWW来进行数据的下载或加载

        //注意：
        //1.该类一般配合协同程序使用
        //2.该类在较新Unity版本中会提示过时，但是仍可以使用，新版本将其功能整合进了UnityWebRequest类（之后讲解）
        #endregion

        #region 知识点二 WWW类的常用方法和变量
        #region 常用方法
        //1.WWW：构造函数，用于创建一个WWW请求
        WWW www = new WWW("http://192.168.50.109:8000/Http_Server/实战就业路线.jpg");
        //2.GetAudioClip：从下载数据返回一个音效切片AudioClip对象
        //www.GetAudioClip()
        //3.LoadImageIntoTexture：用下载数据中的图像来替换现有的一个Texture2D对象
        //Texture2D tex = new Texture2D(100, 100);
        //www.LoadImageIntoTexture(tex);
        //4.LoadFromCacheOrDownload：从缓存加载AB包对象，如果该包不在缓存则自动下载存储到缓存中，以便以后直接从本地缓存中加载
        //WWW.LoadFromCacheOrDownload("http://192.168.50.109:8000/Http_Server/test.assetbundle", 1);
        #endregion

        #region 常用变量
        //1.assetBundle：如果加载的数据是AB包，可以通过该变量直接获取加载结果
        //www.assetBundle
        //2.audioClip：如果加载的数据是音效切片文件，可以通过该变量直接获取加载结果
        //www.GetAudioClip
        //3.bytes：以字节数组的形式获取加载到的内容
        //www.bytes
        //4.bytesDownloaded：过去已下载的字节数
        //www.bytesDownloaded
        //5.error：返回一个错误消息，如果下载期间出现错误，可以通过它获取错误信息
        //www.error != null
        //6.isDone：判断下载是否已经完成
        //www.isDone
        //7.movie：如果下载的视频，可以获取一个MovieTexture类型结果
        //www.GetMovieTexture()
        //8.progress:下载进度
        //www.progress
        //9.text：如果下载的数据是字符串，以字符串的形式返回内容
        //www.text
        //10.texture：如果下载的数据是图片，以Texture2D的形式返回加载结果
        //www.texture
        #endregion
        #endregion

        #region 知识点三 利用WWW类来异步下载或加载文件
        #region 1.下载HTTP服务器上的内容
        //StartCoroutine(DownLoadHttp());
        #endregion

        #region 2.下载FTP服务器上的内容（FTP服务器一定要支持匿名账户）
        //StartCoroutine(DownLoadFtp());
        #endregion

        #region 3.本地内容加载（一般移动平台加载数据都会使用该方式）
        StartCoroutine(DownLoadLocal());
        #endregion
        #endregion

        #region 总结
        //Unity中的WWW类比使用C#中的Http相关类更加的方便
        //建议大家使用Unity当中为我们封装好的类来处理下载、加载相关逻辑
        #endregion
    }

    [System.Obsolete]
    IEnumerator DownLoadHttp()
    {
        //1.创建WWW对象
        WWW www = new WWW("https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fi2.hdslb.com%2Fbfs%2Farchive%2F8cc2b9a7868b266800f98d42fc5d257021e75103.jpg&refer=http%3A%2F%2Fi2.hdslb.com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=auto?sec=1654745686&t=b7691b6e546367610e4331039d1a10ec");

        //2.就是等待加载结束
        while (!www.isDone)
        {
            print(www.bytesDownloaded);
            print(www.progress);
            yield return null;
        }

        print(www.bytesDownloaded);
        print(www.progress);

        //or 2 直到www完成执行(有一种异步的感觉)
        //yield return www;

        //3.使用加载结束后的资源
        if (www.error == null)
        {
            image.texture = www.texture;
        }
        else
            print(www.error);
    }

    [System.Obsolete]
    IEnumerator DownLoadFtp()
    {
        //1.创建WWW对象
        WWW www = new WWW("ftp://127.0.0.1/实战就业路线.jpg");

        //2.就是等待加载结束
        while (!www.isDone)
        {
            print(www.bytesDownloaded);
            print(www.progress);
            yield return null;
        }

        print(www.bytesDownloaded);
        print(www.progress);

        //3.使用加载结束后的资源
        if (www.error == null)
        {
            image.texture = www.texture;
        }
        else
            print(www.error);
    }

    [System.Obsolete]
    IEnumerator DownLoadLocal()
    {
        //1.创建WWW对象
        WWW www = new WWW("file://" + Application.streamingAssetsPath + "/test.png");

        //2.就是等待加载结束
        while (!www.isDone)
        {
            print(www.bytesDownloaded);
            print(www.progress);
            yield return null;
        }

        print(www.bytesDownloaded);
        print(www.progress);

        //3.使用加载结束后的资源
        if (www.error == null)
        {
            image.texture = www.texture;
        }
        else
            print(www.error);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

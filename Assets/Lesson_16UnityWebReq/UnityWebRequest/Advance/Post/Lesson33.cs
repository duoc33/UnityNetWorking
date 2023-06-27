using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Lesson33 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 回顾高级操作中的获取数据
        //主要做法：将2进制字节数组处理，独立到下载处理对象中进行处理
        //         主要就是设置UnityWebRequest对象中 downloadHandler 变量
        //Unity写好的类有
        //1.DownloadHandlerBuffer 用于简单的数据存储，得到对应的2进制数据。
        //2.DownloadHandlerFile 用于下载文件并将文件保存到磁盘（内存占用少）。
        //3.DownloadHandlerTexture 用于下载图像。
        //4.DownloadHandlerAssetBundle 用于提取 AssetBundle。
        //5.DownloadHandlerAudioClip 用于下载音频文件。

        //自己拓展处理方式
        //继承 DownloadHandlerScript
        //并重写其中的固定方法，自己处理字节数组
        #endregion

        #region 知识点二 自定义上传数据UploadHandler相关类
        //注意：
        //由于UnityWebRequest类的常用操作中
        //上传数据相关内容已经封装的很好了
        //我们可以很方便的上传参数和文件
        //我们使用常用操作已经能够满足常用需求了
        //所以以下内容主要做了解

        //UploadHandler相关类
        //1.UploadHandlerRaw  用于上传字节数组
        StartCoroutine(UpLoad());
        //2.UploadHandlerFile 用于上传文件

        //其中比较重要的变量是
        //contentType 内容类型，如果不设置，模式是 application/octet-stream 2进制流的形式
        #endregion

        #region 总结
        //由于上传数据相关 UnityWebRequest原本已经提供了较为完善的
        //参数上传、文件上传相关功能
        //所以高级操作中的 上传数据相关内容拓展较少，使用也较少
        //我们使用常用操作的上传数据相关功能就足够了
        //高级操作的上传数据知识点主要做了解
        #endregion
    }

    IEnumerator UpLoad()
    {
        UnityWebRequest req = new UnityWebRequest("http://192.168.50.109:8000/Http_Server/", UnityWebRequest.kHttpVerbPOST);

        //1.UploadHandlerRaw  用于上传字节数组
        //byte[] bytes = Encoding.UTF8.GetBytes("123123123123123");
        //req.uploadHandler = new UploadHandlerRaw(bytes);
        //req.uploadHandler.contentType = "类型/细分类型";

        //2.UploadHandlerFile 用于上传文件
        req.uploadHandler = new UploadHandlerFile(Application.streamingAssetsPath + "/test.png");

        yield return req.SendWebRequest();

        print(req.result);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Lesson31_Post : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 上传相关数据类
        //父接口
        //IMultipartFormSection
        //数据相关类都继承该接口
        //我们可以用父类装子类
        List<IMultipartFormSection> dataList = new List<IMultipartFormSection>();

        //子类数据
        //MultipartFormDataSection
        //1.二进制字节数组
        dataList.Add(new MultipartFormDataSection(Encoding.UTF8.GetBytes("123123123123123")));
        //2.字符串
        dataList.Add(new MultipartFormDataSection("12312312312312312dsfasdf"));
        //3.参数名，参数值（字节数组，字符串），编码类型，资源类型（常用）
        dataList.Add(new MultipartFormDataSection("Name", "MrTang", Encoding.UTF8, "application/...."));
        dataList.Add(new MultipartFormDataSection("Msg", new byte[1024], "appl....."));

        //MultipartFormFileSection
        //1.字节数组
        dataList.Add(new MultipartFormFileSection(File.ReadAllBytes(Application.streamingAssetsPath + "/test.png")));
        
        //2.文件名，字节数组（常用）
        dataList.Add(new MultipartFormFileSection("上传的文件.png", File.ReadAllBytes(Application.streamingAssetsPath + "/test.png")));
        //3.字符串数据，文件名（常用）
        dataList.Add(new MultipartFormFileSection("12312313212312", "test.txt"));
        //4.字符串数据，编码格式，文件名（常用）
        dataList.Add(new MultipartFormFileSection("12312313212312", Encoding.UTF8, "test.txt"));
        
        //5.表单名，字节数组，文件名，文件类型
        dataList.Add(new MultipartFormFileSection("file", new byte[1024], "test.txt", ""));
        //6.表单名，字符串数据，编码格式，文件名
        dataList.Add(new MultipartFormFileSection("file", "123123123", Encoding.UTF8, "test.txt"));
        #endregion

        #region 知识点二 Post发送相关
        StartCoroutine(Upload());
        #endregion

        #region 知识点三 Put上传相关
        //注意：Put请求类型不是所有的web服务器都认，必须要服务器处理该请求类型那么才能有相应
        #endregion

        #region 总结
        //我们可以利用Post上传数据或上传文件
        //Put主要用于上传文件，但是必须资源服务器支持Put请求类型
        //为了通用性，我们可以统一使用Post请求类型进行数据和资源的上传
        //它的使用和之前的WWW类似，只要前后端制定好规则就可以相互通信了
        #endregion
    }

    IEnumerator Upload()
    {
        //准备上传的数据 
        List<IMultipartFormSection> data = new List<IMultipartFormSection>();
        //键值对相关的 信息 字段数据
        data.Add(new MultipartFormDataSection("Name", "MrTang"));
        //PlayerMsg msg = new PlayerMsg();
        //data.Add(new MultipartFormDataSection("Msg", msg.Writing()));
        //添加一些文件上传文件
        //传2进制文件
        data.Add(new MultipartFormFileSection("TestTest123.png", File.ReadAllBytes(Application.streamingAssetsPath + "/test.png")));
        //传文本文件
        data.Add(new MultipartFormFileSection("123123123123123", "Test123.txt"));

        UnityWebRequest req = UnityWebRequest.Post("http://192.168.50.109:8000/Http_Server/", data);

        req.SendWebRequest();

        while (!req.isDone)
        {
            print(req.uploadProgress);
            print(req.uploadedBytes);
            yield return null;
        }

        print(req.uploadProgress);
        print(req.uploadedBytes);

        if (req.result == UnityWebRequest.Result.Success)
        {
            print("上传成功");
            //req.downloadHandler.data
        }
        else
            print("上传失败" + req.error + req.responseCode + req.result);
    }


    IEnumerator UpLoadPut()
    {
        UnityWebRequest req = UnityWebRequest.Put("http://192.168.50.109:8000/Http_Server/", File.ReadAllBytes(Application.streamingAssetsPath + "/test.png"));

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            print("Put 上传成功");
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

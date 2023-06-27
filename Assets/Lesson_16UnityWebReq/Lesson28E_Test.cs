using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Lesson28E_Test : MonoBehaviour
{
    public RawImage image;
    // Start is called before the first frame update
    void Start()
    {
        //只要保证一运行时 进行该判断 进行动态创建
        // if(NetWWWMgr.Instance == null)
        // {
        //     GameObject obj = new GameObject("WWW");
        //     obj.AddComponent<NetWWWMgr>();
        // }

        // //在任何地方使用NetWWWMgr都没有问题

        // NetWWWMgr.Instance.LoadRes<Texture>("http://192.168.50.109:8000/Http_Server/实战就业路线.jpg", (obj) =>
        // {
        //     //使用加载结束的资源
        //     image.texture = obj;
        // });

        // NetWWWMgr.Instance.LoadRes<byte[]>("http://192.168.50.109:8000/Http_Server/实战就业路线.jpg", (obj) =>
        // {
        //     //使用加载结束的资源
        //     //把得到的字节数组存储到本地
        //     print(Application.persistentDataPath);
        //     File.WriteAllBytes(Application.persistentDataPath + "/www图片.jpg", obj);
        // });

        // NetWWWMgr.Instance.LoadRes<string>("http://192.168.50.109:8000/Http_Server/test.txt", (str) =>
        // {
        //     print(str);
        // });
        //StartCoroutine(GetMsg());

        NetWWWMgr.Instance.UnityWebRequestLoad<Texture>("http://192.168.50.109:8000/Http_Server/实战就业路线.jpg",(tex)=>{
            image.texture = tex;
        });
        NetWWWMgr.Instance.UnityWebRequestLoad<byte[]>("http://192.168.50.109:8000/Http_Server/实战就业路线.jpg", (bytes) =>
        {
            print(bytes.Length);
        });
        NetWWWMgr.Instance.UnityWebRequestLoad<object>("http://192.168.50.109:8000/Http_Server/实战就业路线.jpg", (bytes) =>
        {
            print("保存成功");
        },Application.persistentDataPath+"/UnityWebRequest.jpg");
    }
    IEnumerator GetMsg() {
        UnityWebRequest req = new UnityWebRequest("address",UnityWebRequest.kHttpVerbPOST);
        DownloadHandlerMsg msgHandler = new DownloadHandlerMsg();
        req.downloadHandler = msgHandler;
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success) {
            PlayerMsg msg = msgHandler.GetMsg<PlayerMsg>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class Main : MonoBehaviour
{
    public Button btn;
    public Button btn1;
    public Button btn2;
    public Button btn3;

    public InputField input;
    private void Awake()
    {
        if (NetManager.instance == null)
        {
            GameObject go = new GameObject("Net");
            go.AddComponent<NetManager>();
        }
        NetManager.instance.Connect("127.0.0.1", 8080);
    }
    private void Start()
    {
        //想服务端发送消息
        btn.onClick.AddListener(() =>
        {
            PlayerMsg ms = new PlayerMsg();
            ms.playerID = 1111;
            ms.playerData = new PlayerData();
            ms.playerData.name = "CC's Client Name";
            ms.playerData.atk = 22;
            ms.playerData.lev = 10;
            // HeartMsg hm = new HeartMsg();
            // NetManager.instance.Send(hm);
            NetManager.instance.Send(ms);
        });
        //黏包测试
        btn1.onClick.AddListener(() =>
        {
            PlayerMsg ms = new PlayerMsg();
            ms.playerID = 1001;
            ms.playerData = new PlayerData();
            ms.playerData.name = "CC_1's Client Name";
            ms.playerData.atk = 90;
            ms.playerData.lev = 80;

            PlayerMsg ms1 = new PlayerMsg();
            ms1.playerID = 1002;
            ms1.playerData = new PlayerData();
            ms1.playerData.name = "CC_2's Client Name";
            ms1.playerData.atk = 110;
            ms1.playerData.lev = 100;

            //黏包
            byte[] bytes=new byte[ms.GetBytesNum()+ms1.GetBytesNum()];

            ms.Writing().CopyTo(bytes,0);
            ms1.Writing().CopyTo(bytes,ms.GetBytesNum());
            NetManager.instance.SendTest(bytes);
        });
        //分包测试
        //lambda 表达式参数前面是可以加异步关键字的
        btn2.onClick.AddListener(async () =>
        {
            PlayerMsg ms = new PlayerMsg();
            ms.playerID = 1003;
            ms.playerData = new PlayerData();
            ms.playerData.name = "CC_3's Client Name";
            ms.playerData.atk = 30;
            ms.playerData.lev = 30;


            byte[] bytes=ms.Writing();
            //分包
            byte[] bytes1=new byte[10];
            byte[] bytes2=new byte[bytes.Length-10];
            //第一个包
            Array.Copy(bytes,0,bytes1,0,10);
            //第二个包
            Array.Copy(bytes,10,bytes2,0,bytes.Length-10);

            NetManager.instance.SendTest(bytes1);
            await Task.Delay(500);
            NetManager.instance.SendTest(bytes2);
        });
        //分包黏包测试
        btn3.onClick.AddListener(async () =>
        {
            PlayerMsg ms = new PlayerMsg();
            ms.playerID = 1001;
            ms.playerData = new PlayerData();
            ms.playerData.name = "CC_1's Client Name";
            ms.playerData.atk = 90;
            ms.playerData.lev = 80;

            PlayerMsg ms1 = new PlayerMsg();
            ms1.playerID = 1002;
            ms1.playerData = new PlayerData();
            ms1.playerData.name = "CC_2's Client Name";
            ms1.playerData.atk = 110;
            ms1.playerData.lev = 100;

            byte[] bytes1=ms.Writing();
            byte[] bytes2=ms1.Writing();
            //将消息B二分成两个数组
            byte[] bytes_2_1=new byte[10];
            byte[] bytes_2_2=new byte[bytes2.Length-10];

            Array.Copy(bytes2,0,bytes_2_1,0,10);
            Array.Copy(bytes2,10,bytes_2_2,0,bytes2.Length-10);

            //消息A、B前一段的黏包
            byte[] bytes=new byte[bytes1.Length+bytes_2_1.Length];
            bytes1.CopyTo(bytes,0);
            bytes_2_1.CopyTo(bytes,bytes1.Length);

            NetManager.instance.SendTest(bytes);
            await Task.Delay(500);
            NetManager.instance.SendTest(bytes_2_2);
        });

    }
}

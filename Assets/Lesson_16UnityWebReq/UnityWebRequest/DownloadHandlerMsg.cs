using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
/// <summary>
/// 
/// </summary>
public class DownloadHandlerMsg : DownloadHandlerScript
{
    //需要下载的目标对象
    private BaseMsg msg;
    //用于装载收到的字节数组
    private byte[] cacheBytes;
    private int index=0;
    public DownloadHandlerMsg():base() { }

    //外部等待获取完成后可以得到msg
    public T GetMsg<T>() where T : BaseMsg {
        return msg as T;
    }

    //获取数据的方法
    protected override byte[] GetData()
    {
        return cacheBytes;
    }
    //收到消息时执行的
    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        data.CopyTo(cacheBytes,index);
        index += dataLength;
        return true;
    }
    //收到数据长度的信息时需要执行的
    protected override void ReceiveContentLengthHeader(ulong contentLength)
    {
        cacheBytes = new byte[contentLength];
    }
    //收消息过后,解析数据
    protected override void CompleteContent()
    {
        index = 0;
        int msgID = BitConverter.ToInt32(cacheBytes,index);
        index += 4;
        int msgLength = BitConverter.ToInt32(cacheBytes,index);
        index += 4;
        switch (msgID)
        {
            case 101:
                msg = new PlayerMsg();
                msg.Reading(cacheBytes,index);
                break;
        }
        if(msg==null)
            Debug.Log("no this msg");
        else
            Debug.Log("handle completely");
    }
}

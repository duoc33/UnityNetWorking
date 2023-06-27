using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class PlayerMsg : BaseMsg
{
    public int playerID;
    public PlayerData playerData;
    public override int GetBytesNum()
    {
        return 4+//消息ID的长度
        4+//消息体长度
        4+//playerID的长度
        playerData.GetBytesNum();//继承BaseData类的数据类型的长度
    }
    public override int GetID()
    {
        return 101;
    }
    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        //反序列化不需要解析ID,在解析该对象前，就应该决定好解析哪一个类对象。要在收消息的时候进行解析
        int index =beginIndex;
        playerID = ReadInt(bytes,ref index);
        playerData =ReadData<PlayerData>(bytes,ref index);
        return index-beginIndex;
    }
    public override byte[] Writing()
    {
        int index=0;
        int bytesNum =GetBytesNum();
        byte[] bytes=new byte[GetBytesNum()];
        //先写消息ID
        WriteInt(bytes,GetID(),ref index);
        //再写消息长度L
        WriteInt(bytes,bytesNum-8,ref index);//除去前8个字节存储消息ID和消息Length，真正的消息体长度
        //消息成员变量
        WriteInt(bytes,playerID,ref index);
        WriteData(bytes,playerData,ref index);
        return bytes;
    }
}

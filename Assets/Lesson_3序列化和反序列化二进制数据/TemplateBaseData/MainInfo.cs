using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class MainInfo : BaseData
{
    public short lev;
    public Player player;
    public int hp;
    public string name;
    public bool sex;
    //list<> 类型 同理先序列化一个list的长度int 再序列化list<> 里的每一个类型
    public MainInfo(){
        player=new Player();
    }
    public override int GetBytesNum()
    {
        return 2+
        player.GetBytesNum()+
        4+
        4+
        Encoding.UTF8.GetBytes(name).Length+
        1;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        lev = ReadShort(bytes,ref index);
        player =ReadData<Player>(bytes,ref index);
        hp = ReadInt(bytes,ref index);
        name = ReadString(bytes,ref index);
        sex = ReadBool(bytes,ref index);
        return index -beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteShort(bytes,lev,ref index);
        WriteData(bytes,player,ref index);
        WriteInt(bytes,hp,ref index);
        WriteString(bytes,name,ref index);
        WriteBool(bytes,sex,ref index);
        return bytes;
    }
}
public class Player : BaseData
{
    public int atk;
    public override int GetBytesNum()
    {
        return 4;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index =beginIndex;
        atk = ReadInt(bytes,ref index);
        return index-beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes =new byte[GetBytesNum()];
        WriteInt(bytes,atk,ref index);
        return bytes;
    }
}

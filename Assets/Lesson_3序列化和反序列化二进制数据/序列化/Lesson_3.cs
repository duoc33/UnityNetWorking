using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Lesson_3 : MonoBehaviour
{
   private void Start() {
    //非字符串类型转换成字节数组
    //类BitConverter
    //主要作用：除字符串的其他常用类型(int,char,long,float etc.)和字节数组互相转换
    byte[] bytes = BitConverter.GetBytes(123f);
    byte[] bytes1 = BitConverter.GetBytes(1234);
    // Debug.Log(bytes.Length);
    // Debug.Log(bytes1[0]);

    //字符串类型转换成字节数组
    //Encoding类
    //主要作用：字符串类型转换成字节数组，并且决定转换的字符编码类型，使用UTF8最好
    byte[] bytes2 = Encoding.UTF8.GetBytes("sdkaoospfakpofso");
    // for (int i = 0; i < bytes2.Length; i++)
    // {
    //     Debug.Log(bytes2[i]);
    // }

    //一个类对象转换成二进制
    //1.确定字节数组的容量（转字符串到字节数组中时，先存字符串的字节数组的大小4(UTF8可能的最大值)）
    PlayerInfo playerInfo=new PlayerInfo();
    playerInfo.lev=10;//4个字节
    playerInfo.name="cc";//字符串类型需要事先确定长度，UTF8是可变的编码形式
    playerInfo.atk=70;//2个字节
    playerInfo.sex=true;//1个字节
    // Debug.Log(sizeof(long));
   }
}
public class PlayerInfo
{
    public int lev;
    public string name;
    public short atk;
    public bool sex;
    public byte[] GetBytes()
    {
        int indexNum = sizeof(int) +//lev的字节数组长度
             sizeof(int) +//name字符串转换成字节数组后的数组长度(UTF8转换后可能是1，2，3，4个字节),先存一个字节的长度。
             Encoding.UTF8.GetBytes(name).Length +//得到字符串字节数组的具体长度
             sizeof(short) +
             sizeof(bool);
        byte[] bytes3 = new byte[indexNum];
        int index = 0;
        //等级
        BitConverter.GetBytes(lev).CopyTo(bytes3, index);
        index += sizeof(int);//存储起始点变为0  +  该数据类型的字节长度

        //姓名  
        //存储两部分一个是姓名的长度n个字节(UTF8可能的最大长度4)，一个是具体姓名的字节数组
        byte[] strBytes = Encoding.UTF8.GetBytes(name);
        int num = strBytes.Length;

        BitConverter.GetBytes(num).CopyTo(bytes3, index);
        index += sizeof(int);//更改存储起始点
                             //存储字符串的字节数组
        strBytes.CopyTo(bytes3, index);
        index += num;
 
        //攻击力
        BitConverter.GetBytes(atk).CopyTo(bytes3, index);
        index += sizeof(short);

        //性别
        BitConverter.GetBytes(sex).CopyTo(bytes3, index);
        return bytes3;
    }
}

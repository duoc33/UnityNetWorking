using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Lesson_3_1 : MonoBehaviour
{
   private void Start() {
    //字节数组转非字符串简单类型
    //BitConverter Encoding 一样的关键类，只是用法变化一下
    byte[] bytes =BitConverter.GetBytes(99);
    int i =BitConverter.ToInt32(bytes,0);
    //ToInt16就是short,点击查看BitConverter的方法就知道对应转化方式了
    //Debug.Log(i); //99

    //字节数组转字符串
    byte[] bytes2=Encoding.UTF8.GetBytes("陈创");
    //Debug.Log(bytes2.Length);
    string str = Encoding.UTF8.GetString(bytes2,0,bytes2.Length);//从0开始后面的对应的字节长度的字节转换成字符串
    //Debug.Log(str);

    //将二进制的字节数组转换为 一个类对象

    PlayerInfo playerInfo=new PlayerInfo();
    playerInfo.lev=10;
    playerInfo.name="cc";
    playerInfo.atk=70;
    playerInfo.sex=true;
    
    //1.获取字节数组
    byte[] infoBytes=playerInfo.GetBytes();
    
    //2.将字节数组按照序列化时的顺序进行反序列化（按序列化时的顺序）
    PlayerInfo p1 = new PlayerInfo();
    int index= 0;
    //lev
    p1.lev=BitConverter.ToInt16(infoBytes,index);
    index+=4;
    //print(p1.lev);
    //name, length序列化时候的存的字符串字节数组长度，
    int Length =BitConverter.ToInt32(infoBytes,index);
    index+=4;
    p1.name=Encoding.UTF8.GetString(infoBytes,index,Length);
    index+=Length;
    //print(p1.name);
    //atk
    p1.atk=BitConverter.ToInt16(infoBytes,index);
    index+=2;
    //print(p1.atk);
    //sex
    p1.sex=BitConverter.ToBoolean(infoBytes,index);
    index+=1;
    //print(p1.sex);

    MainInfo info =new MainInfo();
    info.lev=20;
    //info.player=new Player();//必须要实例化里面的自定义类，可以在new MainInfo();的时候实例化
    info.player.atk=60;
    info.hp=100;
    info.name="陈创";
    info.sex=true;

    byte[] bytes1 = info.Writing();
    MainInfo info1 =new MainInfo();
    int info1_length = info1.Reading(bytes1);
    Debug.Log(info1_length);
    Debug.Log(info1.name);
    Debug.Log(info1.player.atk);

   }
}

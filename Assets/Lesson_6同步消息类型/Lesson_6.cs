using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Lesson_6 : MonoBehaviour
{
   private void Start() {
    #region 如何发送自定义类型
    //1.继承BaseData类
    //2.实现其中的序列化和反序列化、获取字节数等相关方法
    //3.发送自定义数据时，序列化
    //4.接收自定义数据时，反序列化
    #endregion

    #region 如何区分消息
    //需要序列化和反序列化的数据很多
    //需要给每一个类编一个消息ID，在每个二进制信息前面加2个字节的short或者4个字节的short去表示一类对象。
    #endregion

    #region 实践
    //1.创建消息基类，基类继承BaseData，消息基类添加获取信息的ID的方法或属性。
    //2.让想要被发送的消息继承该类，实现反序列化或序列化方法
    //3.修改客户端和服务端收发消息逻辑
    #endregion
   }
}

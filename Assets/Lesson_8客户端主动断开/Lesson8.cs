using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// 
/// </summary>
public class Lesson8 : MonoBehaviour
{
    #region 目前的客户端主动断开连接
    //虽然Unity里的客户端断开连接了，但是服务端里的客户端socket仍然是连接的。
    //服务端无法及时获得信息
    #endregion

    #region 解决目前客户端主动断开连接

    //方法一：
    //1.客户端使用Disconnect方法主动断开
    //2.服务端
    //2.1收发消息时判断socket是否断开（ClientSocket.Connected语法）
    //2.2处理并删除记录的socket的相关逻辑(线程锁),因为有服务器有许多人访问，
    //所以在操作删除存储的clientSocket字典时，用一个锁，锁住空间。
    //如果多个线程同时操作这个内存，容易报错

    //方法二：
    //自定义退出消息
    //让服务端收到该消息后就知道客户端要主动断开了
    //然后服务器处理释放socket相关工作
    
    //两个方法结合，才能让服务端知道客户端断开了

    #endregion
}

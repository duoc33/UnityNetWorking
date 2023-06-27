using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Lesson9 : MonoBehaviour
{
    private void Start() {
        #region 心跳消息
        //心跳消息：在长连接中，客户端和服务端之间定期发送的一种特殊的数据包
        //用于通知对方自己还在线，以确保长连接的有效性，发送间隔是固定的，就像心跳一样一直存在
        #endregion
        #region 为什么需要心跳消息
        //1.避免非正常关闭客户端时，服务器无法正常收到关闭消息
        //通过心跳消息我们可以自定义超时判断，如果超时没有收到客户端消息，证明客户端断开连接
        //避免停电或者强制结束进程后客户端无法及时发送断开连接的信息给服务器

        //2.避免客户端长期不发送消息，防火墙或路由器会断开连接，我们可以通过心跳消息一直保持活跃状态
        #endregion
        #region 实现心跳消息
        //客户端
        //定时发送消息

        //服务端
        //不停检查上次收到的某客户端消息的时间，如果超时则认为连接已断开。
        #endregion
    }
}

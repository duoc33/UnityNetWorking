using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Lesson_7 : MonoBehaviour
{
    private void Start() {
    #region 分包 黏包
        //分包 黏包：在网络通信中由于各种因素（网络环境、API规则等）造成消息与消息之间出现的两种状态。
        //分包：一个消息分成了多个消息发送
        //黏包：一个消息和另一个消息黏在了一起
        //分包和黏包可能同时发生
        #endregion

        #region 如何解决分包黏包问题？
        //在消息类里再存储一个长度L
        #endregion

        #region 实践解决
        //1.为所有消息添加头部信息，用于存储消息长度
        //2.根据分包黏包的表现情况，修改接收消息的逻辑
        #endregion
    }
}

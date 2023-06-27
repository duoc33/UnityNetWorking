using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson15 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 搭建HTTP服务器的几种方式
        //1.使用别人做好的HTTP服务器软件，一般作为资源服务器时使用该方式（学习阶段建议使用）
        //2.自己编写HTTP服务器应用程序，一般作为Web服务器 或者 短连接游戏服务器 时使用该方式（工作后由后端程序员来做）

        //一般在工作中不会由我们来完成这部分工作
        #endregion

        #region 知识点二 使用别人做好的HTTP服务器软件来搭建HTTP资源服务器
        //下载hfs等HTTP服务器软件
        //在想要作为HTTP资源服务器的电脑上运行之
        #endregion

        #region 知识点三 使用别人做好的Web服务器进行测试
        //我们在学习过程中，可以直接在别人做好的Web服务器上获取信息和资源
        //比如我们可以下载任意网站上可被下载的图片
        #endregion

        #region 总结
        //在实际商业项目开发当中
        //HTTP 资源服务器 可以自己写也可以用别人做好的软件
        //HTTP 网站服务器 或 游戏服务器 需要自己根据需求进行实现
        //这些工作一般都是由后端或者运维程序员来进行制作
        //我们主要做了解
        //我们之后主要着重学习前端HTTP相关的知识点

        //在游戏开发时，我们更多时候需要的是HTTP的资源服务器
        //除非你要做短连接游戏，那么后端程序可以以HTTP协议为基础来开发服务端应用程序
        //我们只需要学习前端用于进行HTTP通信的相关知识即可
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

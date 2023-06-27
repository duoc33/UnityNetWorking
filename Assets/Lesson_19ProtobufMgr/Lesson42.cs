using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson42 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 回顾Protobuf
        //Protobuf全称是 protocol-buffers（协议缓冲区）
        //是谷歌提供给开发者的一个开源的协议生成工具
        //它的主要工作原理和我们之前做的自定义协议工具类似
        //只不过它更加的完善，可以基于协议配置文件生成
        //C++、Java、C#、Objective-C、PHP、Python、Ruby、Go
        //等等语言的代码文件

        //我们之前学习了如何使用它
        //已经能够使用Protobuf来配置协议，生成协议，使用协议了
        #endregion

        #region 知识点二 Protobuf-Net是什么
        //早期的Protobuf并不支持C#
        //所以国外大神Marc Gravell在Protobuf的基础上进行了.net环境下的移植
        //并发布到了GitHub
        //让我们可以基于Protobuf的规则进行C#的代码生成，对象的序列化和反序列化

        //Protobuf-Net的Github地址：https://github.com/protobuf-net/protobuf-net

        //注意：
        //1. Protobuf不支持.Net3.5及以下版本
        //   所以如果想在Unity的老版本中使用Protobuf我们只能使用Protobuf-Net
        //   而在较新版本的Unity中不存在这个问题
        //2. 如何判断是否支持？
        //   只要把Protobuf相关dll包导入后能够正常使用不报错，则证明支持
        #endregion

        #region 知识点三 下载获取Protobuf-Net
        //Protobuf-Net的Github地址：https://github.com/protobuf-net/protobuf-net

        //我们需要在Github上去获取对应的工程生成后获取
        //1. DLL库文件
        //2. 根据配置生成脚本的编译器可执行程序 
        #endregion

        #region 总结
        //Protobuf-net相对来说是较老的生产方式
        //但是它可以解决老版本Unity使用Protobuf的问题
        //它的使用方式和之前学习的Protobuf相关知识类似
        //只是获取DLL文件、protoc.exe文件的方式不同而已
        //如果想要详细学习，可以前往Github看相关说明
        //在这里我们就不详细讲解了

        //目前我们使用的较新Unity版本直接使用之前学习的Protobuf相关知识完成需求即可
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

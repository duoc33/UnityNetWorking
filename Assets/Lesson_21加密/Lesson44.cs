using GamePlayerTest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson44 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 什么是消息加密解密
        //我们在网路传输时，会把数据转换为字节数组以2进制的形式进行传输
        //理论上来说，如果有人截取篡改了消息，或者从前端发假消息给后端
        //就可能产生作弊行为
        //消息的加密解密 可以有效避免作弊行为的产生

        //加密
        //采用一些方式对数据进行处理后，使数据从表面上看，已经不能表达出原有的意思
        //别人就算获取到了你的信息，也无法知道你的内容的含义和规则
        //这样可以让我们的数据更加的安全，降低被篡改的可能性

        //解密
        //通过对加密过的数据采用某些方法，去还原原有数据，从而获取目标数据

        //这部分知识点 和我们在数据持久化四部曲中——2进制 学习的加密内容类似
        //其实就是在
        //发消息时，对我们的消息2进制数据进行加密（一般只对消息体加密）
        //收到消息时，对2进制数据进行解密（一般只对消息体解密）
        #endregion

        #region 知识点二 加密是否是100%安全？
        //一定记住加密只是提高破解门槛，没有100%保密的数据
        //通过各种尝试始终是可以破解加密规则的，只是时间问题
        //加密只能提升一定的安全性

        //对于大多数情况下已经够用了，除非专门有人针对你们的产品进行破解
        //但是遇到这种情况 也证明你的产品已经足够成功了
        #endregion

        #region 知识点三 加密解密的相关名词解释
        //明文：待加密的报文（内容）
        //密文：加密后的报文（内容）
        //密钥：加密过程中或解密过程中输入的数据
        //算法：将明文和密钥相结合进行处理，生成密文的方法，叫加密算法
        //     将密文和密钥相结合进行处理，生成明文的方法，叫解密算法
        #endregion

        #region 知识点四 了解加密算法分类
        //1.单向加密
        //  将数据进行计算变成另一种固定长度的值，这种加密是不可逆的

        //  常用算法
        //  MD5、SHA1、SHA256等

        //  用途：这种加密在网络传输中不会使用，主要用到其它功能当中，比如密码的单向加密

        //2.对称加密技术
        //  使用同一个密钥，对数据镜像加密和解密（用密钥对明文加密，用密钥对密文解密）

        //  常用算法
        //  DES、3DES、IDEA、AES等

        //  优点：计算量小，加密速度快、效率高
        //  缺点：如果知道了密钥和算法，就可以进行解密

        //  用途：网路通讯中可以使用对称加密技术，这个密钥可以是由后端下发的，每次建立通讯后都会变化的

        //3.非对称加密技术
        //  在加密过程中，需要一对密钥，不公开的密钥称为私钥，公开的那一个密钥称为公钥
        //  也可以称为公开密钥加密
        //  从一对密钥中的任何一个密钥都不能计算出另一个密钥
        //  使用一对密钥中的任何一个加密，只有另一个密钥才能解密。如果截获公钥加密数据，没有私钥也无法解密

        //  常用算法
        //  RSA、DSA等

        //  优点：安全性高，即使获取到了公钥，没有私钥也无法进行解密
        //  缺点：算法复杂，加密速度较慢

        //  用途：对安全性要求较高的场景，并且可以接受较慢的加密速度的需求可以使用非对称加密技术
        //        以后在对接一些支付SDK时经常会看到平台提供的就是非对称加密技术

        //关于这些加密算法
        //有很多的别人写好的第三发加密算法库
        //可以直接获取用于在程序中对数据进行加密
        //也可以自己基于加密算法原理来设计自己的规则
        //这里我们不深究 感兴趣的同学可以自己去了解
        #endregion

        #region 知识点五 用简单的异或加密感受加密的作用
        //异或加密特点
        //密钥为一个整数
        //明文 异或 密钥 得到 密文
        //密文 异或 密钥 得到 明文

        TestMsg msg = new TestMsg();
        msg.ListInt.Add(1);
        msg.TestBool = false;
        msg.TestD = 5.5;
        msg.TestInt32 = 99;
        msg.TestMap.Add(1, "唐老狮");
        msg.TestMsg2 = new TestMsg2();
        msg.TestMsg2.TestInt32 = 88;
        msg.TestMsg3 = new TestMsg.Types.TestMsg3();
        msg.TestMsg3.TestInt32 = 66;

        msg.TestHeart = new GameSystemTest.HeartMsg();
        msg.TestHeart.Time = 7777;

        byte[] bytes = NetTool.GetProtoBytes(msg);
        //异或加密算法
        //密钥声明
        byte s = 55;
        //异或加密
        for (int i = 0; i < bytes.Length; i++)
            bytes[i] ^= s;

        //异或解密
        for (int i = 0; i < bytes.Length; i++)
            bytes[i] ^= s;

        TestMsg msg2 = NetTool.GetProtoMsg<TestMsg>(bytes);
        print(msg2.TestMsg3.TestInt32);
        #endregion

        #region 总结
        //有各种各样的加密算法可以应用在网络通讯的消息加密中
        //由于加密算法完全是可以单独开一门课来讲解的内容
        //所以我们在这里只做了解
        //我们只要知道加密对于我们意义即可
        //当需要用到时，再去学习对应的加密算法也是可以的
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

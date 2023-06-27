using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// namespace GamePlayerTest
// {
//     public enum ENUM_NAME
//     {
//         MAIN = 1,
//         OTHER,
//     }

//     public class PlayerData : BaseData
//     {
//         public int id;
//         public float atk;
//         public long lev;
//         public int[] arrays;
//         public List<int> list;
//         public Dictionary<int, string> dic;

//         public override int GetBytesNum()
//         {
//             throw new System.NotImplementedException();
//         }

//         public override int Reading(byte[] bytes, int beginIndex = 0)
//         {
//             throw new System.NotImplementedException();
//         }

//         public override byte[] Writing()
//         {
//             throw new System.NotImplementedException();
//         }
//     }

//     public class PlayerMsg:BaseMsg
//     {
//         public override int GetID()
//         {
//             return base.GetID();
//         }
//     }
// }



public class Lesson35 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 选择哪种格式配置协议？
        //1.xml
        //2.json
        //3.excel
        //4.自定义
        //等等
        //我们可以根据自己的喜好选择
        //选择方便配置的，好用的即可
        //配置的主要目的是确定
        //类名、成员变量名

        //之后根据读取的这些配置信息
        //再通过代码按照规则自动生成对应的类文件

        //我个人喜欢使用xml作为协议配置文件
        //学会xml配置，其它的方式都是大同小异的
        //我们主要是学习制作思路和流程
        //以后的项目中，大家根据自己的喜好选择即可
        #endregion

        #region 知识点二 我们以xml配置为例 —— 制定配置规则
        //xml相关知识，可以在数据持久化之xml中进行学习
        //1. 创建xml配置文件
        //2. 制定配置规则
        //   1.枚举规则 
        //   2.数据类规则
        //   3.消息类规则
        #endregion

        #region 知识点三 读取配置信息
        //1.读取xml文件信息
        XmlDocument xml = new XmlDocument();
        xml.Load(Application.dataPath + "/Lesson_17CustomProtocols/ProtocolsConfig/Lesson35.xml");
        //2.读取各节点元素
        //2-1: 跟节点读取
        XmlNode root = xml.SelectSingleNode("messages");
        //2-2: 读取出所有枚举结构类节点
        XmlNodeList enumList = root.SelectNodes("enum");
        foreach (XmlNode enumNode in enumList)
        {
            print("**************");
            print("******枚举******");
            print("枚举名字：" + enumNode.Attributes["name"].Value);
            print("枚举所在命名空间：" + enumNode.Attributes["namespace"].Value);
            print("******枚举成员******");
            XmlNodeList fields = enumNode.SelectNodes("field");
            foreach (XmlNode field in fields)
            {
                string str = field.Attributes["name"].Value;
                if (field.InnerText != "")
                    str += " = " + field.InnerText;
                str += ",";
                print(str);
            }
        }
        //2-3: 读取出所有数据结构类节点
        XmlNodeList dataList = root.SelectNodes("data");
        foreach (XmlNode data in dataList)
        {
            print("**************");
            print("******数据结构类******");
            print("数据类名：" + data.Attributes["name"].Value);
            print("数据类所在命名空间：" + data.Attributes["namespace"].Value);
            print("******数据类成员******");
            XmlNodeList fields = data.SelectNodes("field");
            foreach (XmlNode field in fields)
            {
                print(field.Attributes["type"].Value + " " + field.Attributes["name"].Value + ";");
            }
        }
        //2-4: 读取出所有消息节点
        XmlNodeList msgList = root.SelectNodes("message");
        foreach (XmlNode msg in msgList)
        {
            print("**************");
            print("******消息类******");
            print("消息类名：" + msg.Attributes["name"].Value);
            print("消息类所在命名空间：" + msg.Attributes["namespace"].Value);
            print("消息ID：" + msg.Attributes["id"].Value);
            print("******数据类成员******");
            XmlNodeList fields = msg.SelectNodes("field");
            foreach (XmlNode field in fields)
            {
                print(field.Attributes["type"].Value + " " + field.Attributes["name"].Value + ";");
            }
        }
        #endregion

        #region 总结
        //利用配置文件配置消息、数据结构、枚举的目的
        //1.减少工作量，配置一次，之后自动化生成各种语言对应的类文件
        //2.减少沟通成本，避免前后端语言不同时，手动写代码出现前后端不统一的问题
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

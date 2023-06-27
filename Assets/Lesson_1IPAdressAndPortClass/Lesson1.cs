using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
/// <summary>
/// IP Port
/// </summary>
public class Lesson1 : MonoBehaviour
{
    //4个字节的字节数组，4x8=32位的ip地址，IPv4,
    //花括号里10进制，需要转成
   byte[] ipAddress=new byte[]{118,102,111,11};
   //可以将括号里10进制或则8进制ip转成2进制,
   IPAddress ip1=new IPAddress(new byte[]{118,102,111,11});
   IPAddress ip2=new IPAddress(0x79666F0B);
   IPAddress ip3=IPAddress.Parse("118.102.111.11");

   //特殊ip地址127.0.0.1 本机地址

   //一些静态成员
   //获取可用的IPv6地址
   IPAddress ip4 = IPAddress.IPv6Any;


   //IPEndPoint ip和portd的组合类 实例的时候给ip(long的16进制长整型)和prot端口赋值

   //某个远程计算机的地址和某个应用程序的端口号
   IPEndPoint ipPoint1 = new IPEndPoint(0x79666F0B,8080);

   IPEndPoint ipPonint2 =new IPEndPoint(IPAddress.Parse("118.102.111.11"),8080);
}

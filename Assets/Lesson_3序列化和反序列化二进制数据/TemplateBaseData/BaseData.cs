using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public abstract class BaseData
{
    /// <summary>
    /// 用于子类重写的，获取字节数组容器大小的方法
    /// </summary>
    /// <returns></returns>
    public abstract int GetBytesNum();
    /// <summary>
    /// 把成员变量 序列化为 对应的的字节数组
    /// </summary>
    /// <returns></returns>
    public abstract byte[] Writing();
    /// <summary>
    /// 反序列字节数组为一个类对象的方法 beginIndex可以用来计算该类在字节数组的长度，可以给它赋值为外部的字节数组的index值
    /// </summary>
    /// <param name="bytes">反序列化的字节数组</param>
    /// <param name="beginIndex">开始反序列化该对象的字节数组索引，默认为0</param>
    /// <returns>返回该对象类字节数组的长度</returns>
    public abstract int Reading(byte[] bytes,int beginIndex =0);

    #region 类序列化为字节数组
    protected void WriteInt(byte[] bytes, int value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += 4;
    }
    protected void WriteShort(byte[] bytes, short value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += 2;
    }
    protected void Writelong(byte[] bytes, long value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += 8;
    }
    protected void WriteFloat(byte[] bytes, float value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += 4;
    }
    protected void WriteByte(byte[] bytes, byte value, ref int index)
    {
        bytes[index] = value;
        index += 1;
    }
    protected void WriteBool(byte[] bytes, bool value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += 1;
    }
    /// <summary>
    /// 字符串字节存进字节数组至少需要4+n个字节空间
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="value"></param>
    /// <param name="index"></param>
    protected void WriteString(byte[] bytes, string value, ref int index)
    {
        byte[] strBytes=Encoding.UTF8.GetBytes(value);
        //先存string字节数组的长度(最大4),其实就是存一个int类型的字符串长度信息
        WriteInt(bytes,strBytes.Length,ref index);
        //再存string字节数组
        strBytes.CopyTo(bytes,index);
        index+=strBytes.Length;
    }
    /// <summary>
    /// 当自定义类继承自BaseData类里又包含了BaseData类时，处理里面的BaseData类型所需要用到的方法
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="data"></param>
    /// <param name="index"></param>
    protected void WriteData(byte[] bytes,BaseData data,ref int index)
    {
        data.Writing().CopyTo(bytes,index);
        index+=data.GetBytesNum();
    }
    #endregion

    #region 字节数组反序列化为类对象
    protected int ReadInt(byte[] bytes,ref int index)
    {
        int value = BitConverter.ToInt32(bytes,index);
        index+=4;
        return value;
    }
    protected short ReadShort(byte[] bytes,ref int index)
    {
        short value = BitConverter.ToInt16(bytes,index);
        index+=2;
        return value;
    }
    protected long ReadLong(byte[] bytes,ref int index)
    {
        long value = BitConverter.ToInt64(bytes,index);
        index+=8;
        return value;
    }
    protected Double ReadDouble(byte[] bytes,ref int index)
    {
        Double value = BitConverter.ToDouble(bytes,index);
        index+=sizeof(double);
        return value;
    }
    protected float ReadFloat(byte[] bytes,ref int index)
    {
        float value = BitConverter.ToSingle(bytes,index);
        index+=4;
        return value;
    }
    protected byte ReadByte(byte[] bytes,ref int index)
    {
        byte value =bytes[index];
        index+=1;
        return value;
    }
    protected bool ReadBool(byte[] bytes,ref int index)
    {
        bool value =BitConverter.ToBoolean(bytes,index);
        index+=1;
        return value;
    }
    protected string ReadString(byte[] bytes,ref int index)
    {
        //读长度
        int length = ReadInt(bytes,ref index);
        string value =Encoding.UTF8.GetString(bytes,index,length);
        index+=length;
        return value;
    }
    //后面加约束表示该数据必须有 无参构造函数，并继承自BaseData
    protected T ReadData<T>(byte[] bytes,ref int index) where T:BaseData,new()
    {
        T value =new T();
        index += value.Reading(bytes,index);
        return value;
    }
    #endregion
}

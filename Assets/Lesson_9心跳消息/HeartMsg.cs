using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class HeartMsg : BaseMsg
{
    public override int GetBytesNum()
    {
        return 8;//ID+Length
    }
    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        return 0;
    }
    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes,GetID(),ref index);
        WriteInt(bytes, 0, ref index);
        return bytes;
    }
    public override int GetID()
    {
        return 999;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public interface IModel
{
   bool Login(string username, string password);
}
public class Model : IModel
{
    public bool Login(string username, string password)
    {
        return true;
    }
}

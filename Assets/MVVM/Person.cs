using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public void Birthday()
    {
        Age++;
    }
}

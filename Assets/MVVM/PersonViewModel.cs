using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class PersonViewModel
{
    private Person _person;

    public string Name
    {
        get { return _person.Name; }
        set { _person.Name = value; }
    }

    public int Age
    {
        get { return _person.Age; }
        set { _person.Age = value; }
    }

    public PersonViewModel(Person person)
    {
        _person = person;
    }
}

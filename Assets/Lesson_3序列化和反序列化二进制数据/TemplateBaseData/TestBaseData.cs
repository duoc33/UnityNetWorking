using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class TestBaseData : MonoBehaviour
{
   private void Start() {
    MainInfo mainInfo =new MainInfo();
    mainInfo.lev=87;
    mainInfo.player=new Player();
    mainInfo.player.atk=77;
    mainInfo.hp=100;
    mainInfo.name="cc";
    mainInfo.sex=true;
    byte[] bytes=mainInfo.Writing();
   }
}

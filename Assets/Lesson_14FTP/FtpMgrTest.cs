using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class FtpMgrTest : MonoBehaviour
{
    private void Start()
    {
        FtpMgr.Instance.UploadFile("MyPic.png", Application.streamingAssetsPath + "/test.png", () =>
        {
            print("upload successfully,delegate invoke");
        });
        FtpMgr.Instance.DownloadFile("scenery.jpg", Application.persistentDataPath + "/MyScenery1.jpg", () =>
        {
            print(Application.persistentDataPath);
            print("download successfully,delegate invoke");
        });
        print("test");
    }
}

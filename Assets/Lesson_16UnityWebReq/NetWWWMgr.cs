using System.Net.Mime;
using System.Linq.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetWWWMgr : MonoBehaviour
{
    private static NetWWWMgr instance;

    public static NetWWWMgr Instance => instance;
    private string HTTP_SERVER_PATH = "http://192.168.50.109:8000/Http_Server/";

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// �ṩ���ⲿ������Դ�õķ���
    /// </summary>
    /// <typeparam name="T">��Դ������</typeparam>
    /// <param name="path">��Դ��·�� http ftp file��֧��</param>
    /// <param name="action">���ؽ�����Ļص����� ��ΪWWW��ͨ�����Эͬ�����첽���ص� ���Բ������ϻ�ȡ��� ��Ҫ�ص���ȡ</param>
    [Obsolete]
    public void LoadRes<T>(string path, UnityAction<T> action) where T : class
    {
        StartCoroutine(LoadResAsync<T>(path, action));
    }

    [Obsolete]
    private IEnumerator LoadResAsync<T>(string path, UnityAction<T> action) where T : class
    {
        //����www���� �������ػ����
        WWW www = new WWW(path);
        //�ȴ����ػ��߼��ؽ������첽��
        yield return www;
        //���û�д��� ֤�����سɹ�
        if (www.error == null)
        {
            //����T���͵�����  ����ʹ���������͵���Դ ���ݸ��ⲿ
            if(typeof(T) == typeof(AssetBundle))
            {
                action?.Invoke(www.assetBundle as T);
            }
            else if (typeof(T) == typeof(Texture))
            {
                action?.Invoke(www.texture as T);
            }
            else if (typeof(T) == typeof(AudioClip))
            {
                action?.Invoke(www.GetAudioClip() as T);
            }
            else if (typeof(T) == typeof(string))
            {
                action?.Invoke(www.text as T);
            }
            else if (typeof(T) == typeof(byte[]))
            {
                action?.Invoke(www.bytes as T);
            }
            //�Զ���һЩ���� ������Ҫ��bytes ת���ɶ�Ӧ��������ʹ��
        }
        //������� ����ʾ����
        else
        {
            Debug.LogError("www������Դ����" + www.error);
        }
    }

    [Obsolete]
    public void SendMsg<T>(BaseMsg msg, UnityAction<T> action) where T : BaseMsg
    {
        StartCoroutine(SendMsgAsync<T>(msg, action));
    }

    [Obsolete]
    private IEnumerator SendMsgAsync<T>(BaseMsg msg, UnityAction<T> action) where T : BaseMsg
    {
        //��Ϣ����
        WWWForm data = new WWWForm();
        //׼��Ҫ���͵���Ϣ����
        data.AddBinaryData("Msg", msg.Writing());

        WWW www = new WWW(HTTP_SERVER_PATH, data);
        //����Ҳ����ֱ�Ӵ��� 2�����ֽ����� ֻҪ�ͺ�˶��ù��� ��ô�����ǿ��Ե�
        //WWW www = new WWW("HTTP_SERVER_PATH", msg.Writing());

        //�첽�ȴ� ���ͽ��� �Ż����ִ�к���Ĵ���
        yield return www;

        //������Ϲ��� �յ���Ӧ 
        //��Ϊ ��˷����������� Ҳ��һ���̳���BaseMsg���һ���ֽ��������
        if (www.error == null)
        {
            //�Ƚ��� ID����Ϣ����
            int index = 0;
            int msgID = BitConverter.ToInt32(www.bytes, index);
            index += 4;
            int msgLength = BitConverter.ToInt32(www.bytes, index);
            index += 4;
            //�����л� BaseMsg
            BaseMsg baseMsg = null;
            switch (msgID)
            {
                case 1001:
                    baseMsg = new PlayerMsg();
                    baseMsg.Reading(www.bytes, index);
                    break;
            }
            if (baseMsg != null)
                action?.Invoke(baseMsg as T);
        }
        else
            Debug.LogError("����Ϣ������" + www.error);
    }

    /// <summary>
    /// UnityWebRequset �ⲿ������Դ����
    /// </summary>
    /// <typeparam name="T">��Դ����(byte[]��objcet(File)��AssetBundle��Texture��Audioclip�ȵ�)</typeparam>
    /// <param name="path">��Դ����·��</param>
    /// <param name="action">������ɺ�Ļص�</param>
    /// <param name="localPath">�����Ҫ�洢�ڱ��ص�·��</param>
    /// <param name="type">�������Ƶ�ļ�����Ƶ�ļ�������</param>
    public void UnityWebRequestLoad<T>(string path,UnityAction<T> action,string localPath="",AudioType type=AudioType.MPEG)where T :class {
        StartCoroutine(UnityWebRequestLoadAsync<T>(path, action,localPath = "",type = AudioType.MPEG));
    }
    private IEnumerator UnityWebRequestLoadAsync<T>(string path, UnityAction<T> action, 
    string localPath = "", AudioType type = AudioType.MPEG) where T : class{
        UnityWebRequest req = new UnityWebRequest(path,UnityWebRequest.kHttpVerbGET);
        if (typeof(T) == typeof(byte[])) 
            req.downloadHandler = new DownloadHandlerBuffer();
        else if (typeof(T) == typeof(Texture))
            req.downloadHandler = new DownloadHandlerTexture();
        else if (typeof(T) == typeof(AssetBundle))
            //�ڶ���������AB����У���룬Ĭ��0
            req.downloadHandler = new DownloadHandlerAssetBundle(req.url,0);
        else if (typeof(T) == typeof(object))//�����object�����ļ�������ΪFileΪ��̬�࣬������Ϊ���Ͳ���
            req.downloadHandler = new DownloadHandlerFile(localPath);
        else if (typeof(T) == typeof(AudioClip))
            req = UnityWebRequestMultimedia.GetAudioClip(path,type);
        else
        {
            Debug.Log("δ֪��Ϣ");
            yield break;
        }
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success) {
            if (typeof(T) == typeof(byte[]))
                action?.Invoke(req.downloadHandler.data as T);
            else if (typeof(T) == typeof(Texture))
                //action?.Invoke((req.downloadHandler as DownloadHandlerTexture).texture as T);
                action?.Invoke(DownloadHandlerTexture.GetContent(req) as T);
            else if (typeof(T) == typeof(AssetBundle))
                action?.Invoke((req.downloadHandler as DownloadHandlerAssetBundle).assetBundle as T);
            else if (typeof(T) == typeof(object))
                action?.Invoke(null);
            else if (typeof(T) == typeof(AudioClip))
                action?.Invoke(DownloadHandlerAudioClip.GetContent(req) as T);
            else
            {
                Debug.Log("δ֪��Ϣ");
                yield break;
            }
        }
        else
        {
            Debug.Log("��ȡ��Ϣʧ��");
        }
    }


    /// <summary>
    /// �ϴ��ļ��ķ���
    /// </summary>
    /// <param name="fileName">�ϴ���ȥ���ļ���</param>
    /// <param name="localPath">������Ҫ�ϴ��ļ���·��</param>
    /// <param name="action">�ϴ���ɺ�Ļص�����</param>
    public void UploadFile(string fileName, string localPath, UnityAction<UnityWebRequest.Result> action)
    {
        StartCoroutine(UploadFileAsync(fileName, localPath, action));
    }

    private IEnumerator UploadFileAsync(string fileName, string localPath, UnityAction<UnityWebRequest.Result> action)
    {
        //���Ҫ�ϴ��ļ�������
        List<IMultipartFormSection> dataList = new List<IMultipartFormSection>();
        dataList.Add(new MultipartFormFileSection(fileName, File.ReadAllBytes(localPath)));

        UnityWebRequest req = UnityWebRequest.Post(HTTP_SERVER_PATH, dataList);

        yield return req.SendWebRequest();

        action?.Invoke(req.result);
        //������ɹ�
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("�ϴ���������" + req.error + req.responseCode);
        }
    }

}

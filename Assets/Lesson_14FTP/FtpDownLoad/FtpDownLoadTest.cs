using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class FtpDownLoadTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ ʹ��FTP�����ļ��ؼ���
        //1.ͨ��ƾ֤
        //  ����Ftp���Ӳ���ʱ��Ҫ���˺�����
        //2.�������� WebRequestMethods.Ftp
        //  ��������Ҫ���е�Ftp����
        //3.�ļ������ FileStream �� Stream
        //  �ϴ�������ʱ����ʹ�õ��ļ���
        //  �����ļ���ʹ��FtpWebResponse���ȡ
        //4.��֤FTP�������Ѿ�����
        //  �����ܹ���������
        #endregion

        #region ֪ʶ��� FTP����
        try
        {
            //1.����һ��Ftp����
            //������ϴ���ͬ���ϴ����ļ��� ���Լ������  ���ص��ļ��� һ������Դ���������е�
            FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://127.0.0.1/scenery.jpg")) as FtpWebRequest;
            //2.����ͨ��ƾ֤(�����֧������ �ͱ���������һ��)
            req.Credentials = new NetworkCredential("DuoChen", "duoc199945");
            //������Ϻ� �Ƿ�رտ������ӣ����Ҫ���ж�β��� ��������Ϊfalse
            req.KeepAlive = false;
            //3.���ò�������
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            //4.ָ����������
            req.UseBinary = true;
            //��������Ϊ��
            req.Proxy = null;
            //5.�õ��������ص�������
            //�൱�ڰ������͸�FTP������ ����ֵ �ͻ�Я��������Ҫ����Ϣ
            FtpWebResponse res = req.GetResponse() as FtpWebResponse;
            //��������ص���
            Stream downLoadStream = res.GetResponseStream();

            //6.��ʼ����
            print(Application.persistentDataPath);//(��streamingAssets��һ��(���ƶ��˲���д)������ɶ���д)
            //File.Create�Ǵ���һ���ļ�����ȡ����,File.OpenRead�Ƕ�ȡ��
            using (FileStream fileStream = File.Create(Application.persistentDataPath + "/MyScenery.jpg"))
            {
                byte[] bytes = new byte[1024];
                //��ȡ����������������
                int contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                //һ��һ��� ���ص���������
                while (contentLength != 0)
                {
                    //�Ѷ�ȡ�������ֽ����� д�뵽�����ļ�����
                    fileStream.Write(bytes, 0, contentLength);
                    //�����Ǽ�����
                    contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                }
                //���ؽ��� �ر���
                downLoadStream.Close();
                fileStream.Close();
            }
            print("���ؽ���");
        }
        catch (Exception e)
        {
            print("���س���" + e.Message);
        }
        #endregion

        #region �ܽ�
        //C#�Ѿ���Ftp��ز�����װ�ĺܺ���
        //����ֻ��Ҫ��ϤAPI��ֱ��ʹ�����ǽ���FTP���ؼ���
        //������Ҫ���Ĳ�����
        //�������ļ���FTP�������ֽ�����д�뵽�����ļ�����
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

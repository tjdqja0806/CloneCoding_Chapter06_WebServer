using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GameResult
{
    public string UserName;
    public int Score;
}

//Login => (Auth Token)
//Game Start - DB(Redis, RDBMS(
//Game Result - Save

public class WebManager : MonoBehaviour
{
    string _baseUrl = "https://localhost:7146/api";

    void Start()
    {
        GameResult res = new GameResult()
        {
            UserName = "Synolax",
            Score = 888
        };

        SendPostRequest("ranking", res, (uwr) =>
        {
            Debug.Log("TOTD : UI 갱신 진행");
        });

        SendGetAllRequest("ranking", (uwr) =>
        {
            Debug.Log("TOTD : UI 갱신 진행");
        });
    }

    public void SendPostRequest(string url, object obj, Action<UnityWebRequest> callback)
    {
        StartCoroutine(CoSendWebRequest("ranking", "POST", obj, callback));
    }

    public void SendGetAllRequest(string url, Action<UnityWebRequest> callback)
    {
        StartCoroutine(CoSendWebRequest("ranking", "GET", null, callback));
    }

    IEnumerator CoSendWebRequest(string url, string method, object obj, Action<UnityWebRequest> callback)
    {
        yield return null;

        string sendUrl = $"{_baseUrl}/{url}/";

        byte[] jsonByte = null;

        if(obj != null)
        {
            string jsonStr = JsonUtility.ToJson(obj);
            jsonByte = Encoding.UTF8.GetBytes(jsonStr);
        }

        var uwr = new UnityWebRequest(sendUrl, method);
        uwr.uploadHandler = new UploadHandlerRaw(jsonByte);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-type", "application/json");

        yield return uwr.SendWebRequest();

        if(uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            Debug.Log("Recv" + uwr.downloadHandler.text);
            callback.Invoke(uwr);
        }
    }
}

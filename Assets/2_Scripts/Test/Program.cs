using System;
using UnityEngine;

public class Program : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Hello, World!");
        Publisher publisher = new Publisher();
        publisher.msg += ResultProcess;
        publisher.msg += OtherProcess;


        publisher.SendMessage("추가 문제 주세요");

        Debug.Log("작업 완료");
    }

    void ResultProcess(string msg)
    {
        Debug.Log($"메세지 수신: { msg}");
    }
    void OtherProcess(string text)
    {
        Debug.Log($"다른처리: {text}");
    }
}

public class Publisher
{
    public delegate void OnMessage(string msg);
    public event OnMessage msg;

    public void SendMessage(string text)
    {
        Debug.Log($"Chat GPT API와 통신합니다.(1분 걸림)...{text}");

        msg?.Invoke(text);
    }
}
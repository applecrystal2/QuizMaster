using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatGPTClient : MonoBehaviour
{
    public delegate void QuizGenerateHandler(List<QuestionSO> questions);
    public event QuizGenerateHandler quizGenerateHandler;

    public void GenerateQuestions(int questionCount, string topicToUse)
    {
        Debug.Log($"Generating {questionCount} questions on the Topic: {topicToUse}...");
        
        StartCoroutine(GenerateWithDelay());
    }

    private IEnumerator GenerateWithDelay()
    {
        yield return new WaitForSeconds(3f);
        List<QuestionSO> questions = new List <QuestionSO>();
        QuestionSO so1 = CreateQuestion("GPT 생성 질문 1",
            new string[] { "1번답(정답)", "2번답", "3번답", "4번답" }, 
            0);
        questions.Add(so1);
        QuestionSO so2 = CreateQuestion("GPT 생성 질문 1",
            new string[] { "1번답", "2번답(정답)", "3번답", "4번답" },
            1);
        questions.Add(so2);
        QuestionSO so3 = CreateQuestion("GPT 생성 질문 1",
            new string[] { "1번답", "2번답", "3번답(정답)", "4번답" },
            2);
        questions.Add(so3);

        quizGenerateHandler?.Invoke(questions);
        Debug.Log("Finished GenerateWithDelay.....");

    }

    QuestionSO CreateQuestion(string q, string[] a, int correctIndex)
    {
        QuestionSO so = ScriptableObject.CreateInstance<QuestionSO>();
        so.SetData(q, a, correctIndex);

        return so;

    }


}

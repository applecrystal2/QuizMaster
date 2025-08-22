using UnityEngine;

public class Array : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int[] numbers;

        numbers = new int[5];

        string[] strs = new string[3];

        int[] scores = { 90, 85, 70, 100, 60 };
        bool[] flags = { true, false, true };

        numbers[0] = 10;
        numbers[1] = 20;
        numbers[2] = 30;
        numbers[3] = 40;
        numbers[4] = 50;

        int num = numbers[3];
    }
    void Loop()
    {
        int[] score = { 90, 85, 70, 100, 60 };

        for (int i = 0; i < score.Length; i++)
        {
            Debug.Log("Score: " + score[i]);
        }

        foreach (int s in score)
        {
            Debug.Log("Score: " + s);
        }

        int index = 0;
        while (index < score.Length)
        {
            Debug.Log("Score: " + score[index]);
            index++;
        }
    }

}

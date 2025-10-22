using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float problmTime = 20f;
    [SerializeField] float solutionTime = 3f;
    float time = 0f;

    [HideInInspector] public bool isProblemTime = true;
    [HideInInspector] public float fillAmount;
    [HideInInspector] public bool loadNextQuestion;

    
    public event Action OnTenSecondsLeft; // 10초 남았을 때 발생하는 이벤트

    private void Start()
    {
        time = problmTime;
        loadNextQuestion = true;
    }

    private void Update()
    {
        TimerCountDown();
        UpdateFillAmount();
    }

    private void UpdateFillAmount()
    {
        if (isProblemTime)
            fillAmount = time / problmTime;
        else
            fillAmount = time / solutionTime;
    }

    private void TimerCountDown()
    {
        time -= Time.deltaTime;

        // 10초 남았을 때 이벤트 발생
        if (isProblemTime && time <= 10f && time > 9.9f)
        {
            OnTenSecondsLeft?.Invoke();
        }

        if (time <= 0f)
        {
            if (isProblemTime)
            {
                isProblemTime = false;
                time = solutionTime;
            }
            else
            {
                isProblemTime = true;
                time = problmTime;
                loadNextQuestion = true;
            }
        }
    }

    public void CancelTimer()
    {
        time = 0f;
    }

    internal void ResetTimer()
    {
        //throw new NotImplementedException();
    }
}


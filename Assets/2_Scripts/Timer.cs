using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Image 클래스를 사용하기 위한 네임스페이스 추가

public class Timer : MonoBehaviour
{
    [SerializeField] float problmTime = 20f;
    [SerializeField] float solutionTime = 3f;
    float time = 0f;

    [HideInInspector] public bool isProblemTime = true;
    [HideInInspector] public float fillAmount;
    [HideInInspector] public bool loadNextQuestion;

    [Header("Timer Text")]
    [SerializeField] TextMeshProUGUI timerText; // 남은 시간을 표시할 텍스트 UI
    [SerializeField] Image timerImage; // 타이머 스프라이트 이미지

    public event Action OnTenSecondsLeft; // 10초 남았을 때 발생하는 이벤트

    private void Start()
    {
        time = problmTime;
        loadNextQuestion = true;
        UpdateTimerUI(); // 초기 UI 업데이트
    }

    private void Update()
    {
        TimerCountDown();
        UpdateTimerUI(); // 매 프레임 UI 업데이트
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

    private void UpdateTimerUI()
    {
        if (isProblemTime)
        {
            // 남은 시간 텍스트 업데이트
            if (timerText != null)
            {
                timerText.text = Mathf.CeilToInt(time).ToString(); // 남은 시간을 정수로 표시
            }

            // 타이머 스프라이트 fillAmount 업데이트
            if (timerImage != null)
            {
                timerImage.fillAmount = time / problmTime;
            }
        }
        else
        {
            // 문제 풀이 시간이 아닐 때 텍스트와 스프라이트를 비웁니다.
            if (timerText != null)
            {
                timerText.text = "";
            }

            if (timerImage != null)
            {
                timerImage.fillAmount = 0f;
            }
        }
    }

    public void CancelTimer()
    {
        time = 0f;
        UpdateTimerUI(); // 타이머 취소 시 UI 업데이트
    }

    // ResetTimer 메서드 추가
    public void ResetTimer()
    {
        isProblemTime = true;
        time = problmTime;
        loadNextQuestion = false;
        UpdateTimerUI(); // 타이머 초기화 시 UI 업데이트
    }
}


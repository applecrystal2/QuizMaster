using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float problmTime = 10f;
    [SerializeField] float solutionTime = 3f;
    float time = 0f;


    [HideInInspector] public bool isProblemTime = true;
    [HideInInspector] public float fillAmount;

    private void Start()
    {
        time = problmTime;
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
            }
        }
    }
}


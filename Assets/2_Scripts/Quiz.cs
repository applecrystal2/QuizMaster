using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("질문")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("보기")]
    [SerializeField] GameObject[] answerButtons;

    [Header("버튼 색깔")]
    [SerializeField] Sprite defaltAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("별 색깔")]
    [SerializeField] Sprite defaltSprite;
    [SerializeField] Sprite changeSprite;

    [Header ("Timer")]
    [SerializeField] Image timerImage;
    [SerializeField] Sprite problemTimerSprite;
    [SerializeField] Sprite solutionTimerSprite;
    [SerializeField] Timer timer;

    [Header ("Sooring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header ("ProgressBar")]
    [SerializeField] Slider progressBar;

    [Header("ChatGPTClient")]
    [SerializeField] ChatGPTClient chatGPTClient;
    [SerializeField] int questionCount = 3;
    [SerializeField] TextMeshProUGUI loadingText;

    [Header("Hint")]
    [SerializeField] TextMeshProUGUI hintText;

    bool isGeneratingQuestions = false;
    bool chooseAnswer = false;

    private int answerCount = 0; // 답변 횟수를 추적하는 변수 추가
    private bool isQuizComplete = false; // 퀴즈 완료 상태를 추적하는 변수 추가
    private Coroutine hideHintCoroutine; // 힌트를 숨기는 코루틴을 관리

    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        chatGPTClient.quizGenerateHandler += QuizGenerateHandler;

        
        timer.OnTenSecondsLeft += HandleTenSecondsLeft; // 10초 남았을 때 이벤트 구독

        if (questions.Count == 0)
        {
            GenerateQuestionsIfNeeded();
        }
        else
        {
            InitializeProgressBar();
        }

        GetNextQuestion();
    }

    

    private void HandleTenSecondsLeft()
    {
        if (currentQuestion != null)
        {
            hintText.text = "힌트를 가져오는 중...";
            string question = currentQuestion.GetQuestion();
            //chatGPTClient.RequestHint(question, DisplayHint);
        }
    }

    private void DisplayHint(string hint)
    {
        hintText.text = hint; // 힌트를 표시

        // 기존 코루틴이 실행 중이면 중지
        if (hideHintCoroutine != null)
        {
            StopCoroutine(hideHintCoroutine);
        }

        // 10초 뒤에 힌트를 비활성화하는 코루틴 실행
        hideHintCoroutine = StartCoroutine(HideHintAfterDelay(10f));
    }

    private IEnumerator HideHintAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hintText.text = ""; // 힌트를 비활성화
    }

    private void GenerateQuestionsIfNeeded()
    {
        if (isGeneratingQuestions) return;

        isGeneratingQuestions = true;
        GameManager.Instance.ShowLoadingScreen();

        string topicToUse = GetTrendingTopic();
        chatGPTClient.GenerateQuizQuestions(questionCount, topicToUse);
        Debug.Log($"GernrateQuestionsIfNeeded: {topicToUse}");
    }

    private string GetTrendingTopic()
    {
        string[] topics = new string[]
        {
            "과학", "역사", "음악", "영화", "스포츠", "기술", "문학", "예술", "지리", "정치"
        };
        int randomIndex = UnityEngine.Random.Range(0, topics.Length);
        return topics[randomIndex];
    }

    void QuizGenerateHandler(List<QuestionSO> generatedQuestions)
    {
        isGeneratingQuestions = false;

        if (generatedQuestions == null || generatedQuestions.Count == 0)
        {
            Debug.LogError("문제 생성에 실패했습니다. ChatGPT에서 질문을 생성하지 못했습니다.");
            loadingText.text = "문제 생성에 실패했습니다.\n인터넷 연결 확인 후 다시 시도하세요.";
            return;
        }

        Debug.Log($"생성된 질문 수: {generatedQuestions.Count}");
        questions.AddRange(generatedQuestions);
        progressBar.maxValue = questions.Count;

        GetNextQuestion();
    }


    private void InitializeProgressBar()
    {
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    private void Update()
    {
        //타이머 이미지 업데이트
        timerImage.fillAmount = timer.fillAmount;
        if (timer.isProblemTime)   
            timerImage.sprite = problemTimerSprite;
        else        
            timerImage.sprite = solutionTimerSprite;       
        timerImage.fillAmount = timer.fillAmount;


        //다음 질문 불러오기
        if (timer.loadNextQuestion)
        {
            if (questions.Count <= 0)
            {
                GenerateQuestionsIfNeeded();
                GameManager.Instance.ShowEndScreen();
            }
            else
            {
                //timer.loadNextQuestion = false;
                GetNextQuestion();
            }

        }

        //SolutionTime에서 정답을 선택하지 않았을 때
        if (timer.isProblemTime == false && chooseAnswer == false)
        {
            DisplaySolution(-1);
        }
    }

    private void GetNextQuestion()
    {
        if (questions.Count <= 0)
        {
            Debug.LogError("질문 리스트가 비어 있습니다. ChatGPT에서 질문을 생성하지 못했거나 추가되지 않았습니다.");
            GameManager.Instance.ShowEndScreen();
            return;
        }

        timer.ResetTimer(); // 타이머 초기화
        GameManager.Instance.ShowQuizScene();
        chooseAnswer = false;
        SetButtonState(true);
        SetDefaultButtonSprites();
        GetRandomQuestion();
        OnDisplayQuestion();
        scoreKeeper.IncrementQuestionSeen();
        progressBar.value++;
    }

    private void GetRandomQuestion()
    {
        int randomIndex = UnityEngine.Random.Range(0, questions.Count);
        currentQuestion = questions[randomIndex];

        questions.RemoveAt(randomIndex); //중복 질문 방지

    }

    private void OnDisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.GetAnswer(i);
        }

    }
    public void OnAnswerButtonClick(int index)
    {
        if (isQuizComplete) return; // 이미 완료된 경우 실행하지 않음

        chooseAnswer = true;
        DisplaySolution(index);
        timer.CancelTimer();
        scoreText.text = $"Score: {scoreKeeper.CalculateScore()}%";

        answerCount++; // 답변 횟수 증가

        // 답변이 3번 완료되었을 때 처리
        if (answerCount >= 3)
        {
            isQuizComplete = true; // 퀴즈 완료 상태로 설정
            GameManager.Instance.ShowEndScreen(); // EndScreen 실행
        }
    }

    private void DisplaySolution(int index)
    {
        Debug.Log("currentQuestion : " + currentQuestion);
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "정답입니다!";
            answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            questionText.text = "틀렸습니다. 정답은.." + currentQuestion.GetCorrectAnswerIndex();
        }

        SetButtonState(false);
    }

    private void SetDefaultButtonSprites()
    {
        //모든 answerButtons의 sprite를 loop 돌면서 defaltAnswerSprite로 변경
        foreach (GameObject buttonObj in answerButtons)
        {
            buttonObj.GetComponent<Image>().sprite = defaltAnswerSprite;
        }
    }
    private void SetButtonState(bool state)
    {
        foreach (GameObject obj in answerButtons)
        {
            obj.GetComponent<Button>().interactable = state;
        }
    }


}
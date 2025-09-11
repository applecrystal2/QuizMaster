using System;
using TMPro;
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

    [Header ("Timer")]
    [SerializeField] Image timerImage;
    [SerializeField] Sprite problemTimerSprite;
    [SerializeField] Sprite solutionTimerSprite;
    Timer timer;

    [Header ("Sooring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header ("ProgressBar")]
    [SerializeField] Slider progressBar;

    [Header("ChatGPT")]
    [SerializeField] ChatGPTClient chatGPTClient;
    [SerializeField] int questionCount = 3;

    bool isGeneratingQuestions = false;
    bool chooseAnswer = false;

    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        chatGPTClient.quizGenerateHandler += QuizGenerateHandler;

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


    private void GenerateQuestionsIfNeeded()
    {
        if (isGeneratingQuestions) return;

        isGeneratingQuestions = true;
        GameManager.Instance.ShowLoadingScreen();

        string topicToUse = GetTrendingTopic();
        chatGPTClient.GenerateQuestions(questionCount, topicToUse);
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
        Debug.Log($"QuizGenerateHandler: {questions.Count} questions received.");
        isGeneratingQuestions = false;
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
                //GameManager.Instance.ShowEndScreen();
            }
            else
            {
                timer.loadNextQuestion = false;
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
        if(questions.Count <= 0)
        {
            Debug.Log("더 이상 질문이 없습니다.");
            return;
        }

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
        chooseAnswer = true;
        DisplaySolution(index);
        timer.CancelTimer();
        scoreText.text = $"Score: {scoreKeeper.CalculateScore()}%";


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
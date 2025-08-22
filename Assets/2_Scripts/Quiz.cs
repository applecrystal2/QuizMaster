using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO question;
    //[SerializeField] TextMeshProUGUI[] answerTextArr;
    [SerializeField] GameObject[] answerButtons;
    [SerializeField] Sprite defaltAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    void Start()
    {
        questionText.text = question.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = question.GetAnswer(i);
            //answerTextArr[i].text = question.GetAnswer(i);
        }
    }

    public void OnAnswerButtonClick(int index)
    {
        if (index == question.GetCorrectAnswerIndex())
        {
            questionText.text = "정답입니다!";
            answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
        }
    }   
}
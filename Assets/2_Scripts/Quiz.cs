using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO question;
    [SerializeField] GameObject[] answerButtons;
    [SerializeField] Sprite defaltAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    void Start()
    { 
        GetNextQuestion();
    }

    private void GetNextQuestion()
    {
        SetButtonState(true);
        SetDefaultButtonSprites();
        OnDisplayQuestion();
    }

    private void OnDisplayQuestion()
    {
        questionText.text = question.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = question.GetAnswer(i);
        }

    }
    public void OnAnswerButtonClick(int index)
    {
        if (index == question.GetCorrectAnswerIndex())
        {
            questionText.text = "정답입니다!";
            answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
        }
        else 
        {
            questionText.text = "오답입니다!" + question.GetCorrectAnswerIndex();
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
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2, 6  )]
    [SerializeField] string question = "여기에 질문을 입력하세요.";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerIndex;
    //[SerializeField] string hint = "힌트를 입력하세요.";

    public string GetQuestion()
    {
        return question; 
    }

    public string GetAnswer (int i)
    {
        return answers[i];
    }
    public string GetAnswers(int index)
    {
        return answers[correctAnswerIndex];
    }

    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }

    public void SetData(string q, string[] a, int correctIndex, string hint)
    {
        SetData(q, a, correctIndex);
        //hint = hint;
    } 
    public void SetData(string q, string[] a, int correctIndex) 
    {
        question = q;
        answers = a;
        correctAnswerIndex = correctIndex;
    }
}

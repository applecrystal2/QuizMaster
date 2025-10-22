using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] ScoreKeeper scoreKeeper;

    [Header("Star Images")]
    [SerializeField] Image[] stars; // 별 이미지를 관리하는 배열
    [SerializeField] Sprite emptyStarSprite; // 빈 별 스프라이트
    [SerializeField] Sprite filledStarSprite; // 채워진 별 스프라이트

    public void ShowFinalScore()
    {
        // 점수 표시
        int score = scoreKeeper.CalculateScore();
        finalScoreText.text = "축하합니다!\r\n" +
            $"당신의 점수는 {score}점 입니다.";

        // 별 이미지 업데이트
        UpdateStarImages(score);
    }

    private void UpdateStarImages(int score)
    {
        int starCount = CalculateStarCount(score);

        for (int i = 0; i < stars.Length; i++)
        {
            if (i < starCount)
            {
                stars[i].sprite = filledStarSprite; // 채워진 별
            }
            else
            {
                stars[i].sprite = emptyStarSprite; // 빈 별
            }
        }
    }

    private int CalculateStarCount(int score)
    {
        // 점수에 따라 별 개수를 계산
        if (score >= 50) return 2; // 50점 이상: 별 2개
        if (score >= 33) return 1; // 33점 이상: 별 1개
        return 0; // 0점: 별 0개
    }
}

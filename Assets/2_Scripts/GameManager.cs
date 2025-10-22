using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Quiz quiz;
    [SerializeField] private EndScreen endScreen;
    [SerializeField] private GameObject loadingCanvas;
    SceneManager sceneManager;

    // 오디오 관련 필드 추가
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip mainSceneMusic;
    [SerializeField] private AudioClip buttonClickSound; // 일반 버튼 클릭 효과음
    [SerializeField] private AudioClip correctAnswerSound; // 정답 클릭 효과음

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 오브젝트 유지
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // 중복 생성 방지
        }
    }

    void Start()
    {
        // MainScene에서 음악 재생
        if (SceneManager.GetActiveScene().name == "MainScene" && audioSource != null && mainSceneMusic != null)
        {
            audioSource.clip = mainSceneMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void OnStartButtonClick()
    {
        ButtonClickSound(); // 일반 버튼 클릭 효과음 재생
        SceneManager.LoadScene("MainScene");
    }

    public void ShowQuizScene()
    {
        ButtonClickSound(); // 일반 버튼 클릭 효과음 재생
        quiz.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(false);
    }

    public void ShowEndScreen()
    {
        quiz.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(true);
        endScreen.ShowFinalScore();
        loadingCanvas.SetActive(false);
    }

    public void ShowLoadingScreen()
    {
        ButtonClickSound(); // 일반 버튼 클릭 효과음 재생
        quiz.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(true);
    }

    public void OnReplayLevel()
    {
        ButtonClickSound(); // 일반 버튼 클릭 효과음 재생
        Debug.Log("Restarting the game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);     
    }

    public void OnExitButtonClick()
    {
        ButtonClickSound(); // 일반 버튼 클릭 효과음 재생
        Debug.Log("게임 종료");
        Application.Quit(); // 게임 종료
        UnityEditor.EditorApplication.isPlaying = false;
    }

    // 일반 버튼 클릭 효과음 재생 메서드
    private void ButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    // 정답 클릭 효과음 재생 메서드
    public void PlayCorrectAnswerSound()
    {
        if (audioSource != null && correctAnswerSound != null)
        {
            audioSource.PlayOneShot(correctAnswerSound);
        }
    }
}

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

    // 1. 오디오 관련 필드 추가
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip mainSceneMusic;

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
        // 2. MainScene에서 음악 재생
        if (SceneManager.GetActiveScene().name == "MainScene" && audioSource != null && mainSceneMusic != null)
        {
            audioSource.clip = mainSceneMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
        //ShowQuizScene();
    }

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void ShowQuizScene()
    {
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
        quiz.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(true);
    }
    public void OnReplayLevel()
    {
        Debug.Log("Restarting the game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);     
    }
   
}

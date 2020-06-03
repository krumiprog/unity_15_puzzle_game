using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private GameObject _menuScreen;
    [SerializeField] private GameObject _solvedScreen;

    private static ScreenManager _instance;
    
    public static ScreenManager Instance
    {
        get
        {
            if (_instance == null) Debug.LogError("is Null");

            return _instance;
        }
    }

    public void OnStartButtonClicked()
    {
        GameManager.Instance.StartGame();

        _menuScreen.SetActive(false);
        _gameScreen.SetActive(true);
    }

    public void OnRestartButtonClicked()
    {
        GameManager.Instance.RestartGame();
    }

    public void OnMenuButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    public void ShowGameScreen()
    {
        if (_solvedScreen.activeSelf)
        {
            _solvedScreen.SetActive(false);
            _gameScreen.SetActive(true);
        }
    }

    public void ShowSolvedScreen()
    {
        _gameScreen.SetActive(false);
        _solvedScreen.SetActive(true);
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _menuScreen.SetActive(true);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public PlacementSystem placementSystem;
    public bool isOpen;
    public GameObject pauseMenuCanvas;


    Controls _controls;
    GameplayControls _gameUIControls;


    void Start()
    {
        _controls = GetComponent<Controls>();
        _gameUIControls = GetComponent<GameplayControls>();
    }
    void Update()
    {
        if(_controls.isEnabled == true) return;
        if(placementSystem.isActive) return;

        if(Input.GetKeyDown(KeyCode.Escape) && !isOpen)
        {
            isOpen = true;
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            Time.timeScale = _gameUIControls.currTimeScale;
            isOpen = false;
            pauseMenuCanvas.SetActive(false);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        isOpen = false;
        pauseMenuCanvas.SetActive(false);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        Application.Quit();
    }
}

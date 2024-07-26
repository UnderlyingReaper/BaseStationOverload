using UnityEngine;

public class Controls : MonoBehaviour
{
    public GameObject controls;
    public GameObject[] pages;
    public GameObject nextButton, backButton;
    public int currentPage = 1;

    [HideInInspector] public bool isEnabled = true;


    void Start()
    {
        controls.SetActive(true);
        Time.timeScale = 0;
    }
    public void StartGame()
    {
        controls.SetActive(false);
        Time.timeScale = 1;
        isEnabled = false;
    }

    public void Next()
    {
        currentPage++;

        pages[currentPage - 2].SetActive(false);
        pages[currentPage - 1].SetActive(true);
        backButton.SetActive(true);

        if(currentPage == pages.Length) nextButton.SetActive(false);
    }
    public void Back()
    {
        currentPage--;

        pages[currentPage].SetActive(false);
        pages[currentPage - 1].SetActive(true);
        nextButton.SetActive(true);

        if(currentPage == 1) backButton.SetActive(false);
    }
}

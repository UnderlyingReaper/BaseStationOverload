using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public EndGameResult result;

    public GameObject canvas;
    public TextMeshProUGUI heading;
    public TextMeshProUGUI reasonOfLoss;

    public enum EndGameResult {
        Lost,
        Won
    }


    public void ShowEndGame(string reasonForLosing = "")
    {
        if(result == EndGameResult.Lost)
        {
            heading.color = Color.red;
            heading.text = "You Lost!";
            reasonOfLoss.text = reasonForLosing;
            canvas.SetActive(true);
            Time.timeScale = 0;
        }
        else if(result == EndGameResult.Won)
        {
            heading.color = Color.green;
            heading.text = "You Won!";
            reasonOfLoss.text = "All Houses Are Now Connected!";
            canvas.SetActive(true);
            Time.timeScale = 0;
        }
    }
}

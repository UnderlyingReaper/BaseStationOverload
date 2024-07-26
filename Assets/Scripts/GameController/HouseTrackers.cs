using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HouseTrackers : MonoBehaviour
{
    public List<Consumers> buildings = new();
    public int totalPeople = 0;
    public int connectedPeople;
    public TextMeshProUGUI peopleConnectedDisplay;
    public GameObject gameController;

    EndGame _endGame;



    void Start() => _endGame = GetComponent<EndGame>();


    public void CheckForHouses()
    {
        foreach(Transform obj in gameController.transform)
        {
            if(obj.GetComponentInChildren<Consumers>() != null)
            {
                Consumers consumers = obj.GetComponentInChildren<Consumers>();

                buildings.Add(consumers);
                totalPeople += consumers.peopleUsing;
            }
        }

        peopleConnectedDisplay.text = connectedPeople.ToString();
    }

    public void AddPeople(int people)
    {
        connectedPeople += people;
        peopleConnectedDisplay.text = connectedPeople.ToString();

        if(connectedPeople >= totalPeople)
        {
            _endGame.result = EndGame.EndGameResult.Won;
            _endGame.ShowEndGame();
        }
    }

    public void RemovePeople(int people)
    {
        connectedPeople -= people;
        if(connectedPeople < 0) connectedPeople = 0;

        peopleConnectedDisplay.text = connectedPeople.ToString();
    }
}

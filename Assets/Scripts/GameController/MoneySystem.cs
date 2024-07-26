using TMPro;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    public ObjectPlacer objectPlacer;
    public TextMeshProUGUI totalIncomeText, totalCostText;
    public int cash;
    public TextMeshProUGUI cashTextUI;
    public int startingAmt;


    EndGame _endGame;


    void Start()
    {
        _endGame = GetComponent<EndGame>();

        cash = startingAmt;
        cashTextUI.text = $"Cash: ${cash}";
    }

    public void AddCash(int amt)
    {
        cash += amt;
        cashTextUI.text = $"Cash: ${cash}";

        int totalIncome = 0;
        foreach(GameObject obj in objectPlacer._placedGameObjects)
        {
            if(obj.TryGetComponent(out Relay relay) && relay.consumersConnected.Count != 0)
            {
                foreach(Consumers consumer in relay.consumersConnected)
                {
                    totalIncome += consumer.actualPayAmt;
                }
            }
        }

        totalIncomeText.text = "Total Income: $" + totalIncome.ToString();
    }
    public void RemoveCash(int amt)
    {
        cash -= amt;
        cashTextUI.text = $"Cash: ${cash}";

        if(cash <= 0)
        {
            _endGame.result = EndGame.EndGameResult.Lost;
            _endGame.ShowEndGame("You Have Gone Bankrupt!");
        }

        int totalCost = 0;
        foreach(GameObject obj in objectPlacer._placedGameObjects)
        {
            if(obj.TryGetComponent(out PowerPlant powerPlant))
            {
                totalCost += powerPlant.productionCost;
            }
            else if(obj.TryGetComponent(out Relay relay))
            {
                totalCost += relay.chargeAmt;
            }
            else if(obj.TryGetComponent(out PowerStorage powerStorage))
            {
                totalCost += powerStorage.price;
            }
        }

        totalCostText.text = "Total Cost: $" + totalCost.ToString();
    }
}

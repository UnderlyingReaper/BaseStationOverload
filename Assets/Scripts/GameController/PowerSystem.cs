using TMPro;
using UnityEngine;

public class PowerSystem : MonoBehaviour
{
    public PowerPlant baseStationPlant;
    public ObjectPlacer objectPlacer;
    public int warningAmt;
    public int totalPower;
    public int maxPower = 100;
    public TextMeshProUGUI powerText, totalPowerProducedText, totalPowerConsumedText;
    public int startingPower;

    EndGame _endGame;

    void Start()
    {
        _endGame = GetComponent<EndGame>();

        totalPower = startingPower;
        powerText.text = $"Power: {totalPower}W";
    }

    public void ProducePower(int amt)
    {
        totalPower += amt;
        if(totalPower > maxPower) totalPower = maxPower;

        powerText.text = $"Power: {totalPower}W";

        if(totalPower >= warningAmt) powerText.color = Color.white;


        int totalProductionAmt = 0;
        foreach(GameObject obj in objectPlacer._placedGameObjects)
        {
            if(obj.TryGetComponent(out PowerPlant powerPlant))
            {
                totalProductionAmt += powerPlant.productionAmt;
            }
        }
        totalProductionAmt += baseStationPlant.productionAmt;
        totalPowerProducedText.text = "Power Production: " + totalProductionAmt.ToString() + "W";
    }

    public void ConsumePower(int amt)
    {
        totalPower -= amt;
        powerText.text = $"Power: {totalPower}W";

        if(totalPower <= 0)
        {
            _endGame.result = EndGame.EndGameResult.Lost;
            _endGame.ShowEndGame("You Ran Out Of Power!");
        }

        if(totalPower <= warningAmt) powerText.color = Color.red;

        int totalConsumptionAmt = 0;
        foreach(GameObject obj in objectPlacer._placedGameObjects)
        {
            if(obj.TryGetComponent(out Relay relay))
            {
                totalConsumptionAmt += relay.powerConsumption;
            }
        }

        totalPowerConsumedText.text = "Power Consumption: " + totalConsumptionAmt.ToString() + "W";
    }

    public void IncreasePowerStorageCapacity(int amt)
    {
        maxPower += amt;
    }

    public void DecreasePowerStorageCapacity(int amt)
    {
        maxPower -= amt;

        if(totalPower > maxPower) totalPower = maxPower;
        powerText.text = $"Power: {totalPower}W";
    }
}

using System;
using System.Collections;
using UnityEngine;

public class PowerPlant : MonoBehaviour
{
    public bool isPlaced;
    public int destructionPrice;
    public int productionAmt;
    public int prodcutionTime;
    public int productionCost;


    PowerSystem _powerSystem;
    MoneySystem _moneySystem;


    void Start()
    {
        _powerSystem = GameObject.FindGameObjectWithTag("GameController").GetComponent<PowerSystem>();
        _moneySystem = _powerSystem.GetComponent<MoneySystem>();

        StartCoroutine(ProduceEnergy());
        if(productionCost > 0) StartCoroutine(ChargeMoney());
    }

    IEnumerator ChargeMoney()
    {
        while(!isPlaced)
        {
            yield return new WaitForSeconds(prodcutionTime);
        }
        while(true)
        {
            _moneySystem.RemoveCash(productionCost);
            yield return new WaitForSeconds(prodcutionTime);
        }
    }

    IEnumerator ProduceEnergy()
    {
        while(!isPlaced)
        {
            yield return new WaitForSeconds(prodcutionTime);
        }
        while(true)
        {
            _powerSystem.ProducePower(productionAmt);
            yield return new WaitForSeconds(prodcutionTime);
        }
    }
}

using System;
using System.Collections;
using UnityEngine;

public class PowerStorage : MonoBehaviour
{
    public bool isPlaced;
    public int destructionPrice;
    public int increaseAmt;
    public int price;
    public float priceTimeInterval;


    PowerSystem _powerSystem;
    MoneySystem _moneySystem;
    bool _appliedEffect = false;

    void Start()
    {
        _powerSystem = GameObject.FindGameObjectWithTag("GameController").GetComponent<PowerSystem>();
        _moneySystem = _powerSystem.GetComponent<MoneySystem>();

        StartCoroutine(ChargePrice());
    }

    void Update()
    {
        if(_appliedEffect) return;

        if(isPlaced)
        {
            _powerSystem.IncreasePowerStorageCapacity(increaseAmt);
            _appliedEffect = true;
        }
    }

    void OnDestroy()
    {
        if(_appliedEffect) _powerSystem.DecreasePowerStorageCapacity(increaseAmt);
    }

    IEnumerator ChargePrice()
    {
        while(!isPlaced)
        {
            yield return new WaitForSeconds(priceTimeInterval);
        }
        while(true)
        {
            _moneySystem.RemoveCash(price);
            yield return new WaitForSeconds(priceTimeInterval);
        }
    }
}

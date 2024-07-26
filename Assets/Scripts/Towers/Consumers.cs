using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Consumers : MonoBehaviour
{
    public int actualPayAmt;
    public Relay relayConnectedTo;
    public bool isConnected;
    public int peopleUsing;
    public int payAmt;
    public float paymentTimeInterval;
    public AudioClip connectClip;

    public Image signalDisplay;
    public GameObject crossDisplay;

    public Color strongColor, mediumColor, slowColor, noSignal;
    public Sprite threeBars, twoBars, oneBar;

    MoneySystem _moneySystem;
    [HideInInspector] public float _signalStrength = 0;
    int _payDivider;
    AudioSource _source;
    HouseTrackers _houseTrackers;



    void Start()
    {
        _moneySystem = GameObject.FindGameObjectWithTag("GameController").GetComponent<MoneySystem>();
        _houseTrackers = _moneySystem.GetComponent<HouseTrackers>();

        _source = Camera.main.transform.parent.GetComponentInChildren<AudioSource>();
    }
    void Update()
    {
        if(isConnected) ShowConnection();
        else
        {
            signalDisplay.color = noSignal;
            signalDisplay.sprite = threeBars;
            crossDisplay.SetActive(true);
        }

        if(isConnected && !_isPayFeesRunning) StartCoroutine(PayFees());
    }

    public void ShowConnection()
    {
        if(_signalStrength < 0.3)
        {
            signalDisplay.color = slowColor;
            signalDisplay.sprite = oneBar;
            crossDisplay.SetActive(false);
        }
        else if(_signalStrength < 0.6)
        {
            signalDisplay.color = mediumColor;
            signalDisplay.sprite = twoBars;
            crossDisplay.SetActive(false);
        }
        else if(_signalStrength < 1)
        {
            signalDisplay.color = strongColor;
            signalDisplay.sprite = threeBars;
            crossDisplay.SetActive(false);
        }
        
    }

    public void ConnectConsumer(float signalStrength, Relay relayToConnectTo)
    {
        if(isConnected)
        {
            relayConnectedTo.consumersConnected.Remove(this);
            relayConnectedTo = null;
            _signalStrength = signalStrength;
            relayConnectedTo = relayToConnectTo;
            return;
        }

        isConnected = true;
        relayConnectedTo = relayToConnectTo;
        _houseTrackers.AddPeople(peopleUsing);
        _signalStrength = signalStrength;
        _source.PlayOneShot(connectClip);
    }
    public void DisconnectConsumer()
    {
        isConnected = false;
        _houseTrackers.RemovePeople(peopleUsing);
        _signalStrength = 0;
    }

    bool _isPayFeesRunning = false;
    IEnumerator PayFees()
    {
        _isPayFeesRunning = true;
        while(isConnected)
        {
            if(_signalStrength < 0.3) _payDivider = 3;
            else if(_signalStrength < 0.6) _payDivider = 2;
            else _payDivider = 1;
            
            actualPayAmt = payAmt/_payDivider;
            _moneySystem.AddCash(actualPayAmt);
            yield return new WaitForSeconds(paymentTimeInterval);
        }
        _isPayFeesRunning = false;
    }
}

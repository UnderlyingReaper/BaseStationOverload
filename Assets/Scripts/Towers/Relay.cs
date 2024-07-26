using System.Collections;
using System.Collections.Generic;
using GogoGaga.OptimizedRopesAndCables;
using Unity.VisualScripting;
using UnityEngine;

public class Relay : MonoBehaviour
{
    public bool isPlaced;
    public GameObject particleEffect;
    public GameObject rangeVisual;
    public List<Consumers> consumersConnected = new();
    public bool isConnectedToRelay;
    public float radius;
    public AnimationCurve strengthOverDistance;
    public int destructionPrice;
    public int powerConsumption;
    public int chargeAmt;
    public float chargeTimeInterval;

    public Transform SignalEmitter;
    public LayerMask buildingMask;
    public LayerMask relayMask;

    MoneySystem _moneySytem;
    PowerSystem _powerSystem;
    Rope _rope;
    Collider[] _buildingColliders, _relayColliders;
    PlacementSystem _placementSystem;


    void Start()
    {
        _moneySytem = GameObject.FindGameObjectWithTag("GameController").GetComponent<MoneySystem>();
        _powerSystem = _moneySytem.GetComponent<PowerSystem>();
        _rope = GetComponentInChildren<Rope>();

        _placementSystem = GameObject.FindGameObjectWithTag("BuildingSystem").GetComponentInChildren<PlacementSystem>();
        _placementSystem.startPlacing += DisplayRange;
        _placementSystem.stopPlacing += StopRangeDisplay;

        StartCoroutine(ChargePrice());
        rangeVisual.SetActive(false);
    }
    void Update()
    {
        if(!isPlaced) return;
        
        if(isConnectedToRelay &&!particleEffect.activeSelf) particleEffect.SetActive(true);
        else if(!isConnectedToRelay && particleEffect.activeSelf) particleEffect.SetActive(false);

        if(isConnectedToRelay &&!_isConsumingPower && chargeAmt > 0) StartCoroutine(ConsumePower());



        _buildingColliders = Physics.OverlapSphere(SignalEmitter.position, radius, buildingMask);
        foreach(Collider collider in _buildingColliders)
        {
            float distance = Vector3.Distance(collider.transform.position, SignalEmitter.position);
            
            float signalStrength = strengthOverDistance.Evaluate(distance);
            
            if(isConnectedToRelay && collider.TryGetComponent(out Consumers consumers) && signalStrength > consumers._signalStrength)
            {
                consumers.ConnectConsumer(signalStrength, this);
                consumersConnected.Add(consumers);
                Debug.DrawLine(SignalEmitter.position, collider.transform.position, Color.red);
            }
        }


        
        if(!isConnectedToRelay)
        {
            _relayColliders = Physics.OverlapSphere(SignalEmitter.position, radius, relayMask);
            foreach(Collider collider in _relayColliders)
            {
                float distance = Vector3.Distance(collider.transform.position, SignalEmitter.position);
                if(distance == 0) return;

                Debug.DrawLine(SignalEmitter.position, collider.transform.position, Color.yellow);
                Relay relay = collider.GetComponentInParent<Relay>();

                if(relay.isConnectedToRelay)
                {
                    isConnectedToRelay = true;
                    _rope.endPoint = relay.SignalEmitter;
                    _rope.ropeLength = distance + 3;
                    break;
                }
            }
        }
        
    }
    void OnDestroy()
    {
        foreach(Consumers consumer in consumersConnected)
        {
            consumer.isConnected = false;
        }
    }

    void DisplayRange()
    {
        if(rangeVisual != null) rangeVisual.SetActive(true);
    }
    void StopRangeDisplay()
    {
        if(rangeVisual != null) rangeVisual.SetActive(false);
    }


    IEnumerator ChargePrice()
    {
        while(!isPlaced)
        {
            yield return new WaitForSeconds(chargeTimeInterval);
        }
        while(true)
        {
            _moneySytem.RemoveCash(chargeAmt);
            yield return new WaitForSeconds(chargeTimeInterval);
        }
    }

    bool _isConsumingPower;
    IEnumerator ConsumePower()
    {
        _isConsumingPower = true;

        while(isConnectedToRelay)
        {
            _powerSystem.ConsumePower(powerConsumption);
            yield return new WaitForSeconds(chargeTimeInterval);
        }

        _isConsumingPower = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(SignalEmitter.position, radius);
    }
}

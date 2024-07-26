using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public List<GameObject> _placedGameObjects = new();
    public MoneySystem moneySystem;

    public GameObject placeVfx, destroyVfx;

    public AudioClip placeClip;
    public AudioClip breakClip;
    public AudioSource sfxSource;

    public int PlaceObject(GameObject prefab, Vector3 pos)
    {
        GameObject gameObj = Instantiate(prefab);
        gameObj.transform.position = pos;

        GameObject particleFX = Instantiate(placeVfx);
        particleFX.transform.position = pos;

        sfxSource.PlayOneShot(placeClip);
        
        if(gameObj.TryGetComponent(out Relay relay)) relay.isPlaced = true;
        else if(gameObj.TryGetComponent(out PowerPlant powerPlant)) powerPlant.isPlaced = true;
        else if(gameObj.TryGetComponent(out PowerStorage powerStorage)) powerStorage.isPlaced = true;

        _placedGameObjects.Add(gameObj);
        Destroy(particleFX, 5);

        return _placedGameObjects.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if(_placedGameObjects.Count <= gameObjectIndex || _placedGameObjects[gameObjectIndex] == null) return;

        if(_placedGameObjects[gameObjectIndex].TryGetComponent(out Relay relay)) moneySystem.AddCash(relay.destructionPrice);
        else if(_placedGameObjects[gameObjectIndex].TryGetComponent(out PowerPlant powerPlant)) moneySystem.AddCash(powerPlant.destructionPrice);
        else if(_placedGameObjects[gameObjectIndex].TryGetComponent(out PowerStorage powerStorage)) moneySystem.AddCash(powerStorage.destructionPrice);

        GameObject particleFX = Instantiate(destroyVfx);
        particleFX.transform.position = _placedGameObjects[gameObjectIndex].transform.position;

        sfxSource.PlayOneShot(breakClip);

        Destroy(_placedGameObjects[gameObjectIndex]);
        _placedGameObjects.Remove(_placedGameObjects[gameObjectIndex]);
        Destroy(particleFX, 5);

        _placedGameObjects[gameObjectIndex] = null;
    }
}

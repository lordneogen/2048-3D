using System;
using System.Collections.Generic;
using UnityEngine;

public class Store:MonoBehaviour
{
    public List<BoosterStore> BoosterStores;

    private void Awake()
    {
        foreach (var boosterStore in BoosterStores)
        {
            // throw new NotImplementedException();
            boosterStore.Load();
        }
    }
}
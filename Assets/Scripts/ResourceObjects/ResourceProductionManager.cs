using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResourceProductionManager : Singleton<ResourceProductionManager>
{
    private List<ResourceObject> _resources = new List<ResourceObject>();

    public override void Awake()
    {
        base.Awake();

        foreach (ResourceObject r in Resources.LoadAll("", typeof(ResourceObject)))
        {
            _resources.Add(r);
        }
    }

    public void FreshStartProtocol()
    {
        foreach (var res in _resources)
        {
            res.FreshStartProtocol();
        }
    }

    void Update()
    {
        foreach (var resource in _resources)
        {
            if (resource.unlocked == false)
                continue;

            resource.IncreaseProductionProcess(Time.deltaTime);
        }
    }

    public List<ResourceObject> GetUnlockedResources(bool unlocked)
    {
        var tempList = new List<ResourceObject>();

        foreach (var res in _resources)
        {
            if (res.unlocked == unlocked)
            {
                tempList.Add(res);
            }
        }

        return tempList;
    }

    public ResourceObject TryGetResource(string name)
    {
        name = name.ToLower();

        foreach (var resource in _resources)
        {
            if (name == resource.resourceName.ToLower())
                return resource;
        }

        return null;
    }
    
    public float SellResource(ResourceObject resource, int sellAmount)
    {
        var profit = resource.Sell(sellAmount);
        GameController.Instance.gold += profit;

        return profit;
    }
}
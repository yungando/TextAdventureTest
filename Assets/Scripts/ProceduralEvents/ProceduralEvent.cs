using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Resources/Procedural Event", order = 3)]
public class ProceduralEvent : ScriptableObject
{
    [HideInInspector] public string eventName;
    [TextArea(1, 1000)] public string eventDescription;
    public float eventWeight;
    public float minTimeToNextEvent;
    public float maxTimeToNextEvent;

    public int duration;
    public bool isActive = false;

    public List<ResourceModifier> resourceModifiers = new List<ResourceModifier>();
    public List<Requirement> eventRequirements = new List<Requirement>();

    private Coroutine runningCoroutine;

    void OnValidate()
    {
        eventName = name;
    }

    public void Init()
    {
        isActive = false;
    }

    public void TriggerEvent()
    {
        var modifiersTriggered = 0;

        foreach (var e in resourceModifiers)
        {
            var resource = ResourceProductionManager.Instance.TryGetResource(e.targetResource);

            if (resource == null)
            {
                Debug.LogError($"No Resource exists with name: {e.targetResource}");
                continue;
            }

            if (resource.unlocked == false)
            {
                continue;
            }
            
            //Debug.Log($"Adding {resource.name} effect {e.targetResourceAttribute}");

            switch (e.targetResourceAttribute)
            {
                case(ResourceModifier.TargetResourceAttribute.timeToProduce):
                    resource.AddTimeToProduceModiferValue(e.modifierValue);
                    break;
                case(ResourceModifier.TargetResourceAttribute.goldValue):
                    resource.AddGoldModifierValue(e.modifierValue);
                    break;
                case(ResourceModifier.TargetResourceAttribute.supplyCapacity):
                    resource.AddSupplyCapacityModifierValue(e.modifierValue);
                    break;
                case(ResourceModifier.TargetResourceAttribute.supplyPerProduction):
                    resource.AddProductionPerCycleModifierValue(e.modifierValue);
                    break;
                case(ResourceModifier.TargetResourceAttribute.addToResourceSupply):
                    resource.AddToResourceSupply(e.modifierValue);
                    break;
                case(ResourceModifier.TargetResourceAttribute.addToResourceProgress):
                    resource.AddToResourceProgress(e.modifierValue);
                    break;
            }

            modifiersTriggered++;
        }

        if (modifiersTriggered == 0)
            return;

        GameController.Instance.LogStringWithReturn($"Event Triggered:\t<color=#54b948>{eventName}</color>\n{eventDescription}");
        GameController.Instance.UpdateMainText();

        isActive = true;

        if (duration > 0)
            RemoveEventAfterSeconds();
    }

    public bool EventRequirementsMet()
    {
        foreach (var req in eventRequirements)
        {
            if(req.IsMet() == false)
            {
                return false;
            }
        }

        return true;
    } 

    public async void RemoveEventAfterSeconds()
    {
        //TODO: Remove async and transfer over to a lifetime variable that is decreased and checked then removed at <= 0
        await System.Threading.Tasks.Task.Delay(duration * 1000);

        if (Application.IsPlaying(this) == false)
            return;

        foreach (var e in resourceModifiers)
        {
            var resource = ResourceProductionManager.Instance.TryGetResource(e.targetResource);
            
            //Debug.Log($"Removing {resource.name.ToString()} effect {e.targetResourceAttribute.ToString()}");

            switch (e.targetResourceAttribute)
            {
                case(ResourceModifier.TargetResourceAttribute.timeToProduce):
                    resource.RemoveTimeToProduceModiferValue(e.modifierValue);
                    break;
                case(ResourceModifier.TargetResourceAttribute.goldValue):
                    resource.RemoveGoldModifierValue(e.modifierValue);
                    break;
                case(ResourceModifier.TargetResourceAttribute.supplyCapacity):
                    resource.RemoveSupplyCapacityModifierValue(e.modifierValue);
                    break;
                case(ResourceModifier.TargetResourceAttribute.supplyPerProduction):
                    resource.RemoveProductionPerCycleModifierValue(e.modifierValue);
                    break;
            }
        }

        isActive = false;

        GameController.Instance.LogStringWithReturn($"Event Ended:\t<color=#54b948>{eventName}</color>");
        GameController.Instance.UpdateMainText();
    }
}
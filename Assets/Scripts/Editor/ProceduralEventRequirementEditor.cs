#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Requirement))]
public class ProceduralEventRequirementEditor : Editor
{
    Requirement req;
    void OnEnable()
    {
        req = (Requirement)target;
    }

    public override void OnInspectorGUI()
    {
        req.requirementType = (Requirement.RequirementType)EditorGUILayout.EnumPopup("Requirement Type", req.requirementType);
        switch (req.requirementType)
        {
            case Requirement.RequirementType.resourceUnlocked:
                {
                    req.resourceName = EditorGUILayout.TextField("Resource Name", req.resourceName);
                    EditorUtility.SetDirty(req);
                    break;
                }
            case Requirement.RequirementType.resourceSupplyThreshold:
                {
                    req.resourceName = EditorGUILayout.TextField("Resource Name", req.resourceName);
                    req.resourceSupplyThreshold = EditorGUILayout.IntField("Resource Supply Threshold", req.resourceSupplyThreshold);
                    EditorUtility.SetDirty(req);
                    break;
                }
            case Requirement.RequirementType.resouceUpgradeApplied:
                {
                    req.upgradeName = EditorGUILayout.TextField("Resource Upgrade Name", req.upgradeName);
                    EditorUtility.SetDirty(req);
                    break;
                }
            case Requirement.RequirementType.playerGoldThreshold:
                {
                    req.playerGoldThreshold = EditorGUILayout.IntField("Player Gold Threshold", req.playerGoldThreshold);
                    EditorUtility.SetDirty(req);
                    break;
                }
        }
    }
}
#endif
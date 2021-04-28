using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceObjectAttribute
{
    [SerializeField] private float _baseValue;
    private float _currentValue;

    private List<ModifierValue> _valueModifiers = new List<ModifierValue>();

    public float CurrentValue => _currentValue;

    public void Init()
    {
        _currentValue = _baseValue;
        _valueModifiers.Clear();
    }

    public void AddModifier(ModifierValue _modifier)
    {
        _valueModifiers.Add(_modifier);

        if(_modifier.percent == true)
        {
            _currentValue *= ((_modifier.valueToChange / 100.0f) + 1);
        }
        else
        {
            _currentValue += _modifier.valueToChange;
        }
    }

    public void RemoveModifier(ModifierValue _modifier)
    {
        _valueModifiers.Remove(_modifier);

        _currentValue = _baseValue;

        foreach (var mod in _valueModifiers)
        {
            if(mod.percent == true)
            {
                _currentValue *= ((_modifier.valueToChange / 100.0f) + 1);
            }
            else
            {
                _currentValue += mod.valueToChange;
            }
        }
    }
}
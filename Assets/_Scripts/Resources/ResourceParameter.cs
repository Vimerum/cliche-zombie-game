using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ResourceParameter {

    NumberFormatInfo nfi = new NumberFormatInfo {NumberGroupSeparator = ".", NumberDecimalDigits = 0};

    public Resource resource;

    [Header("Settings")]
    public Texture icon;
    [SerializeField]
    private int initialValue;
    [Header("References")]
    public RawImage iconGUI;
    public TextMeshProUGUI valueLabel;

    public int Value {
        get { return _currValue; }
        set {
            _currValue = value;
            valueLabel.text = _currValue.ToString("n", nfi);
        }
    }
    private int _currValue;

    public void Initialize () {
        Value = initialValue;
        iconGUI.texture = icon;
    }

    public void Deposit (int value) {
        Value += value;
    }

    public bool CheckWithdraw (int value) {
        if (Value >= value) {
            return true;
        }

        return false;
    }

    public void Withdraw (int value) {
        if (Value >= value) {
            Value -= value;
        }
    }
}

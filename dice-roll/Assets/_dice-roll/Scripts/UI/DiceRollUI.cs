using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DiceRollUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resultValue;
    [SerializeField] private TextMeshProUGUI _totalValue;
    [SerializeField] private Button _rollButton;

    private int _specialValue;

    private void Start()
    {
        Assert.IsNotNull(_resultValue, "Result value TMP is missing");
        Assert.IsNotNull(_totalValue, "Total value TMP is missing");
        
    }

    public void Initialize(int specialValue, Action onButtonClick)
    {
        _specialValue = specialValue;
        UpdateValues(specialValue, 0);

        _rollButton.onClick.RemoveAllListeners();
        _rollButton.onClick.AddListener(() => onButtonClick());
    }

    //It should display "?" for result while dice is rolling 
    public void UpdateValues(int currentRollValue, int totalValue)
    {
        _resultValue.text = currentRollValue == _specialValue ? "?" : currentRollValue.ToString();
        _totalValue.text = totalValue.ToString();
    }
}
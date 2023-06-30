using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class DiceRollUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resultValue;
    [SerializeField] private TextMeshProUGUI _totalValue;

    private void Start()
    {
        Assert.IsNotNull(_resultValue, "Result value TMP is missing");
        Assert.IsNotNull(_totalValue, "Total value TMP is missing");
    }

    //It should display "?" for result while dice is rolling 
    public void UpdateValues(int currentRollValue, int totalValue)
    {
        _resultValue.text = currentRollValue < 0 ? "?" : currentRollValue.ToString();
        _totalValue.text = totalValue.ToString();
    }
}
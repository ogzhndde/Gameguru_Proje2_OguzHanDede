using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EarnAmount : MonoBehaviour
{
    [SerializeField] private TextMeshPro TMP_EarnAmount;


    public void SetEarnAmount(int amount)
    {
        TMP_EarnAmount.text = "+" + amount;
    }

    public void DestroyEvent()
    {
        Destroy(gameObject);
    }
}

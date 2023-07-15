using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _bloodAmount;

    void Update()
    {
        _bloodAmount.text = Convert.ToString(BookManager.Instance.blood);
    }
}
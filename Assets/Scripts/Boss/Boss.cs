using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Boss : MonoBehaviour
{
    private float maxHP = 100f;
    private float nowHP = 100f;
    private float normalDamage;
    [SerializeField] Slider hpSlider;
    
    public void GetDamaged(int attackBookLevel)
    {
        nowHP -= normalDamage + attackBookLevel;
    }
}

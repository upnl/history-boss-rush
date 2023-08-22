using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletCountUI : MonoBehaviour
{
    public List<Image> bulletImages;

    private void Start()
    {
        if (bulletImages == null)
        {
            Debug.LogError("BulletImages not Set");
        }
    }

    public void OnBulletChanged(int bulletCount)
    {
        for (int i = 0; i < bulletImages.Count; i++)
        {
            Debug.Log("AAAAAAAAA");
            Image bulletImage = bulletImages[i];
            var tempColor = bulletImage.color;
            tempColor.a = i >= bulletCount ? 0.3f : 1f;
            bulletImage.color = tempColor;
        }
    }
}

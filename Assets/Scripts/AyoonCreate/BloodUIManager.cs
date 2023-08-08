using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodUIManager : MonoBehaviour
{
    private float _amount;
    public int spCount = 15;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        _amount = Mathf.RoundToInt(BookManager.Instance.maxBlood / 15);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeBloodUI();
    }

    private void ChangeBloodUI()
    {
        if (BookManager.Instance.Blood < _amount)
        {
            for (int i = 0; i < spCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(0).gameObject.SetActive(true);
        }

        else if (BookManager.Instance.Blood < _amount * 2)
        {
            for (int i = 0; i < spCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(1).gameObject.SetActive(true);
        }

        else if (BookManager.Instance.Blood < _amount * 3)
        {
            for (int i = 0; i < spCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(2).gameObject.SetActive(true);
        }

        else if (BookManager.Instance.Blood < _amount * 4)
        {
            for (int i = 0; i < spCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(3).gameObject.SetActive(true);
        }

        else if (BookManager.Instance.Blood < _amount * 5)
        {
            for (int i = 0; i < spCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(4).gameObject.SetActive(true);
        }

        else if (BookManager.Instance.Blood < _amount * 6)
        {
            for (int i = 0; i < spCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(5).gameObject.SetActive(true);
        }

        else if (BookManager.Instance.Blood < _amount * 7)
        {
            for (int i = 0; i < spCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(6).gameObject.SetActive(true);
        }

        else if (BookManager.Instance.Blood < _amount * 8)
        {
            for (int i = 0; i < spCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(7).gameObject.SetActive(true);
        }

        else if (BookManager.Instance.Blood < _amount * 9)
        {
            for (int i = 0; i < spCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(8).gameObject.SetActive(true);
        }

        else if (BookManager.Instance.Blood < _amount * 10)
        {
            for (int i = 0; i < spCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(9).gameObject.SetActive(true);
        }

        else if (BookManager.Instance.Blood < _amount * 11)
        {
            for (int i = 0; i < spCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(10).gameObject.SetActive(true);
        }

        else if (BookManager.Instance.Blood < _amount * 12)
        {
            for (int i = 0; i < spCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(11).gameObject.SetActive(true);
        }

        else if (BookManager.Instance.Blood < _amount * 13)
        {
            for (int i = 0; i < spCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(12).gameObject.SetActive(true);
        }

        else if (BookManager.Instance.Blood < _amount * 14)
        {
            for (int i = 0; i < spCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(13).gameObject.SetActive(true);
        }

        else
        {
            for (int i = 0; i < spCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(14).gameObject.SetActive(true);
        }
    }

}

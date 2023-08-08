using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BookBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _priceLabelPrefab;
    [SerializeField] private GameObject _contentLabelPrefab;
    [SerializeField] private GameObject _warningPrefab;
    private GameObject _canvasParent;
    private GameObject _bookDescription;

    private string _content;
    private int _level;
    private int _price;

    private string _koreanName;
    private string _description;

    public void SetProperties(string content, int level, int price, string koreanName, string description)
    {
        /*
        _content = content;
        _level = level;
        _price = price;

        _koreanName = koreanName;
        _description = description;

        var priceLabelPosition = transform.position;
        priceLabelPosition.y += 1;
        var priceLabel = Instantiate<GameObject>(_priceLabelPrefab, priceLabelPosition, Quaternion.identity);
        var textField = priceLabel.GetComponent<Text>();
        textField.text = Convert.ToString(_price);

        Debug.Log(_canvasParent);
        priceLabel.transform.SetParent(_canvasParent.transform, false);
        textField.transform.position = new Vector3(0, 0, 0);

        var rectTransform = priceLabel.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(priceLabelPosition.x * 110, priceLabelPosition.y * 107 - 20, 0f);

        if (_level == 1)
        {
            var contentLabel = Instantiate<GameObject>(_contentLabelPrefab, priceLabelPosition, Quaternion.identity);
            var contentTextField = contentLabel.GetComponent<Text>();
            contentTextField.text = Convert.ToString(_koreanName);
            contentLabel.transform.SetParent(_canvasParent.transform, false);
            contentTextField.transform.position = new Vector3(0,0,0);

            rectTransform = contentLabel.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector3(priceLabelPosition.x * 107, priceLabelPosition.y * 107 - 170, 0f);
        }
        */
    }

}
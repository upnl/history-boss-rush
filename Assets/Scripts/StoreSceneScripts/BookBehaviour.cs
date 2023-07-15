using System;
using UnityEngine;
using UnityEngine.UI;

public class BookBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _priceLabelPrefab;
    private GameObject _canvasParent;

    private string _content;
    private int _level;
    private float _price;

    private void Awake()
    {
        _canvasParent = GameObject.FindWithTag("EditorOnly");
    }

    public void SetProperties(string content, int level, float price)
    {
        _content = content;
        _level = level;
        _price = price;

        var priceLabelPosition = transform.position;
        priceLabelPosition.y += 1;
        var priceLabel = Instantiate<GameObject>(_priceLabelPrefab, priceLabelPosition, Quaternion.identity);
        var textField = priceLabel.GetComponent<Text>();
        textField.text = Convert.ToString(_price);
        priceLabel.transform.SetParent(_canvasParent.transform, false);
        textField.transform.position = new Vector3(0, 0, 0);

        var rectTransform = priceLabel.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(priceLabelPosition.x * 100, priceLabelPosition.y * 100, 0f);
    }

    public void OnMouseDown()
    {
        BookManager.Instance.SetBookEquipped(_content, _level, _price);
    }
}
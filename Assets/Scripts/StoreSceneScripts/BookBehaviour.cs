using System;
using UnityEngine;
using UnityEngine.UI;

public class BookBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _priceLabelPrefab;
    [SerializeField] private GameObject _contentLabelPrefab;
    [SerializeField] private Text _bookDescription;
    private GameObject _canvasParent;

    private string _content;
    private int _level;
    private float _price;

    private string _koreanName;
    private string _description;

    private void Awake()
    {
        _canvasParent = GameObject.FindWithTag("EditorOnly");
        _bookDescription.text = "가져갈 책을 구매하자";
    }

    public void SetProperties(string content, int level, float price, string koreanName, string description)
    {
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
    }

    public void OnMouseDown()
    {
        BookManager.Instance.SetBookEquipped(_content, _level, _price);
        transform.position = new Vector3(10000f, 10000f, -15f);
    }

    public void OnMouseEnter()
    {
        _bookDescription.text = _description;
    }
}
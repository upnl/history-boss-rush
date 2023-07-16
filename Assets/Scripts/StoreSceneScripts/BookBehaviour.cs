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
    private GameObject _uiManager;
    private GameObject _bookDescription;

    private string _content;
    private int _level;
    private int _price;

    private string _koreanName;
    private string _description;

    private void Awake()
    {
        _canvasParent = GameObject.FindWithTag("EditorOnly");
        _uiManager = GameObject.FindWithTag("GameController");
        _bookDescription = GameObject.FindWithTag("Player");
    }

    public void SetProperties(string content, int level, int price, string koreanName, string description)
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
        if (!_uiManager.GetComponent<UIManager>().dialogueActive)
        {
            if (BookManager.Instance.blood >= _price && BookManager.Instance.CheckBookEquipped(_content) < _level)
            {
                var currentPosition = transform.position;
                currentPosition.z = -15f;
                transform.position = currentPosition;
                AudioManager.Instance.PlaySfx(0);
            }
            if (BookManager.Instance.blood < _price)
            {
                var warningPosition = transform.position;
                warningPosition.z = -5f;
                var warning = Instantiate<GameObject>(_warningPrefab, warningPosition, Quaternion.identity);
            }
            BookManager.Instance.SetBookEquipped(_content, _level, _price);
        }
    }

    public void OnMouseEnter()
    {
        BookManager.Instance.bookDescription = _description;
        var bookPosition = transform.position;
        _bookDescription.GetComponent<Text>().transform.position = new Vector3(bookPosition.x * 107 + 960, bookPosition.y * 107 + 540, 0f);
    }
}
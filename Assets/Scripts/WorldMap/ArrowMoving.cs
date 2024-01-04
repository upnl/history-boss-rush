using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrowMoving : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float margin = 10.0f;
    public float speed = 5.0f;
    public int arrowDirection;
    public GameObject Earth;
    public Sprite NormalImage;
    public Sprite ClickedImage;

    private Image _image;
    private EarthAnimation _earthAnimation;
    private Vector3 _startPosition;

    void Awake()
    {
        _image = GetComponent<Image>();
        _image.sprite = NormalImage;
        _earthAnimation = Earth.GetComponent<EarthAnimation>();
    }

    void Start()
    {
        _startPosition = gameObject.transform.position;
    }

    void Update()
    {
        Vector3 position = gameObject.transform.position;
        if (-margin < position.x - _startPosition.x && position.x - _startPosition.x < margin)
        {
            position.x = position.x + arrowDirection * speed * Time.deltaTime;
            gameObject.transform.position = position;
        }
        else
        {
            position.x = _startPosition.x;
            gameObject.transform.position = position;
        }
        if (EarthAnimation.FieldOpen && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _image.sprite = ClickedImage;
        if (arrowDirection == -1)
        {
            _earthAnimation.LeftMoving();
        }
        else if (arrowDirection == 1)
        {
            _earthAnimation.RightMoving();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _image.sprite = NormalImage;
    }
}

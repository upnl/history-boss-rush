using UnityEngine;

public class BookBehaviour : MonoBehaviour
{

    private string _content;
    private int _level;
    private int _price;

    public int price => _price;

    public void setProperties(string content, int level, int price)
    {
        _content = content;
        _level = level;
        _price = price;
    }

    public void OnMouseDown()
    {
        BookManager.Instance.setBookEquipped(_content, _level, _price);
        Debug.Log(BookManager.Instance.blood);
        Debug.Log(BookManager.Instance.checkBookEquipped(_content));
    }
}
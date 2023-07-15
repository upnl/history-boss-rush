using UnityEngine;

public enum BookContent
    {
        Challenge
    }

public class BookBehaviour : MonoBehaviour
{

    private BookContent _content;
    private int _level;
    private int _price;

    public int price => _price;

    public void setProperties(BookContent content, int level, int price)
    {
        _content = content;
        _level = level;
        _price = price;
    }

    public void OnMouseDown()
    {
        Debug.Log("Clicked");
    }
}
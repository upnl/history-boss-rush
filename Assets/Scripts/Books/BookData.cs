using UnityEngine;
using System.Collections.Generic;

public class BookData
{
    public static BookData instance {get; private set;}

    public string[] bookList = new string[] {"judgement", "challenge", "sanctuary", "alertness"};

    public Dictionary<string, int> bookUnlocked;
    public Dictionary<string, int> bookEquipped;
}

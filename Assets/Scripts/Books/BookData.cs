using UnityEngine;
using System.Collections.Generic;

public class BookData
{
    public static BookData instance {get; private set;}

    public Dictionary<string, int> bookUnlocked = new Dictionary<string, int>();
    public Dictionary<string, int> bookEquipped = new Dictionary<string, int>();
}

using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShelfData", order = 1)]

public class ShelfData: ScriptableObject
{
    public float xMin;
    public float xMax;
    public float yRow1;
    public float yRow2;
    public float yRow3;
    public float yRow4;

    public float overlapRange;
    public float dummyOverlapRange;
}
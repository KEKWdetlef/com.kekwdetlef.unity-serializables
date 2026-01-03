using UnityEngine;

public class ExampleMono : MonoBehaviour
{
    [SerialiizeField] private SList<int> ints;
    [SerialiizeField] private SUniqueList<GameObject> gameObjects;
}

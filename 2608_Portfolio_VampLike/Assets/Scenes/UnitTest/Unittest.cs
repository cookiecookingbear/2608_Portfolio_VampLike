using UnityEngine;

public class Unittest : MonoBehaviour
{
    private void Awake()
    {
        print($"cos90: {Mathf.Cos(90)}");
        print($"cos180: {Mathf.Cos(180)}");
        print($"cos270: {Mathf.Cos(270)}");
        print($"cos0: {Mathf.Cos(0)}");

        print($"sin90: {Mathf.Sin(90)}");
        print($"sin180: {Mathf.Sin(180)}");
        print($"sin270: {Mathf.Sin(270)}");
        print($"sin0: {Mathf.Sin(0)}");
    }
}

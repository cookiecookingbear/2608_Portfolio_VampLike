using UnityEngine;

[CreateAssetMenu(fileName = "Gear_Stats", menuName = "Scriptable Objects/Gear_Stats")]
public class Gear_Stats : ScriptableObject
{
    [SerializeField] private float[] stats;

    public float[] Stats => stats;
}

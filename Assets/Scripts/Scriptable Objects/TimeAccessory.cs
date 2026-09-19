using UnityEngine;

[CreateAssetMenu(fileName = "Time Accessory", menuName = "Scriptable Objects/Time Accessory")]
public class TimeAccessory : TimeItem
{
    public string AccessoryName { get { return _accessoryName; } }

    [Tooltip("Name of the accessory object to check for")]
        [SerializeField] private string _accessoryName; //NB : maybe it's better to change this a prefab for comparison ?
}
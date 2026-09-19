using UnityEngine;

[CreateAssetMenu(fileName = "Time Cloth", menuName = "Scriptable Objects/Time Cloth")]
public class TimeCloth : TimeItem
{
    public ClothType Type { get { return _clothType; } }
    public Material ClothMaterial { get { return _clothMaterial; } }

    public enum ClothType
    {
        Shirt = 0,
        Pants = 1
    }

    [Tooltip("Type of the cloth")]
        [SerializeField] private ClothType _clothType;

    [Tooltip("Material associated to the cloth")]
        [SerializeField] private Material _clothMaterial;
}
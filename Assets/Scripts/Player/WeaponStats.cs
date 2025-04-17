// WeaponStats.cs
using UnityEngine;

[DisallowMultipleComponent]
public class WeaponStats : MonoBehaviour
{
    [Tooltip("How much damage this weapon does per hit or per bullet.")]
    public int damage = 10;
}

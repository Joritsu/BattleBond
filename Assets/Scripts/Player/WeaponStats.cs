using UnityEngine;

[DisallowMultipleComponent]
public class WeaponStats : MonoBehaviour
{
    [Tooltip("How much damage this weapon does per hit or per bullet.")]
    public int damage = 10;
    [Tooltip("Impulse force applied to the target.")]
    public float knockbackForce = 5f;
    [Tooltip("How long the enemy is stunned (no AI) after knockback).")]
    public float knockbackStunDuration = 0.25f;
}
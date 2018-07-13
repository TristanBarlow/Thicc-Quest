using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractParent : MonoBehaviour
{
    public float Health = 1.0f;
    private HealthBarScript hbar;
    bool HasHealth = false;

     void Start()
    {
        hbar = GetComponentInChildren<HealthBarScript>();
        if (hbar != null)
        {
            HasHealth = true;
        }
    }

    public virtual void Interact() { }
    public virtual void Hit(float rawDamage) { }

    public void UpdateHealthBar()
    {
        if (!HasHealth) return;
        if (Health <= 0) Destroy(gameObject);
        hbar.UpdateHealth(Health);
    }
}

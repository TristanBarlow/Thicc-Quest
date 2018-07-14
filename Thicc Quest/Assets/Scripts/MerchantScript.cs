using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantScript : InteractParent
{

	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact()
    {
        base.Interact();
        WeaponMaker.Instance.gameObject.SetActive(true);
    }

    public override void Hit(float rawDamage)
    {
        base.Hit(rawDamage);
        Health -= rawDamage;
        UpdateHealthBar();
    }
}

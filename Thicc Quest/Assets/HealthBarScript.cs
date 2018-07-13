using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    private Image foreground;

	// Use this for initialization
	void Start ()
    {
       foreground = GetComponent<Image>();
	}

    public void UpdateHealth(float newAmount)
    {
        foreground.fillAmount = newAmount;
        foreground.color =new Color((1-newAmount), (newAmount), 0, 1);
    }
}

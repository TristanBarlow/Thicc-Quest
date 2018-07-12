using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractScript : MonoBehaviour
{

    bool Interact = false;

    public ContactFilter2D filter;

    public float InteractRange = 1;

    public Collider2D collider;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact = true;
            Collider2D[] overlaps = new Collider2D[6];
            collider.OverlapCollider(filter, overlaps);
            foreach (Collider2D c in overlaps)
            {
                if (c != null)
                {
                    Debug.Log(c.gameObject);
                    if (c.gameObject.GetComponent<ChestScript>())
                    {
                        c.gameObject.GetComponent<ChestScript>().TryOpen();
                    }
                }
            }
            //RaycastHit2D[] hits = new RaycastHit2D[12];
            //Physics2D.Raycast(transform.position, transform.forward, filter, hits, InteractRange);
            //foreach (RaycastHit2D hit in hits)
            //{
            //    if (hit.collider != null)
            //    {
            //        Debug.Log(hit.collider.gameObject);
            //        if (hit.collider.gameObject.GetComponent<ChestScript>())
            //        {
            //            hit.collider.gameObject.GetComponent<ChestScript>().TryOpen();
            //        }
            //    }
            //}
        }
        else
        {
            Interact = false;
        }
		
	}

    public bool IsInteracting() { return Interact; }
}

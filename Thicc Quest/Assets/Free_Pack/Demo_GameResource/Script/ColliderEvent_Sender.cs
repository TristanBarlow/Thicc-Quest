using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEvent_Sender : MonoBehaviour {
    private CharacterController_2D m_parent;
    void Start()
    {

        m_parent = this.transform.root.transform.GetComponent<CharacterController_2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
   

        //other.GetComponent<WoodDoll_Mgr>().Sword_Hitted();

        if (this.GetComponent<BoxCollider2D>().enabled)
        {
            m_parent.Once_Attack = true;

        }

        if (other.GetComponent<InteractParent>())
        {
            other.GetComponent<InteractParent>().Hit(0.1f);
        }

    }


}

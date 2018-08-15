using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ControlScheme
{
    map,
    inventory,
    weaponMaker,
    none
}

public class CharacterController_2D : MonoBehaviour
{
 
    public static CharacterController_2D Instance { set; get; }
    Rigidbody2D m_rigidbody;
    Animator m_Animator;
    Transform m_tran;

    private float h = 0;
    private float v = 0;

    public float MoveSpeed = 40;

    public SpriteRenderer[] m_SpriteGroup;

    public GameObject[] MovementNodes;

    private float health = 1.0f;

    public GameObject weaponSlot;

    public InteractScript interact;

    private ControlScheme controls = ControlScheme.map;

    public bool Once_Attack = false;

    public bool IsAttacking = false;

    public int sortingOrder = 0;
    public int sortingOrderOrigine = 0;

    private float Update_Tic = 0;
    private float Update_Time = 0.1f;

    // character Filp 
    bool B_Attack = false;
    bool B_FacingRight = true;

    // Use this for initialization
    void Start () {
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        m_Animator = this.transform.Find("BURLY-MAN_1_swordsman_model").GetComponent<Animator>();
        m_tran = this.transform;
        m_SpriteGroup = this.transform.Find("BURLY-MAN_1_swordsman_model").GetComponentsInChildren<SpriteRenderer>(true);

        Instance = this;
    }

    public List<Vector2> GetNodePos()
    {
        List<Vector2> list = new List<Vector2>();
        foreach (GameObject obj in MovementNodes)
        {
            list.Add(obj.transform.position);
        }
        return list;
    }

    public void ChangeWeapon(Sprite s)
    {
        weaponSlot.GetComponent<SpriteRenderer>().sprite = s;
    }

    public void SwitchControls(ControlScheme nType) { controls = nType; }
    public ControlScheme GetControls() { return controls;}

    private void AttackControls()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Once_Attack = false;
            m_Animator.SetTrigger("Attack");

           // m_rigidbody.velocity = new Vector3(0, 0, 0);

            IsAttacking = true;
        }

        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Once_Attack = false;
            Debug.Log("Rclick");
            m_Animator.SetTrigger("Attack2");

            IsAttacking = true;

            // m_rigidbody.velocity = new Vector3(0, 0, 0);
        }

    }

    private void CheckForInventoryActions()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            InventoryUIHandler.Instance.RefreshSlots(true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            InventoryUIHandler.Instance.RefreshSlots();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIManager.Instance.ChangeCanvas("inventory");
        }
    }

    private void CheckForMapActions()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIManager.Instance.ChangeCanvas("inventory");
        }
        AttackControls();
        interact.CheckForInteract();
        Move_Fuc();
    }


    // Update is called once per frame
    void Update ()
    {
        
        spriteOrder_Controller();

        switch (controls)
        {
            case ControlScheme.inventory:
                CheckForInventoryActions();
                break;
            case ControlScheme.map:
                CheckForMapActions();
                break;
            case ControlScheme.weaponMaker:
                break;
            case ControlScheme.none:
                break;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            MessageManager.Instance.Dismiss();
        }


    }



    void spriteOrder_Controller()
    {

        Update_Tic += Time.deltaTime;

        if (Update_Tic > 0.1f)
        {
            sortingOrder = Mathf.RoundToInt(this.transform.position.y * 100);
            //Debug.Log("y::" + this.transform.position.y);
            //  Debug.Log("sortingOrder::" + sortingOrder);
            for (int i = 0; i < m_SpriteGroup.Length; i++)
            {

                m_SpriteGroup[i].sortingOrder = sortingOrderOrigine - sortingOrder;

            }

            Update_Tic = 0;
        }

     

    }

    // character Move Function
    void Move_Fuc()
    {
        if (Input.GetKey(KeyCode.A))
        {
          //  Debug.Log("Left");
            m_rigidbody.AddForce(Vector2.left * MoveSpeed * Time.deltaTime);
            if (B_FacingRight)
                Filp();


        }
        else if (Input.GetKey(KeyCode.D))
        {
          //  Debug.Log("Right");
            m_rigidbody.AddForce(Vector2.right * MoveSpeed*Time.deltaTime);
            if (!B_FacingRight)
                Filp();
        }

        if (Input.GetKey(KeyCode.W))
        {
           // Debug.Log("up");
            m_rigidbody.AddForce(Vector2.up * MoveSpeed * Time.deltaTime);
          
        }
        else if (Input.GetKey(KeyCode.S))
        {
           // Debug.Log("Down");
            m_rigidbody.AddForce(Vector2.down * MoveSpeed * Time.deltaTime);
          
            
        }


        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        m_Animator.SetFloat("MoveSpeed", Mathf.Abs(h) + Mathf.Abs(v));
    }


    void Filp()
    {
        B_FacingRight = !B_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;

        m_tran.localScale = theScale;
    }



    //   Sword,Dagger,Spear,Punch,Bow,Gun,Grenade


    public void FinishedAttack()
    {
        IsAttacking = false;
    }

  
}

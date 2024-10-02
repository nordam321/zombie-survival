using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementStateManager : MonoBehaviour
{
    public float moveSpead;
    [HideInInspector] public Vector3 dir;
    public float hzInput,vInput;
    CharacterController controller;


    //Gravity 
    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;
    Vector3 spharePos;
    [SerializeField] float gravity = -200.01f;
    [SerializeField] float jumpForce = 2f;
    [HideInInspector] public bool jumped;
    Vector3 velocity;

    //abstract class instance to use th functions that have inside it 
    public MoveBaseState previousState;
    public MoveBaseState currentState;

    //ستايتس عشان تحريك الانيمشين وتنظيمه لكي لا يشتغل كل الانيمشين دون استخدامه
    public WalkState Walk = new WalkState();
    public RunState Run= new RunState();
    public CrouchState crouch= new CrouchState();
    public IdleState Idle = new IdleState();    
    public JumpState jump = new JumpState();

    [HideInInspector] public Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        SwitchState(Idle);
    }

    // Update is called once per frame
    void Update()
    {
        GetDirectionAndMove();
        Gravity();
        anim.SetFloat("hzInput",hzInput);
        anim.SetFloat("vInput",vInput);

        currentState.UpdateState(this);
    }

    public void SwitchState(MoveBaseState state){
        currentState = state;
        currentState.EnterState(this);
    }

    void GetDirectionAndMove(){
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");    
        dir = transform.forward * vInput + transform.right * hzInput;
        controller.Move(dir.normalized * moveSpead * Time.deltaTime);
    }
    public bool IsGrounded(){
        spharePos =  new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if(Physics.CheckSphere(spharePos , controller.radius - 1f,groundMask))
        {
        return true;
        }   
        return false;
    }

    void Gravity()
    {
        if(!IsGrounded()) { velocity.y += gravity * Time.deltaTime;}
        else if (velocity.y < 0) velocity.y = -2f;
        controller.Move(velocity * Time.deltaTime);

    }
    private void OnDrawGizoms(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spharePos,controller.radius - 0.05f);
    }
    public void JumpForce() => velocity.y += jumpForce;

    public void Jumped() => jumped = true;
}

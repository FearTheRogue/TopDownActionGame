using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{ 

     public static Player Instance {get; private set; }

	 [SerializeField] private float movingSpeed = 4f;

	 private Rigidbody2D rb;

	 public VectorValue startingPosition;
	 private float minMovingSpeed = 0.1f;
	 private bool isRunning = false;
	 [SerializeField] private GameObject attackPoint;

	 [SerializeField] private Animator anim;
	 private float cooldownTimer = Mathf.Infinity;
	 [SerializeField] private float attackCooldown;

	private PlayerInputActions playerInputActions;

	Vector2 inputVector;
    public float moveSpeed;

     private void Awake() 
	 {
		 Instance = this;
		 rb = GetComponent<Rigidbody2D>();		 
		 attackPoint.SetActive(false);
		// startingPosition.initialValue = new Vector2(0, 0);
		 transform.position = startingPosition.initialValue;


        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private void Update()
    {
        inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
    }

    private void FixedUpdate() 
	 {
        Vector2 move = inputVector;

        if (move.sqrMagnitude > 1f)
            move.Normalize();

        rb.linearVelocity = move * moveSpeed;



        //  inputVector = inputVector.normalized;
        //  rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        //  if(Input.GetKey(KeyCode.Mouse0) && cooldownTimer > attackCooldown)
        //  {
        //	  Debug.Log("Mouse");
        //	  anim.SetTrigger("PlayerAttack");
        //	  StartCoroutine(AttackCoroutine());
        //	  cooldownTimer = 0;
        //  }

        //  cooldownTimer += Time.deltaTime;
    }

	 private IEnumerator AttackCoroutine()
     {
        yield return new WaitForSeconds(0.5f);
		attackPoint.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		attackPoint.SetActive(false);
	 }

	 public bool IsRunning()
		  {
			  return isRunning;
		  }	 

	
}

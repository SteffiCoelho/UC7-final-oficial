using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    private Rigidbody rb;
    private bool isOnGround;
    private int doubleJump = 0;
    private bool isAttacking;
    private Vector3 angleRotation;
    private bool isInteracting;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private List<GameObject> inventory = new List<GameObject>();



    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        angleRotation = new Vector3(0, 90, 0);
        isInteracting = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Andar 
        float fowardInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * fowardInput;
        Vector3 moveFoward = rb.position + moveDirection * speed * Time.deltaTime;
        rb.MovePosition(moveFoward);

        //Rotacionar
        float sideInput = Input.GetAxis("Horizontal");
        Quaternion deltaRotation = Quaternion.Euler(angleRotation * sideInput * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);

        //Animação
        if (Input.GetKey(KeyCode.W))
        {
            playerAnimator.SetBool("Walk", true);

            if (Input.GetKey(KeyCode.W) && playerAnimator.GetBool("WalkBack"))
            {
                playerAnimator.SetBool("WalkToBack", true);
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerAnimator.SetBool("WalkBack", true);

            if (Input.GetKey(KeyCode.S) && playerAnimator.GetBool("Walk"))
            {
                playerAnimator.SetBool("WalkToBack", false);
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            playerAnimator.SetBool("Walk", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerAnimator.SetBool("Walk", true);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            playerAnimator.SetTrigger("Interact");
            isInteracting = true;
        }
        else
        {
            playerAnimator.SetBool("Walk", false);
            playerAnimator.SetBool("WalkBack", false);
            playerAnimator.SetBool("WalkToBack", false);

        }

        //Movimento e Animação do Pulo
        if (Input.GetKey(KeyCode.Space) && isOnGround)
        {
            JumpMove();
            JumpAnimation();
        }

        //Animação do ataque
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.KeypadEnter))
        {
            PlayerAttack();
        }
        else
        {
            isAttacking = false;
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerAnimator.SetBool("IsOnGround", true);
            isOnGround = true;
        }

    }

    private void JumpMove()
    {
        isOnGround = false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void JumpAnimation()
    {
        playerAnimator.SetTrigger("Jumping");
        playerAnimator.SetBool("IsOnGround", false);
    }

    private void PlayerAttack()
    {
        playerAnimator.SetTrigger("Attack");
        isAttacking = true;

    }

    public bool IsInteracting()
    {
        return isInteracting;
    }
}

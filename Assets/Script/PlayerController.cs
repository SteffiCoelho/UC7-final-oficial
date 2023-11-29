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
    private OpenGate gate;
    private bool isInteracting;
    private AudioSource player;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private AudioClip passo;
    [SerializeField]
    private List<GameObject> inventory = new List<GameObject>();



    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        angleRotation = new Vector3(0, 90, 0);
        isInteracting = false;
        player = GetComponent<AudioSource>();
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
            if (!player.isPlaying)
            {
                player.PlayOneShot(passo, 0.6f);
            }

            if (Input.GetKey(KeyCode.W) && playerAnimator.GetBool("WalkBack"))
            {
                playerAnimator.SetBool("WalkToBack", true);
                if (!player.isPlaying)
                {
                    player.PlayOneShot(passo, 0.6f);
                }
            }
        }
      
        else if (Input.GetKey(KeyCode.S))
        {
            playerAnimator.SetBool("WalkBack", true);
            if (!player.isPlaying)
            {
                player.PlayOneShot(passo, 0.6f);
            }

            if (Input.GetKey(KeyCode.S) && playerAnimator.GetBool("Walk"))
            {
                playerAnimator.SetBool("WalkToBack", false);
                if (!player.isPlaying)
                {
                    player.PlayOneShot(passo, 0.6f);
                }
            }
        }
        
        else if (Input.GetKey(KeyCode.A))
        {
            playerAnimator.SetBool("Walk", true);
            if (!player.isPlaying)
            {
                player.PlayOneShot(passo, 0.6f);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerAnimator.SetBool("Walk", true);
            if (!player.isPlaying)
            {
                player.PlayOneShot(passo, 0.6f);
            }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            Debug.Log("Colide with door");
            OpenGate actualGate = other.GetComponent<OpenGate>();
            gate = actualGate;
            isInteracting = true;
        }
    }



    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerAnimator.SetBool("IsOnGround", false);
            isOnGround = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            OpenGate actualGate = other.GetComponent<OpenGate>();
            Debug.Log("Leaving Gate");
            if (gate == actualGate)
            {
                gate = null;
                isInteracting = false;
            }
        }
    }

    private void JumpMove()
    {
        isOnGround = false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void InteractToGate()
    {
        if (gate)
        {
            Debug.Log("Tentando abrir porta");
            gate.GateOpen();

            if (gate.GateIsLocked())
            {
                Debug.Log("Porta Trancada");
                foreach (GameObject item in inventory)
                {
                    Debug.Log("Varrendo Inventário");
                    if (gate.HasKey(item.name))
                    {
                        Debug.Log("Destrancando Porta");
                        gate.UnlockGate();
                        inventory.Remove(item);
                    }
                }

                gate.GateOpen();
            }
        }
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

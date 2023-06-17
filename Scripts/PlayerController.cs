using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //variables movimiento 

    public float jumpForce = 6f;
    public float runningSpeed = 2f;
    private Rigidbody2D rigidBody;
    Animator animator;
    Vector3 startPosition;

    private const string STATE_ALIVE = "isAlive";
    private const string STATE_ON_THE_GROUND = "isOnTheGround";

    public float jumpRaycastDistance = 1.5f;

    [SerializeField]
    private int healthPoints, manaPoints;

    public const int INITAL_HEALTH = 100, INITIAL_MANA = 15,
        MAX_HEALTH = 200, MAX_MANA = 30,
        MIN_HEALTH = 10,MIN_MANA=0;

    public const int SUPERJUMP_COST = 5;
    public const float SUPERJUMP_FORCE = 1.5f;

    public LayerMask groundMask;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

        startPosition = this.transform.position;
    }

   public void StartGame()
    {
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, true);

        healthPoints = INITAL_HEALTH;
        manaPoints = INITIAL_MANA;
        Invoke("RestartPosition", 0.2f);
 
    }

    void RestartPosition()
    {
        this.transform.position = startPosition;
        this.rigidBody.velocity = Vector2.zero;
        GameObject maincamera = GameObject.Find("Main Camera");
        maincamera.GetComponent<CameraFollow>().ResetCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        // space or get mouse to play!!!
        //classe 9
        if (Input.GetButtonDown("Jump"))
        {
            Jump(false);
        }
        if (Input.GetButtonDown("Superjump"))
        {
            Jump(true);
        }

        animator.SetBool(STATE_ON_THE_GROUND, isTouchingTheGround());

        Debug.DrawRay(this.transform.position,
            Vector2.down * jumpRaycastDistance, Color.red);
    }

     void FixedUpdate()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (rigidBody.velocity.x < runningSpeed)
            {
                rigidBody.velocity = new Vector2(runningSpeed,
                    rigidBody.velocity.y);
            }
        }
        else
        {//si no estamos dentro dela partida
            rigidBody.velocity = new Vector2(0,rigidBody.velocity.y);
        }
  
    }

    void Jump(bool superjump)
    {
        float jumpForceFactor = jumpForce;
        if (superjump && manaPoints >=SUPERJUMP_COST)
        {
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (isTouchingTheGround())
            {
                
                GetComponent<AudioSource>().Play();
                rigidBody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);

            }
        }

    }

    //metodo que nos indica si elpersonaje esta tocando o no el suelo
    bool isTouchingTheGround()
    {
        if(Physics2D.Raycast(this.transform.position,
            Vector2.down,
                jumpRaycastDistance,
                groundMask))
        {
            //TODO : programar logica del contacto con el suelo
            //animator.enabled = true;
         //   GameManager.sharedInstance.currentGameState = GameState.inGame;
            return true;
        }
        else
        {
            //Todo : programar logica del no cotactoo
           // animator.enabled = false;
            return false;
        }
    }

    public void Die()
    {
        float travelledDistance = GetTravelDistance();
        float previousMaxDistance = PlayerPrefs.GetFloat("maxscore",0f);
        if (travelledDistance> previousMaxDistance)
        {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);
        }
        this.animator.SetBool(STATE_ALIVE, false);
       // GetComponent<AudioSource>().Play();

        GameManager.sharedInstance.GameOver();

    }

    public void CollectHealt(int points)
    {
        this.healthPoints += points;
        if (this.healthPoints>=MAX_HEALTH)
        {
            this.healthPoints = MAX_HEALTH;
        }
        if (this.healthPoints <=0)
        {
            Die();
        }
    }
    public void CollectMana(int points)
    {
        this.manaPoints += points;
        if (this.manaPoints >= MAX_MANA)
        {
            this.manaPoints = MAX_MANA;
        }
    }
    public int GetHealth()
    {
        return healthPoints;
    }
    public int GetMana()
    {
        return manaPoints;
    }
    public float GetTravelDistance()
    {
        return this.transform.position.x - startPosition.x;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType
{
    healthPotion,
    manaPotion,
    money
}

public class Collectable : MonoBehaviour
{

    public CollectableType type = CollectableType.money;

    private SpriteRenderer sprite;
    private CircleCollider2D itemCollider;

    bool hasBeenCollected = false;

    public int value = 1;

    GameObject player;


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<CircleCollider2D>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.Find("Player");
    }
    void Show()
    {
        sprite.enabled = true;
        itemCollider.enabled = true;
        hasBeenCollected = false;
    }
    void Hide()
    {
        sprite.enabled = false;
        itemCollider.enabled = false;
    }
    void Collect()
    {
        Hide();
        hasBeenCollected = true;

        switch (this.type)
        {
            case CollectableType.healthPotion:
                player.GetComponent<PlayerController>().CollectHealt(this.value);
                break;
            case CollectableType.manaPotion:
                player.GetComponent<PlayerController>().CollectMana(this.value);
                break;
            case CollectableType.money:

                GameManager.sharedInstance.CollectObject(this);
                GetComponent<AudioSource>().Play();
                //sonido money

                break;
            default:
                break;
        }
    }
     void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Collect();
        }
    }
}

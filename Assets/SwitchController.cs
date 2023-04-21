using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Sprite offSprite;
    [SerializeField] Sprite onSprite;
    [SerializeField] GameObject child;
    AudioSource src;
    public bool on = false;

    private void Awake()
    {
        src = gameObject.GetComponentInChildren<AudioSource>();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            child.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            child.SetActive(false);
        }
    }

    private void Update()
    {
        if (child.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift))
            {
                src.Play();
                if (!on)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = onSprite;
                    on = true;
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = offSprite;
                    on = false;
                }
            }
        }
        
    }
}

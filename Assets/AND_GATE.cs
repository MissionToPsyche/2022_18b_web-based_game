using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AND_GATE : MonoBehaviour
{
    // Start is called before the first frame update

    // variables holding different sprites to change how the object appears
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;
    public List<SwitchController> switches;

    // On variable to keep track of whether this light bulb is on or off
    public bool on = false;

    // Required binary combination to turn the light on (in this example we assume there'es only
    //      one switch and it should be turned ON for the light to turn ON
    public string binary = "1";

    // TurnOn and TurnOff functions to be called by the manager class
    public void TurnOn()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = onSprite;
        on = true;
    }


    public void TurnOff()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = offSprite;
        on = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageLevel : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject[] switches;
    GameObject[] lightbulbs;
    [SerializeField] AudioSource src;
    [SerializeField] GameObject pausePanel;
    bool pause = false;
    void Start()
    {
        switches = GameObject.FindGameObjectsWithTag("Switch");
        lightbulbs = GameObject.FindGameObjectsWithTag("Lightbulb");
    }

    void Pause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
            if (pause)
            {
                src.Pause();
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                src.Play();
                pausePanel.SetActive(false);
                Time.timeScale = 1;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(currentBinary);
        Pause();
        foreach(GameObject lightbulb in lightbulbs)
        {
            Lightbulb lb = lightbulb.GetComponent<Lightbulb>();
            string currentBinary = "";
            foreach(SwitchController lever in lb.switches)
            {
                if (lever.on)
                {
                    currentBinary += "1";
                }
                else
                {
                    currentBinary += "0";
                }
            }
            if (lb.binary.Equals(currentBinary))
            {
                if(!lb.on)
                {
                    lb.TurnOn();
                }
            }
            else if(lb.on)
            {
                lb.TurnOff();
            }
        }
    }
}

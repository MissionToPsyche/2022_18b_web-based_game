using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelLoad : MonoBehaviour
{
    public void loadLevel(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }
}

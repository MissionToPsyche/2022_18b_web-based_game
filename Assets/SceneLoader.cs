using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Collider2D doorCollider;

    void Update()
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        int nextLevelBuildIndex = scene.buildIndex + 1;
        SceneManager.LoadScene(nextLevelBuildIndex);
    }
}
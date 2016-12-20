using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string level;

    public void Load()
    {
        SceneManager.LoadScene(level);
    }
}

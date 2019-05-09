using UnityEngine;
using System.Collections;

public class AvoidFilledSpace : MonoBehaviour {
    
    void Update()
    {
        switch(GameManager.instance.state)
        {
            case GameManager.States.Over:
                Destroy(gameObject);
                break;
            case GameManager.States.Menu:
                Destroy(gameObject);
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Tail")
            Destroy(gameObject);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour {
    public Sprite[] sprites;
    public Transform topLeftCorner;
    public Transform bottomRightCorner;
    public float timeRate;
    public int spots;
    public GameObject tailPrefab;
    public Text bestScoreDisplay;
    public Text finalScoreDisplay;
    public Text spotDisplay;

    int bestScore,h,v;
    bool ate = false;
    List<Transform> tail = new List<Transform>();
    Vector2 dir = Vector2.right;
    float yMin, yMax, xMin, xMax;
    SpriteRenderer spriteRenderer;

    void Start () {
        if (PlayerPrefs.HasKey("Best Score"))
            bestScore = PlayerPrefs.GetInt("Best Score");
        else
            bestScore = 0;
        yMin = bottomRightCorner.position.y;
        yMax = topLeftCorner.position.y;
        xMin = topLeftCorner.position.x;
        xMax = bottomRightCorner.position.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
		Invoke("Move", timeRate);
	}

	void Update () {
        float x = transform.position.x;
        float y = transform.position.y;

        if (x > xMax+0.3f)
        {
            transform.position = new Vector3(xMin, y, 0);
        }
        else if (x < xMin-0.3f)
        {
            transform.position = new Vector3(xMax, y, 0);
        }
        else if (y > yMax+0.3f)
        {
            transform.position = new Vector3(x, yMin, 0);
        }
        else if (y < yMin-0.3f)
        {
            transform.position = new Vector3(x, yMax, 0);
        }
#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
        if (Input.GetAxis("Horizontal") > 0)
        {
            h = 1;
            v = 0;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            h = -1;
            v = 0;
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            v = 1;
            h = 0;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            v = -1;
            h = 0;
        }
#else
        if(Input.touchCount>0)
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.touches[Input.touchCount-1].position);
            float dx = touchPos.x;
            float dy = touchPos.y;
            if(Mathf.Abs(dx)>Mathf.Abs(dy))
            {
                if (dx > 0)
                {
                    h = 1;
                    v = 0;
                }
                else
                {
                    h = -1;
                    v = 0;
                }
            }
            else
            {
                if (dy > 0)
                {
                    h = 0;
                    v = 1;
                }
                else
                {
                    h = 0;
                    v = -1;
                }
            }
        }
#endif
    }

    void Move()
	{
        if (h > 0 && dir != -Vector2.right)
        {
            dir = Vector2.right;
            spriteRenderer.sprite = sprites[0];
        }
        else if (h < 0 && dir != Vector2.right)
        {
            dir = -Vector2.right;
            spriteRenderer.sprite = sprites[1];
        }
        else if (v > 0 && dir != -Vector2.up)
        {
            dir = Vector2.up;
            spriteRenderer.sprite = sprites[2];
        }
        else if (v < 0 && dir != Vector2.up)
        {
            dir = -Vector2.up;
            spriteRenderer.sprite = sprites[3];
        }

        Vector2 pos = transform.position;
        transform.Translate(dir);
        if (ate)
        {
            GameObject g = (GameObject)Instantiate(tailPrefab, pos, Quaternion.identity);

            tail.Insert(0, g.transform);
            ate = false;
        }
        else if (tail.Count > 0)
        {
            tail.Last().position = pos;
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
        Invoke("Move", timeRate);
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Spot")
        {
            if (timeRate > 0.1f)
                timeRate -= 0.005f;
            Destroy(coll.gameObject);
            spots++;
            if (spots > bestScore)
                bestScore = spots;
            spotDisplay.text = spots.ToString();
            ate = true;
            if (spots % 10 == 0)
                GameManager.instance.SpawnBoost();
        }
        if(coll.gameObject.tag=="Tail")
        {
            GameManager.instance.EndGame();
            finalScoreDisplay.text = spots.ToString();
            if(bestScore==spots)
            {
                PlayerPrefs.SetInt("Best Score", bestScore);
                bestScoreDisplay.text = "New Record";
            }
            else
                bestScoreDisplay.text = bestScore.ToString();
        }
        if(coll.gameObject.tag=="Eraser")
        {
            Destroy(coll.gameObject);
            spots += 5;
            spotDisplay.text = spots.ToString();
            if (tail.Count>0)
            {
                Destroy(tail.Last().gameObject);
                tail.RemoveAt(tail.Count - 1);
            }
        }
        if(coll.gameObject.tag=="RedSpot")
        {
            spots += 10;
            spotDisplay.text = spots.ToString();
            Destroy(coll.gameObject);
        }
    }

    public void Reset()
    {
        while (tail.Count > 0)
        {
            Destroy(tail.Last().gameObject);
            tail.RemoveAt(tail.Count - 1);
        }
        transform.position = Vector3.zero;
        timeRate = 0.3f;
        spots = 0;
        spotDisplay.text = spots.ToString();
    }

    public void SaveScore()
    {
        if (bestScore == spots)
            PlayerPrefs.SetInt("Best Score", bestScore);
    }
}

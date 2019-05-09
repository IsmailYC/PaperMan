using UnityEngine;
using System.Collections;

public class SpawnSpot : MonoBehaviour {
    public bool spawnOnEmpty, spawnOnTime;
	public Transform topLeftCorner;
	public Transform bottomRightCorner;
	public GameObject[] spots;
	public float secondsBetweenSpawn;

	int yMin,yMax,xMin,xMax;
	// Use this for initialization
	void Start () {
		yMin = (int) bottomRightCorner.position.y;
		yMax = (int) topLeftCorner.position.y+1;
		xMin = (int) topLeftCorner.position.x;
		xMax = (int) bottomRightCorner.position.x+1;
        if (spawnOnTime && GameManager.instance.state == GameManager.States.Play)
            Invoke("Spawn", 0.0f);

	}
	void Update()
	{
		if (spawnOnEmpty && transform.childCount==0 && GameManager.instance.state==GameManager.States.Play)
			Spawn ();
	}
	void Spawn()
	{
        int index = Random.Range(0, spots.Length);
        int x = Random.Range(xMin, xMax);
        int y = Random.Range(yMin, yMax);
        GameObject spot = (GameObject)Instantiate(spots[index], new Vector2(x, y), Quaternion.identity);
        spot.transform.parent = transform;
        if (spawnOnTime && GameManager.instance.state==GameManager.States.Play)
            Invoke("Spawn", secondsBetweenSpawn);
    }

    public void TriggerSpawn()
    {
        Spawn();
    }
}

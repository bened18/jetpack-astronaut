using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour
{

    public GameObject player;
    public GameObject laser1;
    public GameObject laser2;
    public GameObject laser3;
    public GameObject laser4;
    public GameObject laserPrefab;
    
    public float minObstacleY;
    public float maxObstacleY;

    public float minObstacleSpacing;
    public float maxObstacleSpacing;

    public float minObstacleScaleY;
    public float maxObstacleScaleY;
    void Start()
    {
        laser1 = GenerateLaser(player.transform.position.x + 20f);

        laser2 = GenerateLaser(laser1.transform.position.x);

        laser3 = GenerateLaser(laser2.transform.position.x);

        laser4 = GenerateLaser(laser3.transform.position.x);
    }

    GameObject GenerateLaser(float referenceX)
    {
        GameObject laser = GameObject.Instantiate(laserPrefab);
        SetTransform(laser, referenceX);
        return laser;
    }

    void SetTransform(GameObject laser, float referenceX)
    {
        laser.transform.position = new Vector3(referenceX + Random.Range(minObstacleSpacing, maxObstacleSpacing), Random.Range(minObstacleY, maxObstacleY), 0);
        laser.transform.localScale = new Vector3(laser.transform.localScale.x, Random.Range(minObstacleScaleY, maxObstacleScaleY), laser.transform.localScale.z );
    }

    void Update()
    {
        if (player.transform.position.x > laser2.transform.position.x)
        {
            var tempLaser = laser1;
            laser1 = laser2;
            laser2 = laser3;
            laser3 = laser4;

            SetTransform(tempLaser, laser3.transform.position.x);
            laser4 = tempLaser;
        }
    }
}

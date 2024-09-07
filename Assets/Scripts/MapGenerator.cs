using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject prevCeiling;
    public GameObject prevFloor;
    public GameObject ceiling;
    public GameObject floor;
    
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > floor.transform.position.x)
        {
            var tempCeiling = prevCeiling;
            var tempFloor  = prevFloor;
            prevCeiling = ceiling;
            prevFloor = floor;
            tempCeiling.transform.position += new Vector3(80, 0, 0);
            tempFloor.transform.position += new Vector3(80, 0, 0);
            ceiling = tempCeiling;
            floor = tempFloor;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public Player player;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.gameOver)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        } 
    }
}

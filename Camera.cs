using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    GameObject cam;
    [SerializeField]
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 a = cam.transform.position;
        cam.transform.position = new Vector3(Player.transform.position.x, a.y, a.z);
    }
}

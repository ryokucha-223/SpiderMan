using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATKcol : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(this.gameObject);
    }
}

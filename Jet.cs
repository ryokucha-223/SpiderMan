using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jet : MonoBehaviour
{
    Animator anim;

    [SerializeField]
    AudioClip laugh;
    AudioSource vc;
    [Header("Å‘åˆÚ“®‹——£")] public float maxDistance = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        vc = gameObject.AddComponent<AudioSource>();
        anim = GetComponent<Animator>();
        vc.PlayOneShot(laugh);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.right * 0.5f );
        if(transform.position.x>=maxDistance)
        {
            Destroy(this.gameObject);
        }
    }
}

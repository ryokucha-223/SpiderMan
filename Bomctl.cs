using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bomctl : MonoBehaviour
{
    [Header("�X�s�[�h")] public float speed = 3.0f;
    [Header("�ő�ړ�����")] public float maxDistance = 100.0f;
    private Rigidbody2D rb;
    private Vector3 defaultPos;

   // public bool lr;
    GameObject robo;
    // Start is called before the first frame update
    void Start()
    {
      //  robo = transform.root.gameObject;
        rb = GetComponent<Rigidbody2D>();
        /* if (rb == null)
         {
             Debug.Log("�ݒ肪����܂���");
             Destroy(this.gameObject);
         }:*/
        defaultPos = transform.position;
       // lr = robo.GetComponent<SpriteRenderer>().flipX;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float d = Vector3.Distance(transform.position, defaultPos);

        //�ő�ړ������𒴂��Ă���
        if (d > maxDistance)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Goblin")
        {
            Destroy(this.gameObject);
        }
    }
}
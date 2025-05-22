using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zako : MonoBehaviour
{
    int HP,num;
    int rg = 1;
    public float pldis = 0.5f;
    public int moveSpeed = 10;
    public float atkInterval = 1.0f;
    private Rigidbody2D rb;
    public Collider2D attackCollider;
    private Animator anim;
    public GameObject PlayerObject; // playerオブジェクトを受け取る器
    public Transform Player; // プレイヤーの座標情報などを受け取る器
    bool Isdamage,Ismuteki;
    private float wait = 0f;
    [SerializeField]
    AudioClip ht;
    AudioSource bg;
    [SerializeField]
    GameObject plHitObject;
    [SerializeField]
    LayerMask Pl;
    bool canAtk = false;
   [SerializeField] GameObject PunchHit;
    private GameObject atkobj;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        bg = gameObject.AddComponent<AudioSource>();
        HP = 4;
        anim.SetBool("dead", false);
        Isdamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(wait>0)
        {
            wait -= Time.deltaTime;
        }
        else
        {
            checkpl();
            move();
            attack();
        }
        if(HP<=0)
        {
            anim.SetBool("dead", true);
            Isdamage=true;
        }
    }
    void checkpl()
    {
        canAtk=Physics2D.OverlapCircle(plHitObject.transform.position, 0.2f, Pl);
    }
    void move()
    {
        Vector2 e_pos = transform.position;  // 自分(敵キャラクタ)の座標
        Vector2 p_pos = Player.position;  // プレイヤーの座標
        Vector3 direction = new Vector3(p_pos.x - e_pos.x, 0f, 0f);
        float dir = Mathf.Abs(direction.x);
        if (direction.x > 0)
        {
            num = 1;
            transform.localScale = new Vector3(2, 2, 2);
            anim.SetFloat("speed", 10);
        }
        else
        {
            num = -1;
            transform.localScale = new Vector3(-2, 2, 2);
            anim.SetFloat("speed", 10);
        }
        // 方向ベクトルに速度を掛けて移動する
        if (Isdamage == false)
        {
            if(dir < 10)
            {
                transform.position += direction * moveSpeed * Time.deltaTime * rg;
            }
        }
    }
    void attack()
    {
        if(canAtk)
        {
            anim.SetTrigger("atk");
            rg = 0; ;
        }
        else
        {
            rg = 1;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("ImpWeb"))
        {
            HP--;
            anim.SetTrigger("damage");

            // ノックバック処理
            ApplyKnockback(col.transform.position);
        }
        else if (col.gameObject.CompareTag("P") || col.gameObject.CompareTag("K") || col.gameObject.CompareTag("RshWeb"))
        {
            anim.SetTrigger("Hit");
           

            // ノックバック処理
            ApplyKnockback(col.transform.position);
        }
        else if (col.gameObject.CompareTag("Ak") || col.gameObject.CompareTag("U"))
        {
            anim.SetTrigger("damage");
            bg.PlayOneShot(ht);

            // ノックバック処理
            ApplyKnockback(col.transform.position);
        }
    }

    void ApplyKnockback(Vector3 collisionPosition)
    {
        if (rb != null)
        {
            // ノックバックの方向を計算
            Vector2 knockbackDirection = (transform.position - collisionPosition).normalized;
            float knockbackForce = 5f; // ノックバックの強さを調整

            // ノックバックを適用
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    void Hit()
    {
        HP--;
        Isdamage = true;
        wait = 1f;
    }
    void damage()
    {
        HP -= 2;
        attackCollider.enabled = false;
        Isdamage = true;
        transform.Translate(-transform.right * 3 * num);
        wait = 1f;
    }
    void damaged()
    {
        Isdamage = false;
        attackCollider.enabled = true;
    }
    void Punch()
    {
        Vector2 slashPos = new Vector2(transform.position.x + num, transform.position.y);
        atkobj = Instantiate(PunchHit, slashPos, Quaternion.identity);
    }
    void Destroyobj()
    {
        Destroy(atkobj);
    }
    void deth()
    {
        Destroy(this.gameObject);
    }
}

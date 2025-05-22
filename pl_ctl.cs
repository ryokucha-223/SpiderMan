using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;//文字
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; // 非同期/待機のため

public class pl_ctl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private float playerSpeed;
    bool prevOnGround;
    bool OnGround;//地面判定true=地面の上
    bool muki = true;//最初から右向き
    bool isdead;
    [SerializeField]
    GameObject groundHitObject;//地面判定を調べるオブジェクト
    [SerializeField]
    LayerMask groundLayer;//地面のレイヤー
    [SerializeField] float JumpForce = 30f;
    [SerializeField] float backForce = 10f;
    [SerializeField] float speed = 3f;
    float WebWait = 0;
    int numjump;
    [SerializeField] int HP, maxHP = 10;
    [SerializeField] Slider HPbar;
    int WebMode = 1;//1でインパクト、2でラッシュ

    [SerializeField]
    TextMeshProUGUI wm;//文字
    [SerializeField] Sprite ImpRG;
    [SerializeField] Sprite RshRG;
    [SerializeField] Image WebRG;
    [SerializeField] AudioClip webshot, se_damage, se_change;
    [SerializeField] GameObject webBom, punchHit, kickHit, upperHit, AirHit;
    private GameObject atkobj;
    AudioSource snd;
    bool isKnockback = false;//ノックバック判定
    bool isDamage = false;

    private DistanceJoint2D wireJoint;//ウェブスイング用の奴
    private LineRenderer line;
    private Vector2 wireTarget;
    private bool isSwinging;

    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        snd = gameObject.AddComponent<AudioSource>();
        WebRG = GameObject.Find("WebRg").GetComponent<Image>();
        numjump = 0;
        isdead = false;
        isKnockback = false;
        isDamage = false;
        WebMode = 1;
        HP = maxHP;
        HPbar.value = 1;
        anim.SetBool("dead", false);
        anim.SetBool("RushWeb", false);
        anim.SetBool("ImpactWeb", true);
        wm.text = "IMPACTWEB";

        wireJoint = gameObject.AddComponent<DistanceJoint2D>();//ウェブスイング用の奴
        wireJoint.enabled = false;

        line = gameObject.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.white;
        line.endColor = Color.white;
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        checkGround();
        if (!isKnockback)
        {
            move(); // 通常の移動
        }
        jump();
        if (shot()) { return; };
        Atk();
        ChangeWeb();

          if (Input.GetKeyDown(KeyCode.S))
    {
        swing();
    }
        if (isSwinging && Input.GetKeyDown(KeyCode.Z))
        {
            wireJoint.enabled = false;
            isSwinging = false;
            rb.velocity = new Vector2(rb.velocity.x, JumpForce); // スイング解除
            anim.SetBool("isSwinging", false);
        }

        if (HP <= 0)
        {
            anim.SetBool("dead", true);
        }//死亡処理
    }
    void checkGround()
    {
        prevOnGround = OnGround;
        //地面判定（足元のオブジェクト取得）0.1は補正値
        OnGround = Physics2D.OverlapCircle(groundHitObject.transform.position, 0.2f, groundLayer);
    }
    void move()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerSpeed = -speed;
            transform.localScale = new Vector3(-2, 2, 2);
            muki = false;
        }
        // 右キーを押したら右方向へ進む
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            playerSpeed = speed;
            transform.localScale = new Vector3(2, 2, 2);
            muki = true;
        }
        // 何もおさなかったら止まる
        else
        {
            playerSpeed = 0;
        }
        anim.SetFloat("Speed", Mathf.Abs(playerSpeed * speed));
        Vector2 velocity = rb.velocity;
        velocity.x = playerSpeed;
        rb.velocity = velocity;
    }
    void jump()
    {
        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (OnGround)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + JumpForce);
                numjump++;
                anim.SetBool("issJump", true);
            }
            if (!OnGround)
            {
                swing();
            }
        }
        if (!OnGround)
        {
            anim.SetBool("isFoll", true);
        }
        else if (!prevOnGround && OnGround)//前回空中今回地面の時
        {
            anim.SetBool("issJump", false);
            anim.SetBool("isFoll", false);
        }
    }
    void swing()
    {
        if (isSwinging) return;

        Vector2 fireDir = muki ? new Vector2(1, 1).normalized : new Vector2(-1, 1).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, fireDir, 10f, groundLayer);

        if (hit.collider != null)
        {
            wireTarget = hit.point;
        }
        else
        {
            wireTarget = (Vector2)transform.position + fireDir * 8f;
        }

        wireJoint.enabled = true;
        wireJoint.connectedAnchor = wireTarget;
        wireJoint.autoConfigureDistance = false;

        // ワイヤー距離短めにして速く移動
        wireJoint.distance = Vector2.Distance(transform.position, wireTarget) * 0.8f;

        isSwinging = true;
        anim.SetBool("isSwinging", true);
    }


    bool shot()
    {
        if (WebWait > 0)
        {
            WebWait -= Time.deltaTime;
            return true;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetTrigger("webshot");
            //web();
        }
        return false;
    }
    async void Atk() // 非同期に変更
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            anim.SetTrigger("atk");
            await Task.Delay(500); // 0.5秒待機
        }
    }
    void ChangeWeb()
    {
        if (Input.GetKeyDown(KeyCode.A))//ラッシュ
        {
            snd.PlayOneShot(se_change);
            if (WebMode == 1)
            {
                WebMode = 2;
                anim.SetBool("RushWeb", true);
                anim.SetBool("ImpactWeb", false);
                wm.text = "RUSHWEB";
                WebRG.sprite = RshRG;
            }
            else//インパクト
            {
                WebMode = 1;
                anim.SetBool("RushWeb", false);
                anim.SetBool("ImpactWeb", true);
                wm.text = "IMPACTWEB";
                WebRG.sprite = ImpRG;
            }
        }
    }
    void web()
    {
        float rgi = muki ? 1 : -1;
        Vector3 c = transform.position;
        c.x += 0.8f * rgi;
        c.y += 0.2f;
        if (WebMode == 1)
        {
            webBom = (GameObject)Resources.Load("WebBom");
        }
        else if (WebMode == 2)
        {
            webBom = (GameObject)Resources.Load("RushWebBom");
        }
        GameObject a = Instantiate(webBom, c, Quaternion.identity);
        Rigidbody2D b = a.GetComponent<Rigidbody2D>();
        snd.PlayOneShot(webshot);
        b.AddForce(new Vector3(1000 * rgi, 0, 0));
        if (WebMode == 1)
        {
            WebWait = 0.5f;
        }
        else
        {
            WebWait = 0.2f;
        }
        Debug.Log(rgi);
    }
    void Punch()
    {
        float kjl = muki ? 1 : -1;
        Vector2 slashPos = new Vector2(transform.position.x + kjl, transform.position.y);
        atkobj = Instantiate(punchHit, slashPos, Quaternion.identity);
    }
    void Upper()
    {
        float kjl = muki ? 1 : -1;
        Vector2 slashPos = new Vector2(transform.position.x + kjl, transform.position.y + 0.5f);
        atkobj = Instantiate(upperHit, slashPos, Quaternion.identity);
    }
    void AirAtk()
    {
        float kjl = muki ? 1 : -1;
        Vector2 slashPos = new Vector2(transform.position.x + kjl, transform.position.y - 0.5f);
        atkobj = Instantiate(AirHit, slashPos, Quaternion.identity);
    }
    void Kick()
    {
        float kjl = muki ? 1 : -1;
        Vector2 slashPos = new Vector2(transform.position.x + kjl, transform.position.y - 0.5f);
        atkobj = Instantiate(kickHit, slashPos, Quaternion.identity);
    }
    void Destroyobj()
    {
        if (atkobj != null)
        {
            Destroy(atkobj);
        }
    }

    void LateUpdate()
    {
        if (isSwinging)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, wireTarget);
        }
        else
        {
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);
        }
    }//ウェブスイング中の動き


    void dead()
    {
        isdead = true;
        SceneManager.LoadScene("GameOver");
    }

    public void EndDamage()
    {
        isDamage = false;
        isKnockback = false; // ノックバック解除
    }
    //アニメーションおわりに呼び出し

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goblin" && !isDamage)
        {
            anim.SetTrigger("damage");
            snd.PlayOneShot(se_damage);
            HP--;
            HPbar.value = (float)HP / (float)maxHP;

            // ノックバック処理を呼び出す
            Vector2 hitDirection = (transform.position - collision.transform.position).normalized;
            StartCoroutine(DoKnockback(hitDirection));
        }
    }


    IEnumerator DoKnockback(Vector2 dir)
    {
        isKnockback = true;
        isDamage = true;
        rb.velocity = Vector2.zero;

        float yForce = 0.3f; // または 0f など調整
        rb.AddForce(new Vector2(dir.x * backForce, yForce), ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.3f);

        // ノックバックとダメージ終了はアニメーション側で管理
    }




}

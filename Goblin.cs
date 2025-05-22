using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;//����
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; // �񓯊�/�ҋ@�̂���

public class Goblin : MonoBehaviour
{
    private Vector2 pos;
    private Rigidbody2D rb;
    Vector2 Rpos;
    public int num = 1,sped=1;
    public float stpdir = 2f;
    private Animator anim;
    public GameObject Bom; // ��ѓ���̃v���n�u
    public float shootingInterval = 1.0f; // �U���Ԋu
    public float forceAmount = 50f;
    private float shootingTimer = 0.0f;
    bool muki,isdead;
    [SerializeField] int maxHP = 10;
    int HP = 10;
    [SerializeField]
    TextMeshProUGUI HPtxt;
    [SerializeField] Slider HPbar;
    [SerializeField]
    AudioClip laugh,buy;
    AudioSource vc;

    public bool isTakingDamage = false;
    public Collider2D attackCollider; // �U�������Collider

    public GameObject PlayerObject; // player�I�u�W�F�N�g���󂯎���
    public Transform Player; // �v���C���[�̍��W���Ȃǂ��󂯎���
    private void Start()
    {
        vc= gameObject.AddComponent<AudioSource>();
        anim = GetComponent<Animator>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        HP = maxHP;
        HPbar.value = 1;
        anim.SetBool("dead", false);
        HPtxt.enabled = true;
        vc.PlayOneShot(laugh);
        isdead=false;
    }
    void Update()
    {
        move();
        shootingTimer -= Time.deltaTime;

        if (shootingTimer <= 0)
        {
            anim.SetTrigger("Shot");
            shootingTimer = shootingInterval;
        }
        HPtxt.text = "GreenGoblin:" + HP;
        if(HP<=0)
        {
            if (isdead == false)
            {
                deth();
                isdead = true;
            }
            transform.Translate(-transform.right  * 0.9f * num);
        }
    }
    private void move()
    {
        pos = transform.position;
        Rpos = transform.localScale;
        Vector2 p_pos = Player.position;
        Vector3 dir = new Vector3(p_pos.x - pos.x, 0f, 0f);
        // �i�|�C���g�j�}�C�i�X�������邱�Ƃŋt�����Ɉړ�����B
        float dTP = Mathf.Abs(pos.x - p_pos.x);
        if (dir.x <= 0)
        {
            num = -1;
            transform.localScale = new Vector3(-2, 2, 2);
            muki = false;
        }
        if (dir.x > 1)
        {
            num = 1;
            transform.localScale = new Vector3(2, 2, 2);
            muki = true;
        }
        if (dTP > stpdir)
        {
            transform.position += dir * sped * Time.deltaTime;
        }
    }
    void Shoot()
    {
        float rgi = 0;
        Vector3 c = transform.position;
        if (muki == true)
        {
            rgi = 1;
        }
        else if (muki == false)
        {
            rgi = -1;
        }
        c.x += 1f * rgi;
        c.y += 0.8f;
        GameObject a= Instantiate(Bom, c, Quaternion.identity);
        Rigidbody2D b = a.GetComponent<Rigidbody2D>();
        b.AddForce(new Vector3(1000 * rgi, -900, 0));
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ImpWeb")
        {
            HP--;
            HPbar.value = (float)HP / (float)maxHP;
        }
        if (col.gameObject.tag == "Ak")
        {
            HP-=2;
            HPbar.value = (float)HP / (float)maxHP;
            attackCollider.enabled = false;
            anim.SetTrigger("damage");
           // transform.Translate(-transform.right * 5 * num);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "ImpWeb")
        {
            HP--;
            HPbar.value = (float)HP / (float)maxHP;
        }
        if (other.gameObject.tag == "Ak")
        {
            HP -= 2;
            HPbar.value = (float)HP / (float)maxHP;
            attackCollider.enabled = false;
            anim.SetTrigger("damage");
            // transform.Translate(-transform.right * 5 * num);
        }
        if (other.CompareTag("MainCamera")) // �J�����Ƃ̏Փ˂����m
        {
            HPtxt.enabled = false;
        }
    }
    void damaged()
    {
        attackCollider.enabled = true;
    }
    void deth()
    {
        StartCoroutine(end());
    }
    IEnumerator end()//se���Ȃ�I���܂őҋ@
    {
        //���y��炷
        vc.PlayOneShot(buy);
        //�I���܂őҋ@
        yield return new WaitWhile(() => vc.isPlaying);
        SceneManager.LoadScene("Clear");
    }
}
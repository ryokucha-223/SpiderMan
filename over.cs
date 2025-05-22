using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class over : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI yes;
    [SerializeField]
    TextMeshProUGUI no;
    int state = 0;
    [SerializeField]
    AudioClip se_sel;
    [SerializeField]
    AudioClip se_start;
    [SerializeField]
    AudioClip se_end;

    AudioSource snd;
    // Start is called before the first frame update
    void Start()
    {
        snd = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            yes.color = new Color(0.9528302f, 0.03056241f, 0.03056241f, 1f);
            no.color = new Color(255f, 255f, 255f, 1f);
            state = 1;
            snd.PlayOneShot(se_sel);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            no.color = new Color(0.9528302f, 0.03056241f, 0.03056241f, 1f);
            yes.color = new Color(255f, 255f, 255f, 1f);
            state = 2;
            snd.PlayOneShot(se_sel);
        }
        if (Input.GetKeyDown(KeyCode.Z) && !snd.isPlaying)
        {
            if (state == 1)
            {
                StartCoroutine(str());
            }
            else if (state == 2)
            {
                StartCoroutine(end());
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    IEnumerator str()//seがなり終わるまで待機
    {
        //音楽を鳴らす
        snd.PlayOneShot(se_start);
        //終了まで待機
        yield return new WaitWhile(() => snd.isPlaying);
        SceneManager.LoadScene("Game");
    }
    IEnumerator end()//seがなり終わるまで待機
    {
        //音楽を鳴らす
        snd.PlayOneShot(se_end);
        //終了まで待機
        yield return new WaitWhile(() => snd.isPlaying);
        SceneManager.LoadScene("TitleScene");
    }
}

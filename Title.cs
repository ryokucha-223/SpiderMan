using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class Title : MonoBehaviour
{
    [SerializeField]
    AudioClip se_start;
    AudioSource snd;
    // Start is called before the first frame update
    void Start()
    {
        snd = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(str());
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    IEnumerator str()//se���Ȃ�I���܂őҋ@
    {
        //���y��炷
        snd.PlayOneShot(se_start);
        //�I���܂őҋ@
        yield return new WaitWhile(() => snd.isPlaying);
        SceneManager.LoadScene("Game");
    }
}
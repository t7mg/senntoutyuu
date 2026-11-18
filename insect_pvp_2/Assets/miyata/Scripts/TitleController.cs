using UnityEngine;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public AudioClip StartTitl;
    public AudioClip title;
    public AudioSource play_SE;

    void Start()
    {
        play_SE = GetComponent<AudioSource>();
    }

    public void onClickStertButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void StartTitle()
    {
        play_SE.PlayOneShot(title);
    }
}

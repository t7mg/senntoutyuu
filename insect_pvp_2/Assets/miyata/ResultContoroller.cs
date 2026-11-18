using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultContoroller : MonoBehaviour
{
    public AudioClip replay;
    public AudioClip title;

    public AudioSource play_SE;
    //public AudioSource not_choice;

    void Start()
    {
        play_SE = GetComponent<AudioSource>();
        
    }
    public void Retry()
    {
        Debug.Log("Ç‡Ç§àÍìxëŒêÌÇ∑ÇÈ");
        play_SE.PlayOneShot(replay);
    }

    public void ReturnTitle()
    {
       // SceneManager.LoadScene("Title");
        play_SE.PlayOneShot(title);
        
    }
}

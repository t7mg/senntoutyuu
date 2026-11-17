using UnityEngine;

public class atkctrlbox2P_hati : MonoBehaviour
{
    public GameObject atkbox2P_hati;
    public GameObject getattackbox_Normal;
    public GameObject getattackbox_Strong;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        atkbox2P_hati = GameObject.Find("hati - 2P");
        getattackbox_Normal = GameObject.Find("attack_box_2P_hati");
        getattackbox_Strong = GameObject.Find("strongattack_box_2P_hati");
    }

    //Update is called once per frame
    void Update()
    {
        if (atkbox2P_hati.GetComponent<Player2PController_hati>().strong == true)
        {
            getattackbox_Normal.SetActive(false);
            getattackbox_Strong.SetActive(true);
        }
        else if (atkbox2P_hati.GetComponent<Player2PController_hati>().strong == false)
        {
            getattackbox_Normal.SetActive(true);
            getattackbox_Strong.SetActive(false);
        }
    }
}

using UnityEngine;

public class atkctrlbox2P_batta : MonoBehaviour
{
    public GameObject atkbox2P_batta;
    public GameObject getattackbox_Normal;
    public GameObject getattackbox_Strong;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        atkbox2P_batta = GameObject.Find("batta - 2P");
        getattackbox_Normal = GameObject.Find("attack_box_2P_batta");
        getattackbox_Strong = GameObject.Find("strongattack_box_2P_batta");
    }

    // Update is called once per frame
    void Update()
    {
        if (atkbox2P_batta.GetComponent<Player2PController_batta>().strong == true)
        {
            getattackbox_Normal.SetActive(false);
            getattackbox_Strong.SetActive(true);
        }
        else if (atkbox2P_batta.GetComponent<Player2PController_batta>().strong == false)
        {
            getattackbox_Normal.SetActive(true);
            getattackbox_Strong.SetActive(false);
        }
    }
}

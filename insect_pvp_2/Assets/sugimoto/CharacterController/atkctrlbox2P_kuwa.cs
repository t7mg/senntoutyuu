using UnityEngine;

public class atkctrlbox2P_kuwa : MonoBehaviour
{
    public GameObject atkbox2P_kuwa;
    public GameObject getattackbox_Normal;
    //public GameObject getattackbox_Strong;
    public GameObject getattackbox_airN;

    void Start()
    {
        atkbox2P_kuwa = GameObject.Find("kuwagata - 2P");
        getattackbox_Normal = GameObject.Find("attack_box_2P_kuwagata");
        //getattackbox_Strong = GameObject.Find("strongattack_box_2P_kuwa");
        getattackbox_airN = GameObject.Find("airNattack_box_2P_kuwa");
    }

    // Update is called once per frame
    void Update()
    {
        if (atkbox2P_kuwa.GetComponent<Player2PController_kuwagata>().aerial_attack == false)
        {
            getattackbox_Normal.SetActive(true);
            //getattackbox_Strong.SetActive(true);
            getattackbox_airN.SetActive(false);
        }
        //else if (atkbox1P_kuwa.GetComponent<Player1PController_kuwagata>().aerial_attack == false)
        //{
        //    getattackbox_Normal.SetActive(true);
        //    //getattackbox_Strong.SetActive(false);
        //    getattackbox_airN.SetActive(false);
        //}
        else if (atkbox2P_kuwa.GetComponent<Player2PController_kuwagata>().aerial_attack == true)
        {
            getattackbox_Normal.SetActive(false);
            //getattackbox_Strong.SetActive(false);
            getattackbox_airN.SetActive(true);
        }
    }
}


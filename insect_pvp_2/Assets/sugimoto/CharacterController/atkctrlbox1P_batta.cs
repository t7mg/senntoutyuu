using UnityEngine;

public class atkctrlbox1P_batta : MonoBehaviour
{
    public GameObject atkbox1P_batta;
    public GameObject getattackbox_Normal;
    public GameObject getattackbox_Strong;

    void Start()
    {
        atkbox1P_batta = GameObject.Find("batta - 1P");
        getattackbox_Normal = GameObject.Find("attack_box_1P_batta");
        getattackbox_Strong = GameObject.Find("strongattack_box_1P_batta");
    }

    // Update is called once per frame
    void Update()
    {
        if (atkbox1P_batta.GetComponent<Player1PController_batta>().strong == true)
        {
            getattackbox_Normal.SetActive(false);
            getattackbox_Strong.SetActive(true);
        }
        else if (atkbox1P_batta.GetComponent<Player1PController_batta>().strong == false)
        {
            getattackbox_Normal.SetActive(true);
            getattackbox_Strong.SetActive(false);
        }
    }
}

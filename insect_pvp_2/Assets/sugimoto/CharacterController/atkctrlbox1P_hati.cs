using UnityEngine;

public class atkctrlbox1P_hati : MonoBehaviour
{
    public GameObject atkbox1P_hati;
    public GameObject getattackbox_Normal;
    public GameObject getattackbox_Strong;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        atkbox1P_hati = GameObject.Find("hati - 1P");
        getattackbox_Normal = GameObject.Find("attack_box_1P");
        getattackbox_Strong = GameObject.Find("strongattack_box_1P_kabuto");
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (atkbox1P_kabuto.GetComponent<PlayerController>().strong == true)
    //    {
    //        getattackbox_Normal.SetActive(false);
    //        getattackbox_Strong.SetActive(true);
    //    }
    //    else if (atkbox1P_kabuto.GetComponent<PlayerController>().strong == false)
    //    {
    //        getattackbox_Normal.SetActive(true);
    //        getattackbox_Strong.SetActive(false);
    //    }
    //}
}
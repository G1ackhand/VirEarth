using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chargerDialRotate : MonoBehaviour
{
    public GameObject back;
    public GameObject front;
    public GameObject particle;
    public GameObject vaccine;

    // Start is called before the first frame update
    void Start()
    {
        back.SetActive(false);
        front.SetActive(false);
        particle.SetActive(false);
        vaccine.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Dial.instance.clearChargerInst)    // Ŭ����
        {
            back.SetActive(false);
            front.SetActive(false);
            particle.SetActive(true);
            vaccine.SetActive(true);
        }
        else                            // ���̾� �̺�Ʈ ���� ��
        {
            back.SetActive(true);
            front.SetActive(true);
            particle.SetActive(false);
            vaccine.SetActive(false);
            front.transform.localEulerAngles = new Vector3(0, 0, Dial.instance.degree);
        }
    }
}

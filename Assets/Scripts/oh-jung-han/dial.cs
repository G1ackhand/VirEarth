using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dial : MonoBehaviour
{
    public static Dial instance; // �ν��Ͻ�

    [SerializeField] private Image dialimg;
    [SerializeField] private Image dialimg_back;
    [SerializeField] private Text dialtext;
    [SerializeField] private Text thumbtext;
    private int degree = 0;
    private int goal;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        dialimg.enabled = false;
        dialimg_back.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //DialCheck();
    }

    public bool DialCheck()
    {
        if (HandTracking.instance.IsFoldFinger(false, false, true, true, true))
        {
            dialimg.enabled = true;
            dialimg_back.enabled = true;
            //thumbtext.text = "finger on";
            //return false;
        }
        else
        {
            dialimg.enabled = false;
            dialimg_back.enabled = false;
            return false;
        }
        
        if (((int)ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_class) != 2)
        {
            return false;
        }
        
        //����
        float x1 = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[4].x;
        float y1 = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[4].y;
        //����
        float x2 = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[8].x;
        float y2 = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[8].y;


        // ������ ȭ�� �߾ӿ� ��ġ
        if (ThumbCenter(x1,y1))
        {
            //���̾� ����
            if(DialTurn(x2, y2) == 0) //�ݽð� ȸ��
            {
                degree += 30;
                dialimg.transform.localEulerAngles = new Vector3(0,0,degree);
                dialtext.text = "CCW";
            }
            else if(DialTurn(x2, y2) == 1) //ȸ��X
            {
                //dialimg.transform.localEulerAngles = new Vector3(0, 0, 0);
                dialtext.text = "NONE";
            }
            else if(DialTurn(x2, y2) == 2) //�ð� ȸ��
            {
                degree -= 30;
                dialimg.transform.localEulerAngles = new Vector3(0, 0, degree);
                dialtext.text = "CW";
            }
            else
            {
                dialtext.text = "EEE";
            }
            
        }
        return false; //@@@@@@
    }

    private bool ThumbCenter(float x, float y)
    {
        if (x < 0.65 && x > 0.35 && y < 0.6 && y > 0.4)
        {
            thumbtext.text = "IN";
            return true;
        }
        else
        {
            thumbtext.text = "OUT";
            return false;
        }
    }

    private int DialTurn(float x, float y)
    {
        if (x < 0.45)
        {
            return 0; //�ݽð� ���� ȸ��
        }
        else if (x >= 0.45 && x < 0.55)
        {
            return 1; //����
        }
        else if (x >= 0.55)
        {
            return 2; //�ð� ���� ȸ��
        }
        else return -1;
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1.0f);
    }
}

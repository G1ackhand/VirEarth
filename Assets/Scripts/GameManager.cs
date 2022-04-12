using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ������Ʈ
    [SerializeField] private GameObject ARObject;
    [SerializeField] private GameObject HPObject;
    [SerializeField] private GameObject PatternObject;
    [SerializeField] private GameObject GPSObject;
    [SerializeField] private GameObject ARPortalObject;

    // flag
    private bool flag_pattern;
    private bool flag_hp;
    private bool flag_startGame;
    private bool flag_arPortal;

    // �ν�����
    [SerializeField] private Text debug;

    private void Awake()
    {
        // ������Ʈ
        HPObject.SetActive(false);
        PatternObject.SetActive(false);
        //GPSObject.SetActive(false);

        // flag
        flag_pattern = false;
        flag_hp = false;
        flag_startGame = false;
        flag_arPortal = false;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

        if (ARObject.GetComponent<ARTrackedMultiImageManager>().imageTrackedText.text == "charger")
        {
            if (!flag_startGame)
            {
                flag_startGame = GameStartEffect.instance.gameStartEffect();
                //ARNavigator.instance.ARNavigatorEvent(); //@@@@ ��ġ ���� �ʿ�
            }
            ARNavigator.instance.ARNavigatorEvent(); //@@@@ ��ġ ���� �ʿ�

        }
        if (ARObject.GetComponent<ARTrackedMultiImageManager>().imageTrackedText.text == "board")
        {
            // ������ �繰������ ����! (ar navigate)
        }
        if (ARObject.GetComponent<ARTrackedMultiImageManager>().imageTrackedText.text == "locker")
        {
            /*if (!�÷���)
            {
                // ����Ʈ ����(?)
                HPObject.SetActive(true);
                if (ARNavigator.instance.SpawnLimit != 1)
                    ARNavigator.instance.Limit_init();
            }*/
                       
            //ARNavigator.instance.ARNavigatorEvent();

            //GPSObject.SetActive(true);    // GPS ON!
        }
        
        if (GPSObject.GetComponent<gps>().isInB == true)
                {
            if (!flag_hp)
            {
                HPObject.SetActive(false); //@@@@ true�� ���� �ʿ�
            }
        }
        else
        {
            if (flag_hp)
            {
                HPObject.SetActive(false);
            }
        }
        if (ARObject.GetComponent<ARTrackedMultiImageManager>().imageTrackedText.text == "fireplug")
        {
            if (!flag_pattern)
            {
                PatternObject.SetActive(true);
                InventoryManager.instance.set_inventoryManagement_enable(false);
                flag_pattern = Pattern.instance.PatternCheck();
                if (flag_pattern)
                {
                    // Ű ī�� Ȯ��
                    InventoryManager.instance.set_inventoryManagement_enable(true);
                    GameStartEffect.instance.gameStartEffect(); // test / ���� ���� ����Ʈ�� ����
                    PatternObject.SetActive(false);
                }

                if (ARNavigator.instance.SpawnLimit != 1)
                    ARNavigator.instance.Limit_init();                
            }
            ARNavigator.instance.ARNavigatorEvent();
        }
        if (ARObject.GetComponent<ARTrackedMultiImageManager>().imageTrackedText.text == "exitdoor")
        {
            if (!flag_arPortal)
            {
                ARPortalObject.SetActive(true);
                flag_arPortal = true;
            }
        }

        InventoryManager.instance.InventoryManagement(); // �κ��丮, inventoryManagement_enable�� false�� �۵�����


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARHintText : MonoBehaviour
{
    TextMesh text_mesh;

    void Start()
    {
        text_mesh = GetComponent<TextMesh>();
        test test = GameObject.Find("AR Session Origin").GetComponent<test>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (test.EventCount)
        {
            case 1:
                text_mesh.text = "A�� 1�� �޴��� ���� �������� �ν��ϸ� ����";
                break;

            case 2:
                text_mesh.text = "A�� ���������� ���� ���� �ȳ��ǿ� �ܼ��� �ִ�.";
                break;

            case 3:
                text_mesh.text = "4�� �繰��, 17�й� ������";
                break;

            case 4:
                text_mesh.text = "����� 502��";
                break;
        }

    }
}

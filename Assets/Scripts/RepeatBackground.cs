using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    //���� ������ ����� ��� ������������ ������� ����, ���� �� �� ��������

    private Vector3 startPos; //��������� ������� �������, � �������� ��������� ���� ������ (�� ��������� � ���� �����������)
    private float repeatWidth; //������ ��� ������� ��������� ����

    void Start()
    {
        startPos = transform.position; //���������� ��������� ������� ������� ����� ��� �������, ������� � ���������� �������
        repeatWidth = GetComponent<BoxCollider>().size.x / 2; //����� ������� ������ ����������� ���������� ������ ������� �� �����������, ������� �� 2 (size.x / 2) � ���������� BoxCollider (GetComponent<BoxCollider>())
    }

    void Update()
    {
        if (transform.position.x < startPos.x - repeatWidth) //���� ������� �� ��� � � ������� ������, ��� ��������� ������� ������� ����� repeatWidth (��������� startPos = 0, �� ���� ������ -repeatWidth), ��:
                                                             //(������ ������, ������ ��� ��� �������� ����� �������� �������� �� �������������)
        {
            transform.position = startPos; //������� ������� ������������ �� ���������
        }
    }
}

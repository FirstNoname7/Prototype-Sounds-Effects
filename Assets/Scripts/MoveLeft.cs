using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    //���� ������ ���������� ��������� ����� (����������� � ����) ��� �������� ������� ��������

    private float speed = 10.0f; //�������� �����������
    private PlayerController playerControllerScript; //�������� ������ � ������ PlayerController
    private float leftBound = -20.0f; //��������� �����������, ��� ������� �� ��� �� ����� � ����� (������ ����� ��� �������)

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>(); //��������� ����� ������� ������ � �������� ������� � ������ Player � ������� � ������ PlayerController
    }

    void Update()
    {
        if (playerControllerScript.gameOver == false) //���� ���������� � ������ gameOver �� ������� PlayerController = �� ������, ������ ���� �� �������� � ����������� ���������:
        {
            if (playerControllerScript.doubleSpeed) //���� � ������� playerControllerScript doubleSpeed = true, ������ ������ � ����� ��������� �������� ����, �� ����:
            {
                transform.Translate(Vector3.left * Time.deltaTime * (speed*2)); //����������� (transform.Translate). ���������� ����������, ��� �������� � ������� Update.
                                                                            //����������� �������� - ����� (Vector3.left). ����� ��� ���� ���������� ��� ���� ��������� ����� Time.deltaTime � �������� �� ��������� �������� (speed*2), � �� ������ �� ���������

            }
            else
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed); //����������� (transform.Translate). ���������� ����������, ��� �������� � ������� Update.
                                                                            //����������� �������� - ����� (Vector3.left). ����� ��� ���� ���������� ��� ���� ��������� ����� Time.deltaTime � �������� �� �������� (speed), � �� ������ �� ���������
            }

        }

        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle")) //���� ������� �������, � �������� ��������� ������, �� ��� � (�� �����������) ������ ��������� �����������, ��� ������� ��� ��� �� ����� � �����,
                                                                                   //� ��� � ����� ������� � ���������� ������ ��� Obstacle (������ ���� �������� �������� �����������), ��:
        {
            Destroy(gameObject); //������ ������ ��������� (����������� ���������)
        }

    }
}

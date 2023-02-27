using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float score; //����
    public Transform startingPoint; //��� ����������� ������� ����� ������, ����� �� ��� �������� ������
    public float lerpSpeed; //�������� ����� � ������ ���� (����� �� ��� �� �����, � ��� �� �����)
    private PlayerController playerControllerScript; //�������� ������ � ������ PlayerController

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>(); //��������� ����� ������� ������ � �������� ������� � ������ Player � ������� � ������ PlayerController
        score = 0; //��� ������ ���-�� ����� = 0

        playerControllerScript.gameOver = true; //��� ������ ���� ����������� (����� ��� ����, ����� ��� �� ��������)
        StartCoroutine(PlayIntro()); //��������� �� ��, ��� ������� � IEnumerator (������������������ ��������) � ��������� PlayIntro
    }

    void Update()
    {
        if (!playerControllerScript.gameOver) //���� ���� �� �����������, ��:
        {
            if (playerControllerScript.doubleSpeed) //���� � ����� ������ ������ ��������� ��������, ��:
            {
                score += 2; //���������� 2 ����, �� ����
            }
            else //� �������� ������ (���� � ����� ������� ��������):
            { 
                score++; //���������� 1 ����
            }
            Debug.Log("����:" + score); //�� ������� ����� ���������� ���-�� �����, ������� ��������� ���� ������
        }
    }

    IEnumerator PlayIntro() //������������������ �������� ��� ������ ����
    {
        Vector3 startPos = playerControllerScript.transform.position; //�������� ������� ����� ������ ��� ������ ����� ������ PlayerController
        Vector3 endPos = startingPoint.position; //�������� ������� ����� ������ ����� startingPoint, ���� ������������� �����, ����� �� ���� ���, �����������, � �������� ����
        float jorneyLength = Vector3.Distance(startPos, endPos); //��������� ��������� �� �������, ����� ���� ���� ���, �� �������, ����� ���������� ������� ������� (��������� �����)
        float startTime = Time.time; //�����, ���������� �� ������������ �������� (�����, ����� ���� ���� ���) �� ������ �������� ���� (������ ����)

        float distanceCovered = (Time.time - startTime) * lerpSpeed; //��� ����������� ���������� �� ������ ����. (Time.time - startTime) - �� ������������ ������� ���������� �����, ���������� �� ������������ ��������.
                                                                     //���������� ����� ���������� �� �������� ����� � ������ ���� (lerpSpeed). ����� ������� ��� ������ ���� ������, ��� ������ ��������, � ��� ������ ��������� �� ����� �� ������ ����.
        float fractionOfJorney = distanceCovered / jorneyLength; //���� ���������� �� ����� �����������. ���� �� ������ ����. ����� ��������� ������� �� ������� �� ����� �� ������ ����.

        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_Multiplier", 0.5f); //����� ������ PlayerController ���������� ��������� ��������� (Animator) � ������������� �������� �������� �� 50% ���� ����������� (0.5f = 50%)

        while (fractionOfJorney < 1) //����. ����� �����������, ���� ���������� �� ������ ���� ������ 1 (�� ���� ���� ��� �� ����� �� ������ ����, ��� ����� ���)
        {
            distanceCovered = (Time.time - startTime) * lerpSpeed; //��� ����������� ���������� �� ������ ����. (Time.time - startTime) - �� ������������ ������� ���������� �����, ���������� �� ������������ ��������.
                                                                   //���������� ����� ���������� �� �������� ����� � ������ ���� (lerpSpeed). ����� ������� ��� ������ ���� ������, ��� ������ ��������, � ��� ������ ��������� �� ����� �� ������ ����.

            fractionOfJorney = distanceCovered / jorneyLength; //���� ���������� �� ����� �����������. ���� �� ������ ����. ����� ��������� ������� �� ������� �� ����� �� ������ ����.

            playerControllerScript.transform.position = Vector3.Lerp(startPos, endPos, fractionOfJorney); //������� ����� ������. ������� = ���� ���������� (fractionOfJorney) ����� ��������� startPos � endPos
            yield return null; //�������� �����, ���� ������� ��������� ������ �� ������ �������� ��������
        }
        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_Multiplier", 1.0f); //����� ������ PlayerController ���������� ��������� ��������� (Animator) � ���������� �������� �������� �� 100% (�� ���� �����������, ��� 1.0f = 100%)
        playerControllerScript.gameOver = false; //�������� ����.

    }
}

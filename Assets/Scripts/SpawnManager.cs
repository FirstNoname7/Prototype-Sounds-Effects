using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefab; //������ ��� ������

    private float startDelay = 2; //����� ������� ������ �������� ��������� �������
    private float spawnInterval = 5; //��������, � ������� ������� ������ ���������� (����� ������� ������)
    private Vector3 spawnPos = new Vector3(25, 0, 0); //����� ������ (�� ��������� ������ ������)
    private PlayerController playerControllerScript; //�������� ������ � ������ PlayerController

    void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, spawnInterval); //����� InvokeRepeating: ����� startDelay ������ ���������� ������������ ������� � ������ SpawnObstacle � ���������� � spawnInterval �������
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>(); //��������� ����� ������� ������ � �������� ������� � ������ Player � ������� � ������ PlayerController
    }


    void SpawnObstacle()
    {
        int obstacleIndex = Random.Range(0,obstaclePrefab.Length);
        if (playerControllerScript.gameOver == false) //���� ���������� � ������ gameOver �� ������� PlayerController = �� ������, ������ ���� �� �������� � ����������� ���������:
        {
            Instantiate(obstaclePrefab[obstacleIndex], spawnPos, obstaclePrefab[obstacleIndex].transform.rotation); //��������� ������ obstaclePrefab � �������� spawnPos (���� ���� � ������� ���������) � ��������� obstaclePrefab.transform.rotation (������� � ����������)
        }
    }
}

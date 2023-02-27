using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefab; //объект дл€ спауна

    private float startDelay = 2; //через сколько секунд начнутс€ спаунитс€ объекты
    private float spawnInterval = 5; //интервал, с которым объекты спауна по€вл€ютс€ (через сколько секунд)
    private Vector3 spawnPos = new Vector3(25, 0, 0); //место спауна (за пределами экрана справа)
    private PlayerController playerControllerScript; //вызываем скрипт с именем PlayerController

    void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, spawnInterval); //спаун InvokeRepeating: через startDelay секунд непрерывно используетс€ функци€ с именем SpawnObstacle с интервалом в spawnInterval секунды
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>(); //разрешаем этому скрипту доступ к игровому объекту с именем Player и скрипту с именем PlayerController
    }


    void SpawnObstacle()
    {
        int obstacleIndex = Random.Range(0,obstaclePrefab.Length);
        if (playerControllerScript.gameOver == false) //если переменна€ с именем gameOver из скрипта PlayerController = не истина, значит игра не окончена и выполн€етс€ следующее:
        {
            Instantiate(obstaclePrefab[obstacleIndex], spawnPos, obstaclePrefab[obstacleIndex].transform.rotation); //спаунитс€ объект obstaclePrefab с позицией spawnPos (чуть выше в скрипте прописана) и поворотом obstaclePrefab.transform.rotation (который в инспекторе)
        }
    }
}

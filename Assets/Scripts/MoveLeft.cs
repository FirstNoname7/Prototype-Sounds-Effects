using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    //этот скрипт показывает прокрутку влево (препятствий и фона) для создания эффекта движения

    private float speed = 10.0f; //скорость перемещения
    private PlayerController playerControllerScript; //вызываем скрипт с именем PlayerController
    private float leftBound = -20.0f; //положение препятствия, при котором он уже не виден в сцене (значит можно его удалить)

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>(); //разрешаем этому скрипту доступ к игровому объекту с именем Player и скрипту с именем PlayerController
    }

    void Update()
    {
        if (playerControllerScript.gameOver == false) //если переменная с именем gameOver из скрипта PlayerController = не истина, значит игра не окончена и выполняется следующее:
        {
            if (playerControllerScript.doubleSpeed) //если в скрипте playerControllerScript doubleSpeed = true, значит сейчас у перса удвоенная скорость игры, то есть:
            {
                transform.Translate(Vector3.left * Time.deltaTime * (speed*2)); //перемещение (transform.Translate). Происходит непрерывно, ибо записано в функции Update.
                                                                            //направление движения - влево (Vector3.left). Чтобы оно было одинаковым для всех устройств пишем Time.deltaTime и умножаем на удвоенную скорость (speed*2), а то ничего не сдвинется

            }
            else
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed); //перемещение (transform.Translate). Происходит непрерывно, ибо записано в функции Update.
                                                                            //направление движения - влево (Vector3.left). Чтобы оно было одинаковым для всех устройств пишем Time.deltaTime и умножаем на скорость (speed), а то ничего не сдвинется
            }

        }

        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle")) //если позиция объекта, к которому прикреплён скрипт, по оси Х (по горизонтали) меньше положения препятствия, при котором оно уже не видно в сцене,
                                                                                   //и ещё у этого объекта в инспекторе указан тег Obstacle (значит этим объектом является препятствие), то:
        {
            Destroy(gameObject); //данный объект удаляется (препятствие удаляется)
        }

    }
}

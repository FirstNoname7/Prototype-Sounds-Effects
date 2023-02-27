using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    //этот скрипт нужен для бексонечного повтора фона, чтоб он не пропадал

    private Vector3 startPos; //стартовая позиция объекта, к которому прикреплён этот скрипт (он прикреплён к фону движущемуся)
    private float repeatWidth; //ширина для повтора прокрутки фона

    void Start()
    {
        startPos = transform.position; //изначально стартовая позиция объекта равна его позиции, которая в инспекторе указана
        repeatWidth = GetComponent<BoxCollider>().size.x / 2; //чтобы сделать ширину оптимальной подключаем размер объекта по горизонтали, делённый на 2 (size.x / 2) у компонента BoxCollider (GetComponent<BoxCollider>())
    }

    void Update()
    {
        if (transform.position.x < startPos.x - repeatWidth) //если позиция по оси Х у объекта меньше, чем стартовая позиция объекта минус repeatWidth (поскольку startPos = 0, то если меньше -repeatWidth), то:
                                                             //(именно меньше, потому что при движении влево значение меняется на отрицательное)
        {
            transform.position = startPos; //текущая позиция сбрасывается на стартовую
        }
    }
}

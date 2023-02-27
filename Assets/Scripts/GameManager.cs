using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float score; //очки
    public Transform startingPoint; //тут указывается позиция перса игрока, когда он уже начинает играть
    public float lerpSpeed; //скорость перса в начале игры (когда он ещё не бежит, а идёт по карте)
    private PlayerController playerControllerScript; //вызываем скрипт с именем PlayerController

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>(); //разрешаем этому скрипту доступ к игровому объекту с именем Player и скрипту с именем PlayerController
        score = 0; //при старте кол-во очков = 0

        playerControllerScript.gameOver = true; //при старте игра завершается (нужно для того, чтобы фон не двигался)
        StartCoroutine(PlayIntro()); //применяем всё то, что указано в IEnumerator (последовательности действий) с названием PlayIntro
    }

    void Update()
    {
        if (!playerControllerScript.gameOver) //если игра не закончилась, то:
        {
            if (playerControllerScript.doubleSpeed) //если у перса игрока сейчас удвоенная скорость, то:
            {
                score += 2; //прибавляем 2 очка, не одно
            }
            else //в обратном случае (если у перса обычная скорость):
            { 
                score++; //прибавляем 1 очко
            }
            Debug.Log("Очки:" + score); //на консоли будет выводиться кол-во очков, которое заработал перс игрока
        }
    }

    IEnumerator PlayIntro() //последовательность действий при старте игры
    {
        Vector3 startPos = playerControllerScript.transform.position; //выражаем позицию перса игрока при старте через скрипт PlayerController
        Vector3 endPos = startingPoint.position; //выражаем позицию перса игрока через startingPoint, если вступительная часть, когда он тупо идёт, закончилась, и началась игра
        float jorneyLength = Vector3.Distance(startPos, endPos); //указываем дистанцию от момента, когда перс тупо идёт, до момента, когда начинается игровой процесс (дистанция интро)
        float startTime = Time.time; //время, проходящее от предигрового процесса (интро, когда перс тупо идёт) до начала активной игры (начала бега)

        float distanceCovered = (Time.time - startTime) * lerpSpeed; //тут объявляется расстояние до начала игры. (Time.time - startTime) - от определённого времени вычитается время, проходящее от предигрового процесса.
                                                                     //Полученное число умножается на скорость перса в начале игры (lerpSpeed). Таким образом чем дальше перс прошёл, тем меньше значение, и тем меньше дистанция от интро до старта игры.
        float fractionOfJorney = distanceCovered / jorneyLength; //Доля расстояния по длине путешествия. Путь до начала игры. Общая дистанция делится на отрезок от интро до старта игры.

        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_Multiplier", 0.5f); //через скрипт PlayerController подключаем компонент аниматора (Animator) и устанавливаем скорость анимации на 50% ниже стандартной (0.5f = 50%)

        while (fractionOfJorney < 1) //цикл. будет выполняться, если расстояние до начала игры меньше 1 (то есть перс ещё не дошёл до начала игры, ещё интро идёт)
        {
            distanceCovered = (Time.time - startTime) * lerpSpeed; //тут указывается расстояние до начала игры. (Time.time - startTime) - от определённого времени вычитается время, проходящее от предигрового процесса.
                                                                   //Полученное число умножается на скорость перса в начале игры (lerpSpeed). Таким образом чем дальше перс прошёл, тем меньше значение, и тем меньше дистанция от интро до старта игры.

            fractionOfJorney = distanceCovered / jorneyLength; //Доля расстояния по длине путешествия. Путь до начала игры. Общая дистанция делится на отрезок от интро до старта игры.

            playerControllerScript.transform.position = Vector3.Lerp(startPos, endPos, fractionOfJorney); //двигаем перса игрока. Позиция = доля расстояния (fractionOfJorney) между маркерами startPos и endPos
            yield return null; //обнуляем интро, чтоб никакие настройки отсюда не мешали игровому процессу
        }
        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_Multiplier", 1.0f); //через скрипт PlayerController подключаем компонент аниматора (Animator) и возвращаем скорость анимации на 100% (то есть стандартную, ибо 1.0f = 100%)
        playerControllerScript.gameOver = false; //начинаем игру.

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb; //вызываем компонент жёсткого тела при помощи переменной с именем playerRb
    private Animator playerAnim; //добавляем анимацию перса
    private AudioSource playerAudio; //вызываем источник аудио у объекта, к которому прикреплён этот скрипт (у перса игрока)

    public float jumpForce; //регулятор силы прыжка (чем больше число, тем сильнее прыжок перса)
    public float gravityModifier; //регулятор силы притяжения (чем больше число, тем сильнее сила притяжения по оси у) (то есть тем труднее персу прыгнуть высооко)
    public bool isOnGround=true; //проверка, стоит перс на земле или нет (true - стоит на земле, false - не стоит на земле)
    public bool gameOver; //переменная для проверки, закончилась игра или нет
    public ParticleSystem explosionParticle; //переменная для манипуляции анимационным эффектом дыма (анимационный эффект указан в инспекторе)
    public ParticleSystem dirtParticle; //переменная для манипуляции анимационным эффектом грязи (анимационный эффект указан в инспекторе)
    public AudioClip jumpSound; //звук для прыжка (в инспекторе)
    public AudioClip crashSound; //звук для сталкивания с препятствием (в инспекторе)
    public bool doubleJumpUsed = false; //по дефолту двойной прыжок для перса отключен
    public float doubleJumpForce; //в инспекторе указывается сила двойного прыжка
    public bool doubleSpeed = false; //по дефолту у перса игрока обычная скорость

    void Start()
    {
        playerRb = GetComponent<Rigidbody>(); //вызываем жёсткое тело, чтоб им можно было манипулировать
        Physics.gravity *= gravityModifier; //сила притяжения (когда находится в Rigidbody в блоке Physics и строке gravity) будет действовать исходя из значения переменной gravityModifier (значение указано в инспекторе)
        playerAnim = GetComponent<Animator>(); //добавляем компонент Animator, чтоб подрубать разные анимашки
        playerAudio = GetComponent<AudioSource>(); //добавляем компонент AudioSource, чтоб подрубать звуки


    }

    void Update()
    {
        MultiJump();

        //if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver) //при нажатии на пробел (и ещё если перс стоит на земле) (и ещё если игра не закончена (gameOver==false) (если это не поставить, то перс мёртвым будет прыгать при нажатии на пробел)):
        //{
        //    playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //перс прыгнет вверх (ибо указано направление Vector3.up) на jumpForce юнитов вверх с импульсом ForceMode.Impulse
        //    isOnGround = false; //перс не соприкасается с землёй (ну логично, он же прыгнул с помощью строки чуть выше)

        //    playerAnim.SetTrigger("Jump_trig"); //подрубается анимация прыжка за счёт использования переменной Jump_trig

        //    dirtParticle.Stop(); //анимация грязи останавливается

        //    playerAudio.PlayOneShot(jumpSound, 1.0f); //проигрывается звук, указанный в переменной jumpSound. проигрывается он один раз (ибо указано One). 1.0f - громкость звука: 0.1 - минимальная громкость, 1.0 - максимальная громкость.

        //    doubleJumpUsed = false; //то есть если перс игрока прыгает, то ему можно совершить двойной прыжок

        //}
        //else if (Input.GetKeyDown(KeyCode.Space) && !gameOver && !doubleJumpUsed) //при нажатии на пробел (и если игра не закончена) (и если двойной прыжок ещё не совершён), то:
        //{
        //    doubleJumpUsed = true; //теперь, когда перс игрока совершает двойной прыжок, ему нельзя будет ещё прыгать (нельзя совершить тройной прыжок)
        //    playerRb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse); //перс прыгнет вверх (ибо указано направление Vector3.up) на doubleJumpForce юнитов вверх с импульсом ForceMode.Impulse
        //    playerAnim.SetTrigger("Jump_trig"); //подрубается анимация прыжка за счёт использования переменной Jump_trig
        //    dirtParticle.Stop(); //анимация грязи останавливается
        //    playerAudio.PlayOneShot(jumpSound, 1.0f); //проигрывается звук, указанный в переменной jumpSound. проигрывается он один раз (ибо указано One). 1.0f - громкость звука: 0.1 - минимальная громкость, 1.0 - максимальная громкость.

        //}

        if (Input.GetKey(KeyCode.Q)) //при нажатии на кнопку Q:
        {
            doubleSpeed = true; //применяется двойная скорость перса
            playerAnim.SetFloat("Speed_Multiplier", 2.0f); //анимация бега ускоряется в 2 раза (2.0f = 200%) (здесь нет какого-либо transform или AddForce, потому что фактически персонаж не двигается, двигается мир вокруг него, за счёт чего создаётся иллюзия, что персонаж бежит)
        }
        else if (doubleSpeed) //если игрок не нажал на клавишу Q, то:
        {
            doubleSpeed = false; //отключается двойная скорость перса
            playerAnim.SetFloat("Speed_Multiplier", 1.0f); //анимация бега снова стандартна (1.0f = 100%)
        }


    }

    int jumpCount;
    bool isStopJumping;
    void MultiJump()
    {
        if (isOnGround && !gameOver) //если мы на земле и игра не закончилась, то:
        {
            jumpCount = 0; //значит не прыгаем
            isStopJumping = false; //соответственно ограничений на прыжки нет
        }

        if (!isStopJumping && (playerRb.velocity.y < 0 || isOnGround)) //если ограничений на прыжки нет и если мы в этот момент не прыгаем (чтоб прыжками флудить нельзя было) или мы не на земле, то:
        {
            if (Input.GetKeyDown(KeyCode.Space)) //если нажали на пробел:
            {
                isOnGround = false; //мы не на земле
                jumpCount++; //увеличиваем ограничитель прыжков

                if (jumpCount <= 2) //сравниваем ограничитель прыжков с максимальным кол-вом прыжков за 1 раз
                {
                    playerRb.velocity = Vector3.zero;
                    playerAnim.SetTrigger("Jump_trig"); //анимация прыжка
                    playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //прыгаем
                    dirtParticle.Stop();
                    playerAudio.PlayOneShot(jumpSound, 1.0f);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision) //стандартная функция, вызывается когда жёсткое тело объекта, к которому прикреплён этот скрипт (то есть перс) соприкасается с другим жёстким телом
                                                       //(в скобках Collision collision нужно для условий)
    {

        if (collision.gameObject.CompareTag("Ground")) //если объект, к которому прикреплён этот скрипт, столкнулся с объектом с тегом Ground (то есть с землёй), то:
        {
            isOnGround = true; //перс стоит на земле (логично, если он соприкоснулся с ней после прыжка, значит он на ней стоит)

            dirtParticle.Play(); //начинает проигрываться анимация грязи из-под ног



        }
        else if (collision.gameObject.CompareTag("Obstacle")) //если объект, к которому прикреплён этот скрипт, столкнулся с объектом с тегом Obstacle (то есть с препятствием), то:
        {
            Debug.Log("Game over!"); //выводится текст Game over! на консоли
            gameOver = true; //игра заканчивается

            playerAnim.SetBool("Death_b",true); //посмотри в Animator->Death->жми на стрелку-переход между Alive и Death_01->в инспекторе в Conditions указано, что анимация будет работать при Death_b = true и DeathType_int = 1. Эти переменные указаны в Parameters у Animator
            playerAnim.SetInteger("DeathType_int", 1); //см.комментарий строки чуть выше

            explosionParticle.Play(); //воспроизводится анимационный эффект дыма

            dirtParticle.Stop(); //анимация грязи останавливается

            playerAudio.PlayOneShot(crashSound, 1.0f); //проигрывается звук, указанный в переменной crashSound. проигрывается он один раз (ибо указано One). 1.0f - громкость звука: 0.1 - минимальная громкость, 1.0 - максимальная громкость.
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb; //�������� ��������� ������� ���� ��� ������ ���������� � ������ playerRb
    private Animator playerAnim; //��������� �������� �����
    private AudioSource playerAudio; //�������� �������� ����� � �������, � �������� ��������� ���� ������ (� ����� ������)

    public float jumpForce; //��������� ���� ������ (��� ������ �����, ��� ������� ������ �����)
    public float gravityModifier; //��������� ���� ���������� (��� ������ �����, ��� ������� ���� ���������� �� ��� �) (�� ���� ��� ������� ����� �������� �������)
    public bool isOnGround=true; //��������, ����� ���� �� ����� ��� ��� (true - ����� �� �����, false - �� ����� �� �����)
    public bool gameOver; //���������� ��� ��������, ����������� ���� ��� ���
    public ParticleSystem explosionParticle; //���������� ��� ����������� ������������ �������� ���� (������������ ������ ������ � ����������)
    public ParticleSystem dirtParticle; //���������� ��� ����������� ������������ �������� ����� (������������ ������ ������ � ����������)
    public AudioClip jumpSound; //���� ��� ������ (� ����������)
    public AudioClip crashSound; //���� ��� ����������� � ������������ (� ����������)
    public bool doubleJumpUsed = false; //�� ������� ������� ������ ��� ����� ��������
    public float doubleJumpForce; //� ���������� ����������� ���� �������� ������
    public bool doubleSpeed = false; //�� ������� � ����� ������ ������� ��������

    void Start()
    {
        playerRb = GetComponent<Rigidbody>(); //�������� ������ ����, ���� �� ����� ���� ��������������
        Physics.gravity *= gravityModifier; //���� ���������� (����� ��������� � Rigidbody � ����� Physics � ������ gravity) ����� ����������� ������ �� �������� ���������� gravityModifier (�������� ������� � ����������)
        playerAnim = GetComponent<Animator>(); //��������� ��������� Animator, ���� ��������� ������ ��������
        playerAudio = GetComponent<AudioSource>(); //��������� ��������� AudioSource, ���� ��������� �����


    }

    void Update()
    {
        MultiJump();

        //if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver) //��� ������� �� ������ (� ��� ���� ���� ����� �� �����) (� ��� ���� ���� �� ��������� (gameOver==false) (���� ��� �� ���������, �� ���� ������ ����� ������� ��� ������� �� ������)):
        //{
        //    playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //���� ������� ����� (��� ������� ����������� Vector3.up) �� jumpForce ������ ����� � ��������� ForceMode.Impulse
        //    isOnGround = false; //���� �� ������������� � ����� (�� �������, �� �� ������� � ������� ������ ���� ����)

        //    playerAnim.SetTrigger("Jump_trig"); //����������� �������� ������ �� ���� ������������� ���������� Jump_trig

        //    dirtParticle.Stop(); //�������� ����� ���������������

        //    playerAudio.PlayOneShot(jumpSound, 1.0f); //������������� ����, ��������� � ���������� jumpSound. ������������� �� ���� ��� (��� ������� One). 1.0f - ��������� �����: 0.1 - ����������� ���������, 1.0 - ������������ ���������.

        //    doubleJumpUsed = false; //�� ���� ���� ���� ������ �������, �� ��� ����� ��������� ������� ������

        //}
        //else if (Input.GetKeyDown(KeyCode.Space) && !gameOver && !doubleJumpUsed) //��� ������� �� ������ (� ���� ���� �� ���������) (� ���� ������� ������ ��� �� ��������), ��:
        //{
        //    doubleJumpUsed = true; //������, ����� ���� ������ ��������� ������� ������, ��� ������ ����� ��� ������� (������ ��������� ������� ������)
        //    playerRb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse); //���� ������� ����� (��� ������� ����������� Vector3.up) �� doubleJumpForce ������ ����� � ��������� ForceMode.Impulse
        //    playerAnim.SetTrigger("Jump_trig"); //����������� �������� ������ �� ���� ������������� ���������� Jump_trig
        //    dirtParticle.Stop(); //�������� ����� ���������������
        //    playerAudio.PlayOneShot(jumpSound, 1.0f); //������������� ����, ��������� � ���������� jumpSound. ������������� �� ���� ��� (��� ������� One). 1.0f - ��������� �����: 0.1 - ����������� ���������, 1.0 - ������������ ���������.

        //}

        if (Input.GetKey(KeyCode.Q)) //��� ������� �� ������ Q:
        {
            doubleSpeed = true; //����������� ������� �������� �����
            playerAnim.SetFloat("Speed_Multiplier", 2.0f); //�������� ���� ���������� � 2 ���� (2.0f = 200%) (����� ��� ������-���� transform ��� AddForce, ������ ��� ���������� �������� �� ���������, ��������� ��� ������ ����, �� ���� ���� �������� �������, ��� �������� �����)
        }
        else if (doubleSpeed) //���� ����� �� ����� �� ������� Q, ��:
        {
            doubleSpeed = false; //����������� ������� �������� �����
            playerAnim.SetFloat("Speed_Multiplier", 1.0f); //�������� ���� ����� ���������� (1.0f = 100%)
        }


    }

    int jumpCount;
    bool isStopJumping;
    void MultiJump()
    {
        if (isOnGround && !gameOver) //���� �� �� ����� � ���� �� �����������, ��:
        {
            jumpCount = 0; //������ �� �������
            isStopJumping = false; //�������������� ����������� �� ������ ���
        }

        if (!isStopJumping && (playerRb.velocity.y < 0 || isOnGround)) //���� ����������� �� ������ ��� � ���� �� � ���� ������ �� ������� (���� �������� ������� ������ ����) ��� �� �� �� �����, ��:
        {
            if (Input.GetKeyDown(KeyCode.Space)) //���� ������ �� ������:
            {
                isOnGround = false; //�� �� �� �����
                jumpCount++; //����������� ������������ �������

                if (jumpCount <= 2) //���������� ������������ ������� � ������������ ���-��� ������� �� 1 ���
                {
                    playerRb.velocity = Vector3.zero;
                    playerAnim.SetTrigger("Jump_trig"); //�������� ������
                    playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //�������
                    dirtParticle.Stop();
                    playerAudio.PlayOneShot(jumpSound, 1.0f);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision) //����������� �������, ���������� ����� ������ ���� �������, � �������� ��������� ���� ������ (�� ���� ����) ������������� � ������ ������ �����
                                                       //(� ������� Collision collision ����� ��� �������)
    {

        if (collision.gameObject.CompareTag("Ground")) //���� ������, � �������� ��������� ���� ������, ���������� � �������� � ����� Ground (�� ���� � �����), ��:
        {
            isOnGround = true; //���� ����� �� ����� (�������, ���� �� ������������� � ��� ����� ������, ������ �� �� ��� �����)

            dirtParticle.Play(); //�������� ������������� �������� ����� ��-��� ���



        }
        else if (collision.gameObject.CompareTag("Obstacle")) //���� ������, � �������� ��������� ���� ������, ���������� � �������� � ����� Obstacle (�� ���� � ������������), ��:
        {
            Debug.Log("Game over!"); //��������� ����� Game over! �� �������
            gameOver = true; //���� �������������

            playerAnim.SetBool("Death_b",true); //�������� � Animator->Death->��� �� �������-������� ����� Alive � Death_01->� ���������� � Conditions �������, ��� �������� ����� �������� ��� Death_b = true � DeathType_int = 1. ��� ���������� ������� � Parameters � Animator
            playerAnim.SetInteger("DeathType_int", 1); //��.����������� ������ ���� ����

            explosionParticle.Play(); //��������������� ������������ ������ ����

            dirtParticle.Stop(); //�������� ����� ���������������

            playerAudio.PlayOneShot(crashSound, 1.0f); //������������� ����, ��������� � ���������� crashSound. ������������� �� ���� ��� (��� ������� One). 1.0f - ��������� �����: 0.1 - ����������� ���������, 1.0 - ������������ ���������.
        }
    }
}

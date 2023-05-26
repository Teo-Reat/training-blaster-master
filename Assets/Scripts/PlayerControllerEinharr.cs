using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerEinharr : MonoBehaviour
{
    public float moveSpeed = 3f; // Скорость движения персонажа
    public float acceleration = 3f; // Ускорение персонажа
    public float jumpForce = 3f; // Сила прыжка
    public float groundRaycastDistance = 0.2f; // Расстояние, на которое выпускается рэйкаст для проверки земли
    public float wallRaycastDistance = 0.2f; // Расстояние, на которое выпускается рэйкаст для проверки стены

    private PlayerHangableDetector hangableDetector; //Ссылка на виселко и цеплялко детектор

    private Rigidbody rb;
    private Animator animator;
    private bool isJumping = false;

    public bool isRunning;
    public bool isWallRunning;
    public bool isGrounded;
    public bool isNearWall;


    private float currentSpeed = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        hangableDetector = GetComponent<PlayerHangableDetector>();
    }

    private void Update()
    {
        // Получаем инпут от игрока
        float moveInput = Input.GetAxis("Horizontal");

        // Проверяем окружение персонажа с помощью рэйкастов
        CheckSurroundings();

        //Управление

        // Вычисляем вектор движения только по оси X, потому что иначе он будет влиять на скорость падения персонажа(данные получены горьким опытом)
        Vector3 moveDirection = new Vector3(moveInput, 0f, 0f);

        MoveCharacter(moveDirection, moveInput);

        // Если персонаж на земле и нажата клавиша прыжка, выполняем прыжок
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // Проверяем, если персонаж на земле, рядом стена и нажата клавиша движения и клавиша прыжка. Если звезды сошлись - включаем бег по стене
        if (isGrounded && isNearWall && (moveInput != 0f) && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Персонаж взбегает по стене");
            isWallRunning = true;
            Jump();
        }

        // Если рядом есть объект "hangable" и нажата клавиша цепляния ("F") - цепляемся.
        if (hangableDetector.IsHangableNearby() && Input.GetKeyDown(KeyCode.F))
        {
            GameObject hangableObject = hangableDetector.GetHangableObject();
            // Выполните цепляние за объект "hangable"
            HangOn(hangableObject);
        }

        // Обновляем параметры аниматора
        animator.SetFloat("MoveSpeed", Mathf.Abs(moveInput));
    }

    private void MoveCharacter(Vector3 moveDirection, float moveInput)
    {

        if (moveInput != 0f)
        {
            // Если есть ввод движения, устанавливаем параметр "isRunning" в true. Тут надо оптимизировать, но аниматор всяко временный
            animator.SetBool("isRunning", true);
            isRunning = true;
            // Поворачиваем персонаж в сторону движения
            transform.LookAt(transform.position + moveDirection);
        }
        else
        {
            // Если нет ввода движения, устанавливаем параметр "isRunning" в false. Тут надо оптимизировать, но аниматор всяко временный
            animator.SetBool("isRunning", false);
            isRunning = false;
        }

        if (moveInput != 0f && !isNearWall) // Проверяем наличие ввода движения и отсутствие стены рядом
        {
            // Если есть ввод движения и персонаж не рядом со стеной, устанавливаем параметр "isRunning" в true
            animator.SetBool("isRunning", true);
            isRunning = true;
            // Поворачиваем персонаж в сторону движения
            transform.LookAt(transform.position + moveDirection);

            // Применяем силу к Rigidbody персонажа с использованием инерции
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            // Если нет ввода движения или персонажа рядом со стеной, устанавливаем параметр "isRunning" в false, чтобы в стену не бежал
            animator.SetBool("isRunning", false);
            isRunning = false;
            // Останавливаем персонажа
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.deltaTime);
        }

        // Применяем скорость только к горизонтальному движению
        rb.velocity = new Vector3(moveDirection.x * currentSpeed, rb.velocity.y, rb.velocity.z);
    }

    private void CheckSurroundings()
    {
        // Проверим наличие земли под ногами с помощью рэйкаста
        bool hitGround = Physics.Raycast(transform.position, Vector3.down, out RaycastHit groundHit, groundRaycastDistance);
        isGrounded = hitGround;

        // Проверяем наличие стены перед персонажем с помощью рэйкаста
        bool hitWall = Physics.Raycast(transform.position, transform.forward, out RaycastHit wallHit, wallRaycastDistance);
        isNearWall = hitWall;

        // Визуализируем рэйкасты дебагу ради и веселья для
        Debug.DrawRay(transform.position, Vector3.down * groundRaycastDistance, isGrounded ? Color.green : Color.red);
        Debug.DrawRay(transform.position, transform.forward * wallRaycastDistance, isNearWall ? Color.yellow : Color.red);
    }


    private void Jump()
    {
        // Применяем силу прыжка к Rigidbody персонажа
        if (isWallRunning)
        {
            // Устанавливаем только вертикальную составляющую силы прыжка, тут зарылась какая-то жопа
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // Отключаем горизонтальную скорость, чтобы персонаж не отрывался от стены
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            isWallRunning = false;
        }
        else
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
    }
    private void HangOn(GameObject hangableObject)
    {
        // Логика цепляния и висения будет тут
        Debug.Log("Условно зацепился за ", hangableObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerEinharr : MonoBehaviour
{
    public float moveSpeed = 3f; // Скорость движения персонажа
    public float acceleration = 3f; // Ускорение персонажа
    public float jumpForce = 1f; // Сила прыжка
    public float groundRaycastDistance = 0.2f; // Расстояние, на которое выпускается рэйкаст для проверки земли
    public float wallRaycastDistance = 0.2f; // Расстояние, на которое выпускается рэйкаст для проверки стены

    private PlayerHangableDetector hangableDetector; //Ссылка на виселко и цеплялко детектор

    private Rigidbody rb;
    private Animator animator;
    public bool isJumping;

//Базовое перемещение
    public bool isRunning;
    public bool isWallRunning;
    public bool isGrounded;
    public bool isNearWall;
//Цепляние и взбирание
    public bool isHanging;
    public GameObject hangingObject;
    public Vector3 hangingPoint;


    public float currentSpeed = 0f;

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
        // Если персонаж на земле, обновляем состояние переменной isJumping
        if (isGrounded)
        {
            OnGrounded();
        }
        // Вычисляем вектор движения только по оси X, потому что иначе он будет влиять на скорость падения персонажа(данные получены горьким опытом)
        Vector3 moveDirection = new Vector3(moveInput, 0f, 0f);

        // Проверяем,что персонаж не находится в воздухе
        if (isGrounded && !isWallRunning && !isHanging)
        {
            MoveCharacter(moveDirection, moveInput);
            animator.SetFloat("MoveSpeed", Mathf.Abs(moveInput));
        }

        // Если персонаж висит и нажата клавиша прыжка, выполняем прыжок
        if (isHanging && Input.GetButtonDown("Jump"))
        {

        }

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

        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isHanging", isHanging);
      //  animator.SetBool("isWallRunning", isWallRunning);
    }

    private void MoveCharacter(Vector3 moveDirection, float moveInput)
    {

        if (moveInput != 0f)
        {
            // Если есть ввод движения, устанавливаем параметр "isRunning" в true. Тут надо оптимизировать, но аниматор всяко временный
            isRunning = true;
            // Поворачиваем персонаж в сторону движения
            transform.LookAt(transform.position + moveDirection);
        }
        else
        {
            // Если нет ввода движения, устанавливаем параметр "isRunning" в false. Тут надо оптимизировать, но аниматор всяко временный

            isRunning = false;
        }

        if (moveInput != 0f && !isNearWall) // Проверяем наличие ввода движения и отсутствие стены рядом
        {
            // Если есть ввод движения и персонаж не рядом со стеной, устанавливаем параметр "isRunning" в true
            isRunning = true;
            // Поворачиваем персонаж в сторону движения
            transform.LookAt(transform.position + moveDirection);

            // Применяем силу к Rigidbody персонажа с использованием инерции
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            // Если нет ввода движения или персонажа рядом со стеной, устанавливаем параметр "isRunning" в false, чтобы в стену не бежал
            isRunning = false;
            // Останавливаем персонажа
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.deltaTime);
        }

        // Применяем скорость только к горизонтальному движению
        rb.velocity = new Vector3(moveDirection.x * currentSpeed, rb.velocity.y, rb.velocity.z);
    }

    private void CheckSurroundings()
    {
      // Получаем центр коллайдера
      Vector3 raycastOrigin = GetComponent<Collider>().bounds.center;

      // Проверяем наличие земли под ногами с помощью рэйкаста
      bool hitGround = Physics.Raycast(raycastOrigin, Vector3.down, out RaycastHit groundHit, groundRaycastDistance, LayerMask.GetMask("Ground"));
      isGrounded = hitGround;

      // Проверяем наличие стены перед персонажем с помощью рэйкаста
      bool hitWall = Physics.Raycast(raycastOrigin, transform.forward, out RaycastHit wallHit, wallRaycastDistance, LayerMask.GetMask("Ground"));
      isNearWall = hitWall;


        // Визуализируем рэйкасты дебагу ради и веселья для
        Debug.DrawRay(raycastOrigin, Vector3.down * groundRaycastDistance, isGrounded ? Color.green : Color.red);
        Debug.DrawRay(raycastOrigin, transform.forward * wallRaycastDistance, isNearWall ? Color.yellow : Color.red);
    }


    private void Jump()
    {
        // Применяем силу прыжка к Rigidbody персонажа
        if (isWallRunning)
        {
          // Устанавливаем горизонтальную и вертикальную составляющую силы прыжка
          Vector3 wallJumpForce = transform.up * jumpForce * 0.3f;
          rb.AddForce(wallJumpForce, ForceMode.Impulse);

          // Отключаем горизонтальную скорость, чтобы персонаж не отрывался от стены
          rb.velocity = new Vector3(0f, 0f, rb.velocity.z);
        }
        else
        {
            isJumping = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        }
        isWallRunning = false;

    }

    private void OnGrounded()
    {
        if (isJumping)
        {
            StartCoroutine(DisableJumping());
        }
    }

    private IEnumerator DisableJumping()
    {
        yield return new WaitForSeconds(0.1f); // Задержка в 0.1 секунды
        isJumping = false;
        // Дополнительные действия при приземлении
    }
    public void HangOn(GameObject hangableObject)
    {
        // Получаем ссылку на компонент Hangable
        Hangable hangable = hangableObject.GetComponent<Hangable>();

        if (hangable != null)
        {
            // Проверяем, что персонаж может цепляться за данный объект

            // Устанавливаем состояние цепляния
            isHanging = true;
            hangingObject = hangableObject;
            hangingPoint = hangable.center;

            // Отключаем физическое взаимодействие
            rb.isKinematic = true;

            // Вычисляем позицию персонажа под точкой крепления, учитывая размер коллайдера
            Collider characterCollider = GetComponent<Collider>();
            float characterHeight = characterCollider.bounds.size.y;
            float chatacerWidth = characterCollider.bounds.size.x/2;
            Vector3 hangPosition = hangingPoint - Vector3.up * characterHeight + Vector3.left * chatacerWidth;
            transform.position = hangPosition;

            // Выполняем дополнительные действия при цеплянии
            // hangable.OnHang();
        }
    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerEinharr : MonoBehaviour
{
	public float moveSpeed = 3f; // �������� �������� ���������
	public float acceleration = 3f; // ��������� ���������
	public float jumpForce = 1f; // ���� ������
	public float groundRaycastDistance = 0.2f; // ����������, �� ������� ����������� ������� ��� �������� �����
	public float wallRaycastDistance = 0.2f; // ����������, �� ������� ����������� ������� ��� �������� �����

	private PlayerHangableDetector hangableDetector; //������ �� ������� � �������� ��������

	private Rigidbody rb;
	private Animator animator;
	private bool isJumping = false;

	public bool isRunning;
	public bool isWallRunning;
	public bool isGrounded;
	public bool isNearWall;
    // новый комментарий на русском


    private float currentSpeed = 0f;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		hangableDetector = GetComponent<PlayerHangableDetector>();
	}

	private void Update()
	{
		// �������� ����� �� ������
		float moveInput = Input.GetAxis("Horizontal");

		// ��������� ��������� ��������� � ������� ���������
		CheckSurroundings();

		//����������

		// ��������� ������ �������� ������ �� ��� X, ������ ��� ����� �� ����� ������ �� �������� ������� ���������(������ �������� ������� ������)
		Vector3 moveDirection = new Vector3(moveInput, 0f, 0f);

		MoveCharacter(moveDirection, moveInput);

		// ���� �������� �� ����� � ������ ������� ������, ��������� ������
		if (isGrounded && Input.GetButtonDown("Jump"))
		{
			Jump();
		}

		// ���������, ���� �������� �� �����, ����� ����� � ������ ������� �������� � ������� ������. ���� ������ ������� - �������� ��� �� �����
		if (isGrounded && isNearWall && (moveInput != 0f) && Input.GetButtonDown("Jump"))
		{
			Debug.Log("�������� �������� �� �����");
			isWallRunning = true;
			Jump();
		}

		// ���� ����� ���� ������ "hangable" � ������ ������� �������� ("F") - ���������.
		if (hangableDetector.IsHangableNearby() && Input.GetKeyDown(KeyCode.F))
		{
			GameObject hangableObject = hangableDetector.GetHangableObject();
			// ��������� �������� �� ������ "hangable"
			HangOn(hangableObject);
		}

		// ��������� ��������� ���������
		animator.SetFloat("MoveSpeed", Mathf.Abs(moveInput));
	}

	private void MoveCharacter(Vector3 moveDirection, float moveInput)
	{

		if (moveInput != 0f)
		{
			// ���� ���� ���� ��������, ������������� �������� "isRunning" � true. ��� ���� ��������������, �� �������� ����� ���������
			animator.SetBool("isRunning", true);
			isRunning = true;
			// ������������ �������� � ������� ��������
			transform.LookAt(transform.position + moveDirection);
		}
		else
		{
			// ���� ��� ����� ��������, ������������� �������� "isRunning" � false. ��� ���� ��������������, �� �������� ����� ���������
			animator.SetBool("isRunning", false);
			isRunning = false;
		}

		if (moveInput != 0f && !isNearWall) // ��������� ������� ����� �������� � ���������� ����� �����
		{
			// ���� ���� ���� �������� � �������� �� ����� �� ������, ������������� �������� "isRunning" � true
			animator.SetBool("isRunning", true);
			isRunning = true;
			// ������������ �������� � ������� ��������
			transform.LookAt(transform.position + moveDirection);

			// ��������� ���� � Rigidbody ��������� � �������������� �������
			currentSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * Time.deltaTime);
		}
		else
		{
			// ���� ��� ����� �������� ��� ��������� ����� �� ������, ������������� �������� "isRunning" � false, ����� � ����� �� �����
			animator.SetBool("isRunning", false);
			isRunning = false;
			// ������������� ���������
			currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.deltaTime);
		}

		// ��������� �������� ������ � ��������������� ��������
		rb.velocity = new Vector3(moveDirection.x * currentSpeed, rb.velocity.y, rb.velocity.z);
	}

	private void CheckSurroundings()
	{
		// �������� ������� ����� ��� ������ � ������� ��������
		bool hitGround = Physics.Raycast(transform.position, Vector3.down, out RaycastHit groundHit, groundRaycastDistance);
		isGrounded = hitGround;

		// ��������� ������� ����� ����� ���������� � ������� ��������
		bool hitWall = Physics.Raycast(transform.position, transform.forward, out RaycastHit wallHit, wallRaycastDistance);
		isNearWall = hitWall;

		// ������������� �������� ������ ���� � ������� ���
		Debug.DrawRay(transform.position, Vector3.down * groundRaycastDistance, isGrounded ? Color.green : Color.red);
		Debug.DrawRay(transform.position, transform.forward * wallRaycastDistance, isNearWall ? Color.yellow : Color.red);
	}


	private void Jump()
	{
		// ��������� ���� ������ � Rigidbody ���������
		if (isWallRunning)
		{
			// ������������� ������ ������������ ������������ ���� ������, ��� �������� �����-�� ����
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

			// ��������� �������������� ��������, ����� �������� �� ��������� �� �����
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
		// ������ �������� � ������� ����� ���
		Debug.Log("������� ��������� �� ", hangableObject);
	}
}

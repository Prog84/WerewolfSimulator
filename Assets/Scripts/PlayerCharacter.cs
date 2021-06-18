using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	public float distance = 3f;
	public Animator animator;
	public Rigidbody playerPhysics;
	private CharacterController cc;
	public float jumpPower = 12f;
	public float groundCheckDistance = 0.3f;

	private bool isGrounded;
	private float turnAmount;
	private float forwardAmount;

    private void Start()
    {
		cc = GetComponent<CharacterController>();
    }

    public void Move(Vector3 move)
	{
		move = transform.InverseTransformDirection(move);
		move = Vector3.ProjectOnPlane(move, Vector3.up);
		turnAmount = Mathf.Atan2(move.x, move.z);
		forwardAmount = move.z;

		isGrounded = Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out _, groundCheckDistance);

		/*if (isGrounded)
		{
			HandleGroundedMovement(jump);
		}
		else
		{
			HandleAirborneMovement();
		}*/

		ApplyExtraTurnRotation();
		UpdateAnimator();

		cc.Move(move);
	}

	void UpdateAnimator()
	{
		//animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
		//animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
		/*animator.SetBool("OnGround", isGrounded);
		animator.SetFloat("Jump", playerPhysics.velocity.y);*/
	}

	// Move down when jumped
	void HandleAirborneMovement()
	{
		//playerPhysics.AddForce(Physics.gravity);
		//groundCheckDistance = playerPhysics.velocity.y < 0 ? groundCheckDistanceSaved : 0.01f;
	}

	// Jump
	void HandleGroundedMovement(bool jump)
	{
		/*if (!jump || !isGrounded)
			return;*/

		playerPhysics.velocity = new Vector3(playerPhysics.velocity.x, jumpPower, playerPhysics.velocity.z);
		isGrounded = false;
		groundCheckDistance = 0.1f;
	}

	// Rotate Character
	void ApplyExtraTurnRotation()
	{
		var turnSpeed = Mathf.Lerp(180, 360, forwardAmount);
		transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
	}

	// Move by animator
	public void OnAnimatorMove()
	{
		/*if (!isGrounded || Time.deltaTime < 0.001f)
			return;*/

		var velocity = (animator.deltaPosition) / Time.deltaTime;

		velocity.y = playerPhysics.velocity.y;
		playerPhysics.velocity = velocity;
	}
}

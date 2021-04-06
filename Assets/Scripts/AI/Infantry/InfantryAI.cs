using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[System.Serializable]
public class InfantryAI : MonoBehaviour
{ 
	public Transform Target;

	public Transform weaponFirePoint;
	public Transform UpperBody;
	public float bulletVelocity;
	public float bulletSpread;
	public float fireRate;
	
	
	public GameObject bulletPrefab;
	public GameObject muzzleFlashPrefab;
	public GameObject infantryCharacterPrefab;
	public GameObject deathPrefab;

	public LayerMask Ground, Player;

	private NavMeshAgent Agent;

	/// <summary>
	///		Patrolling
	/// </summary>
	public Vector3 movementDirection;
	public float movementRange;
	public float movementWaypointTimer = 3f;

	[SerializeField] private float minimumMovementRange = 5f;
	[SerializeField] private float maximumMovementRange = 10f;
	[SerializeField] private bool movementDirectionSet;

	/// <summary>
	///		Attacking 
	/// </summary>
	[SerializeField] private float attackWaitTimer;
	[SerializeField] private bool isAttackingPlayer;

	/// <summary>
	///		States
	/// </summary>
	public float viewDistanceAlertedRange = 50f;
	public float viewDistanceAttackRange = 75f;
	[SerializeField] private bool aiAlerted;
	[SerializeField] private bool aiAggressive;


	[SerializeField] private Vector3 startPosition;
	[SerializeField] private Quaternion startingRotation;

	// public InfantryHealth infantryHealth = new InfantryHealth();
	// public WeaponEffects weaponFx = new WeaponEffects();

	private Animator m_animator;

		
	private void Awake()
	{
		if (GetComponent<NavMeshAgent>() != null)
		{
			Agent = GetComponent<NavMeshAgent>();
			Agent.autoBraking = false;
		}

		if (GetComponent<Animator>() != null)
		{
			m_animator = GetComponent<Animator>();
		}
	}

	private void Start()
	{

		startPosition = transform.position;
		startingRotation = transform.rotation;
		movementDirectionSet = false;

	}


	private void Update()
	{
		
		CheckViewDistance(transform.position);

		// If the AI is not alerted or aggressive, They should be looking for a player (Setting a movement point) 
		if (!aiAlerted && !aiAggressive)
		{
			SearchForPlayer();
		}
		else if (aiAlerted && !aiAggressive)
		{

		}
		else if (!aiAlerted && aiAggressive)
		{

		}

	}


	/// <summary>
	///		Checks for the player within the view distance that is set 
	/// </summary>
	/// <param name="pos"></param>
	private void CheckViewDistance(Vector3 pos)
	{
		aiAlerted = Physics.CheckSphere(pos, viewDistanceAlertedRange, Player);
		aiAggressive = Physics.CheckSphere(pos, viewDistanceAttackRange, Player);

	}



	#region AI States 

	private void SearchForPlayer()
	{

		// If there isnt a movement direction point set 
		if (!movementDirectionSet)
		{
			// Then we should set one 
			StartCoroutine(SetMovementWaypoint());
			return;
		}
	

		if (movementDirectionSet && movementDirection.magnitude < 1f)
		{ 
			movementDirectionSet = false;
		}
		else
		{
			if (movementDirectionSet == true)
			{

				Debug.Log("[InfantryAI.SearchForPlayer]: " + "Searching for player on direction " + movementDirection);
				Agent.SetDestination(movementDirection);
			}
		}

			
		


	}

	private IEnumerator SetMovementWaypoint()
	{

		yield return new WaitForSeconds(movementWaypointTimer);

		float randomXPosition = Random.Range(minimumMovementRange, maximumMovementRange);
		float randomZPosition = Random.Range(minimumMovementRange, maximumMovementRange);

		randomXPosition += transform.position.x;
		randomZPosition += transform.position.z;

		movementDirection = new Vector3(randomXPosition, 0, randomZPosition);


		if (Physics.Raycast(movementDirection, -transform.up, 2f, Ground))
		{
			movementDirectionSet = true;
		}



		yield return null;
	}


	private void AttackPlayer()
	{
			// If we are within attack range we either want to stop the enemy completely or 
			// slow them down? 

		Agent.SetDestination(transform.position);

		UpperBody.transform.LookAt(Target);

		// If the bot isnt already attacking the player 
		if (!isAttackingPlayer == true)
		{
			Rigidbody rb = Instantiate(bulletPrefab, weaponFirePoint.position, Quaternion.identity).GetComponent<Rigidbody>();

			// Add force to the bullet (bullet velocity) 

			rb.AddForce(weaponFirePoint.forward * bulletVelocity, ForceMode.VelocityChange);


			isAttackingPlayer = true;
			Invoke(nameof(ResetAttack), attackWaitTimer);
		}
	}

	private void ResetAttack()
	{
		isAttackingPlayer = false;
	}
	#endregion

	#region Events


	/// <summary>
	///		Hit event 
	/// </summary>
	/// <param name="Character"></param>
	/// <param name="Damage"></param>
	private void OnHit(Transform Character, float Damage)
	{
		if (Character != transform)
		{
			return;
		}
		else
		{
			// do the health change 
		}
	}

		/// <summary>
		/// Death event 
		/// </summary>
		/// <param name="AI"></param>
	private void OnDeath(Transform AI)
	{
		if (AI != transform)
		{
			return;
		}



		GameObject death = Instantiate(deathPrefab, transform.position, deathPrefab.transform.rotation);

		Destroy(death, 3);

		gameObject.SetActive(false);
	}
	#endregion


	#region Debugging
		
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, viewDistanceAttackRange);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, viewDistanceAlertedRange);
	}

	#endregion
}

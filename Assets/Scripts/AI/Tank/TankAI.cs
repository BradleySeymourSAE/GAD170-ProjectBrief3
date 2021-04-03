using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class TankAI : MonoBehaviour
{
	public Transform PlayerTarget;
	

	public Transform TurretTransform;
	public Transform TurretFirePoint;
	public GameObject shellPrefab;
	public GameObject shellExplosionPrefab;
	
	public LayerMask GroundMask, PlayerMask;
	
	public NavMeshAgent Agent;

	/// <summary>
	///		Patrolling 
	/// </summary>
	public Vector3 movePoint;
	[SerializeField] private bool movePointSet;
	public float movementPointRange;
	
	/// <summary>
	///		Attacking 
	/// </summary>
	[SerializeField] public float attackWaitTime;
	[SerializeField] bool alreadyAttacking;

	/// <summary>
	///		States 
	/// </summary>
	/// 
	public float viewDistanceRange;
	public float viewDistanceFiringRange;
	[SerializeField] private bool playerInViewDistanceRange;
	[SerializeField] private bool playerInViewDistanceFiringRange;

	[SerializeField] Vector3 startPos;
	[SerializeField] Vector3 startLocalRot;
	[SerializeField] Quaternion startingRot;

	public bool enableSpawn = false;
	public TankHealth tankHealth = new TankHealth();
	public GameObject enemyDeathPrefab; 

	private void OnEnable()
	{
		FireModeEvents.OnObjectDestroyedEvent += OnDeath;
		FireModeEvents.OnDamageReceivedEvent += OnDamageEvent;
	}

	private void OnDisable()
	{
		FireModeEvents.OnObjectDestroyedEvent -= OnDeath;
		FireModeEvents.OnDamageReceivedEvent -= OnDamageEvent;
	}

	private void Awake()
	{
		Agent = GetComponent<NavMeshAgent>();
	}

	private void Start()
	{
		tankHealth.Setup(transform); // setup the enemy ai's health 

		startPos = transform.position;
		startingRot = transform.rotation;
		startLocalRot = transform.rotation.eulerAngles;
		enableSpawn = true;
	}


	private void Update()
	{
		playerInViewDistanceRange = Physics.CheckSphere(transform.position, viewDistanceRange, PlayerMask);
		playerInViewDistanceFiringRange = Physics.CheckSphere(transform.position, viewDistanceFiringRange, PlayerMask);


		if (!playerInViewDistanceRange && !playerInViewDistanceFiringRange)
		{
			Looking();
		}
		


		if (playerInViewDistanceRange && !playerInViewDistanceFiringRange)
		{
			Alerted();
		}


		if (playerInViewDistanceFiringRange && playerInViewDistanceRange)
		{
			Aggressive();
		}
	}

	private void Looking()
	{
		if (!movePointSet)
		{
			Search();
		}

		if (movePointSet)
		{
			Agent.SetDestination(movePoint);
		}


		Vector3 movementDistance = transform.position - movePoint;

		if (movementDistance.magnitude < 1f)
		{
			movePointSet = false;
		}

	}

	private void Search()
	{
		float randZ = Random.Range(-movementPointRange, movementPointRange);
		float randX = Random.Range(-movementPointRange, movementPointRange);

		movePoint = new Vector3(transform.position.x + randX, transform.position.y, transform.position.z + randZ);

		if (Physics.Raycast(movePoint, -transform.up, 2f, GroundMask))
		{
			movePointSet = true;
		}
	}

	private void Alerted()
	{
		Agent.SetDestination(PlayerTarget.position);
	}

	private void Aggressive()
	{
		// Stop the enemy 
		Agent.SetDestination(transform.position);

		TurretTransform.transform.LookAt(PlayerTarget);

		if (!alreadyAttacking)
		{

			// Shooting / Firing 

			Rigidbody rb = Instantiate(shellPrefab, TurretFirePoint.position, Quaternion.identity).GetComponent<Rigidbody>();

			rb.AddForce(TurretFirePoint.forward * 200f, ForceMode.Impulse);




			alreadyAttacking = true;
			Invoke(nameof(ResetAttack), attackWaitTime);
		}
	}

	private void ResetAttack()
	{
		alreadyAttacking = false;
	}

	private void EnableAggression()
	{
		enableSpawn = true;
	}


	private void OnDamageEvent(Transform TankReference, float DamageAmount)
	{
		if (TankReference != transform)
		{
			return;
		}
		else
		{
			// Apply Damage
			tankHealth.ApplyHealthChange(DamageAmount);
		}
	}

	/// <summary>
	///		Called when the tank is eliminated 
	/// </summary>
	/// <param name="CurrentTank"></param>
	private void OnDeath(Transform CurrentTank)
	{
		if (CurrentTank != transform)
		{
			return;
		}


		GameObject clone = Instantiate(enemyDeathPrefab, transform.position, enemyDeathPrefab.transform.rotation);
	
		Destroy(clone, 3);
	
		gameObject.SetActive(false);
	}




	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, viewDistanceFiringRange);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, viewDistanceRange);
	}


}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class TankAI : MonoBehaviour
{
	private Transform Target;
	

	public Transform TurretTransform;
	public Transform TurretFirePoint;
	public GameObject shellPrefab;
	public GameObject shellExplosionPrefab;
	
	public LayerMask GroundMask, PlayerMask;
	
	public NavMeshAgent Agent;

	/// <summary>
	///		Patrolling 
	/// </summary>
	public Vector3 movementDirection;
	[SerializeField] private bool movementDirectionSet;
	public float movementRange;

	private float maximumMovementRange = 10f;
	private float movementWaypointTimer = 3f;
	
	
	/// <summary>
	///		Attacking 
	/// </summary>
	[SerializeField] public float attackWaitTime;
	[SerializeField] bool alreadyAttacking;

	/// <summary>
	///		States 
	/// </summary>
	/// 
	public float viewDistanceAlertedRange;
	public float viewDistanceAttackRange;
	[SerializeField] private bool aiAlerted;
	[SerializeField] private bool aiAggressive;


	[SerializeField] private bool enableAIMovement = false;


	public TankHealth tankHealth = new TankHealth();
	public TankParticleEffects tankParticleEffects = new TankParticleEffects();
	public TankSoundEffects tankSoundEffects = new TankSoundEffects();
	
	public GameObject enemyDeathPrefab; 

	private void OnEnable()
	{
		FireModeEvents.OnObjectDestroyedEvent += OnDeath;
		FireModeEvents.OnDamageReceivedEvent += OnDamageEvent;
		FireModeEvents.OnWaveStartedEvent += EnableAI;
	}

	private void OnDisable()
	{
		FireModeEvents.OnObjectDestroyedEvent -= OnDeath;
		FireModeEvents.OnDamageReceivedEvent -= OnDamageEvent;
		FireModeEvents.OnWaveStartedEvent -= EnableAI;
	}

	private void Awake()
	{
		if (GetComponent<NavMeshAgent>() != null)
		{ 
			Agent = GetComponent<NavMeshAgent>();
			Agent.autoBraking = false;
		}


		if (FindObjectOfType<Tank>().transform != null)
		{ 
			Target = FindObjectOfType<Tank>().transform;
		}
	}

	private void Start()
	{
		tankHealth.Setup(transform); // setup the enemy ai's health
		tankParticleEffects.SetUpEffects(transform); // Setup the tanks particle effects
		tankParticleEffects.PlayDustTrails(true); // should play dust trail effects
		tankSoundEffects.Setup(transform);
		
		if (enableAIMovement)
		{
			EnableAI();
		}	
	}

	private void EnableMovement(bool Enable)
	{
		enableAIMovement = Enable;
	}

	private void EnableAI()
	{
		// tankWeapon.EnableWeaponFiring(true);
		
		EnableMovement(true);
	}


	private void Update()
	{

		if (!enableAIMovement)
		{
			return;
		}

		Agent.updatePosition = true;
		Agent.updateRotation = false;
		Agent.updateUpAxis = false;

		// Check the view distance 
		CheckViewDistance(transform.position);
		
		



		if (!aiAlerted && !aiAggressive)
		{
			SearchForPlayer();
		}
		else
		{
			if (aiAlerted && !aiAggressive)
			{
				Alerted();
			}
			else if (aiAggressive && aiAlerted)
			{
				Aggressive();
			}
		}
	}

	/// <summary>
	///		Checks for the player within the view distance that is set 
	/// </summary>
	/// <param name="_position"></param>
	private void CheckViewDistance(Vector3 _position)
	{
		aiAlerted = Physics.CheckSphere(_position, viewDistanceAlertedRange, PlayerMask);
		aiAggressive = Physics.CheckSphere(_position, viewDistanceAttackRange, PlayerMask);
	}


	#region AI States 
	private void SearchForPlayer()
	{
		if (!movementDirectionSet)
		{
			StartCoroutine(SetMovementWaypoint());
			return;
		}

		if (movementDirectionSet == true && movementDirection.magnitude <= 1f)
		{
			movementDirectionSet = false;
		}
		


		if (movementDirectionSet)
		{	
			Debug.Log("[TankAI.SearchForPlayer]: " + "Searching for player on direction " + movementDirection);
			Agent.SetDestination(movementDirection);
		}
	}

	private IEnumerator SetMovementWaypoint()
	{
		yield return new WaitForSeconds(movementWaypointTimer);

		float randomXPosition = Random.Range(1, maximumMovementRange);
		float randomZPosition = Random.Range(1, maximumMovementRange);


		movementDirection = new Vector3(transform.position.x + randomXPosition, transform.position.y, transform.position.z + randomZPosition);


		if (Physics.Raycast(movementDirection, -transform.up, 2f, GroundMask))
		{
			movementDirectionSet = true;
		}


		yield return null;
	}

	private void Search()
	{
		float randZ = Random.Range(-movementRange, movementRange);
		float randX = Random.Range(-movementRange, movementRange);

		movementDirection = new Vector3(transform.position.x + randX, transform.position.y, transform.position.z + randZ);

		if (Physics.Raycast(movementDirection, -transform.up, 2f, GroundMask))
		{
			movementDirectionSet = true;
		}
	}

	private void Alerted()
	{
		Agent.SetDestination(Target.position);
	}

	/// <summary>
	///		Within shooting distance 
	/// </summary>
	private void Aggressive()
	{
		// Stop the enemy 
		Agent.SetDestination(transform.position);

		TurretTransform.transform.LookAt(Target);

		if (!alreadyAttacking)
		{

			// Shooting / Firing 

			Rigidbody rb = Instantiate(shellPrefab, TurretFirePoint.position, Quaternion.identity).GetComponent<Rigidbody>();

			rb.AddForce(TurretFirePoint.forward * 200f, ForceMode.Impulse);




			alreadyAttacking = true;
			Invoke(nameof(ResetAttack), attackWaitTime);
		}
	}
	#endregion


	#region Events 
	private void ResetAttack()
	{
		alreadyAttacking = false;
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


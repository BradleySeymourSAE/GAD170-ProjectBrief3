using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[System.Serializable]
public class InfantryAI : MonoBehaviour
{ 
	private Transform Target;

	public Transform weaponFirePoint;
	public GameObject PrimaryHand;
	public GameObject PrimaryHide;
	public GameObject SecondaryHand;
	public GameObject SecondaryHide;
	public Transform UpperBody;
	public float bulletVelocity = 500f;
	public float bulletSpread = 5f;
	public float fireRate = 800f;
	
	
	public GameObject bulletPrefab;
	public GameObject deathPrefab;

	public LayerMask GroundMask, PlayerMask;

	private NavMeshAgent Agent;

	/// <summary>
	///		Patrolling
	/// </summary>
	public Vector3 movementDirection;
	public float movementRange;
	public float movementWaypointTimer = 3f;

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

	private bool enableAIMovement = false;


	public InfantryHealth infantryHealth = new InfantryHealth();
	public InfantryWeapons infantryWeapons = new InfantryWeapons();

	// public WeaponEffects weaponFx = new WeaponEffects();

	private Animator m_animator;


	private void OnEnable()
	{
		FireModeEvents.OnObjectDestroyedEvent += OnDeath;
		FireModeEvents.OnDamageReceivedEvent += OnHit;
		FireModeEvents.OnWaveStartedEvent += EnableMovement;
	}

	private void OnDisable()
	{
		FireModeEvents.OnObjectDestroyedEvent -= OnDeath;
		FireModeEvents.OnDamageReceivedEvent -= OnHit;
		FireModeEvents.OnWaveStartedEvent -= EnableMovement;
	}


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

		if (FindObjectOfType<Tank>() != null)
		{
			Target = FindObjectOfType<Tank>().transform;
		}

		movementDirectionSet = false;
	}

	private void Start()
	{
		infantryHealth.Setup(transform);
		infantryWeapons.Setup(transform);


		if (enableAIMovement)
		{
			EnableMovement();
		}
	}


	private void EnableMovement()
	{
		infantryWeapons.EnableWeaponFiring(true);

		enableAIMovement = true;
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


		
		CheckViewDistance(transform.position);

		// If the AI is not alerted or aggressive, They should be looking for a player (Setting a movement point) 
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
				AttackPlayer();
			}
		}

	}


	/// <summary>
	///		Checks for the player within the view distance that is set 
	/// </summary>
	/// <param name="pos"></param>
	private void CheckViewDistance(Vector3 pos)
	{
		aiAlerted = Physics.CheckSphere(pos, viewDistanceAlertedRange, PlayerMask);
		aiAggressive = Physics.CheckSphere(pos, viewDistanceAttackRange, PlayerMask);
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
	
		
		
		
		if (movementDirectionSet == true)
			{

				Debug.Log("[InfantryAI.SearchForPlayer]: " + "Searching for player on direction " + movementDirection);
				Agent.SetDestination(movementDirection);
			}	
	}

	private IEnumerator SetMovementWaypoint()
	{

		yield return new WaitForSeconds(movementWaypointTimer);

		float randomXPosition = Random.Range(1, maximumMovementRange);
		float randomZPosition = Random.Range(1, maximumMovementRange);

		

		movementDirection = new Vector3(transform.position.x + randomXPosition, 0, transform.position.z + randomZPosition);


		if (Physics.Raycast(movementDirection, -transform.up, 2f, GroundMask))
		{
			movementDirectionSet = true;
		}



		yield return null;
	}


	private void Alerted()
	{
		Agent.SetDestination(Target.position);
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
			Rigidbody rb = Instantiate(bulletPrefab, weaponFirePoint.position, bulletPrefab.transform.rotation).GetComponent<Rigidbody>();

			// Add force to the bullet (bullet velocity) 

			rb.AddForce(weaponFirePoint.forward * bulletVelocity, ForceMode.Impulse);


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
			infantryHealth.ApplyHealthChange(Damage);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[System.Serializable]
public class InfantryAI : MonoBehaviour
{ 
	private Transform Target;
	private Transform m_InfantryAIBody;

	public Transform WeaponFiringPoint;
	public GameObject PrimaryHand;
	public GameObject PrimaryHide;
	public GameObject SecondaryHand;
	public GameObject SecondaryHide;
	public float bulletVelocity = 500f;
	public float bulletSpread = 5f;
	public float fireRate = 800f;
	
	
	public GameObject bulletPrefab;
	[SerializeField] private float bulletResetTimer = 3f;
	public GameObject deathPrefab;

	public LayerMask GroundMask, PlayerMask;

	private NavMeshAgent Agent;

	/// <summary>
	///		Patrolling
	/// </summary>
	public Vector3 movementDirection;
	public float movementRange;
	public float movementWaypointTimer = 3f;

	/// <summary>
	///		Attacking 
	/// </summary>
	[SerializeField] private float attackWaitTimer;
	[SerializeField] private bool isAttackingPlayer;

	/// <summary>
	///		States
	/// </summary>
	public float viewDistanceAttackRange = 75f;
	private bool aiAlerted;
	private bool aiAggressive;

	private bool enableAIMovement = false;

	public InfantryHealth infantryHealth = new InfantryHealth();
	public InfantryWeapons infantryWeapons = new InfantryWeapons();

	// public WeaponEffects weaponFx = new WeaponEffects();

	[Header("Animation")]
	[SerializeField] private Animator m_animator;
	float VelocityZ;
	float VelocityX;
	float Acceleration;
	float Deceleration;
	float maximumWalkingVelocity;
	float maximumRunningVelocity;

	private void OnEnable()
	{
		FireModeEvents.OnObjectDestroyedEvent += OnDeath;
		FireModeEvents.OnReceivedDamageEvent += OnHit;
		FireModeEvents.OnWaveStartedEvent += EnableMovement;
	}

	private void OnDisable()
	{
		FireModeEvents.OnObjectDestroyedEvent -= OnDeath;
		FireModeEvents.OnReceivedDamageEvent -= OnHit;
		FireModeEvents.OnWaveStartedEvent -= EnableMovement;
	}


	private void Awake()
	{

		if (GetComponent<Transform>())
		{
			m_InfantryAIBody = GetComponent<Transform>();
		}


		if (m_InfantryAIBody.GetComponent<NavMeshAgent>())
		{
			Agent = m_InfantryAIBody.GetComponent<NavMeshAgent>();
			Agent.autoBraking = false;
		}

		if (m_InfantryAIBody.GetComponent<Animator>())
		{
			m_animator = m_InfantryAIBody.GetComponent<Animator>();

		}
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

		CheckAttackDistance(transform.position);

		if (Target == null)
		{
			Target = FindObjectOfType<MainPlayerTank>().transform;
		}
		


		// If there is a target player, we just want to move towards the player 
		if (Target)
		{
			Alerted();
		}
		else if (Target && aiAggressive == true)
		{
			AttackPlayer();
		}
	}


	/// <summary>
	///		Checks for the player within the view distance that is set 
	/// </summary>
	/// <param name="pos"></param>
	private void CheckAttackDistance(Vector3 pos)
	{
		aiAggressive = Physics.CheckSphere(pos, viewDistanceAttackRange, PlayerMask);
	}


	#region AI States 

	private void Alerted()
	{
		aiAlerted = true;
		transform.LookAt(Target);
		Agent.SetDestination(Target.position);
	}


	/// <summary>
	///		Attacks the player 
	/// </summary>
	private void AttackPlayer()
	{
		// Set the destination to stop moving towards the player 
		Agent.SetDestination(transform.position);
		transform.LookAt(Target);


		// If the bot isnt already attacking the player 
		if (!isAttackingPlayer)
		{
			// attack the player 
			isAttackingPlayer = true;

			GameObject bulletClone = Instantiate(bulletPrefab, WeaponFiringPoint.position, bulletPrefab.transform.rotation);

			if (bulletClone.GetComponent<Rigidbody>())
			{
				Rigidbody rb = bulletClone.GetComponent<Rigidbody>();

				rb.AddForce(WeaponFiringPoint.forward * bulletVelocity, ForceMode.Force);
			}


			Debug.DrawLine(WeaponFiringPoint.forward, Vector3.forward, Color.blue);


			Destroy(bulletClone, bulletResetTimer);
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
			Debug.Log("[InfantryAI.OnHit]: " + "Applying damage to infantry character " + Damage);
			infantryHealth.ApplyHealthChange(Damage);
		}
	}

		/// <summary>
		///		Destroys the AI character 
		/// </summary>
		/// <param name="AI"></param>
	private void OnDeath(Transform AI)
	{
		if (AI != transform)
		{
			return;
		}

		GameObject death = Instantiate(deathPrefab, transform.position, deathPrefab.transform.rotation);
		Destroy(death, 2f);

		// set the current game object to be inactive 
		gameObject.SetActive(false);
		
		// Call the on object destroyed event 
		Debug.Log("[InfantryAI.OnDeath]: " + "Invoking on object destroyed event!");
		FireModeEvents.OnObjectDestroyedEvent?.Invoke(AI);
		
	}
	#endregion


	#region Debugging
		
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, viewDistanceAttackRange);
	}

	#endregion
}

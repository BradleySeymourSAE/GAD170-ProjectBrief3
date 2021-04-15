#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#endregion




/// <summary>
///     AI Tank Class 
/// </summary>
public class AI : MonoBehaviour
{

	private NavMeshAgent m_Agent;

	private Transform m_MainPlayerReference;

	/// <summary>
	///		 Handles the AI Movement 
	/// </summary>
	public AIMovement Movement;

	/// <summary>
	///		Handles the AI Weapons Class 
	/// </summary>
	public AIWeapon Weapons;

	public GameObject explosionPrefab;

	public LayerMask GroundLayer, PlayerLayer;

	public float attackRange;
	public bool playerInAttackRange;

	private bool enableAI;


	#region Unity References 

	private void OnEnable()
	{
		FireModeEvents.GameStartedEvent += Initialize;


		FireModeEvents.HandleAIDamageEvent += HandleDamage;
		FireModeEvents.HandleAIDestroyedEvent += HandleDeath;
	}

	private void OnDisable()
	{
		FireModeEvents.GameStartedEvent -= Initialize;

		FireModeEvents.HandleAIDamageEvent -= HandleDamage;
		FireModeEvents.HandleAIDestroyedEvent -= HandleDeath;
	}


	private void Awake()
	{
		m_MainPlayerReference = FindObjectOfType<MainPlayerTank>().transform;
		m_Agent = GetComponent<NavMeshAgent>();

		m_Agent.updatePosition = true;
		m_Agent.updateRotation = false;
		m_Agent.updateUpAxis = false;
	}

	/// <summary>
	///		Enables the AI character to move around 
	/// </summary>
	/// <param name="Enable"></param>
	private void EnableAI(bool Enable)
	{
		enableAI = Enable;
	}

	/// <summary>
	///		Sets up the ai class references 
	/// </summary>
	private void Initialize()
	{
		
		Movement.Setup(this, m_Agent);
		Weapons.Setup(this);
		
		
		
		EnableAI(true);
	}



	private void Update()
	{
		// If the AI Character is not enabled we want to return 
		if (!enableAI)
		{
			return;
		}

		playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, PlayerLayer);


		if (!playerInAttackRange && m_MainPlayerReference)
		{
			Movement.Chase();
		}
		else if (playerInAttackRange)
		{
			Movement.Attack();
		}
	}


	private void HandleDamage(float amount)
	{
		Debug.Log("[AI.HandleDamage]: " + "AI has taken damage " + amount);



	}
	

	private void HandleDeath(GameObject DeadAI)
	{
		if (DeadAI.transform != transform)
		{
			return;
		}

		GameObject deathClone = Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);

		Destroy(deathClone, 2); 

		DeadAI.SetActive(false);

	}


	#endregion


	[System.Serializable]
	public class AIWeapon
	{ 
		public Transform WeaponFirePoint;

		public GameObject shellProjectilePrefab;

		public float maximumBulletVelocity = 500f;

		private float currentBulletVelocity; 


		private AI m_AIReference;


		
		public void Setup(AI AI)
		{
			m_AIReference = AI;
			currentBulletVelocity = maximumBulletVelocity;
		}
	
	
		public void StartFiringWeapon()
		{
			Rigidbody rb = Instantiate(shellProjectilePrefab, WeaponFirePoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>();

			rb.AddForce(WeaponFirePoint.forward * currentBulletVelocity, ForceMode.Impulse);
		}
	}


	[System.Serializable]
	public class AIMovement
	{


		public float timeBeforeNextAttack = 1f;

		public Transform PrimaryWeaponTransform;

		private Transform PlayerTarget;

		private AI m_AI;

		private NavMeshAgent m_Agent;

		private bool alreadyAttackedPlayer;

		public void Setup(AI AI, NavMeshAgent Agent)
		{
			m_AI = AI;
			m_Agent = Agent;
			PlayerTarget = m_AI.m_MainPlayerReference;
		}

		public void Chase()
		{
			m_Agent.SetDestination(PlayerTarget.position);
		}

		public void Attack()
		{
			// STOP the enemy AI in its position 
			m_Agent.SetDestination(m_AI.transform.position);

			// Look at player, as well as rotate turret transform to look at the player 
			m_AI.transform.LookAt(PlayerTarget);
			PrimaryWeaponTransform.LookAt(PlayerTarget);

			
			if (!alreadyAttackedPlayer)
			{

				m_AI.Weapons.StartFiringWeapon();


				alreadyAttackedPlayer = true;
				m_AI.StartCoroutine(ResetWeapon());
			}
		}


		/// <summary>
		///		Resets weapon 
		/// </summary>
		IEnumerator ResetWeapon()
		{
			yield return new WaitForSeconds(timeBeforeNextAttack);
			alreadyAttackedPlayer = false;

			yield return null;
		}
	}
}

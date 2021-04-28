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
	/// <summary>
	///		Reference to the AI Navigation Mesh Agent 
	/// </summary>
	private NavMeshAgent m_Agent;

	/// <summary>
	///		Reference to the MainPlayerTank Transform 
	/// </summary>
	private Transform m_MainPlayerReference;

	/// <summary>
	///		 Handles the AI Movement 
	/// </summary>
	public AIMovement Movement;

	/// <summary>
	///		Handles the AI Weapons Class 
	/// </summary>
	public AIWeapon Weapons;

	/// <summary>
	///		Handles the AI Health   
	/// </summary>
	public AIHealth Health;

	/// <summary>
	///		The explosion prefab for when an AI character is defeated 
	/// </summary>
	public GameObject explosionPrefab;

	/// <summary>
	///		What is the ground Layer? 
	///		What is the Player Layer? 
	/// </summary>
	public LayerMask GroundLayer, PlayerLayer;

	/// <summary>
	///		The range that the AI will begin attacking the player 
	/// </summary>
	public float attackRange;

	/// <summary>
	///		Is the player currently in attack range? 
	/// </summary>
	public bool playerInAttackRange;


	/// <summary>
	///		Is the AI currently able to move / enabled? 
	/// </summary>
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
		
		if (GetComponent<NavMeshAgent>())
		{
			m_Agent = GetComponent<NavMeshAgent>();
			m_Agent.updatePosition = true;
			m_Agent.updateRotation = false;
			m_Agent.updateUpAxis = false;
		}
		else
		{
			Debug.LogWarning("[AI.Awake]: " + "Could not find Navigation Mesh Agent on AI!");
		}
	}

	private void Start()
	{
		Movement.Setup(transform, m_Agent);
		Weapons.Setup(transform);
		Health.Setup(transform);

		Initialize();
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


	#endregion



	#region Private Methods 

	/// <summary>
	///		Sets up the ai class references 
	/// </summary>
	private void Initialize()
	{
		Movement.EnableAIMovement(true);
		Weapons.EnableWeaponFiring(true);

		enableAI = true;
	}

	private void HandleDamage(Transform EnemyAI, float amount)
	{
		if (EnemyAI != transform)
		{
			// Debug.LogWarning("[AI.HandleDamage]: " + "AI could not be found!");
			return;
		} 
		else
		{ 
			Debug.Log("[AI.HandleDamage]: " + "AI has taken damage? " + amount);

			Health.SetHealth(amount);
		}
	}
	

	private void HandleDeath(Transform DefeatedAI)
	{
		if (DefeatedAI != transform)
		{
			 // Debug.LogWarning("[AI.HandleDeath]: " + "Not the correct AI! " + DeadAI);
			return;
		}

		AudioManager.Instance.PlaySound(GameAudio.PlayerDeathOOFT);

		GameObject deathClone = Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);
		
		Destroy(deathClone, 2); 
		
		gameObject.SetActive(false);
	}

	#endregion

	#region Public Data Classes 
	[System.Serializable]
	public class AIWeapon
	{ 
		public Transform WeaponFirePoint;

		public GameObject shellProjectilePrefab;

		[Min(850f)] public float maximumBulletVelocity = 1000f;

		private float currentBulletVelocity; 

		[SerializeField] private bool m_WeaponAllowedToFire;

		private Transform m_AIReference;

		/// <summary>
		///		Can our AI character fire its weapon? 
		/// </summary>
		/// <param name="ShouldEnableWeaponFiring"></param>
		public void EnableWeaponFiring(bool ShouldEnableWeaponFiring) => m_WeaponAllowedToFire = ShouldEnableWeaponFiring;
		
		public void Setup(Transform AIReference)
		{
			m_AIReference = AIReference;
			currentBulletVelocity = maximumBulletVelocity;

			EnableWeaponFiring(false);
		}
	
	
		public void StartFiringWeapon()
		{
			if (!m_WeaponAllowedToFire)
			{
				Debug.Log("Cant fire weapon!");
				return;
			}

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

		private Transform m_AIReference;

		private NavMeshAgent m_Agent;

		private Rigidbody m_Rigidbody;

		private AIAudio m_AudioEffects = new AIAudio();

		private AIEffects m_Effects = new AIEffects();

		[SerializeField] private bool alreadyAttackedPlayer;

		[SerializeField] private bool m_EnableAIMovement;

		public void Setup(Transform AIReference, NavMeshAgent Agent)
		{
			m_AIReference = AIReference;
			m_Agent = Agent;
			PlayerTarget = m_AIReference.GetComponent<AI>().m_MainPlayerReference;

			if (m_AIReference.GetComponent<Rigidbody>())
			{
				m_Rigidbody = m_AIReference.GetComponent<Rigidbody>();
			}
			else
			{
				Debug.LogWarning("[AI.AIMovement.Setup]: " + "No rigidbody could be found for the current AI character!");
			}

			m_AudioEffects.Setup(m_AIReference);
			m_Effects.Setup(m_AIReference);
			m_Effects.StartDustEffects(true);
			EnableAIMovement(false);
		}

		public void EnableAIMovement(bool ShouldEnable) => m_EnableAIMovement = ShouldEnable;

		public void Chase()
		{

			if (m_EnableAIMovement == false)
			{
				// If the AI is not enabled we want to return....
				return;
			}

			// Set the agents destination to the players position 
			m_Agent.SetDestination(PlayerTarget.position);

			// Look at the player target 
			m_AIReference.transform.LookAt(PlayerTarget);
			
			// Look at the player with the primary weapon 
			PrimaryWeaponTransform.LookAt(PlayerTarget);
		
			// I dont know if this will work but YOLO
			m_AudioEffects.StartEngine(m_Agent.velocity.x, m_Agent.velocity.z);

		}

		public void Attack()
		{
			// STOP the enemy AI in its position 
			m_Agent.SetDestination(m_AIReference.transform.position);

			// Look at player, as well as rotate turret transform to look at the player 
			m_AIReference.transform.LookAt(PlayerTarget);
			PrimaryWeaponTransform.LookAt(PlayerTarget);

			
			if (!alreadyAttackedPlayer)
			{

				m_AIReference.GetComponent<AI>().Weapons.StartFiringWeapon();


				alreadyAttackedPlayer = true;
				m_AIReference.GetComponent<AI>().StartCoroutine(ResetWeapon());
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

	[System.Serializable]
	public class AIHealth
	{

		#region Public Variables 

		/// <summary>
		///		Minimum health value for an ai character 
		/// </summary>
		public float MinimumHealth = 0;
		
		/// <summary>
		///		The maximum health allowed for an AI character
		/// </summary>
		public float MaximumHealth = 100;

		/// <summary>
		///		Is the AI character currently dead? 
		/// </summary>
		public bool isCurrentlyDead = true;


		#endregion

		#region Private Variables 

		/// <summary>
		///		The current health remaining for an AI character 
		/// </summary>
		[Header("Current Health Status")]
		[SerializeField] private float m_CurrentHealth;
		
		/// <summary>
		///		Reference to the AI character 
		/// </summary>
		private Transform m_CurrentAI;

		#endregion


		#region Public Methods 

		/// <summary>
		///		Getter & Setter for the AI characters Health 
		/// </summary>
		public float CurrentAIHealth
		{ 
			get
			{
				return m_CurrentHealth;
			}
			set
			{
				m_CurrentHealth = value;
				m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, MinimumHealth, MaximumHealth);

				if (m_CurrentHealth <= 0)
				{
					isCurrentlyDead = true;

					Debug.LogWarning("[AI.CurrentAIHealth]: "+ "Invoking Increase Enemies Remaining Event - AI Killed!");
					FireModeEvents.IncreaseEnemiesRemainingEvent?.Invoke(-1);

					Debug.Log("[AI.CurrentAIHealth]: " + "Invoking Handle AI Destroyed Event");
					FireModeEvents.HandleAIDestroyedEvent?.Invoke(m_CurrentAI);
				}
				else
				{
					isCurrentlyDead = false;
				}
			}
		}

		/// <summary>
		///		Sets the AI characters health 
		///		If a positive amount - Adds Health to the AI player 
		///		If a negative amount - Removes health from the ai player 
		/// </summary>
		/// <param name="amount"></param>
		public void SetHealth(float amount)
		{
			// Debug.Log("[AI.AIHealth.SetHealth]: " + "Setting AI TANKS Health: " + amount);

			CurrentAIHealth += amount;
		}
	
		/// <summary>
		///		Sets up the AI Characters Health 
		/// </summary>
		/// <param name="AI"></param>
		public void Setup(Transform CurrentAI)
		{
			m_CurrentAI = CurrentAI;

			CurrentAIHealth = MaximumHealth;
		}

		#endregion
	
	}


	public class AIAudio
	{ 
		
		public bool shouldLoop = true;

		private AudioSource Engine;
		private AudioClip EngineIdle;
		private AudioClip EngineDriving;
	
		public void Setup(Transform AIReference)
		{
			if (AIReference.GetComponent<AudioSource>() != null)
			{
				Engine = AIReference.GetComponent<AudioSource>();

				Engine.loop = shouldLoop;
				EngineIdle = AudioManager.Instance.GetAudioClip(GameAudio.T90_EngineIdle);
				EngineDriving = AudioManager.Instance.GetAudioClip(GameAudio.T90_EngineDriving);
			}
			else
			{
				Debug.LogWarning("[AI.AIAudio.Setup]: " + "Audio Instance could not be found for AI character!");
			}
		}


		public void StartEngine(float Moving, float Rotating)
		{
			if (Mathf.Abs(Moving) < 0.1f && Mathf.Abs(Rotating) < 0.1f)
			{
				if (Engine.clip != EngineIdle)
				{
					Engine.clip = EngineIdle;
					Engine.Play();
				}
			}
			else 
			{
				if (Engine.clip != EngineDriving) 
				{ 
					Engine.clip = EngineDriving;
					Engine.Play();
				}
			}
		}
	}

	public class AIEffects
	{ 
		
		private ParticleSystem[] DustEffects = new ParticleSystem[] { };

		public void Setup(Transform AIReference)
		{
			DustEffects = AIReference.GetComponentsInChildren<ParticleSystem>();
		}

		public void StartDustEffects(bool EnableDustEffects)
		{
			for (int i = 0; i < DustEffects.Length; i++)
			{
				if (EnableDustEffects)
				{
					DustEffects[i].Play();
				}
				else
				{
					DustEffects[i].Stop();
				}
			}
		}
	}
	#endregion

}

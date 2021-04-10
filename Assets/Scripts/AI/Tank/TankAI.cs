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
	
	private NavMeshAgent Agent;

	/// <summary>
	///		Patrolling 
	/// </summary>
	public Vector3 movementDirection = new Vector3(0,0,0);
	[SerializeField] private bool movementDirectionSet;
	public float movementRange = 5;
	public float bulletSpeed = 500f;
	private float shellResetTimer = 2f;
	
	
	/// <summary>
	///		Attacking 
	/// </summary>
	[SerializeField] public float attackWaitTime;
	[SerializeField] bool alreadyAttacking;

	
	/// <summary>
	///		The distance the AI will attack you from 
	/// </summary>
	public float viewDistanceAttackRange = 65f;
	[SerializeField] private bool aiAggressive;
	[SerializeField] private bool isAlerted;

	/// <summary>
	///		Whether the enemy AI should be able to move yet 
	/// </summary>
	private bool enableAIMovement = false;

	public TankHealth tankHealth = new TankHealth();
	public TankParticleEffects tankParticleEffects = new TankParticleEffects();
	public TankSoundEffects tankSoundEffects = new TankSoundEffects();
	public GameObject tankDeathPrefab; 

	private void OnEnable()
	{
		FireModeEvents.OnObjectDestroyedEvent += OnDeath;
		FireModeEvents.OnReceivedDamageEvent += OnDamageEvent;
		FireModeEvents.OnWaveStartedEvent += EnableAI;
	}

	private void OnDisable()
	{
		FireModeEvents.OnObjectDestroyedEvent -= OnDeath;
		FireModeEvents.OnReceivedDamageEvent -= OnDamageEvent;
		FireModeEvents.OnWaveStartedEvent -= EnableAI;
	}

	private void Awake()
	{
		if (transform.GetComponent<NavMeshAgent>())
		{ 
			Agent = transform.GetComponent<NavMeshAgent>();
			Agent.autoBraking = false;
			Agent.updatePosition = true;
			Agent.updateRotation = false;
			Agent.updateUpAxis = false;
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
		
		if (FindObjectOfType<MainPlayerTank>())
		{
			Target = FindObjectOfType<MainPlayerTank>().transform;
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

		if (Target == null && FindObjectOfType<MainPlayerTank>())
		{
			Target = FindObjectOfType<MainPlayerTank>().transform;
		}

		// Check the view distance 
		CheckViewDistanceAttackRange(transform.position);
	
		// If there is a target, move towards the target 
		if (Target)
		{
			// GO the to the target
			Alerted();
		}
		// Otherwise if there is a target but the player is in range of attack
		else if (Target && aiAggressive)
		{
			// Attack the player 
			Aggressive();
		}
	}

	/// <summary>
	///		Checks for the player within the view distance that is set 
	/// </summary>
	/// <param name="_position"></param>
	private void CheckViewDistanceAttackRange(Vector3 _position)
	{
		aiAggressive = Physics.CheckSphere(_position, viewDistanceAttackRange, PlayerMask);
	}

	/// <summary>
	///		Alerts the tank 
	/// </summary>
	private void Alerted()
	{
		transform.LookAt(Target);
		TurretTransform.LookAt(Target);
		
		// Go to the players position 
		Agent.SetDestination(Target.position);
	}

	/// <summary>
	///		Within shooting distance 
	/// </summary>
	private void Aggressive()
	{
		// Stop the enemy 
		transform.LookAt(Target);
		TurretTransform.transform.LookAt(Target);
		Agent.SetDestination(transform.position);



		if (!alreadyAttacking)
		{

			// Shooting / Firing
			GameObject shellClone = Instantiate(shellPrefab, TurretFirePoint.position, TurretFirePoint.rotation);

			if (shellClone.GetComponent<Rigidbody>())
			{
				Rigidbody rb = shellClone.GetComponent<Rigidbody>();

				rb.AddForce(TurretFirePoint.forward * bulletSpeed, ForceMode.Impulse);
			}

			
			
			Destroy(shellClone, shellResetTimer);


			alreadyAttacking = true;
			Invoke(nameof(ResetAttack), attackWaitTime);
		}
	}

	/// <summary>
	///		Resets the tanks attack!
	/// </summary>
	private void ResetAttack()
	{
		alreadyAttacking = false;
	}


	#region Events 

	/// <summary>
	///		Handles when the tank is hit! 
	/// </summary>
	/// <param name="TankReference"></param>
	/// <param name="DamageAmount"></param>
	private void OnDamageEvent(Transform TankReference, float DamageAmount)
	{
		if (TankReference != transform)
		{
			return;
		}
		else
		{
			// Apply Damage
			Debug.Log("[TankAI.OnDamageEvent]: " + "Enemy tank has been hit..." + DamageAmount);
			tankHealth.ApplyHealthChange(DamageAmount);
		}
	}

	/// <summary>
	///		Handles then the AI tank is dead 
	/// </summary>
	/// <param name="CurrentTank"></param>
	private void OnDeath(Transform CurrentTank)
	{
		if (CurrentTank != transform)
		{
			return;
		}


		GameObject clone = Instantiate(tankDeathPrefab, transform.position, tankDeathPrefab.transform.rotation);
	
		Destroy(clone, 3);
	
		gameObject.SetActive(false);


		// Invoke on death.. 
		FireModeEvents.OnObjectDestroyedEvent?.Invoke(CurrentTank);
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


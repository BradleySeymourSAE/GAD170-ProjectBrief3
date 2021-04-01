using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ventiii.DevelopmentTools;


/// <summary>
///		The main Infantry AI Class WIP
/// </summary>
public class InfantryAI_Main : MonoBehaviour
{
	private bool enableCharacterMovement = false;
	
	public InfantryAI_Pathfinder infantryPathfinder = new InfantryAI_Pathfinder();
	public InfantryAI_Health infantryHealth = new InfantryAI_Health();
	public InfantryAI_Weapon infantryPrimaryWeapon = new InfantryAI_Weapon();
	public InfantryAI_SoundEffects infantrySoundEffects = new InfantryAI_SoundEffects();
	public InfantryAI_ParticleEffects infantryParticleEffects = new InfantryAI_ParticleEffects();
	
	public GameObject danceCharacterPrefab; // easter egg for making the infantry character dance lol
	public GameObject deathInfantryPlayerExplodedPrefab; // the exploded infantry prefab xD 

	private Vector3 startPosition;
	private Quaternion startRotation;

	private void Update()
	{
		infantryPathfinder.HandleMovement();
	}

	private void Start()
	{
		infantryPathfinder.Setup(transform); // call pathfinding setup 
		infantryHealth.Setup(transform); // call health setup 
		infantryPrimaryWeapon.Setup(); // call primary weapon setup 

		startPosition = transform.position;
		startRotation = transform.rotation;

		if (enableCharacterMovement)
		{
			EnableAI();
		}
	}

	/// <summary>
	///		On Enable Event 
	/// </summary>
	private void OnEnable()
	{
		FieldOfFireEvents.OnGameStartedEvent += EnableAI;
		FieldOfFireEvents.OnEnemyAIInfantryDeathEvent += Death;
		FieldOfFireEvents.EnemyAIReceivedDamageEvent += DamageReceived;
	}

	/// <summary>
	///		On Disabled Event 
	/// </summary>
	private void OnDisable()
	{
		FieldOfFireEvents.OnGameStartedEvent -= EnableAI;
		FieldOfFireEvents.OnEnemyAIInfantryDeathEvent -= Death;
		FieldOfFireEvents.EnemyAIReceivedDamageEvent -= DamageReceived;
	}

	/// <summary>
	///		Enables the Enemy AI 
	/// </summary>
	private void EnableAI()
	{
		infantryPathfinder.EnablePathfinding(true);
		infantryPrimaryWeapon.EnableAggression(true);
		infantryPrimaryWeapon.EnableFiring(true);
	}

	/// <summary>
	///		Handles when an infantry AI infantry characters dies
	/// </summary>
	/// <param name="EnemyInfantryAI"></param>
	private void Death(Transform EnemyInfantryAI)
	{
		// If the enemy that we are trying to destroy isnt this current enemy 
		if (EnemyInfantryAI != transform)
		{
			// Then we want to return 
			return;
		}
	
		
		// Otherwise, Set the player to explode HAHAHA
	
		GameObject clonedInfantry = Instantiate(deathInfantryPlayerExplodedPrefab, transform.position, deathInfantryPlayerExplodedPrefab.transform.rotation);
		Destroy(clonedInfantry, 2); // clean up particle effect 
		gameObject.SetActive(false); // remove infantry character from the scene 
	}

	/// <summary>
	///		Handles when an infantry player receives damage 
	/// </summary>
	/// <param name="EnemyAIInfantry"></param>
	/// <param name="Damage"></param>
	private void DamageReceived(Transform EnemyAIInfantry, float Damage)
	{
		Debug.Log("[EnemyAI_Infantry.DamageReceived]: " + "Applying damage to enemy infantry character!");

		// Make sure this transform is the correct one 
		if (EnemyAIInfantry != transform)
		{
			Debug.Log("[EnemyAI_Infantry.DamageReceived]: " + "Thats not the correct enemy infantry ai character!");
			return;
		}
		else
		{
			Debug.Log("[EnemyAI_Infantry.DamageReceived]: " + "Enemy AI has taken damage " + Damage);

			infantryHealth.ApplyHealth(Damage);
		}
	}
}

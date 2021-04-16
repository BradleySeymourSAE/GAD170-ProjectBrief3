#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion


[System.Serializable]
/// <summary>
///     Main Player Class 
/// </summary>
public class MainPlayerTank : MonoBehaviour
{

	#region Public Variables 

	public PlayerMovement Movement = new PlayerMovement();

	public PlayerWeapons Weapons = new PlayerWeapons();

	public PlayerHealth Health = new PlayerHealth();

	public PlayerInput Controls = new PlayerInput();

	public bool enablePlayerControl = false;

	#endregion

	#region Private Variables 

	private bool cursorLocked = false;

	#endregion

	#region Unity References 

	private void OnEnable()
	{

		FireModeEvents.SpawnPlayerEvent += EnablePlayerInput;
		FireModeEvents.HandlePlayerDamageEvent += ChangeHealth;
	}

	private void OnDisable()
	{

		FireModeEvents.SpawnPlayerEvent -= EnablePlayerInput;
		FireModeEvents.HandlePlayerDamageEvent -= ChangeHealth;
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		Movement.Setup(transform);
		Weapons.Setup(transform);
		Health.Setup(transform);



		if (enablePlayerControl)
		{
			EnablePlayerInput();
		}
	}

	private void Update()
	{

		HandleCursorLocking();

		if (!enablePlayerControl)
		{
			return;
		}


		Movement.AimPrimaryWeapon();
	}

	private void FixedUpdate()
	{
		if (!enablePlayerControl)
		{
			return;
		}


		Movement.SetMovementInput(Controls.GetUserInput(PlayerInput.UserInput.Move), Controls.GetUserInput(PlayerInput.UserInput.Rotate));

		Weapons.SetWeaponFiringInput(Controls.GetUserInput(PlayerInput.UserInput.Fire));
	}

	#endregion

	#region Private Methods 

	private void EnablePlayerInput()
	{
		Movement.EnablePlayerMovementInput(true);
		Weapons.EnableWeapons(true);
	}


	private void HandleCursorLocking()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !cursorLocked)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			cursorLocked = true;
		}
		else if (Input.GetKeyDown(KeyCode.Escape) && cursorLocked)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			cursorLocked = false;
		}
	}


	private void ChangeHealth(Transform Player, float Amount)
	{
		if (!Player.GetComponent<MainPlayerTank>())
		{
			Debug.Log("Not the correct player!");
			return;
		}
		else
		{
			Debug.Log("Setting Health!");
			Health.SetHealth(Amount);
		}
	}
	#endregion

	#region Public Data Classes 

	/// <summary>
	///		Handles input received by our player 
	/// </summary>
	[System.Serializable]
	public class PlayerInput
	{

		#region Public Variables 

		public enum UserInput { Move, Rotate, Fire };

		public KeyCode Forward = KeyCode.W;
		public KeyCode Backwards = KeyCode.S;
		public KeyCode Left = KeyCode.A;
		public KeyCode Right = KeyCode.D;
		public KeyCode MouseFire = KeyCode.Mouse0;

		#endregion

		#region private Variables 

		private bool leftMouseButtonPressed = false;

		#endregion

		#region Public Methods

		public float GetUserInput(UserInput Code)
		{
			float value = 0;


			switch (Code)
			{
				case UserInput.Move:
					{
						if (Input.GetKey(Forward))
						{
							value = 1;
						}
						else if (Input.GetKey(Backwards))
						{
							value = -1;
						}
						break;
					}
				case UserInput.Rotate:
					{ 
						if (Input.GetKey(Right))
						{
							value = 1;
						}
						else if (Input.GetKey(Left))
						{
							value = -1;
						}
						break;
					}
				case UserInput.Fire:
					{ 
						if (Input.GetKeyDown(MouseFire))
						{
							value = 1;
							leftMouseButtonPressed = true;
						}
						else if (Input.GetKeyUp(MouseFire) && leftMouseButtonPressed == true)
						{
							leftMouseButtonPressed = false;
							value = -1;
						}
						break;
					}
			}


			return value;
		}

		#endregion

	}


	/// <summary>
	///		Handles movement for our player 
	/// </summary>
	[System.Serializable]
	public class PlayerMovement
	{
		#region Public Variables 
		
		[Header("Movement Settings")]
		public Transform MainTurretParent;

		public float MovementSpeed = 16;
		
		public float TurningSpeed = 50;

		public float sensitivity = 50f;

		public float sensitivityMultiplier = 1;


		#endregion

		#region Private Variables 

		[Header("Mouse Settings")]
		[SerializeField] private float minimumTurretRotationY = -0.5f;
		
		[SerializeField] private float maximumTurretRotationY = 7f;

		[HideInInspector] public float clampedCameraXRotation;
		[HideInInspector] public float audioFadeDuration = 1;
		[HideInInspector] public float turretAimingVolumeMin = 0, turretAimingVolumeMax = 100;

		[SerializeField] private float MouseX, MouseY;

		[SerializeField] private Transform m_PlayerRef;

		private Rigidbody m_Rigidbody;

		private TankEffects m_Effects = new TankEffects();

		public TankAudio m_AudioEffects = new TankAudio();

		[SerializeField] private Transform m_CameraReference;

		[SerializeField] private bool m_EnablePlayerMovementInput;

		#endregion

		#region Public Methods 
		
		public void Setup(Transform Player)
		{
			m_PlayerRef = Player;

			if (m_PlayerRef.GetComponent<Rigidbody>())
			{
				m_Rigidbody = m_PlayerRef.GetComponent<Rigidbody>();
			}
			else
			{
				Debug.LogError("[MainPlayerTank.PlayerMovement.Setup]: " + "No Rigidbody could be found on the player!");
			}

			if (m_PlayerRef.GetComponentInChildren<Camera>())
			{ 
				m_CameraReference = m_PlayerRef.GetComponentInChildren<Camera>().transform;
			}
			else
			{
				Debug.LogWarning("[MainPlayerTank.PlayerMovement.Setup]: " + "Could not find a camera attached to the player!");
			}

			m_AudioEffects.Setup(m_PlayerRef);
			m_Effects.Setup(m_PlayerRef);
			m_Effects.PlayDustTrails(true);
			EnablePlayerMovementInput(false);
		}

		public void EnablePlayerMovementInput(bool AllowPlayerMovement) => m_EnablePlayerMovementInput = AllowPlayerMovement;
		
		public void SetMovementInput(float ForwardInput, float RotationInput)
		{
			if (m_EnablePlayerMovementInput == false)
			{
				return;
			}



			Movement(ForwardInput, RotationInput);
			m_AudioEffects.PlayEngineSound(ForwardInput, RotationInput);
		}

		public void AimPrimaryWeapon() => AimWeapon();

		#endregion

		#region Private Methods 

		private void Movement(float vertical, float horizontal)
		{
			Vector3 direction = m_PlayerRef.transform.forward * vertical * MovementSpeed * Time.deltaTime;
			float turnAngle = horizontal * TurningSpeed * Time.deltaTime;


			Quaternion turningRotation = Quaternion.Euler(0, turnAngle, 0);

			m_Rigidbody.MovePosition(m_Rigidbody.position + direction);
			m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turningRotation);
		}
	
		private float desiredX;

		private void AimWeapon()
		{

			MouseX += Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensitivityMultiplier;
			MouseY -= Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensitivityMultiplier;

			MouseY = Mathf.Clamp(MouseY, minimumTurretRotationY, maximumTurretRotationY);

			Vector3 s_AimPosition = new Vector3(MouseX, MouseY, 0);

			AudioSource s_TankAimingAudioSource = AudioManager.Instance.GetAudioSource(GameAudio.T90_PrimaryWeapon_Aiming);

			if (s_AimPosition.magnitude >= 0.1f)
			{
				if (!s_TankAimingAudioSource.isPlaying)
				{
					AudioManager.Instance.PlaySound(GameAudio.T90_PrimaryWeapon_Aiming);
				}
				else if (s_TankAimingAudioSource.isPlaying && s_TankAimingAudioSource.volume < 1.0f)
				{
					AudioManager.Instance.FadeSoundEffect(GameAudio.T90_PrimaryWeapon_Aiming, turretAimingVolumeMax, (audioFadeDuration - 0.15f));
				}

				clampedCameraXRotation -= s_AimPosition.y;

				clampedCameraXRotation = Mathf.Clamp(clampedCameraXRotation, 0, 0);

				m_CameraReference.transform.localRotation = Quaternion.Euler(clampedCameraXRotation, desiredX, 0); 

				MainTurretParent.transform.localRotation = Quaternion.Euler(MouseY, MouseX, 0); 
			}
			else
			{
				if (s_AimPosition.magnitude <= 0.1f)
				{
					AudioManager.Instance.FadeSoundEffect(GameAudio.T90_PrimaryWeapon_Aiming, turretAimingVolumeMin, (audioFadeDuration - 0.4f));
				}
			}
		}

		#endregion

	}

	/// <summary>
	///		Handles weapon firing for our player 
	/// </summary>
	[System.Serializable]
	public class PlayerWeapons
	{

		#region Public Variables 

		public GameObject projectile;

		public Transform weaponFirePoint;

		public int MinimumAmmunition = 0;
		public int MaximumAmmunition = 20;

		public float maximumReloadingTime = 1.5f;

		public float bulletSpeed = 1000f;

		#endregion

		#region Private Variables 

		[SerializeField] private int currentAmmunition;
		
		[SerializeField] private float currentBulletVelocity;

		[SerializeField] private float currentReloadSpeed;

		[SerializeField] private bool isWeaponReloading, weaponHasBeenFired, weaponOutOfAmmo; 

		[SerializeField] private bool m_WeaponAllowedToFire;

		private Transform m_PlayerRef;

		#endregion

		#region Public Methods 

		public void Setup(Transform Player)
		{
			m_PlayerRef = Player;
			
			currentBulletVelocity = bulletSpeed;
			currentReloadSpeed = maximumReloadingTime;
			currentAmmunition = MaximumAmmunition;
			
			
			
			weaponOutOfAmmo = false;
			isWeaponReloading = false;
			EnableWeapons(false); // by default disable weapon firing 
		}

		/// <summary>
		///		Enable Weapon Firing?
		/// </summary>
		/// <param name="ShouldEnableWeapons"></param>
		public void EnableWeapons(bool ShouldEnableWeapons) => m_WeaponAllowedToFire = ShouldEnableWeapons;

		public void SetWeaponFiringInput(float FiringInput)
		{
			if (!m_WeaponAllowedToFire == true || isWeaponReloading == true)
			{
				return;
			}


			if (FiringInput > 0 && !weaponHasBeenFired)
			{
				if (currentAmmunition <= 0)
				{
					Debug.Log("[MainPlayerTank.Weapons.SetWeaponFiringInput]: " + "There is no more ammunition remaining!");
					return;
				}

				FirePrimaryWeapon();

				if (weaponHasBeenFired)
				{
					Debug.Log("[MainPlayerTank.Weapons.SetWeaponFiringInput]: " + "Weapon has fired! Reloading primary weapon...");
					m_PlayerRef.GetComponent<MainPlayerTank>().StartCoroutine(ReloadPrimaryWeapon());
				}
			}
			else if (FiringInput <= 0 && weaponHasBeenFired)
			{
				weaponHasBeenFired = false;
			}
		}

		/// <summary>
		/// Returns the current ammunitin remaining for the players tank 
		/// </summary>
		public int CurrentAmmunitionRemaining
		{
			get
			{
				return currentAmmunition;
			}
			set
			{

				currentAmmunition = value;

				currentAmmunition = Mathf.Clamp(currentAmmunition, MinimumAmmunition, MaximumAmmunition);


				if (currentAmmunition <= 0)
				{
					weaponOutOfAmmo = true;
				}
				else
				{
					weaponOutOfAmmo = false;
				}
			}
		}

		/// <summary>
		///		Sets the players ammunition 
		/// </summary>
		/// <param name="amount"></param>
		public void SetAmmunition(int amount)
		{
			Debug.Log("[MainPlayerTank.PlayerWeapons.SetAmmunition]: " + "Setting Player ammunition change! " + amount);

			CurrentAmmunitionRemaining += amount;
		}

		#endregion

		#region Private Methods 

		/// <summary>
		///		Fires the primary weapon 
		/// </summary>
		private void FirePrimaryWeapon(bool ButtonReleased = false)
		{
			weaponHasBeenFired = true;
			GameObject clone = Instantiate(projectile, weaponFirePoint.position, Quaternion.identity);

			if (clone.GetComponent<Rigidbody>())
			{
				Rigidbody rb = clone.GetComponent<Rigidbody>();

				rb.velocity = currentBulletVelocity * weaponFirePoint.forward;
			}
			else
			{
				Debug.Log("Projectile is missing a rigidbody component!");
			}

			Destroy(clone, 5f);



			if (AudioManager.Instance)
			{
				AudioManager.Instance.PlaySound(GameAudio.T90_PrimaryWeapon_Fire);
			}

			currentBulletVelocity = bulletSpeed;

		

			// I guess this is where we would invoke the ammunition event? 

			if (ButtonReleased)
			{
				weaponHasBeenFired = false;
			}
		}

		private IEnumerator ReloadPrimaryWeapon()
		{
			isWeaponReloading = true;

			if (AudioManager.Instance)
			{
				AudioManager.Instance.PlaySound(GameAudio.T90_PrimaryWeapon_Reload);
			}

			yield return new WaitForSeconds(currentReloadSpeed);

			isWeaponReloading = false;
		}

		#endregion
	
	}

	/// <summary>
	///		Handles Players Health 
	/// </summary>
	[System.Serializable]
	public class PlayerHealth
	{

		#region Public Variables 

		public float MinimumHealth = 0;
		
		public float MaximumHealth = 100;

		public bool playerIsDead = true;

		#endregion

		#region Private Variables 

		private float m_CurrentHealth;

		private Transform m_playerReference;

		#endregion

		#region Public Methods

		public void Setup(Transform Player)
		{
			// Set reference to the current player 
			if (Player.GetComponent<MainPlayerTank>())
			{
				// Set player tank ref 
				m_playerReference = Player.GetComponent<MainPlayerTank>().transform;
			}

			// Set the current players health to the maximum health value 
			PlayersCurrentHealth = MaximumHealth;
		}

		/// <summary>
		///		Returns the players current health 
		/// </summary>
		public float PlayersCurrentHealth
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
					playerIsDead = true;

					
				}
				else
				{
					playerIsDead = false;
				}



				
			}
		}

		/// <summary>
		///		If positive amount, will add health to the player,
		///		If negative amount will decrease the health of the player 
		/// </summary>
		/// <param name="amount"></param>
		public void SetHealth(float amount)
		{
			Debug.Log("[MainPlayerTank.SetHealth]: " + "Setting main player's health " + amount);
			PlayersCurrentHealth += amount;
		}

		#endregion

	}

	/// <summary>
	///		Reference to the main players particle effects 
	/// </summary>
	[System.Serializable]
	public class TankEffects
	{
		private ParticleSystem[] allDustTrails = new ParticleSystem[] { }; // an array to store all our particle effects

		/// <summary>
		/// takes in the transform of the tank and finds the particle effects to play for movement
		/// </summary>
		/// <param name="tank"></param>
		public void Setup(Transform PlayerRef)
		{
			allDustTrails = PlayerRef.GetComponentsInChildren<ParticleSystem>(); // find all the particle systems
		}

		/// <summary>
		/// Turns the dust trails on or off based on the Enabled parameter
		/// </summary>
		/// <param name="Enabled"></param>
		public void PlayDustTrails(bool Enabled)
		{
			// loop through all our dust trails
			for (int i = 0; i < allDustTrails.Length; i++)
			{
				if (Enabled)
				{
					// play the dust
					allDustTrails[i].Play();
				}
				else
				{
					// turn off the dust
					allDustTrails[i].Stop();
				}
			}
		}

	}

	[System.Serializable]
	public class TankAudio
	{
		#region Public Variables 
		
		[Header("Movement Audio")]
		public bool shouldLoop = true;

		#endregion

		#region Private Variables 

		private AudioClip EngineIdle;
		private AudioClip EngineDriving;
		private AudioSource EngineAudioSource;

		#endregion

		#region Public Methods 

		public void Setup(Transform Player)
		{
			if (Player.GetComponent<AudioSource>() != null)
			{
				EngineAudioSource = Player.GetComponent<AudioSource>();

				EngineAudioSource.loop = shouldLoop;
				EngineIdle = AudioManager.Instance.GetAudioClip(GameAudio.T90_EngineIdle);
				EngineDriving = AudioManager.Instance.GetAudioClip(GameAudio.T90_EngineDriving);

				Debug.Log("[MainPlayerTank.T90AudioEffects.Setup]: " + "Audio Instance found for main player tank!");
			}
			else
			{
				Debug.LogError("[MainPlayerTank.T90AudioEffects.Setup]: " + "Audio Source Instance could not be found for the main player tank!");
			}	
		}

		/// <summary>
		/// takes in the movement and the rotation, if either is moving, play the move sound effect, else play the idle sound effect
		/// </summary>
		/// <param name="PlayerMovementInput"></param>
		/// <param name="PlayerRotationInput"></param>
		public void PlayEngineSound(float PlayerMovementInput, float PlayerRotationInput)
		{
			// If we arent moving we should be setting the sources audio to idle 
			if (Mathf.Abs(PlayerMovementInput) < 0.1f && Mathf.Abs(PlayerRotationInput) < 0.1f)
			{
				if (EngineAudioSource.clip != EngineIdle)
				{
					EngineAudioSource.clip = EngineIdle;
					EngineAudioSource.Play();
				}
			}
			else
			{
				// Then we are moving and we should be playing this clip instead 
				if (EngineAudioSource.clip != EngineDriving)
				{
					EngineAudioSource.clip = EngineDriving;
					EngineAudioSource.Play();
				}
			}
		}

		#endregion
	}

	#endregion

}
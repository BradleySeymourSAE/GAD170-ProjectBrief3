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

	/// <summary>
	///		The amount of time to wait betwween flashes 
	/// </summary>
	[SerializeField] private float m_FlashTimer = 0.1f;
	
	/// <summary>
	///		The maximum amount of times a tank should flash on start
	/// </summary>
	[SerializeField] private int maximumFlashCount = 10;

	/// <summary>
	///		The flash routine to turn the players renders on and off 
	/// </summary>
	private Coroutine m_FlashRoutine;

	/// <summary>
	///		The Tank's Mesh Renders
	/// </summary>
	private MeshRenderer[] m_Renders;

	#endregion
	
	#region Private Variables 

	private bool cursorLocked = false;

	#endregion

	#region Unity References 

	private void OnEnable()
	{

		FireModeEvents.SpawnPlayerEvent += EnablePlayerInput;
		FireModeEvents.HandlePlayerDamageEvent += ChangeHealth;
		
		FireModeEvents.IncreasePlayerHealthEvent += ChangeHealth;
		FireModeEvents.IncreasePlayerAmmunitionEvent += ChangeAmmunition;
	}

	private void OnDisable()
	{

		FireModeEvents.SpawnPlayerEvent -= EnablePlayerInput;
		FireModeEvents.HandlePlayerDamageEvent -= ChangeHealth;

		FireModeEvents.IncreasePlayerHealthEvent -= ChangeHealth;
		FireModeEvents.IncreasePlayerAmmunitionEvent -= ChangeAmmunition;
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		Movement.Setup(transform);
		Weapons.Setup(transform);
		Health.Setup(transform);


		if (GetComponentsInChildren<MeshRenderer>() != null)
		{ 
			m_Renders = GetComponentsInChildren<MeshRenderer>();

			BeginFlashing();
		}

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

		Movement.SetMovementInput(Controls.GetUserInput(PlayerInput.UserInput.Move), Controls.GetUserInput(PlayerInput.UserInput.Rotate), Controls.GetUserInput(PlayerInput.UserInput.Boost));

		Weapons.SetWeaponFiringInput(Controls.GetUserInput(PlayerInput.UserInput.Fire));

		Weapons.SetWeaponADSInput(Controls.GetUserInput(PlayerInput.UserInput.ADS));
	}

	#endregion

	#region Private Methods 


	/// <summary>
	///     Locks & hides the players cursor on game start 
	/// </summary>
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

	private void EnablePlayerInput()
	{
		Movement.EnablePlayerMovementInput(true);
		Weapons.EnableWeapons(true);
	}

	/// <summary>
	///		Changes the current players health 
	/// </summary>
	/// <param name="Player"></param>
	/// <param name="Amount"></param>
	private void ChangeHealth(Transform Player, float Amount)
	{
		if (!Player.GetComponent<MainPlayerTank>())
		{
			Debug.LogWarning("[MainPlayerTank.ChangeHealth]: " + "Not the correct player!");
			return;
		}
		else
		{
			Debug.Log("MainPlayerTank.ChangeHealth]: " + "Calling Health.SetHealth for the current players Health");
			Health.SetHealth(Amount);
		}
	}

	/// <summary>
	///		Increases the current players ammunition 
	/// </summary>
	/// <param name="Player"></param>
	/// <param name="Amount"></param>
	private void ChangeAmmunition(Transform Player, int Amount)
	{
		if (!Player.GetComponent<MainPlayerTank>())
		{
			Debug.Log("[MainPlayerTank.ChangeAmmunition]: " + "Not the correct player!");
			return;
		}
		else
		{
			Debug.Log("[MainPlayerTank.ChangeAmmunition]: " + "Setting Player Ammunition: " + (Weapons.CurrentAmmunitionRemaining) + " Adding Change: " + Amount);
			Weapons.SetAmmunition(Amount);
		}
	}

	private void BeginFlashing()
	{
		if (m_FlashRoutine != null)
		{
			StopCoroutine(m_FlashRoutine);
		}
		m_FlashRoutine = StartCoroutine(Flash());
	}

	private IEnumerator Flash()
	{
		for (int i = 0; i < maximumFlashCount; i++)
		{
			EnableRenders(m_Renders, false);
			yield return new WaitForSeconds(m_FlashTimer);
			EnableRenders(m_Renders, true);
			yield return new WaitForSeconds(m_FlashTimer);
		}

		yield return null;
	}

	private void EnableRenders(MeshRenderer[] renderers, bool ShouldEnable)
	{
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].enabled = ShouldEnable;
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

		public enum UserInput { Move, Rotate, Fire, ADS, Boost };

		public KeyCode Forward = KeyCode.W;
		public KeyCode Backwards = KeyCode.S;
		public KeyCode Left = KeyCode.A;
		public KeyCode Right = KeyCode.D;
		public KeyCode Boost = KeyCode.LeftShift;
		public KeyCode MouseFire = KeyCode.Mouse0;
		public KeyCode MouseAim = KeyCode.Mouse1;

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
						if (Input.GetKey(MouseFire))
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
				case UserInput.ADS:
					{ 
						if (Input.GetKey(MouseAim))
						{
							value = 1;	
						}
						else
						{
							value = -1;
						}
						break;
					}
				case UserInput.Boost:
					{
						if (Input.GetKey(Boost))
						{
							value = 1;
						}
						else if (Input.GetKeyUp(Boost))
						{
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

		public float BoostedMovementSpeed = 21;


		#endregion

		#region Private Variables 

		/// <summary>
		///		The maximum downwards amount the turret can rotate on the Y Axis 
		/// </summary>
		[Header("Mouse Settings")]
		[SerializeField] private float minimumTurretRotationY = -0.5f;
		
		/// <summary>
		///		The maximum amount the turret can rotate on the Y Axis 
		/// </summary>
		[SerializeField] private float maximumTurretRotationY = 7f;

		/// <summary>
		///		The clamped camera x rotational value 
		/// </summary>
		[HideInInspector] public float clampedCameraXRotation;

		/// <summary>
		///		The duration to fade the audio for between aiming and not aiming 
		/// </summary>
		[HideInInspector] public float audioFadeDuration = 1;

		/// <summary>
		///		Minimum and Maximum Turret Aiming Volume 
		/// </summary>
		[HideInInspector] public float turretAimingVolumeMin = 0, turretAimingVolumeMax = 100;

		/// <summary>
		///		The players X & Y Mouse Position 
		/// </summary>
		[SerializeField] private float MouseX, MouseY;

		/// <summary>
		///		Reference to the Main Player Tank's Transform 
		/// </summary>
		[SerializeField] private Transform m_PlayerRef;

		private Rigidbody m_Rigidbody;

		/// <summary>
		///		Reference to the Tanks Particle Effects 
		/// </summary>
		private TankEffects m_Effects = new TankEffects();

		/// <summary>
		///		Reference to the Tanks Audio Effects 
		/// </summary>
		[SerializeField] private TankAudio m_AudioEffects = new TankAudio();

		/// <summary>
		///		Reference to the Players Camera 
		/// </summary>
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

		/// <summary>
		///		Once the game has started - The players movement gets enabled 
		/// </summary>
		/// <param name="AllowPlayerMovement"></param>
		public void EnablePlayerMovementInput(bool AllowPlayerMovement) => m_EnablePlayerMovementInput = AllowPlayerMovement;
		

		/// <summary>
		///		Sets the Main Player Tanks Forward and Reverse Input & Rotatation Input 
		/// </summary>
		/// <param name="ForwardInput"></param>
		/// <param name="RotationInput"></param>
		public void SetMovementInput(float ForwardInput, float RotationInput, float BoostingInput)
		{
			if (m_EnablePlayerMovementInput == false)
			{
				return;
			}

			 bool isCurrentlyBoosting = BoostingInput > 0 ? true : false;

	
			// Handles Movement for the player 
			Movement(ForwardInput, RotationInput, isCurrentlyBoosting);
			
			// Begins playing the Tanks Audio Effects 
			m_AudioEffects.PlayEngineSound(ForwardInput, RotationInput);
		}

		public void AimPrimaryWeapon() => AimWeapon();

		#endregion

		#region Private Methods 

		/// <summary>
		///		Handles the Tanks Movement Input 
		/// </summary>
		/// <param name="vertical"></param>
		/// <param name="horizontal"></param>
		private void Movement(float vertical, float horizontal, bool isCurrentlyBoosting)
		{
			float s_CurrentMovementSpeed = isCurrentlyBoosting == true ? BoostedMovementSpeed : MovementSpeed;


			Vector3 direction = m_PlayerRef.transform.forward * vertical * s_CurrentMovementSpeed * Time.deltaTime;
			float turnAngle = horizontal * TurningSpeed * Time.deltaTime;


			Quaternion turningRotation = Quaternion.Euler(0, turnAngle, 0);

			m_Rigidbody.MovePosition(m_Rigidbody.position + direction);
			m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turningRotation);
		}
	
		private float desiredX;



		/// <summary>
		///		Handles Aiming for the Tank's Primary Weapon 
		/// </summary>
		private void AimWeapon()
		{

			MouseX += Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensitivityMultiplier;
			MouseY -= Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensitivityMultiplier;

			MouseY = Mathf.Clamp(MouseY, minimumTurretRotationY, maximumTurretRotationY);

			Vector3 s_AimPosition = new Vector3(MouseX, MouseY, 0).normalized;

			AudioSource s_TankAimingAudioSource = AudioManager.Instance.GetAudioSource(GameAudio.T90_PrimaryWeapon_Aiming);

			if (s_AimPosition.magnitude > 0.1f)
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
		public int MaximumAmmunition = 50;

		public float maximumReloadingTime = 1.1f;
		public float bulletSpeed = 1000f;

		public float fov = 70;

		[Min(50)] [HideInInspector] public float aimDownSightFieldOfView = 55;

		private float m_CurrentFOV;

		private float fovScalingDuration = 3;


		#endregion

		#region Private Variables 

		[SerializeField] private int currentAmmunition;
		
		[SerializeField] private float currentBulletVelocity;

		[SerializeField] private float currentReloadSpeed;

		[SerializeField] private bool isWeaponReloading, weaponHasBeenFired, weaponOutOfAmmo;
		
		[SerializeField] private bool aimingDownWeaponSight;

		[SerializeField] private bool m_WeaponAllowedToFire;

		private Transform m_PlayerRef;

		private Camera m_CameraReference;

		#endregion

		#region Public Methods 

		public void Setup(Transform Player)
		{
			m_PlayerRef = Player;
			m_CurrentFOV = fov;

			if (m_PlayerRef.GetComponentInChildren<Camera>())
			{
				m_CameraReference = m_PlayerRef.GetComponentInChildren<Camera>();
				m_CameraReference.fieldOfView = m_CurrentFOV;
			}

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


				FireModeEvents.IncreaseAmmunitionEventUI?.Invoke(currentAmmunition);
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

		/// <summary>
		///		Sets the players field of view (Aim down sight weapon zooming / scaling) 
		/// </summary>
		/// <param name="Aiming"></param>
		public void SetWeaponADSInput(float Aiming)
		{
			if (Aiming > 0f && !aimingDownWeaponSight)
			{
				// m_CameraReference.fieldOfView = aimDownSightFieldOfView;
				m_CameraReference.fieldOfView = Mathf.Lerp(m_CameraReference.fieldOfView, aimDownSightFieldOfView, fovScalingDuration);
				aimingDownWeaponSight = true;
			}
			else if (Aiming <= 0f && aimingDownWeaponSight)
			{
				
				// m_CameraReference.fieldOfView = fov;
				m_CameraReference.fieldOfView = Mathf.Lerp(m_CameraReference.fieldOfView, fov, fovScalingDuration);
				aimingDownWeaponSight = false;
			}
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

			FireModeEvents.IncreasePlayerAmmunitionEvent?.Invoke(m_PlayerRef, -1);


			if (ButtonReleased)
			{
				weaponHasBeenFired = false;
			}
		}

		/// <summary>
		///		Handles reloading of the primary weapon 
		///		Plays the Weapon Reload Sound 
		/// </summary>
		/// <returns></returns>
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

		[SerializeField] private float m_CurrentHealth;

		private Transform m_playerReference;

		#endregion

		#region Public Methods

		public void Setup(Transform Player)
		{
			// Set reference to the current player 
			if (Player.GetComponent<MainPlayerTank>())
			{
				// Set player tank ref 
				m_playerReference = Player;
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
			
				// If the players current health is less than or equal to zero 
				if (m_CurrentHealth <= 0)
				{
					// Player is dead 
					playerIsDead = true;

					// The players is deadd 
					// Invoke the remove lives event if you still plan to use it 

				}
				else
				{
					// Player isn't dead 
					playerIsDead = false;
				}


				// Convert health to a rounded integer  value to update AI 
				int hp = Mathf.RoundToInt(m_CurrentHealth);


				Debug.Log("SETTING PLAYERS HEALTH: " + hp);

				// We want to set the players health UI i guess
				FireModeEvents.IncreasePlayerHealthEventUI(hp);
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

#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion




/// <summary>
///     Main Player Class 
/// </summary>
public class MainPlayerTank : MonoBehaviour
{

	public PlayerMovement Movement;

	public PlayerWeapons Weapons;

	public PlayerHealth Health;

	public PlayerInput Controls;

	private bool enablePlayerMovement = false;

	private bool cursorLocked = false;

	private void OnEnable()
	{
		FireModeEvents.SpawnPlayerEvent += () =>
		{
			Movement.Setup(this);
		};

		FireModeEvents.SpawnPlayerEvent += () =>
		{
			Weapons.Setup(this);
		};

		FireModeEvents.SpawnPlayerEvent += () =>
		{
			Health.Setup(this);
		};

		FireModeEvents.GameStartedEvent += EnablePlayerInput;
	}

	private void OnDisable()
	{
		FireModeEvents.SpawnPlayerEvent -= () =>
		{
			Movement.Setup(this);
		};

		FireModeEvents.SpawnPlayerEvent -= () =>
		{
			Weapons.Setup(this);
		};
		
		FireModeEvents.SpawnPlayerEvent -= () =>
		{
			Health.Setup(this);
		};

		FireModeEvents.GameStartedEvent -= EnablePlayerInput;
	}


	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		if (enablePlayerMovement)
		{
			EnablePlayerInput();
		}
	}

	private void EnablePlayerInput()
	{
		Movement.EnablePlayerMovementInput(true);
		Weapons.EnableWeapons(true);
	}


	private void Update()
	{

		HandleCursorLocking();

		if (!enablePlayerMovement)
		{
			return;
		}


		Movement.AimWeapon();
	}


	private void FixedUpdate()
	{
		if (!enablePlayerMovement)
		{
			return;
		}


		Movement.SetMovementInput(Controls.GetUserInput(PlayerInput.UserInput.Move), Controls.GetUserInput(PlayerInput.UserInput.Rotate));

		Weapons.SetWeaponFiringInput(Controls.GetUserInput(PlayerInput.UserInput.Fire));
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



	/// <summary>
	///		Handles input received by our player 
	/// </summary>
	[System.Serializable]
	public class PlayerInput
	{ 
		public enum UserInput { Move, Rotate, Fire };

		public KeyCode Forward = KeyCode.W;
		public KeyCode Backwards = KeyCode.S;
		public KeyCode Left = KeyCode.A;
		public KeyCode Right = KeyCode.D;
		public KeyCode MouseFire = KeyCode.Mouse0;
		private bool leftMouseButtonPressed = false;

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
						if (Input.GetMouseButton(0))
						{
							value = 1;
							leftMouseButtonPressed = true;
						}
						else if (Input.GetMouseButtonUp(0) && leftMouseButtonPressed == true)
						{
							leftMouseButtonPressed = false;
							value = -1;
						}
						break;
					}
			}


			return value;
		}
	}


	/// <summary>
	///		Handles movement for our player 
	/// </summary>
	[System.Serializable]
	public class PlayerMovement
	{

		[Header("Movement Settings")]
		public Transform MainTurretParent;

		public float MovementSpeed = 16;
		
		public float TurningSpeed = 50;


		[Header("Mouse Settings")]
		[SerializeField] private float minimumTurretRotationY = -0.5f;
		
		[SerializeField] private float maximumTurretRotationY = 7f;

		public float sensitivity = 50f;

		public float sensitivityMultiplier = 1;

		[Header("Camera")]
		[HideInInspector] float clampedCameraXRotation;
		[HideInInspector] float audioFadeDuration = 1;
		[HideInInspector] float turretAimingVolumeMin = 0, turretAimingVolumeMax = 100;


		[SerializeField] private float MouseX, MouseY;

		#region Private Variables 

		private MainPlayerTank m_PlayerRef;

		private Rigidbody m_Rigidbody;

		private T90ParticleEffects m_Effects;

		private T90AudioEffects m_AudioEffects;

		private Transform m_Camera;

		private bool m_EnablePlayerMovementInput = true;

		#endregion

		public void Setup(MainPlayerTank Player)
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
				m_Camera = m_PlayerRef.GetComponentInChildren<Camera>().transform;
			}

			m_AudioEffects.Setup(m_PlayerRef);
			m_Effects.Setup(m_PlayerRef);
			m_Effects.PlayDustTrails(true);
			EnablePlayerMovementInput(true);
		}


		public void EnablePlayerMovementInput(bool AllowPlayerMovement)
		{
			m_EnablePlayerMovementInput = AllowPlayerMovement;
		}
	
		
		public void SetMovementInput(float ForwardInput, float RotationInput)
		{
			if (m_EnablePlayerMovementInput == false)
			{
				return;
			}



			Movement(ForwardInput, RotationInput);
			m_AudioEffects.PlayEngineSound(ForwardInput, RotationInput);
		}

		private void Movement(float vertical, float horizontal)
		{
			Vector3 direction = m_PlayerRef.transform.forward * vertical * MovementSpeed * Time.deltaTime;
			float turnAngle = horizontal * TurningSpeed * Time.deltaTime;


			Quaternion turningRotation = Quaternion.Euler(0, turnAngle, 0);

			m_Rigidbody.MovePosition(m_Rigidbody.position + direction);
			m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turningRotation);
		}
	

		public void AimWeapon()
		{

			MouseX += Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensitivityMultiplier;
			MouseY -= Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensitivityMultiplier;

			MouseY = Mathf.Clamp(MouseY, minimumTurretRotationY, maximumTurretRotationY);

			Vector3 s_AimPosition = new Vector3(MouseX, MouseY, 0);

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

				m_Camera.transform.localRotation = Quaternion.Euler(clampedCameraXRotation, desiredX, 0); 

				MainTurretParent.transform.localRotation = Quaternion.Euler(MouseY, MouseX, 0); 
			}
			else
			{
				if (s_AimPosition.magnitude < 0.1f)
				{
					AudioManager.Instance.FadeSoundEffect(GameAudio.T90_PrimaryWeapon_Aiming, turretAimingVolumeMin, (audioFadeDuration - 0.4f));
				}
			}
		}


		private float desiredX;
	}

	/// <summary>
	///		Handles weapon firing for our player 
	/// </summary>
	[System.Serializable]
	public class PlayerWeapons
	{

		public GameObject projectile;

		public Transform weaponFirePoint;

		public int maximumAmmunition = 20;

		public float maximumReloadingTime = 1.5f;

		public float bulletSpeed = 1000f;

		
		[SerializeField] private int Ammunition;
		
		[SerializeField] private float currentBulletVelocity;

		[SerializeField] private float currentReloadSpeed;

		[SerializeField] private bool isWeaponReloading, weaponHasBeenFired; 

		[SerializeField] private bool m_WeaponAllowedToFire;

		private MainPlayerTank m_PlayerRef;

		public void Setup(MainPlayerTank Player)
		{
			m_PlayerRef = Player;

			
			currentBulletVelocity = Random.Range(bulletSpeed - 150f, bulletSpeed + 100f);
			currentReloadSpeed = maximumReloadingTime;
			Ammunition = maximumAmmunition;

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
			if (!m_WeaponAllowedToFire || isWeaponReloading)
			{
				return;
			}


			if (FiringInput > 0 && !weaponHasBeenFired)
			{
				if (Ammunition <= 0)
				{
					Debug.Log("[MainPlayerTank.Weapons.SetWeaponFiringInput]: " + "There is no more ammunition remaining!");
					return;
				}

				FirePrimaryWeapon();

				if (weaponHasBeenFired)
				{
					Debug.Log("[MainPlayerTank.Weapons.SetWeaponFiringInput]: " + "Weapon has fired! Reloading primary weapon...");
					m_PlayerRef.StartCoroutine(ReloadPrimaryWeapon());
				}
			}
			else if (FiringInput <= 0 && weaponHasBeenFired)
			{
				weaponHasBeenFired = false;
			}
		}



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
	}

	/// <summary>
	///		Handles Players Health 
	/// </summary>
	[System.Serializable]
	public class PlayerHealth
	{ 
		
		private MainPlayerTank m_PlayerRef;

	
		public void Setup(MainPlayerTank Player)
		{
			m_PlayerRef = Player;
		}

		
	}

	/// <summary>
	///		Reference to the main players particle effects 
	/// </summary>
	[System.Serializable]
	public class T90ParticleEffects
	{
		private ParticleSystem[] allDustTrails = new ParticleSystem[] { }; // an array to store all our particle effects

		/// <summary>
		/// takes in the transform of the tank and finds the particle effects to play for movement
		/// </summary>
		/// <param name="tank"></param>
		public void Setup(MainPlayerTank PlayerRef)
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
	public class T90AudioEffects
	{ 
		[Header("Movement Audio")]
		public bool shouldLoop = true;

		private AudioClip EngineIdle;
		private AudioClip EngineDriving;
		private AudioSource EngineAudioSource;

		public void Setup(MainPlayerTank Player)
		{
			if (Player.GetComponent<AudioSource>())
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
	}
}

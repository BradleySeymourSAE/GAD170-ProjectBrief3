using UnityEngine;


/// <summary>
///	 Helper for hiding game objects once a scene has been loaded 
/// </summary>
public class HideOnPlay : MonoBehaviour
{ 
	private void Start()
	{
		gameObject.SetActive(false);
	}
}

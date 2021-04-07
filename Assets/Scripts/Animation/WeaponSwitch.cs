using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponSwitch : StateMachineBehaviour
{


	public bool IsSecondaryWeapon;


	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (IsSecondaryWeapon)
		{
			if (stateInfo.speed > 0)
			{
				if (!animator.GetComponent<InfantryAI>().SecondaryHand.activeSelf && stateInfo.normalizedTime / stateInfo.length > 0.05)
				{
					animator.GetComponent<InfantryAI>().SecondaryHide.SetActive(false);
					animator.GetComponent<InfantryAI>().SecondaryHand.SetActive(true);
				}
			}
			else
			{
				if (!animator.GetComponent<InfantryAI>().SecondaryHide.activeSelf && stateInfo.normalizedTime / stateInfo.length > 0.95)
				{
					animator.GetComponent<InfantryAI>().SecondaryHand.SetActive(true);
					animator.GetComponent<InfantryAI>().SecondaryHide.SetActive(false);
				}
			}
		}
		else
		{
			if (stateInfo.speed > 0)
			{
				if (!animator.GetComponent<InfantryAI>().PrimaryHand.activeSelf && stateInfo.normalizedTime / stateInfo.length > 0.30)
				{
					animator.GetComponent<InfantryAI>().PrimaryHide.SetActive(false);
					animator.GetComponent<InfantryAI>().PrimaryHand.SetActive(true);
				}
			}
			else
			{
				if (!animator.GetComponent<InfantryAI>().PrimaryHide.activeSelf && stateInfo.normalizedTime / stateInfo.length > 0.70)
				{
					animator.GetComponent<InfantryAI>().PrimaryHide.SetActive(true);
					animator.GetComponent<InfantryAI>().PrimaryHand.SetActive(false);
				}
			}
		}
	}



	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (IsSecondaryWeapon)
		{
			if (stateInfo.speed > 0)
			{
				animator.GetComponent<InfantryAI>().SecondaryHide.SetActive(false);
				animator.GetComponent<InfantryAI>().SecondaryHand.SetActive(true);
			}
			else
			{
				animator.GetComponent<InfantryAI>().SecondaryHide.SetActive(true);
				animator.GetComponent<InfantryAI>().SecondaryHand.SetActive(false);
			}
		}
		else
		{
			if (stateInfo.speed > 0)
			{
				animator.GetComponent<InfantryAI>().PrimaryHide.SetActive(false);
				animator.GetComponent<InfantryAI>().PrimaryHand.SetActive(true);
			}
			else
			{
				animator.GetComponent<InfantryAI>().PrimaryHide.SetActive(true);
				animator.GetComponent<InfantryAI>().PrimaryHand.SetActive(false);
			}
		}
	}
}
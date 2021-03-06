﻿using UnityEngine;
using DG.Tweening;

namespace Fps.Weapon.Animation
{
	public class WeaponRecoilAnimation : MonoBehaviour
	{
		//-----------------------------------------------------------------------------
		// Public Methods
		//-----------------------------------------------------------------------------

		public void Play ()
		{
			// Stop and complete any animation we had on the weapon.
			transform.DOKill (true);

			// Use punch tweens to animate the weapon to slightly randomized values.
			transform.DOPunchPosition (
				recoilTranslation * Random.Range (1, 1.1f), 
				duration,
				vibrate, 
				elasticity
			).SetRelative ();

			transform.DOPunchRotation (
				recoilRotation * Random.Range (1, 1.1f), 
				duration, 
				vibrate, 
				elasticity
			).SetRelative ();
		}

		//-----------------------------------------------------------------------------
		// Attributes
		//-----------------------------------------------------------------------------

		[SerializeField]
		private Vector3 recoilTranslation = new Vector3 (-0.1f, -0.2f, -0.5f);

		[SerializeField]
		private Vector3 recoilRotation = new Vector3 (-5, 0, 0);

		[SerializeField]
		private float duration = 0.4f;

		[SerializeField]
		private int vibrate = 3;

		[SerializeField]
		private float elasticity = 0.2f;
	}
}
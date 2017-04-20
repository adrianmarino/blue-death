﻿using UnityEngine;

namespace Fps
{
	public class Weapon : MonoBehaviour
	{
		//-----------------------------------------------------------------------------
		// Public Methods
		//-----------------------------------------------------------------------------

		public bool Shoot (Transform origin, out RaycastHit target, LayerMask targetMask)
		{
			return Physics.Raycast (
				origin.position, 
				origin.forward, 
				out target, 
				targetMask
			);
		}

		public void HitTarget (Vector3 position, Vector3 normal)
		{
			GameObject hitEffect = Instantiate (
				                       HitEffect,
				                       position,
				                       Quaternion.LookRotation (normal)
			                       );
			Destroy (hitEffect, 2f);
		}

		public void PlayShootEffect ()
		{
			muzzleFlash.Play ();
			ShootSound.Play ();
		}

		public override string ToString ()
		{
			return Name;
		}

		//-----------------------------------------------------------------------------
		// Properties
		//-----------------------------------------------------------------------------

		AudioSource ShootSound {
			get { return GetComponent<AudioSource> (); }
		}

		public GameObject HitEffect {
			get { return hitEffect; }
		}

		public string Name {
			get { return _name; }
		}

		public float Damage {
			get { return damage; }
		}

		public float Range {
			get { return range; }
		}

		public float FireRate {
			get { return fireRate; }
		}

		//-----------------------------------------------------------------------------
		// Attributes
		//-----------------------------------------------------------------------------

		[SerializeField]
		private ParticleSystem muzzleFlash;

		[SerializeField]
		private GameObject hitEffect;

		[SerializeField]
		private string _name = "HeavyBlaster";

		[SerializeField]
		private float damage = 25f;

		[SerializeField]
		private float range = 100f;

		[SerializeField]
		private float fireRate = 2f;
	}
}


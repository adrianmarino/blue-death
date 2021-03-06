﻿using UnityEngine;
using Fps.Weapon.State;
using System.Collections;
using Fps.Weapon.Animation;

namespace Fps.Weapon
{
    [RequireComponent(typeof(WeaponReloadAnimation))]
    public class LoadingWeaponState : WeaponState
    {
        public override string ToString()
        {
            return "Loading";
        }

        //-----------------------------------------------------------------------------
        // Private Methods
        //-----------------------------------------------------------------------------

        private IEnumerator WaitForLoadedState(float time)
        {
            weapon.PlayReloadEffectAction();
            yield return new WaitForSeconds(time);
            weapon.GoToLoadedState();
        }

        //-----------------------------------------------------------------------------
        // Attributes
        //-----------------------------------------------------------------------------

        private readonly RechargeableWeapon weapon;

        //-----------------------------------------------------------------------------
        // Constructors
        //-----------------------------------------------------------------------------

        public LoadingWeaponState(RechargeableWeapon weapon, float loadingTime)
        {
            this.weapon = weapon;
            Debug.LogFormat("Loading {0} by {1} seconds", weapon, loadingTime);
            weapon.StartCoroutine(WaitForLoadedState(loadingTime));
        }
    }
}
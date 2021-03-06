﻿using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Fps.Weapon;

namespace Fps.Player
{
    public class PlayerState : NetworkBehaviour, IPausable
    {
        //-----------------------------------------------------------------------------
        // Event Methods
        //-----------------------------------------------------------------------------

        void Start()
        {
            Setup();
        }

        [ClientRpc]
        public void RpcTakeDamage(float damage)
        {
            if (Dead)
                return;

            DecreaseHealth(damage);

            if (currentHealth <= 0)
            {
                Die();
                Restart();
            }
        }

        void Update()
        {
            if (Pause) return;

            if (!isLocalPlayer)
                return;
            ShowCurrentHealth();

            if (Input.GetKeyDown(KeyCode.K))
                RpcTakeDamage(currentHealth);
        }

        //-----------------------------------------------------------------------------
        // Public Methods
        //-----------------------------------------------------------------------------

        public override string ToString()
        {
            return tag + " " + transform.name;
        }

        public void Setup()
        {
            Dead = false;
            currentHealth = maxHealth;
            LoadActiveStates();

            if (WeaponManager.CurrentWeapon != null)
                WeaponManager.CurrentWeapon.Show();
        }

        //-----------------------------------------------------------------------------
        // Private Methods
        //-----------------------------------------------------------------------------

        void ShowCurrentHealth()
        {
            healthPanel.text = "+ " + currentHealth;
        }

        void DecreaseHealth(float damage)
        {
            currentHealth -= damage;
            Debug.LogFormat("{0} current health: {1}", name, currentHealth);
        }

        void SaveActiveStates()
        {
            SaveBehaviourActiveStates();
            SaveGameObjectActiveStates();
        }

        void SaveBehaviourActiveStates()
        {
            behaviourActiveStates.Clear();
            disableBehaviourOnDeath.ForEach(it => behaviourActiveStates.Add(it, it.enabled));
        }

        void SaveGameObjectActiveStates()
        {
            gameObjectActiveStates.Clear();
            disableGameObjectsOnDeath.ForEach(it => gameObjectActiveStates.Add(it, it.activeSelf));
        }

        void LoadActiveStates()
        {
            LoadBehaviourActiveStates();
            LoadGameObjectActiveStates();
        }

        void LoadBehaviourActiveStates()
        {
            behaviourActiveStates.ToList().ForEach(entry => entry.Key.enabled = entry.Value);
        }

        void LoadGameObjectActiveStates()
        {
            gameObjectActiveStates.ToList().ForEach(it => it.Key.SetActive(it.Value));
        }

        void SetEnableCollider(bool value)
        {
            Collider component = GetComponent<Collider>();
            if (component != null)
                component.enabled = value;
        }

        Rigidbody Rigidbody()
        {
            return GetComponent<Rigidbody>();
        }

        void DisableAllBehaviours()
        {
            Util.BehaviourUtil.DisableAll(disableBehaviourOnDeath);
        }

        void DisableAllGameObjects()
        {
            Util.GameObjects.DisableAll(disableGameObjectsOnDeath);
        }

        void Die()
        {
            Dead = true;
            SaveActiveStates();
            DisableAllBehaviours();
            DisableAllGameObjects();
            WeaponManager.CurrentWeapon.Hide();
            Debug.LogFormat("{0} is dead!", name);
            PerformDeadEffect();
        }

        void PerformDeadEffect()
        {
            Instantiate(deadEffect, transform.position, Quaternion.identity);
        }

        void MoveToStartPoint()
        {
            Transform startPoint = NetworkManager.singleton.GetStartPosition();
            transform.position = startPoint.position;
            transform.rotation = startPoint.rotation;
        }

        void Restart()
        {
            StartCoroutine(Respawn(4f));
        }

        IEnumerator Respawn(float delay)
        {
            yield return new WaitForSeconds(delay);
            Setup();
            MoveToStartPoint();
        }

        //-----------------------------------------------------------------------------
        // Properties
        //-----------------------------------------------------------------------------

        WeaponManager WeaponManager
        {
            get { return GetComponent<WeaponManager>(); }
        }

        public string Name
        {
            get { return transform.name; }
            set { transform.name = value; }
        }

        public bool Dead
        {
            get { return dead; }
            protected set { dead = value; }
        }

        public bool Pause { get; set; }

        //-----------------------------------------------------------------------------
        // Attributes
        //-----------------------------------------------------------------------------

        [SerializeField] private float maxHealth = 100f;

        [SerializeField] public Text healthPanel;

        [SerializeField] public List<Behaviour> disableBehaviourOnDeath;

        [SerializeField] public List<GameObject> disableGameObjectsOnDeath;

        [SerializeField] private GameObject deadEffect;

        [SerializeField] private bool pause;
  
        [SyncVar] private float currentHealth;

        [SyncVar] private bool dead;
        
        private Dictionary<Behaviour, bool> behaviourActiveStates = new Dictionary<Behaviour, bool>();

        private Dictionary<GameObject, bool> gameObjectActiveStates = new Dictionary<GameObject, bool>();
    }
}
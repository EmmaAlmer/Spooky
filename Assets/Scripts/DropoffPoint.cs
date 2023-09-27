﻿using System;
using UnityEngine;

namespace Spooky
{
    [RequireComponent(typeof(AudioSource))]
    public class DropoffPoint : Interactable
    {
        [Tooltip("The pickup point that the player has to interact with to enable this point.")]
        [SerializeField] private PickupPoint _pickupPoint;

        /// <summary>
        /// Whether or not an item has been dropped here.
        /// </summary>
        public bool Enabled { get; private set; } = false;

        [Tooltip(
            "This clip will be played once when you place an item here. It is played in 2D space. " +
            "This can for example be a sound effect combined with voice instructions on what to do next.")]
        [SerializeField]
        private AudioClip _dropoffClip;

        /// <summary>
        /// Event called when the correct item is placed here.
        /// </summary>
        public event Action ItemPlaced;

        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();

            if (!_pickupPoint)
            {
                Debug.LogError($"Drop off point {name} does not have a {nameof(PickupPoint)}");
                return;
            }

            _pickupPoint.ItemPicked += () => _source.volume = 1;
        }

        public override void Interact()
        {
            base.Interact();

            // early return if we're already done here
            if (Enabled)
                return;

            // early return if we have an invalid or non-interacted pickup point
            if (_pickupPoint == null || !_pickupPoint.InteractedWith)
                return;

            Enabled = true;
            ItemPlaced?.Invoke();

            if (_dropoffClip)
                AudioSource.PlayClipAtPoint(_dropoffClip, Vector3.zero);

            Debug.Log($"Dropped off item at {name}");
        }
    }
}
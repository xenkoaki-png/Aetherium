/*
 * Copyright (c) 2026 Xenko / StudioName. All Rights Reserved.
 * Game: Aetherium (Unity Isometric Sci-Fi RPG Space Opera & Fantasy Isekai)
 * Repository: xenkoaki-png/aetherium
 * Unauthorized copying, decompilation, asset extraction, modification, or
 * redistribution of this file is strictly prohibited unless permitted by LICENSE.txt.
 */

using System.Collections.Generic;
using UnityEngine;

namespace Aetherium.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class AetheriumStarshipController : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        public float AggregateMassKg = 45000f;
        public float MainThrusterForceNewtons = 180000f;
        public float RotationalTorqueNewtons = 45000f;
        public float MaxHullHp = 1200f;
        public float CurrentHullHp = 1200f;
        public List<Transform> DockedCrewEntities = new List<Transform>();
        public Transform BridgeConsoleMountPoint;
        public float DeckGravityAcceleration = 9.81f;
        public bool IsCoreboundAssimilated { get; private set; }

        private Rigidbody _rigidbody;
        private Vector3 _lastShipPosition;
        private Quaternion _lastShipRotation;
        private readonly IShipPhysicsBridge _physicsBridge = new RigidBodyShipPhysicsBridge();

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.mass = AggregateMassKg;
            _lastShipPosition = transform.position;
            _lastShipRotation = transform.rotation;
        }

        private void FixedUpdate()
        {
            SyncInteriorDocketPassengers();
            ApplyBilateralGravityToCrew();
        }

        public void SyncInteriorDocketPassengers()
        {
            if (DockedCrewEntities == null)
            {
                return;
            }

            Vector3 deltaPos = transform.position - _lastShipPosition;
            Quaternion deltaRot = transform.rotation * Quaternion.Inverse(_lastShipRotation);

            for (int i = 0; i < DockedCrewEntities.Count; i++)
            {
                Transform crew = DockedCrewEntities[i];
                if (crew == null)
                {
                    continue;
                }

                Vector3 relativePos = crew.localPosition;
                Vector3 target = _lastShipPosition + deltaPos + (deltaRot * relativePos);
                crew.position = Vector3.Lerp(crew.position, target, 0.9f);
            }

            _lastShipPosition = transform.position;
            _lastShipRotation = transform.rotation;
        }

        public void ApplyBilateralGravityToCrew()
        {
            if (DockedCrewEntities == null)
            {
                return;
            }

            foreach (Transform crew in DockedCrewEntities)
            {
                if (crew == null)
                {
                    continue;
                }

                Vector3 gravityVector = _physicsBridge.ComputeBilateralGravityVector(transform, crew.position, DeckGravityAcceleration);
                crew.GetComponent<Rigidbody>()?.AddForce(gravityVector, ForceMode.Acceleration);
            }
        }

        public void TryAssimilateBridge(AetheriumPlayerStats stats, Transform player)
        {
            if (stats == null || stats.Species != SpeciesType.Corebound || stats.EvolutionStage < 3)
            {
                return;
            }

            IsCoreboundAssimilated = true;
            stats.IsAssimilatedWithShip = true;

            if (BridgeConsoleMountPoint != null && player != null)
            {
                player.position = BridgeConsoleMountPoint.position;
                player.rotation = BridgeConsoleMountPoint.rotation;
            }
        }

        public void FireTriBarrelTurbolaserSalvo()
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 direction = transform.forward + (i == 1 ? Vector3.right * 0.1f : Vector3.zero) + (i == 2 ? Vector3.left * 0.1f : Vector3.zero);
                Ray ray = new Ray(transform.position + transform.forward * 5f, direction.normalized);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 256f))
                {
                    Debug.Log("Turbolaser hit: " + hit.collider.name + " / damage 30");
                }
            }
        }
    }

    public class AetheriumIsometricCameraRig : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        public Transform TargetPlayer;
        public Transform AssimilatedStarship;
        public Vector3 StandardOffset = new Vector3(-12f, 15f, -12f);
        public Vector3 ShipAssimilatedOffset = new Vector3(-32f, 40f, -32f);
        public float FollowSpeed = 8f;
        public float OverheadHullCheckDistance = 12f;
        public LayerMask ObstacleMask = ~0;

        private void LateUpdate()
        {
            if (TargetPlayer == null)
            {
                return;
            }

            bool isAssimilated = AssimilatedStarship != null && TargetPlayer.GetComponent<AetheriumPlayerStats>() != null && TargetPlayer.GetComponent<AetheriumPlayerStats>().IsAssimilatedWithShip;
            Transform focusTarget = isAssimilated ? AssimilatedStarship : TargetPlayer;
            Vector3 targetOffset = isAssimilated ? ShipAssimilatedOffset : StandardOffset;
            Vector3 desiredPosition = focusTarget.position + targetOffset;

            if (Physics.Raycast(focusTarget.position, (desiredPosition - focusTarget.position).normalized, out RaycastHit hit, targetOffset.magnitude, ObstacleMask.value))
            {
                desiredPosition = hit.point - (hit.normal * 0.35f);
            }

            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * FollowSpeed);
            transform.LookAt(focusTarget.position + Vector3.up * 1.4f);
            UpdateOverheadHullCutaway();
        }

        public void UpdateOverheadHullCutaway()
        {
            if (TargetPlayer == null)
            {
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(TargetPlayer.position, Vector3.up, out hit, OverheadHullCheckDistance))
            {
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
        }
    }
}

using UnityEngine;

namespace Aetherium.Gameplay
{
    [RequireComponent(typeof(AetheriumEntityHealth))]
    public class AetheriumEnemyController : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        [Header("Targeting")]
        public Transform Target;
        public float DetectionRange = 9f;
        public float AttackRange = 1.6f;

        [Header("Movement")]
        public float MoveSpeed = 2.8f;
        public float TurnSpeed = 8f;

        [Header("Combat")]
        public int DamagePerHit = 12;
        public float AttackCooldown = 1.2f;
        public float KnockbackStrength = 0.45f;
        public float HitFlashDuration = 0.12f;
        public Color HitFlashColor = new Color(1f, 0.5f, 0.5f, 1f);

        private AetheriumEntityHealth _health;
        private float _nextAttackTime;
        private float _hitFlashTimer;
        private Renderer[] _renderers;
        private Color[] _originalColors;

        private void Awake()
        {
            _health = GetComponent<AetheriumEntityHealth>();
            _renderers = GetComponentsInChildren<Renderer>();
            _originalColors = new Color[_renderers.Length];

            for (int i = 0; i < _renderers.Length; i++)
            {
                if (_renderers[i] == null)
                {
                    continue;
                }

                _originalColors[i] = _renderers[i].material.color;
            }
        }

        private void Start()
        {
            if (Target == null && AetheriumWorldRuntime.Instance != null)
            {
                Target = AetheriumWorldRuntime.Instance.Player != null ? AetheriumWorldRuntime.Instance.Player.transform : null;
            }

            if (AetheriumWorldRuntime.Instance != null)
            {
                AetheriumWorldRuntime.Instance.RegisterEnemy(this);
            }
        }

        private void Update()
        {
            if (_health != null && _health.IsDead)
            {
                if (AetheriumWorldRuntime.Instance != null)
                {
                    AetheriumWorldRuntime.Instance.UnregisterEnemy(this);
                }

                return;
            }

            if (_hitFlashTimer > 0f)
            {
                _hitFlashTimer -= Time.deltaTime;
                UpdateHitFlash();
            }

            if (Target == null)
            {
                return;
            }

            float distance = Vector3.Distance(transform.position, Target.position);
            if (distance > DetectionRange)
            {
                return;
            }

            Vector3 direction = (Target.position - transform.position).normalized;
            direction.y = 0f;

            if (distance > AttackRange)
            {
                transform.position += direction * MoveSpeed * Time.deltaTime;
            }
            else if (Time.time >= _nextAttackTime)
            {
                _nextAttackTime = Time.time + AttackCooldown;

                AetheriumPlayerController playerController = Target.GetComponent<AetheriumPlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(DamagePerHit);
                }
            }

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * TurnSpeed);
            }
        }

        public void ApplyHitReaction(int damage, Vector3 sourcePosition)
        {
            _hitFlashTimer = HitFlashDuration;

            Vector3 pushDirection = (transform.position - sourcePosition).normalized;
            pushDirection.y = 0f;
            if (pushDirection.sqrMagnitude > 0.01f)
            {
                transform.position += pushDirection * KnockbackStrength;
            }
        }

        private void UpdateHitFlash()
        {
            if (_renderers == null || _originalColors == null)
            {
                return;
            }

            float t = Mathf.Clamp01(_hitFlashTimer / HitFlashDuration);
            float flashStrength = 1f - t;

            for (int i = 0; i < _renderers.Length; i++)
            {
                if (_renderers[i] == null)
                {
                    continue;
                }

                Color flashColor = Color.Lerp(_originalColors[i], HitFlashColor, flashStrength);
                _renderers[i].material.color = flashColor;
            }
        }
    }
}

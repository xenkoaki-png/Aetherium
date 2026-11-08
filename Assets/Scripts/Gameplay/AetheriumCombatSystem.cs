using UnityEngine;

namespace Aetherium.Gameplay
{
    public class AetheriumCombatSystem : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        [Header("Combat Timing")]
        public float AttackRange = 2.5f;
        public float AttackCooldown = 0.55f;
        public float AttackArcDegrees = 110f;
        public float AttackHeightOffset = 0.8f;

        [Header("Damage")]
        public int DamagePerSwing = 20;

        [Header("Feedback")]
        public bool SpawnSwingEffect = true;
        public Color SwingColor = new Color(0.55f, 0.85f, 1f, 1f);

        private float _nextAttackTime;

        public bool TryAttack()
        {
            if (Time.time < _nextAttackTime)
            {
                return false;
            }

            _nextAttackTime = Time.time + AttackCooldown;

            Vector3 origin = transform.position + Vector3.up * AttackHeightOffset;
            Vector3 forward = transform.forward;
            Collider[] hits = Physics.OverlapSphere(origin, AttackRange);
            bool damaged = false;

            foreach (Collider collider in hits)
            {
                AetheriumEnemyController enemy = collider.GetComponent<AetheriumEnemyController>();
                if (enemy == null)
                {
                    continue;
                }

                Vector3 toEnemy = collider.bounds.ClosestPoint(origin) - origin;
                if (toEnemy.sqrMagnitude <= 0.001f)
                {
                    toEnemy = collider.transform.position - origin;
                }

                float angle = Vector3.Angle(forward, toEnemy.normalized);
                if (angle > AttackArcDegrees * 0.5f)
                {
                    continue;
                }

                var health = enemy.GetComponent<AetheriumEntityHealth>();
                if (health == null || health.IsDead)
                {
                    continue;
                }

                health.ApplyDamage(DamagePerSwing);
                enemy.ApplyHitReaction(DamagePerSwing, origin);
                damaged = true;

                if (health.IsDead)
                {
                    if (AetheriumWorldRuntime.Instance != null)
                    {
                        AetheriumWorldRuntime.Instance.RegisterEnemyDefeat();
                    }
                }
            }

            if (SpawnSwingEffect)
            {
                SpawnSwingEffectObject(origin, forward);
            }

            if (!damaged)
            {
                Debug.Log("Attack connected with empty air.");
            }

            return damaged;
        }

        private void SpawnSwingEffectObject(Vector3 origin, Vector3 direction)
        {
            GameObject slash = GameObject.CreatePrimitive(PrimitiveType.Cube);
            slash.name = "AetheriumSwingFX";
            slash.transform.position = origin + direction.normalized * (AttackRange * 0.52f);
            slash.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            slash.transform.localScale = new Vector3(0.12f, 0.12f, AttackRange * 0.9f);

            Collider slashCollider = slash.GetComponent<Collider>();
            if (slashCollider != null)
            {
                slashCollider.enabled = false;
            }

            Renderer slashRenderer = slash.GetComponent<Renderer>();
            if (slashRenderer != null)
            {
                slashRenderer.material = new Material(Shader.Find("Standard"));
                slashRenderer.material.color = SwingColor;
                slashRenderer.material.SetFloat("_Mode", 3f);
                slashRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                slashRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                slashRenderer.material.SetInt("_ZWrite", 0);
                slashRenderer.material.DisableKeyword("_ALPHATEST_ON");
                slashRenderer.material.EnableKeyword("_ALPHABLEND_ON");
                slashRenderer.material.renderQueue = 3000;
            }

            Destroy(slash, 0.12f);
        }
    }
}

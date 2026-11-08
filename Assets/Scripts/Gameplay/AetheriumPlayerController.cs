using UnityEngine;

namespace Aetherium.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class AetheriumPlayerController : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        public CharacterController CharacterController;
        public AetheriumCombatSystem CombatSystem;
        public float BaseMoveSpeed = 5.5f;
        public float SprintMultiplier = 1.6f;
        public float Gravity = -18f;
        public float RotationSpeed = 8f;
        public int MaxHealth = 100;
        public int CurrentHealth = 100;
        public int DamageResistance = 0;
        public float AttackPulseTime = 0.12f;

        private Vector3 _velocity;
        private float _attackPulseTimer;

        private void Awake()
        {
            CharacterController = GetComponent<CharacterController>();
            CombatSystem = GetComponent<AetheriumCombatSystem>();
            if (CombatSystem == null)
            {
                CombatSystem = gameObject.AddComponent<AetheriumCombatSystem>();
            }
        }

        private void Start()
        {
            if (AetheriumWorldRuntime.Instance != null)
            {
                AetheriumWorldRuntime.Instance.RegisterPlayer(this);
            }
        }

        private void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector3 isoForward = new Vector3(1f, 0f, 1f).normalized;
            Vector3 isoRight = new Vector3(1f, 0f, -1f).normalized;
            Vector3 inputDirection = (isoRight * h + isoForward * v).normalized;

            float speed = BaseMoveSpeed * (Input.GetKey(KeyCode.LeftShift) ? SprintMultiplier : 1f);
            Vector3 motion = inputDirection * speed;

            if (CharacterController.isGrounded && _velocity.y < 0f)
            {
                _velocity.y = -2f;
            }

            _velocity.y += Gravity * Time.deltaTime;
            motion.y = _velocity.y;

            CharacterController.Move(motion * Time.deltaTime);

            if (inputDirection.sqrMagnitude > 0.001f)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * RotationSpeed);
            }

            if (_attackPulseTimer > 0f)
            {
                _attackPulseTimer -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space) && CombatSystem != null)
            {
                _attackPulseTimer = AttackPulseTime;
                CombatSystem.TryAttack();
            }
        }

        public void TakeDamage(int amount)
        {
            if (CurrentHealth <= 0)
            {
                return;
            }

            int adjusted = Mathf.Max(0, amount - DamageResistance);
            CurrentHealth = Mathf.Clamp(CurrentHealth - adjusted, 0, MaxHealth);

            if (CurrentHealth <= 0)
            {
                Debug.Log("AetheriumPlayer death state reached.");
            }
        }

        public void Heal(int amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        }
    }
}

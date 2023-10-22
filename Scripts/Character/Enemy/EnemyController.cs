using Character.Avatar;
using Character.Player;
using Gizmos;
using Mechanics.ObjectsSelection;
using UnityEngine;
using UnityEngine.AI;
using Weapons;

namespace Character.Enemy
{
    public class EnemyController : Human
    {
        [SerializeField] private float _animationSmoothMultiplier;
        
        [Header("Components")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private EnemyHealth _enemyHealth;
        
        [Header("Vision")]
        [SerializeField] private int _visionDistance;

        [Header("Weapon")]
        [SerializeField] private Weapon.Weapon[] _weapons;
        [SerializeField] private Transform _weaponHandler;
        
        [Header("Movement")]
        [SerializeField] private int _attackDistance;
        [SerializeField] private int _speed;
        [SerializeField] private float _acceleration;

        private PlayerController[] _players;
        private EnemyVision _vision;

        private SelectionObject _selectedWeapon;
        
        private Vector2 _moveVector;
        
        public override void Initialize()
        {
            HumanAnimator.Initialize();
            
            SetUpNavMeshAgent();
            SetUpWeapon();
            TryFindPlayers();
            _vision = new EnemyVision(_players, _visionDistance);
            EventManager.OnEnemyDeath.AddListener(OnDeath);
        }

        private void Update()
        {
            if (!_enemyHealth.IsDied)
            {
                _vision.RunVision(transform);
                MoveToTarget();
                Animate();
            }
        }
        
        private void TryFindPlayers()
        {
            _players = FindObjectsOfType<PlayerController>();
        }

        private void SetUpNavMeshAgent()
        {
            _agent.speed = _speed;
            _agent.acceleration = _acceleration;
            _agent.stoppingDistance = _attackDistance;
        }

        private void MoveToTarget()
        {
            Vector3 targetPosition = _vision.GetTargetPosition();
            Vector3 directionToTarget = targetPosition - transform.position;
            directionToTarget.y = transform.position.y;
            _agent.SetDestination(targetPosition);

            if (_vision.IsPlayerVisible())
            {
                transform.rotation = Quaternion.LookRotation(directionToTarget);
            }
        }

        private void Animate()
        {
            Vector2 targetVector = _agent.remainingDistance <= _agent.stoppingDistance ? Vector2.zero : new Vector2(0, 1);
            _moveVector = Vector2.MoveTowards(_moveVector, targetVector,
                _animationSmoothMultiplier * Time.deltaTime);
            HumanAnimator.PlayBodyAnimation(transform, _moveVector, _speed);
        }

        private void SetUpWeapon()
        {
            if (_weapons.Length == 0) return;

            int weaponIndex = Random.Range(0, _weapons.Length - 1);
            Weapon.Weapon weapon = Instantiate(_weapons[weaponIndex]);
            if (weapon.transform.TryGetComponent(out _selectedWeapon))
            {
                //Avatar.BreakBoneConnection(BoneType.HandR);
                _selectedWeapon.PickUp(_weaponHandler);
                _selectedWeapon.transform.SetLayer(LayerMask.NameToLayer("Default"));
            }
        }

        protected override void OnDeath()
        {
            _selectedWeapon.Drop();
            _selectedWeapon.transform.SetLayer(LayerMask.NameToLayer("Weapon"));
            //Avatar.SetBoneConnection(BoneType.HandR);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            UnityEngine.Gizmos.color = new Color(1f, 0.28f, 0.28f);
            GizmosExtensions.DrawCircle(transform.position, transform.forward, transform.right, 30,
                _visionDistance);
            
            UnityEngine.Gizmos.color = new Color(1f, 0.68f, 0.28f);
            GizmosExtensions.DrawCircle(transform.position, transform.forward, transform.right, 30,
                _attackDistance);
        }
#endif
    }
}
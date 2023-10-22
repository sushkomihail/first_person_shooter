using Character.Player;
using UnityEngine;

namespace Character.Enemy
{
    public class EnemyVision
    {
        private readonly PlayerController[] _players;
        private readonly float _visionDistance;
        private PlayerController _nearestPlayer;
        private float _minDistanceToPlayer;
        private Vector3 _targetPosition;

        public EnemyVision(PlayerController[] players, float visionDistance)
        {
            _players = players;
            _visionDistance = visionDistance;
        }
        
        private void FindNearestPlayer(Transform enemyTransform)
        {
            _nearestPlayer = _players[0];
            _minDistanceToPlayer = Vector3.Distance(enemyTransform.position, _nearestPlayer.transform.position);
            foreach (PlayerController player in _players)
            {
                float distance = Vector3.Distance(enemyTransform.position, player.transform.position);
                if (distance < _minDistanceToPlayer)
                {
                    _minDistanceToPlayer = distance;
                    _nearestPlayer = player;
                }
            }
        }

        public bool IsPlayerVisible()
        {
            return _minDistanceToPlayer <= _visionDistance;
        }

        public void RunVision(Transform enemyTransform)
        {
            if (_players == null) return;
            
            FindNearestPlayer(enemyTransform);
            if (IsPlayerVisible())
            {
                _targetPosition = _nearestPlayer.transform.position;
            }
        }

        public Vector3 GetTargetPosition() => _targetPosition;
    }
}

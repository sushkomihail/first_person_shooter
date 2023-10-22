using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletSpec _spec;
    private float _currentDistance;
    private bool _canMove;
    private bool _isCollide;

    private void Update()
    {
        Move();

        if (_isCollide || _currentDistance >= _spec.FlightDistance)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpec(BulletSpec spec)
    {
        _spec = spec;
        _canMove = true;
    }

    private void Move()
    {
        if (_canMove)
        {
            float offset = _spec.Speed * Time.deltaTime;
            _currentDistance += offset;
            transform.position += transform.forward * offset;
        }
    }

    private void OnCollisionEnter()
    {
        _isCollide = true;
    }
}

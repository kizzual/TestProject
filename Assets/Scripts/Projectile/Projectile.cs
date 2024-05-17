using Towers;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Stats")]
    [Min(0.1f)]
    [SerializeField] protected float m_speed = 0.2f;
    [SerializeField] private int m_damage = 10;
    protected Transform _spawnPoint;


    protected Tower _tower;
    protected Monster _target;

    public virtual void Initialize(Monster target, Tower tower, Transform spawnPoint)
    {
        _target = target;
        _tower = tower;
        _spawnPoint = spawnPoint;
        transform.position = spawnPoint.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Monster monster))
        {
            monster.TakeDamage(m_damage);

            _tower.ReturnToPool(this);
            _tower.ReturnAction(this);
        }
    }

    public virtual void Preparation() { }
}

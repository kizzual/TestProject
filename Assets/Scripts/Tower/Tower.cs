using OBJ_pool;
using System.Collections;
using UnityEngine;

namespace Towers
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class Tower : MonoBehaviour
    {
        [Header("Tower attributes")]
        [Min(0.1f)]
        [SerializeField] protected float m_shootInterval = 0.5f;
        [SerializeField] protected float m_range = 4f;

        [Space]
        [Header("Tower components")]
        [SerializeField] protected Projectile m_projectilePrefab;
        [SerializeField] protected Transform m_projectileSpawnPoint;
       

        [Space]
        [Header("Gizmo")]
        [SerializeField] private bool m_showGizmo = false;

        private CapsuleCollider _capsuleCollider;
        protected Monster _targetToAttack;
        private int PreloadCount = 5;
        protected bool _isReloading = false;


        protected ProjectilePool<Projectile> _projectile;
        private Projectile Preload() => Instantiate(m_projectilePrefab);
        private void GetAction(Projectile projectile) => projectile.gameObject.SetActive(true);
        public void ReturnAction(Projectile projectile) => projectile.gameObject.SetActive(false);
        public void ReturnToPool(Projectile obj) => _projectile.Return(obj);

        private void Awake()
        {
            _projectile = new ProjectilePool<Projectile>(Preload, GetAction, ReturnAction, PreloadCount);
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _capsuleCollider.radius = m_range;
            _capsuleCollider.isTrigger = true;
        }

        protected virtual void Update()
        {
            if (!_isReloading && m_projectilePrefab != null && _targetToAttack != null)
            {
                if (!_targetToAttack.IsAlive())
                    CheckNearbyMonsters();
                else
                    Attack();
            }
        }

        protected virtual void Attack()
        {
            Projectile projectile = _projectile.Get();
            projectile.Initialize(_targetToAttack, this, m_projectileSpawnPoint);
            projectile.Preparation();

            StartCoroutine(AttackTimer());
        }

        protected void CheckNearbyMonsters()
        {
           Collider[] colliders = Physics.OverlapSphere(transform.position, m_range);

            Monster closestEnemy = null;
            float shortestDistance = Mathf.Infinity;
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Monster monster))
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, monster.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        closestEnemy = monster;
                    }
                }
            }
            if (closestEnemy != null)
                _targetToAttack = closestEnemy;
            else
                _targetToAttack = null;
        }
        protected virtual IEnumerator AttackTimer()
        {
            _isReloading = true;
            yield return new WaitForSeconds(m_shootInterval);
            _isReloading = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Monster monster) && _targetToAttack == null)
            {
                _targetToAttack = monster;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Monster monster) && monster == _targetToAttack)
            {
                _targetToAttack = null;
                CheckNearbyMonsters();
            }
        }


        private void OnDrawGizmos()
        {
            if(m_showGizmo)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, m_range);
            }
        }
    }
}

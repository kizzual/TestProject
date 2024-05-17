using UnityEngine;
using OBJ_pool;

public class Spawner : MonoBehaviour {

	[Header("Set spawn time")]
    [Min(0.01f)]
    [SerializeField] private float m_interval = 3;

	[Space]
	[Header("Components")]
	[SerializeField] private GameObject m_moveTarget;
	[SerializeField] private Monster m_monsterPrefab;

	private float m_lastSpawn = -1;
    private int _preloadCount = 5;

    private ProjectilePool<Monster> _monster;
    private Monster Preload() => Instantiate(m_monsterPrefab);
    private void GetAction(Monster monster) => monster.gameObject.SetActive(true);
    public void ReturnAction(Monster monster) => monster.gameObject.SetActive(false);
    public void ReturnToPool(Monster obj) => _monster.Return(obj);


    private void Awake()
    {
        _monster = new ProjectilePool<Monster>(Preload, GetAction, ReturnAction, _preloadCount);
    }
    void Update () {
		if (Time.time > m_lastSpawn + m_interval) {

            Monster monster = _monster.Get();
            monster.Initialize(transform.position, m_moveTarget, this);

			m_lastSpawn = Time.time;
		}
	}
}

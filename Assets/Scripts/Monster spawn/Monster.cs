using UnityEngine;

public class Monster : MonoBehaviour 
{
	[Header("Settings")]
	[SerializeField] public float m_speed = 0.1f;
    [SerializeField] private int m_maxHP = 30;

	private GameObject _moveTarget;
	private Spawner _spawner;
	private int _currentHP;
	private bool _isAlive;
	const float m_reachDistance = 0.3f;

	void Start() {
		_currentHP = m_maxHP;
	}

	void Update () {
		if (_moveTarget == null)
			return;
		
		if (Vector3.Distance (transform.position, _moveTarget.transform.position) <= m_reachDistance) {
            _isAlive = false;
            _spawner.ReturnToPool(this);

            return;
		}

		var translation = _moveTarget.transform.position - transform.position;
		if (translation.magnitude > m_speed) {
			translation = translation.normalized * m_speed;
		}
		transform.Translate (translation);
	}

	public void Initialize(Vector3 startPosition, GameObject targetDestination, Spawner spawner)
	{
		_moveTarget = targetDestination;
		transform.position = startPosition;
		_spawner = spawner;
		_isAlive = true;
    }
	public void TakeDamage(int value)
	{
		_currentHP -= value;

		if(_currentHP <= 0)
		{
			//Доп функционал если есть при уничтожении объекта
			_isAlive = false;

            _spawner.ReturnToPool(this);
            _spawner.ReturnAction(this);
        }
	}
	public bool IsAlive() => _isAlive;
}

public class GuidedProjectile : Projectile
{
	void Update()
	{
		if (_target != null && _target.IsAlive())
		{
			var translation = _target.transform.position - transform.position;
			if (translation.magnitude > m_speed)
			{
				translation = translation.normalized * m_speed;
			}
			transform.Translate(translation);
		}
		else
		{
            _tower.ReturnToPool(this);
            _tower.ReturnAction(this);
        }
	}
}
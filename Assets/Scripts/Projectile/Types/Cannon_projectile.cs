using System.Collections;
using UnityEngine;

public class Cannon_projectile : Projectile
{
    [Header("Components")]
    [SerializeField] private ParticleSystem m_particle;

    private Transform _startPoint;
    private Transform _endPoint;
    private Vector3 _controlPoint;
    private float journeyLength;
    private float startTime;


    private void Start()
    {
        


        
    }
    public override void Preparation()
    {
        _startPoint = _spawnPoint;
        _controlPoint = _startPoint.position + _spawnPoint.forward * 4;
        _endPoint = _target.transform;
        journeyLength = Vector3.Distance(_spawnPoint.position, _endPoint.position);
        startTime = Time.time;
        m_particle.Play();
        StartCoroutine(Movement());
    }
    private IEnumerator Movement()
    {

        startTime = Time.time;
        _endPoint = _target.transform;
        _controlPoint = _startPoint.position + _spawnPoint.forward * 4;
        journeyLength = Vector3.Distance(_spawnPoint.position, _endPoint.position);
        m_particle.Play();

        Vector3 startPosition = _startPoint.position; 
        Vector3 endPosition = _endPoint.position; 

        while (true)
        {
           if(_target == null || !_target.IsAlive())
            {
                _tower.ReturnToPool(this);
                _tower.ReturnAction(this);
            }    

            float distCovered = (Time.time - startTime) * m_speed;
            float fracJourney = distCovered / journeyLength;

            transform.position = CalculateBezierPoint(startPosition, _controlPoint, endPosition, fracJourney);

            if (fracJourney >= 1f)
            {
             // доп функционал ( например партикл взрыва)
                _tower.ReturnToPool(this);
                _tower.ReturnAction(this);
                yield break; 
            }

            yield return null;
        }
    }

    Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }
}
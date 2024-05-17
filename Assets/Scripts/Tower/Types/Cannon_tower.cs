using UnityEngine;
using Towers;

public class Cannon_tower : Tower
{
    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private float angleMargin = 2f;
    float angleThreshold = 5f;

    [Space]
    [Header("Components")]
    [SerializeField] private Transform _cannon;
    [SerializeField] private Transform _platform;

    protected override void Update()
    {
        if (_targetToAttack != null)
        {
            RotatePlatformTowardsTarget();
            RotateCannonTowardsTarget();
        }
        base.Update();
    }

    protected override void Attack()
    {
        if (IsAimedAtTarget())
        {
            base.Attack();
        }
    }

    private void RotatePlatformTowardsTarget()
    {
        Vector3 directionToTarget = _targetToAttack.transform.position - _platform.position;
        directionToTarget.y = 0; 

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        _platform.rotation = Quaternion.Slerp(_platform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void RotateCannonTowardsTarget()
    {
        Vector3 directionToTarget = _targetToAttack.transform.position - _cannon.position;

        float distanceToTarget = directionToTarget.magnitude;
        float angleX = Mathf.Atan2(directionToTarget.y, directionToTarget.z) * Mathf.Rad2Deg;

        float maxAngle = Mathf.Lerp(35f, 0f, distanceToTarget / m_range);

        angleX = Mathf.Clamp(angleX, -maxAngle, maxAngle);

        _cannon.localRotation = Quaternion.Euler(angleX, 0, 0);
    }

    private bool IsAimedAtTarget()
    {
        Vector3 targetDirection = _targetToAttack.transform.position - _platform.position;
        targetDirection.y = 0; 
        targetDirection.Normalize();

        Vector3 platformDirection = _platform.forward;
        platformDirection.y = 0; 
        platformDirection.Normalize();

        float platformAngle = Vector3.Angle(platformDirection, targetDirection);

        float angleThresholdWithMargin = angleThreshold + angleMargin;

        return platformAngle < angleThresholdWithMargin;
    }

}
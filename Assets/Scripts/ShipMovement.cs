using UnityEngine;
using System.Collections;

//Vector movement
public class ShipMovement : MonoBehaviour
{
    Transform targetPos;
    float movementSpeed = 3f;
    float rotationSpeed = 1f;
    float attackRange = 10f;
    

    public void Init(EnemyShipData data, Transform targetPos)
    {
        movementSpeed = data.MovementSpeed;
        rotationSpeed = data.RotationSpeed;
        attackRange = data.AttackRange;
        this.targetPos = targetPos;
    }

    void Update()
    {
        if (targetPos == null) return;
        MoveTowardsTarget();
        RotateTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        if (targetPos == null) return;
        
        float dist = Vector3.Distance(transform.position, targetPos.position);
        if (dist <= attackRange) return;
        
        transform.position = Vector3.MoveTowards(transform.position, targetPos.position, movementSpeed * Time.deltaTime);
    }

    void RotateTowardsTarget()
    {
        Vector3 direction = targetPos.position - transform.position;
        if (direction.sqrMagnitude <= 0.01f) return;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);
    }

    public IEnumerator SlowToStop(float duration = 2.5f)
    {
        float startSpeed = movementSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            movementSpeed = Mathf.Lerp(startSpeed, 0f, t);
            
            yield return null;
        }
        
        movementSpeed = 0f;
    }
}

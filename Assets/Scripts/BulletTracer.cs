using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BulletTracer : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float speed = 300f;
    [SerializeField] float tracerLength = 0.75f;

    bool useLineRendererAnimation;

    public void Init(Vector3 start, Vector3 end, bool useLineRendererAnimation)
    {
        if (useLineRendererAnimation)
        {
            StartCoroutine(AnimateTracer(start, end));
        }
        StartCoroutine(MoveTracer(start, end));
    }

    IEnumerator AnimateTracer(Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        float traveled = 0f;
        Vector3 direction = (end - start).normalized;

        while (traveled < distance)
        {
            traveled += speed * Time.deltaTime;

            Vector3 head = start + direction * traveled;
            Vector3 tail = start + direction * Mathf.Max(0f, traveled - tracerLength);

            lineRenderer.SetPosition(0, tail);
            lineRenderer.SetPosition(1, head);

            yield return null;
        }

        lineRenderer.SetPosition(0, end);
        lineRenderer.SetPosition(1, end);

        Destroy(gameObject);
    }
    
    IEnumerator MoveTracer(Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        float traveled = 0f;
        Vector3 direction = (end - start).normalized;

        transform.position = start;
        
        if (direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(direction);

        while (traveled < distance)
        {
            traveled += speed * Time.deltaTime;

            transform.position = start + direction * Mathf.Min(traveled, distance);

            yield return null;
        }

        transform.position = end;

        Destroy(gameObject);
    }
}

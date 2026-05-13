using System.Collections;
using UnityEngine;

// based on Forge3D code
[RequireComponent(typeof(LineRenderer))]
public class IonProjectile : MonoBehaviour
{
    [SerializeField] GameObject hitParticle;
    [SerializeField] Texture[] beamFrames;
    [SerializeField] float frameStep = 0.03f;

    [SerializeField] bool animateUV = true;
    [SerializeField] float uvTime = 3f;

    [SerializeField] LineRenderer lineRenderer;
    int frameNo;
    float animateUVTime;
    float initialBeamOffset;
    Coroutine frameRoutine;

    public void Init(Vector3 start, Vector3 end, float lifetime)
    {
        Vector3 direction = end - start;
        float dist = direction.magnitude;
        
        transform.position = start;
        
        if (direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(direction);

        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, new Vector3(0f, 0f, dist));

        if (hitParticle != null) hitParticle.transform.position = end;
        
        Destroy(gameObject, lifetime);
    }

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        initialBeamOffset = Random.Range(0f, 5f);

        if (!animateUV && beamFrames.Length > 0)
        {
            lineRenderer.material.mainTexture = beamFrames[0];
        }
    }

    void OnEnable()
    {
        if (beamFrames.Length > 1)
        {
            frameRoutine = StartCoroutine(AnimateFrames());
        }
    }

    void OnDisable()
    {
        if (frameRoutine != null)
        {
            StopCoroutine(frameRoutine);
            frameRoutine = null;
        }

        frameNo = 0;
    }

    void Update()
    {
        if (!animateUV) return;

        animateUVTime += Time.deltaTime;

        if (animateUVTime > 1f)
        {
            animateUVTime = 0f;
        }

        float offset = animateUVTime * uvTime + initialBeamOffset;
        lineRenderer.material.SetVector("_Offset", new Vector2(offset, 0f));
    }

    IEnumerator AnimateFrames()
    {
        WaitForSeconds wait = new WaitForSeconds(frameStep);

        frameNo = 0;

        while (true)
        {
            lineRenderer.material.mainTexture = beamFrames[frameNo];

            frameNo++;
            if (frameNo >= beamFrames.Length)
            {
                frameNo = 0;
            }

            yield return wait;
        }
    }
}

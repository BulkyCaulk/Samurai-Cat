using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class WaterfallController : MonoBehaviour
{
    /*────────── Inspector ─────────*/
    [Header("Cycle")]
    [SerializeField] float growTime   = 4.6f;
    [SerializeField] float fullTime   = 3f;
    [SerializeField] float shrinkTime = 4.6f;
    [SerializeField] float offTime    = 1f;

    [Header("Damage")]
    [SerializeField] float dps = 10f;
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule em;
    private BoxCollider2D col;

    private Vector2 fullSize;
    private Vector2 fullOffset;
    private float topY; 
    private float botY;
    void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        em = ps.emission;
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;

        fullSize = col.size;     
        fullOffset = col.offset;   
        // top and bot stay constant
        topY = fullOffset.y + fullSize.y * 0.5f;
        botY = fullOffset.y - fullSize.y * 0.5f;


        col.size   = new Vector2(fullSize.x, 0f);
        col.offset = new Vector2(fullOffset.x, topY);
        col.enabled = false;

        em.enabled = false;
    }

    void Start() => StartCoroutine(Cycle());

    IEnumerator Cycle()
    {
        while (true)
        {
            // Grow from top to down
            em.enabled = true;
            ps.Play();
            col.enabled = true;

            for (float t = 0; t < growTime; t += Time.deltaTime)
            {
                float h = Mathf.Lerp(0f, fullSize.y, t / growTime); // height
                float y = topY - h * 0.5f;                         // center so top fixed
                col.size   = new Vector2(fullSize.x, h);
                col.offset = new Vector2(fullOffset.x, y);
                yield return null;
            }
            col.size   = fullSize;
            col.offset = fullOffset;

            yield return new WaitForSeconds(fullTime);
            em.enabled = false;

            // shrinks top to down
            for (float t = 0; t < shrinkTime; t += Time.deltaTime)
            {
                float h = Mathf.Lerp(fullSize.y, 0f, t / shrinkTime);
                float y = botY + h * 0.5f;                         // bot fixed
                col.size   = new Vector2(fullSize.x, h);
                col.offset = new Vector2(fullOffset.x, y);
                yield return null;
            }
            col.size   = new Vector2(fullSize.x, 0f);
            col.offset = new Vector2(fullOffset.x, topY);
            col.enabled = false;

            yield return new WaitForSeconds(offTime);
        }
    }
}

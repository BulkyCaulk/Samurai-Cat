using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class Bouncy_Mushroom : MonoBehaviour
{
    [Tooltip("Velocity from bounce")]
    [SerializeField] private float bounceVelocity = 12f;
    [SerializeField] private float horizontalBounceVelocity = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private float horizontalSlowBounceDuration = 1f;
    //[SerializeField] private float 
    [Tooltip("Set to 'horizontal' for horizontal bounce")]
    [SerializeField] private string direction = "vertical";
    [SerializeField] private AudioClip bounceSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            animator.SetTrigger("trigger");
            if (direction == "vertical")
            {
                rb.velocity = new Vector2(rb.velocity.x, bounceVelocity);
                SoundFXManager.instance.PlaySoundEffectClip(bounceSound, transform, 1f);
            }
            else if (direction == "horizontal")
            {
                var kb = other.GetComponent<Knockback>();
                float sign = other.transform.position.x < transform.position.x ? -1f : 1f;
                Vector2 hitDir = new Vector2(sign, bounceVelocity);
                IEnumerator SlowTime()
                {
                    Time.timeScale = 0.5f;
                    SoundFXManager.instance.PlaySoundEffectClip(bounceSound, transform, 1f);
                    kb.hitDirectionForce = horizontalBounceVelocity;
                    kb.CallKnockBackAction(hitDir, Vector2.zero, 0f);
                    yield return new WaitForSecondsRealtime(horizontalSlowBounceDuration);
                    Time.timeScale = 1;
                }
                StartCoroutine(SlowTime());
            }
        }


        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.BounceMushroomResetJump();
        }
    }
}

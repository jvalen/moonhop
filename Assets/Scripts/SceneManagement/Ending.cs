using UnityEngine;

public class Ending : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] AudioSource landingSound;
    Rigidbody rb;

    private void Awake()
    {
        Physics.gravity = new Vector3(0, -0.8F, 0);

        rb = GetComponent<Rigidbody>();
        playerAnimator.SetBool("land", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        landingSound.Play();
        Physics.gravity = new Vector3(0, 0, 0);
        rb.velocity = Vector3.zero;
    }
}

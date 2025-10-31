using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;
    public float detectionRange = 5f;
    bool moving = false;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > detectionRange) return;
        if (moving) return;

        Vector3 direction = (target.position - transform.position).normalized;

        direction = direction.normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
        {
            anim.SetBool("Attack", true);
            moving = true;
            Debug.Log("Düþman tetikledi!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("Attack", false);
            moving= false;
        }
    }
}

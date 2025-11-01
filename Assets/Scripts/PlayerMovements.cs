using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;
    public Transform attackPoint;
    public float attackRange = 0.8f;
    public float attackCooldown = 0.8f;
    public int attackDamage = 20;
    public LayerMask enemyLayer;

    Rigidbody rb;
    Animator anim;
    float xRotation = 0f;
    bool isGrounded;
    float nextAttackTime = 0f;

    public AudioSource attackAudio;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isPaused) return;
        if(GameManager.instance != null && GameManager.instance.isFinished) return;

        if (GameManager.instance.hasE && Input.GetKeyDown(KeyCode.E))
        {
            // Can verme iþlemi
            PlayerHealth.instance.Heal(20); // örnek olarak 20 can versin
        }


        Move();
        MouseLook();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }


        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Move()
    {
        float x = 0f;
        float z = 0f;

        // W tuþu her zaman aktif
        if (Input.GetKey(KeyCode.W)) z = 1f;

        // A, S, D harfleri GameManager’a göre açýlýr
        if (GameManager.instance.hasA && Input.GetKey(KeyCode.A)) x = -1f;
        if (GameManager.instance.hasD && Input.GetKey(KeyCode.D)) x = 1f;
        if (GameManager.instance.hasS && Input.GetKey(KeyCode.S)) z = -1f;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 newPos = rb.position + move * currentSpeed * Time.deltaTime;

        rb.MovePosition(newPos);

        float speed = new Vector2(x, z).magnitude * currentSpeed;
        anim.SetFloat("Speed", speed);
    }


    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -10f, 10f);
        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        Debug.Log("Player: saldýrý animasyonu tetiklendi");
        if (attackAudio != null && attackAudio.clip != null)
        {
            attackAudio.Play();
            Debug.Log("Vurma sesi çaldý!");
        }

    }


    public void DealDamage()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning("Player: attackPoint atanmadý.");
            return;
        }

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                eh.TakeDamage(attackDamage);
                Debug.Log($"Player vurdu: {enemy.name} -> {attackDamage} hasar");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("end"))
        {

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            if (GameManager.instance.hasEnter && Input.GetKeyDown(KeyCode.Return))
            {
                GameManager.instance.Finish();
                Debug.Log("Oyun bitiþ tetiklendi!");
            }
        }
    }
}

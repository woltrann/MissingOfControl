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

        Move();
        MouseLook();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }

        // Sald�r� kontrol�
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

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
        Debug.Log("Player: sald�r� animasyonu tetiklendi");
    }

    // ---- Animation Event taraf�ndan �a�r�lacak fonksiyon ----
    // Attack animasyonunda vuru� an�na bir Animation Event ekleyip bu fonksiyonu se�.
    public void DealDamage()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning("Player: attackPoint atanmad�.");
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
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("karakter d��mana temas etti");
        }
    }
}

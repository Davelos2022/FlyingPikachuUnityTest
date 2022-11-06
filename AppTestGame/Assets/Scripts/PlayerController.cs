using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float powerForce;
    [SerializeField] private int maxDistanceY;
    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private GameObject destroyEffect;

    private Rigidbody rbPlayer;

    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        CreateEffect(spawnEffect, transform.position);
    }
    void FixedUpdate()
    {
        Move();   
    }
    private void Move()
    {
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.UpArrow))
        {
            if (rbPlayer.position.y > maxDistanceY)
                rbPlayer.AddForce(-Vector3.up * powerForce);
            else
                rbPlayer.AddForce(Vector3.up * powerForce);
        }

        rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, 
            Mathf.Clamp(rbPlayer.velocity.y, rbPlayer.velocity.y, maxDistanceY), rbPlayer.velocity.z);
    }

    private void CreateEffect(GameObject effectPrefab, Vector3 position)
    {
        GameObject effect = Instantiate(effectPrefab);
        effect.transform.position = position;
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstracle"))
        {
            GameManager.Instance?.LoseGame();

            CreateEffect(destroyEffect, transform.position);
            Destroy(gameObject);
        }
    }
    private void OnDisable()
    {
        Destroy(gameObject);
    }
}

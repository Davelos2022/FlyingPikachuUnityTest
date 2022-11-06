using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAngle;
    [SerializeField] private float rotationSpeed;

    void Update()
    {
        Rotation();
    }

    private void Rotation()
    {
        transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);
    }
}

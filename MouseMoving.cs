using UnityEngine;
public class MouseMoving : MonoBehaviour
{
    public float Mousesensitivity = 100f;
    float xRotation = 0f;
    float yRotation = 0f;
    public float topclamp = -90f;
    public float bottomclamp = 90f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Mousesensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * Mousesensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topclamp, bottomclamp);

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}

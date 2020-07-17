using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{
    [Header("General")]
    [Tooltip("In m/s")][SerializeField] private float xSpeed = 50f;
    [Tooltip("In m/s")] [SerializeField] private float ySpeed = 50f;
    [SerializeField] private float xClamp = 25f;
    [SerializeField] private float yClamp = 16f;
    [SerializeField] GameObject[] guns;

    [Header("Posistion-based aiming")]
    [SerializeField] private float positionPitchFactor = 0.3f;
    [SerializeField] private float positionYawFactor = 1f;

    [Header("Controller-based aiming")]
    [SerializeField] private float controlPitchFactor = 20f;
    [SerializeField] private float controlYawFactor = 20f;
    [SerializeField] private float controlRollFactor = 20f;

    private float xThrow = 0f;
    private float yThrow = 0f;

    private bool controlsEneabled = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!controlsEneabled)
        {
            return;
        }

        ProcessTranslation();
        ProcessRotation();
        ProcessFire();
    }

    private void ProcessRotation()
    {
        float pitch = transform.localPosition.y * positionPitchFactor - yThrow * controlPitchFactor;
        float yaw = transform.localPosition.x * positionYawFactor + xThrow * controlYawFactor;
        float roll = - xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessTranslation()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        float xOffset = xThrow * xSpeed * Time.deltaTime;

        yThrow = CrossPlatformInputManager.GetAxis("Vertical");
        float yOffset = yThrow * ySpeed * Time.deltaTime;

        float newX = Mathf.Clamp(gameObject.transform.localPosition.x + xOffset, - xClamp, xClamp);
        float newY = Mathf.Clamp(gameObject.transform.localPosition.y + yOffset, - yClamp, yClamp);
        float newZ = gameObject.transform.localPosition.z;

        transform.localPosition = new Vector3(newX, newY, newZ);
    }

    private void ProcessFire()
    {
        bool firing = CrossPlatformInputManager.GetButton("Fire");

        ActivateGuns(firing);
    }

    private void ActivateGuns(bool active)
    {
        foreach (GameObject gun in guns)
        {
            gun.SetActive(active);
        }
    }

    public void onPlayerDeath()
    {
        controlsEneabled = false;
        ActivateGuns(false);
    }
}

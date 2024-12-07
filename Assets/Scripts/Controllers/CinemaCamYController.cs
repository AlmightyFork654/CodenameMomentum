using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemaCamYController : MonoBehaviour
{
    HumanoidLandController controller;

    public CinemachineFreeLook freeLookCam;
    public float yAxisSpeed = 1.0f;
    public float yAxisSpeedBase = 1.0f;
    public float smoothTime = 0.2f;

    public float xAxisSpeedBase = 600.0f;
    public float xAxisSpeed = 600.0f;

    private float targetYAxisValue;
    private float currentYAxisValue;
    private float yAxisVelocity = 0.0f;

    private void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<HumanoidLandController>();
    }

    private void Start()
    {

        targetYAxisValue = freeLookCam.m_YAxis.Value;
        currentYAxisValue = freeLookCam.m_YAxis.Value;
    }

    void Update()
    {
        if (!controller.isSlomo)
        {
            yAxisSpeed = yAxisSpeedBase + PlayerPrefs.GetFloat("YMulti");
            xAxisSpeed = xAxisSpeedBase + PlayerPrefs.GetFloat("XMulti");

            freeLookCam.m_XAxis.m_MaxSpeed = xAxisSpeed;

            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (scrollInput != 0)
            {
                targetYAxisValue -= scrollInput * yAxisSpeed;
                targetYAxisValue = Mathf.Clamp(targetYAxisValue, 0f, 1f);
            }

            currentYAxisValue = Mathf.SmoothDamp(currentYAxisValue, targetYAxisValue, ref yAxisVelocity, smoothTime);
        }
        else
        {
            freeLookCam.m_XAxis.m_MaxSpeed = 0f;
        }

        freeLookCam.m_YAxis.Value = currentYAxisValue;
    }
}

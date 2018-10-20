using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float DampTime = 0.15f;
    public Transform Target;

    public Action OnScaleChanged = () => { };

    private Vector3 _velocity = Vector3.zero;
    public float _scale = 0;

    private void Start()
    {
        ChangeScale(0);
    }

    void Update()
    {
        if (Target)
        {
            Vector3 point = Camera.main.WorldToViewportPoint(Target.position);
            Vector3 delta = Target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, DampTime);
        }

        ProcessScale();
    }

    private void ProcessScale()
    {

#if UNITY_EDITOR
        if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0)
        {
            ChangeScale(-Input.GetAxis("Mouse ScrollWheel"));
        }
#endif

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            ChangeScale(deltaMagnitudeDiff/100f);    
        }
    }

    private void ChangeScale(float v)
    {
        _scale = Mathf.Clamp(_scale+v/10f, 0, 1);
        Camera.main.orthographicSize = Mathf.Lerp(20, 2000, _scale); //4000 for 1000 cells on screen
        OnScaleChanged();
    }
}

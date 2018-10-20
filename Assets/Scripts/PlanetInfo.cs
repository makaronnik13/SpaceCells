using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetInfo : MonoBehaviour {

    private static float _showHideTime = 0.3f;
    private Transform _planetTransform;

    private void Update()
    {
        if (_planetTransform)
        {
            transform.position = Camera.main.WorldToScreenPoint(_planetTransform.position);
        }
    }

    public void Init(Transform planetTransform, Planet planet)
    {
        _planetTransform = planetTransform;
        GetComponentInChildren<Text>().text = planet.rating.ToString();
        transform.localScale = Vector3.zero;
        StartCoroutine(ChangeScale(0.3f, Vector3.one));
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScale(0.3f, Vector3.zero, ()=> { Destroy(gameObject);}));
    }

    private IEnumerator ChangeScale(float showTime, Vector3 aimScale, Action callback = null)
    {
        float t = 0;
        Vector3 startScale = transform.localScale;
        while (t<showTime)
        {
            transform.localScale = Vector3.Lerp(startScale, aimScale, t/showTime);
            t += Time.deltaTime * showTime;
            yield return null;
        }

        if (callback!=null)
        {
            callback();
        }
    }
}

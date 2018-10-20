using System;
using UnityEngine;

public class PlanetVisual: MonoBehaviour
{
    private float _rotationSpeed;

    public Planet Planet
    {
        get
        {
            return _planet;
        }
    }
    private Planet _planet;

    public void Init(Planet planet)
    { 
        _rotationSpeed = UnityEngine.Random.Range(0.1f, 0.3f);
        _planet = planet;
        transform.GetChild(0).localScale = planet.size * Vector3.one;
        //GetComponentInChildren<MeshRenderer>().material = planet.material;
        GetComponentInChildren<SpriteRenderer>().sprite = planet.sprite;
    }
}
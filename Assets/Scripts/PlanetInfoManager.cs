using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfoManager : MonoBehaviour {

    public GameObject PlanetPrefab;

    private Dictionary<PlanetVisual, PlanetInfo> _infos = new Dictionary<PlanetVisual, PlanetInfo>();

    private void Start()
    {
        FindObjectOfType<SpaceShip>().OnMovementLockedStateChanged += OnMove;
        StartCoroutine(ShowInfoOnStart());
    }

    private IEnumerator ShowInfoOnStart()
    {
        yield return null;
        OnMove(true);
    }

    private void OnMove(bool v)
    {
        if (v)
        {
            List<PlanetVisual> nearest = FindObjectOfType<BoardData>().GetNearestPlanets(FindObjectOfType<SpaceShip>().Position, 3);
            List<PlanetVisual> removing = new List<PlanetVisual>();
            foreach (KeyValuePair<PlanetVisual, PlanetInfo> pair in _infos)
            {
                if (!nearest.Contains(pair.Key))
                {
                    pair.Value.Hide();
                    removing.Add(pair.Key);
                }
            }

            foreach (PlanetVisual pv in removing)
            {
                _infos.Remove(pv);
            }

            foreach (PlanetVisual pv in nearest)
            {
                if (!_infos.ContainsKey(pv))
                {
                    ShowInfo(pv);
                }
            }
        }
    }

    private void ShowInfo(PlanetVisual pv)
    {
        PlanetInfo info = Instantiate(PlanetPrefab, Camera.main.WorldToScreenPoint(pv.transform.position), Quaternion.identity, transform).GetComponent<PlanetInfo>();
        info.Init(pv.transform, pv.Planet);
        _infos.Add(pv, info);
    }
}

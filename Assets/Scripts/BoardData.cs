using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardData : MonoBehaviour
{
    private int _radarDistance
    {
        get
        {
            return Mathf.RoundToInt(Camera.main.orthographicSize / 4) + 6;
        }
    }

    private Dictionary<Vector2Int, PlanetVisual> _visiblePlanets = new Dictionary<Vector2Int, PlanetVisual>();
    private Dictionary<Vector2Int, Cell> _cells = new Dictionary<Vector2Int, Cell>();
    public static float CellSize
    {
        get
        {
            return Screen.height / 75;
        }
    }

    public List<PlanetVisual> GetNearestPlanets(Vector2Int position, int count)
    {  
        List<KeyValuePair<Vector2Int, PlanetVisual>> nearest = _visiblePlanets.OrderBy(p => Vector2Int.Distance(p.Key, position)).Take(count).ToList();
        return nearest.Select(p=>p.Value).ToList();
    }

    public GameObject PlanetPrefab;

    public Cell GetCell(int x, int y)
    {
        Vector2Int position = new Vector2Int(x,y);


        if (!_cells.ContainsKey(position))
        {
            _cells.Add(position, CreateCell());
        }

        return _cells[position];
    }

    private Cell CreateCell()
    {
        if (UnityEngine.Random.value < 0.7f)
        {
            return new Cell();
        }

        return new Cell(CreatePlanet());
    }

    private Planet CreatePlanet()
    {
        return new Planet(RandomSprite, UnityEngine.Random.Range(0.7f, 1.2f), UnityEngine.Random.Range(0, 1000));
    }

    private Sprite[] _planetMaterials = new Sprite[0];
    private Sprite RandomSprite
    {
        get
        {
            if (_planetMaterials.Length==0)
            {
                _planetMaterials = Resources.LoadAll<Sprite>("Planets");
            }
            return _planetMaterials[UnityEngine.Random.Range(0, _planetMaterials.Length)];
        }
    }

    private void Start()
    {
        Repaint();
        FindObjectOfType<SpaceShip>().OnMovementLockedStateChanged += ShipMoved;
        FindObjectOfType<CameraController>().OnScaleChanged += Repaint;
    }

    private void Repaint()
    {
        bool rotatePlanets = Camera.main.orthographicSize < 100;
        foreach (KeyValuePair<Vector2Int, PlanetVisual> pair in _visiblePlanets)
        {
            pair.Value.enabled = rotatePlanets;
        }

        Vector2Int shipPosition = FindObjectOfType<SpaceShip>().Position;
        foreach (KeyValuePair<Vector2Int, PlanetVisual> pv in _visiblePlanets)
        {
            pv.Value.gameObject.SetActive(false);
        }

        for (int i = shipPosition.x - _radarDistance / 2; i < shipPosition.x + _radarDistance / 2; i++)
        {
            for (int j = shipPosition.y - _radarDistance / 2; j < shipPosition.y + _radarDistance / 2; j++)
            {
                Cell c = GetCell(i, j);

                if (c.planet != null)
                {
                    Vector2Int pos = new Vector2Int(i, j);

                    if (_visiblePlanets.ContainsKey(pos))
                    {
                        _visiblePlanets[pos].gameObject.SetActive(true);
                    }
                    else
                    {

                        PlanetVisual newPlanet = Instantiate(PlanetPrefab, (Vector2)(pos * (int)CellSize), Quaternion.identity, transform).GetComponent<PlanetVisual>();
                        newPlanet.Init(c.planet);
                        _visiblePlanets.Add(pos, newPlanet);
                    }
                }
            }
        }
    }

    private void ShipMoved(bool v)
    {
        if (v)
        {
            Repaint(); 
        }
    }

   
}

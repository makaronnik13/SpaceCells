using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGrid : MonoBehaviour
{
    private Vector3 _offset;

    private bool _drawLines
    {
        get
        {
            return _scale < 75f;
        }
    }

    private int _scale
    {
        get
        {
            return Mathf.CeilToInt(Camera.main.orthographicSize / 4)+6;
        }
    }

    public GameObject LinePrefab;


    private List<LineRenderer> _lines = new List<LineRenderer>();

    private void Start()
    {
        FindObjectOfType<CameraController>().OnScaleChanged += Redraw;
        FindObjectOfType<SpaceShip>().OnMovementLockedStateChanged += ShipMoved;
        Redraw();
    }

    private void ShipMoved(bool v)
    {
        if (v)
        {
            Redraw();
        }
    }

    void Redraw ()
    {
        if (!_drawLines)
        {
            foreach (LineRenderer lr in _lines)
            {
                lr.enabled = false;
            }
            return;
        }

        _offset = (Vector2)FindObjectOfType<SpaceShip>().Position*(int)BoardData.CellSize;

        while (_lines.Count<_scale*2)
        {
            GameObject newLine = ObjectPool.Instance.GetObject(LinePrefab);
            newLine.transform.SetParent(transform);
            _lines.Add(newLine.GetComponent<LineRenderer>());
        }

        int k = 0;
        foreach (LineRenderer lr in _lines)
        {
            lr.enabled = (k < _scale * 2); 
            k++;
            float w = _scale * 0.02f - 0.07f;
            lr.SetWidth(w, w);
        }

        for (int i = 0; i<_scale; i++)
        {
            Vector3 start = new Vector3((i-Mathf.FloorToInt(_scale/2) + 0.5f)  * BoardData.CellSize, -(_scale+2)/2 * BoardData.CellSize);
            Vector3 end = new Vector3((i - Mathf.FloorToInt(_scale / 2)+0.5f) * BoardData.CellSize, (_scale+2)/2 * BoardData.CellSize);

            _lines[i].SetPosition(0, start+_offset);
            _lines[i].SetPosition(1, end + _offset);
        }

        for (int i = _scale; i < _scale*2; i++)
        {

            Vector3 start = new Vector3(-(_scale+2) / 2 * BoardData.CellSize, (i - _scale - Mathf.CeilToInt(_scale / 2) + 0.5f) * BoardData.CellSize);
            Vector3 end = new Vector3((_scale+2) / 2 * BoardData.CellSize, (i -_scale -  Mathf.CeilToInt(_scale / 2) + 0.5f) * BoardData.CellSize);

            _lines[i].SetPosition(0, start+_offset);
            _lines[i].SetPosition(1, end+_offset);
        }
    }

}

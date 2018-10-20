using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    private Vector2Int _position = Vector2Int.zero;

    public Vector2Int Position
    {
        get
        {
            return _position;
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private readonly float _movementTime = 0.4f;

    private bool _movementLocked = false;
    public bool movementLocked
    {
        get
        {
            return _movementLocked;
        }
        private set
        {         
            _movementLocked = value;
            OnMovementLockedStateChanged(_movementLocked);
        }
    }
    public Action<bool> OnMovementLockedStateChanged = (v) => { };

	public void Move(int dir)
    {
        Vector2Int dirVector = DirectionVector((Direction)dir);
        StartCoroutine(MoveShip(_movementTime, dirVector));
        movementLocked = true;
    }

    private IEnumerator MoveShip(float movementTime, Vector2Int dir)
    {
        _position += dir;

        Vector3 basePosition = transform.position;
        Quaternion baseRotation = transform.rotation;
        float t = movementTime;
        while (t>0)
        {
            t -= Time.deltaTime;
            transform.position = Vector3.Lerp(basePosition+(Vector3)(Vector2)(dir*(int)BoardData.CellSize), basePosition, t /movementTime);

            
            var angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
            Quaternion aimRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.rotation = Quaternion.Lerp(aimRotation, baseRotation, t/movementTime);
            yield return null;
        }
        movementLocked = false;
    }

    private Vector2Int DirectionVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.Down:
                return Vector2Int.down;
            case Direction.Up:
                return Vector2Int.up;
            case Direction.Left:
                return Vector2Int.left;
            case Direction.Right:
                return Vector2Int.right;
            default:
                return Vector2Int.zero;
        }
    }
}

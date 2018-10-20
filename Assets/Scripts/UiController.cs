using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public SpaceShip spaceShip;


    public Button Up, Down, Left, Right;

	void Start ()
    {
        spaceShip.OnMovementLockedStateChanged += MovementStateChanged;
	}

    private void MovementStateChanged(bool v)
    {
        Up.interactable = !v;
        Down.interactable = !v;
        Left.interactable = !v;
        Right.interactable = !v;
    }
}

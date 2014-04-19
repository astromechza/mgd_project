﻿using UnityEngine;
using System.Collections;

// from http://unityshorttutorials.blogspot.com/2013/11/drag-and-drop-in-unity.html
public class DragAndDrop : MonoBehaviour {

	private Vector3 screenPoint;
	void OnMouseDown () {
		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
	}

	void OnMouseDrag () {
		Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 currentPos = Camera.main.ScreenToWorldPoint (currentScreenPoint);
		transform.position = currentPos;
	}
}

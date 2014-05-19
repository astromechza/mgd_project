﻿using UnityEngine;
using System.Collections;
using Pathfinding;

public class AstarAI : MonoBehaviour {
	//The point to move to
	private Vector3 targetPosition;
	private GameObject targetObject;	
	private Seeker seeker;
	private Path path;
	
	// Sounds
	public AudioClip citadel_hit;
	
	//The AI's speed per second
	public float speed = 1;
	
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	private float nextWaypointDistanceSqrd;
	
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;
	
	public void Awake(){
		seeker = GetComponent<Seeker>();
		
		// Register callback to update creeps path when a tower placement affects the graph.
		AstarPath.OnGraphsUpdated += RecalculatePath;
	}
	
	// This is where you set the creep's target (i.e. the citadel) and start moving.
	public void DestroyTarget(GameObject theTargetObject){
		targetObject = theTargetObject;
		targetPosition = targetObject.transform.position;
		nextWaypointDistanceSqrd = nextWaypointDistance * nextWaypointDistance;
		
		RecalculatePath (null);
	}
	
	public void RecalculatePath(AstarPath script){
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
		seeker.StartPath (transform.position, targetPosition, OnPathComplete);
	}
	
	public void OnPathComplete (Path p) {
		Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
		if (!p.error) {
			path = p;
			path.Claim(this);
			
			//Reset the waypoint counter
			currentWaypoint = 0;
		}
	}
	
	public void Update () {
		//We have no path to move after yet
		if (path == null) return;
	

		if (currentWaypoint < path.vectorPath.Count) {
			Vector3 diff = path.vectorPath[currentWaypoint] - transform.position;
			Vector3 direction = diff.normalized;
			float distance = diff.sqrMagnitude;
			float stepDistance = speed * Time.deltaTime;
	
			// step toward waypoint
			transform.position += direction * stepDistance;

			// rotate to face direction
			float angle = Vector3.Angle(direction, Vector3.right);
			if (direction.z > 0) angle = -angle;
			transform.rotation = Quaternion.Euler(0, angle + 90, 0);

			// if we moved further than needed or are close enough to the last waypoint, then target next waypoint.
			if (stepDistance > distance || 
			    (transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistanceSqrd ) {
				currentWaypoint++;
			}

		} else { // we are at the end of the list of waypoints

			// are we at the target?
			if ((transform.position - targetPosition).sqrMagnitude <= 1) {
				OnTargetReached();
			}else{
				OnTargetNotReached();
			}
			
			Destroy(gameObject);
		}
	}
	
	private void OnTargetReached(){
		// Add logic here when creep reaches citadel.
		AudioSource.PlayClipAtPoint (citadel_hit, Camera.main.transform.position);
		// Decrement Citadel Lives
		GameController.Instance.citadelLives--;
		Debug.Log ("Creep reached citadel.");
	}
	
	private void OnTargetNotReached(){
		// Add logic here when creeps reach end of path but not citadel.
		
		Debug.Log ("Creep could not reach citadel.");
	}
	
	public void OnDestroy () {
		seeker.pathCallback -= OnPathComplete;
		AstarPath.OnGraphsUpdated -= RecalculatePath;
		if (path != null) {
			path.Release (this);
		}
	} 
} 
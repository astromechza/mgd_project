﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	GameObject sonarTower;
	GameObject missileTower;
	GameObject gunTower;
	GameObject beamTower;
	public GameObject AIController;
	public GameObject Globals;

	public Texture2D creditIcon;
	public Texture2D healthIcon;
	public Texture2D waveIcon;
	
	public GUIStyle guiStyle;

	Vector3 sonarTowerPos;
	Vector3 missileTowerPos;
	Vector3 gunTowerPos;
	Vector3 beamTowerPos;
	Vector3 interfaceTopPos;

	float sonarTowerPosX;
	float sonarTowerPosY;
	float missileTowerPosX;
	float missileTowerPosY;
	float gunTowerPosX;
	float gunTowerPosY;
	float beamTowerPosX;
	float beamTowerPosY;

	int sonarTowerCost;
	int missileTowerCost;
	int gunTowerCost;
	int beamTowerCost;

	bool isPaused = false;
		
	void Start()
	{
		// Only create these objects on the first run. Not when it was paused.
		if (!GameController.Instance.gameWasPaused) {

			Instantiate(AIController);

			// Create Globals
			Instantiate (Globals);
		}

		sonarTower = GameObject.Find ("Sonar Tower Builder");
		missileTower = GameObject.Find ("Missile Tower Builder");
		gunTower = GameObject.Find ("Gun Tower Builder");
		beamTower = GameObject.Find ("Beam Tower Builder");

		// Get the towers positions
		sonarTowerPos = Camera.main.WorldToScreenPoint(sonarTower.transform.position);
		missileTowerPos = Camera.main.WorldToScreenPoint(missileTower.transform.position);
		gunTowerPos = Camera.main.WorldToScreenPoint(gunTower.transform.position);
		beamTowerPos = Camera.main.WorldToScreenPoint(beamTower.transform.position);
		interfaceTopPos = Camera.main.WorldToScreenPoint (GameObject.Find ("Interface top").transform.position);

		sonarTowerCost = missileTower.GetComponent<TowerProperties> ().cost;
		missileTowerCost = missileTower.GetComponent<TowerProperties> ().cost;
		gunTowerCost = missileTower.GetComponent<TowerProperties> ().cost;
		beamTowerCost = missileTower.GetComponent<TowerProperties> ().cost;

		sonarTowerPosX = sonarTowerPos.x;
		sonarTowerPosY = sonarTowerPos.y;
		missileTowerPosX = missileTowerPos.x;
		missileTowerPosY = missileTowerPos.y;
		gunTowerPosX = gunTowerPos.x;
		gunTowerPosY = gunTowerPos.y;
		beamTowerPosX = beamTowerPos.x;
		beamTowerPosY = beamTowerPos.y;

		// Resume Game if it was paused
		resumeGame();
	}
	
	private void resumeGame(){
		Time.timeScale = 1;
	}
	
	private void pauseGame(){
		Time.timeScale = 0;
		GameController.Instance.gameWasPaused = true;
	}
	
	void Update(){

		if (Input.GetKeyDown(KeyCode.Escape)) { 
			// Set timePlayed to the new totalTimePlayed (includes time played in this scene and previous time played)
			AchievementController.Instance.timePlayed = AchievementController.Instance.totalTimePlayed;
			// Pause the game
			pauseGame();
			// Go to Main Menu
			Application.LoadLevel ("menu");
		}

		if (GameController.Instance.citadelLives == 0) {
			Debug.Log("Add Lose Game stuff here");
		}
	}
	
	void OnGUI(){
		
		// Draw Tower coins and prices
		GUI.DrawTexture(new Rect(sonarTowerPosX+7,Screen.height-sonarTowerPosY+7,20,20), creditIcon);
		GUI.Label(new Rect(sonarTowerPosX+13,Screen.height-sonarTowerPosY+10,20,20), sonarTowerCost.ToString(), guiStyle);
		GUI.DrawTexture(new Rect(missileTowerPosX+7,Screen.height-missileTowerPosY+7,20,20), creditIcon);
		GUI.Label(new Rect(missileTowerPosX+13,Screen.height-missileTowerPosY+10,20,20), missileTowerCost.ToString(), guiStyle);
		GUI.DrawTexture(new Rect(gunTowerPosX+7,Screen.height-gunTowerPosY+7,20,20), creditIcon);
		GUI.Label(new Rect(gunTowerPosX+13,Screen.height-gunTowerPosY+10,20,20), gunTowerCost.ToString(), guiStyle);
		GUI.DrawTexture(new Rect(beamTowerPosX+7,Screen.height-beamTowerPosY+7,20,20), creditIcon);
		GUI.Label(new Rect(beamTowerPosX+13,Screen.height-beamTowerPosY+10,20,20), beamTowerCost.ToString(), guiStyle);
		
		// Draw Credits, health and waves
		GUI.DrawTexture(new Rect(Screen.width-170,Screen.height-interfaceTopPos.y-10,20,20), creditIcon);
		GUI.Label(new Rect(Screen.width-145,Screen.height-interfaceTopPos.y-7,100,20), GameController.Instance.citadelCredits.ToString(), guiStyle);
		GUI.DrawTexture(new Rect(Screen.width-110,Screen.height-interfaceTopPos.y-10,20,20), healthIcon);
		GUI.Label(new Rect(Screen.width-85,Screen.height-interfaceTopPos.y-7,100,20), GameController.Instance.citadelLives.ToString(), guiStyle);
		GUI.DrawTexture(new Rect(15,Screen.height-interfaceTopPos.y-10,20,20), waveIcon);
		GUI.Label(new Rect(40,Screen.height-interfaceTopPos.y-7,100,20), "WAVE "+GameController.Instance.currentWave.ToString()+"/"+GameController.Instance.numberOfWaves.ToString(), guiStyle);	
	}
}

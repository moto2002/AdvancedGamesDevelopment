﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class NetManager :  LobbyManager{

	/*----------------------------------------------------------------------------------------------------------------
	----------------------------------------------------- MENU  ------------------------------------------------------
	----------------------------------------------------------------------------------------------------------------*/
	[Header("UI Settings")]
	public GameObject settingsPanel;
	public Dropdown qualityDropdown;
	public Slider volumeSlider;

    public override void Start()
    {
        base.Start();
        SceneManager.sceneLoaded += LevelOnLoad;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= LevelOnLoad;
    }


    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
	{
		var spawnLocations = GameObject.FindObjectsOfType<NetworkStartPosition>().ToList();
		var playerSpawns = spawnLocations.FindAll(x => x.gameObject.CompareTag("playerSpawn")).ToList();
		Transform spawnLocation = playerSpawns[Random.Range(0, playerSpawns.Count)].transform;
		GameObject go = Instantiate(gamePlayerPrefab, spawnLocation.position, spawnLocation.rotation) as GameObject;
		LobbyPlayer player = lobbySlots[conn.connectionId].gameObject.GetComponent<LobbyPlayer>();
		NetworkedThirdPersonCharacter pl = go.GetComponent<NetworkedThirdPersonCharacter>();
		pl.name = player.playerName;
		pl.playerName = player.playerName;
		pl.playerColour = player.playerColor;
		return go;

	}

	public void ShowSettings(bool show)
	{
		settingsPanel.SetActive(show);
	}

	public void ApplyQualitySettings()
	{
		QualitySettings.SetQualityLevel(qualityDropdown.value);
		PlayerPrefs.SetFloat("Setting.Volume", volumeSlider.value);
		ShowSettings(false);
	}

	public void LevelOnLoad(Scene newScene, LoadSceneMode mode)
	{
		if(newScene.buildIndex != 0 && !GameManager.instance.enabled)
		{
			GameManager.instance.enabled = true;
            GameManager.instance.enemiesRemainigText = GameObject.Find("EnemiesRemaining").GetComponent<Text>();
            GameManager.instance.SpawnEnemies();
		    
		}
	}


    public void QuitGame()
    {
        Application.Quit();
    }
}
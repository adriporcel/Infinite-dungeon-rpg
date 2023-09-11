using UnityEngine;
using System.Collections.Generic;

public class GLOBAL : MonoBehaviour
{
	public static GLOBAL instance;

	public string playerName, playerClass;
	public float health, currentHealth, damage, speed, defense, dodge, healthRegen, lifeSteal, damModifierMax, damModifierMin;
	public int luck, level, xp, playerGold, healthPotion, openShop, shopCheapestItem, levelUpAvailable, levelUpHealing, autoPotion, floor;
	public int healthMultiplier, damageMultiplier, speedMultiplier, defenseMultiplier, dodgeMultiplier, healthRegenMultiplier, lifeStealMultiplier, luckMultiplier;
	public Dictionary<string, float> passive;

	public int frameRate, highFps, lowFps;

	// Runtime only, not saved to file
	public string creatureName;
	public float creatureHealth, creatureCurrentHealth, creatureDamage, creatureDefense, creatureDodge, creatureSpeed, creatureHealthRegen, creatureLifeSteal, creatureDamModifierMax, creatureDamModifierMin;
	public int creatureLevel, creatureXp, timeMultiplier;

	void Awake()
    {
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

    private void Start()
    {
		frameRate = Mathf.Max(PlayerPrefs.GetInt("fps"), lowFps);
		SetFps();
    }

    public void SetFps()
    {
		Application.targetFrameRate = frameRate;
		PlayerPrefs.SetInt("fps", frameRate);

		MenuLogic menuLogic = FindObjectOfType<MenuLogic>();
		menuLogic.SetFpsButtonText(frameRate.ToString());
	}

    public void SaveGame()
    {
		SaveSystem.SavePlayer(this);
    }

	public void LoadGame()
    {
		PlayerData data = SaveSystem.LoadPlayer();

		playerName = data.playerName;
		playerClass = data.playerClass;
		
		health = data.health;
		currentHealth = data.currentHealth;
		damage = data.damage;
		speed = data.speed;
		defense = data.defense;
		dodge = data.dodge;
		healthRegen = data.healthRegen;
		lifeSteal = data.lifeSteal;
		damModifierMax = data.damModifierMax;
		damModifierMin = data.damModifierMin;
		passive = data.passive;

		luck = data.luck;
		level = data.level;
		xp = data.xp;
		playerGold = data.playerGold;
		healthPotion = data.healthPotion;
		openShop = data.openShop;
		shopCheapestItem = data.shopCheapestItem;
		levelUpAvailable = data.levelUpAvailable;
		levelUpHealing = data.levelUpHealing;
		autoPotion = data.autoPotion;
		floor = data.floor;

		healthMultiplier = data.healthMultiplier;
		damageMultiplier = data.damageMultiplier;
		speedMultiplier = data.speedMultiplier;
		defenseMultiplier = data.defenseMultiplier;
		dodgeMultiplier = data.dodgeMultiplier;
		healthRegenMultiplier = data.healthRegenMultiplier;
		lifeStealMultiplier = data.lifeStealMultiplier;
		luckMultiplier = data.luckMultiplier;

		// Game is loaded here
		FindObjectOfType<MenuLogic>().StartGame();
	}
}
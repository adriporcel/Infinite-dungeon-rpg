using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
	public string playerName, playerClass;
	public float health, currentHealth, damage, speed, defense, dodge, healthRegen, lifeSteal, damModifierMax, damModifierMin;
	public int luck, level, xp, playerGold, healthPotion, openShop, shopCheapestItem, levelUpAvailable, levelUpHealing, autoPotion, floor;
	public int healthMultiplier, damageMultiplier, speedMultiplier, defenseMultiplier, dodgeMultiplier, healthRegenMultiplier, lifeStealMultiplier, luckMultiplier;
	public Dictionary<string, float> passive;

	public PlayerData(GLOBAL globalData)
	{
		playerName = globalData.playerName;
		playerClass = globalData.playerClass;

		health = globalData.health;
		currentHealth = globalData.currentHealth;
		damage = globalData.damage;
		speed = globalData.speed;
		defense = globalData.defense;
		dodge = globalData.dodge;
		healthRegen = globalData.healthRegen;
		lifeSteal = globalData.lifeSteal;
		damModifierMax = globalData.damModifierMax;
		damModifierMin = globalData.damModifierMin;
		passive = globalData.passive;

		luck = globalData.luck;
		level = globalData.level;
		xp = globalData.xp;
		playerGold = globalData.playerGold;
		healthPotion = globalData.healthPotion;
		openShop = globalData.openShop;
		shopCheapestItem = globalData.shopCheapestItem;
		levelUpAvailable = globalData.levelUpAvailable;
		levelUpHealing = globalData.levelUpHealing;
		autoPotion = globalData.autoPotion;
		floor = globalData.floor;

		healthMultiplier = globalData.healthMultiplier;
		damageMultiplier = globalData.damageMultiplier;
		speedMultiplier = globalData.speedMultiplier;
		defenseMultiplier = globalData.defenseMultiplier;
		dodgeMultiplier = globalData.dodgeMultiplier;
		healthRegenMultiplier = globalData.healthRegenMultiplier;
		lifeStealMultiplier = globalData.lifeStealMultiplier;
		luckMultiplier = globalData.luckMultiplier;
	}
}

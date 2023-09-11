using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClasses : MonoBehaviour
{
    string _playerClass;
    float _health, _damage, _defense, _dodge, _speed, _healthRegen, _lifeSteal;
    int _luck, _healthPotion, _startGold;
    Dictionary<string, float> _passive = new Dictionary<string, float>();

    GLOBAL _GLOBAL;

    private void Start()
    {
        _GLOBAL = FindObjectOfType<GLOBAL>();
    }

    public void SaveStats() //  Called from MenuLogc right before game starts, once per character. This is function "saves" in GLOBAL during Runtime. Permanent save occurs later, not triggered by this
    {
        // Name is set on MenuLogic > AcceptNewNameStartGame()
        _GLOBAL.playerClass = _playerClass;
        _GLOBAL.health = _health;
        _GLOBAL.currentHealth = 0;
        _GLOBAL.damage = _damage;
        _GLOBAL.defense = _defense;
        _GLOBAL.dodge = _dodge;
        _GLOBAL.speed = _speed;
        _GLOBAL.passive = _passive;

        _GLOBAL.healthRegen = _healthRegen;
        _GLOBAL.lifeSteal = _lifeSteal;
        _GLOBAL.luck = _luck;

        _GLOBAL.level = 1;
        _GLOBAL.xp = 0;
        _GLOBAL.damModifierMax = 0.1f;
        _GLOBAL.damModifierMin = 0.4f;
        // Consumibles
        _GLOBAL.healthPotion = _healthPotion;
        // Multipliers
        _GLOBAL.healthMultiplier = 0;
        _GLOBAL.damageMultiplier = 0;
        _GLOBAL.speedMultiplier = 0;
        _GLOBAL.defenseMultiplier = 0;
        _GLOBAL.dodgeMultiplier = 0;
        _GLOBAL.healthRegenMultiplier = 0;
        _GLOBAL.lifeStealMultiplier = 0;
        _GLOBAL.luckMultiplier = 0;

        // Necessary settings to start game, non player specific
        _GLOBAL.autoPotion = 0;
        _GLOBAL.levelUpHealing = 1;
        _GLOBAL.openShop = 0;
        _GLOBAL.levelUpAvailable = 0;
        _GLOBAL.shopCheapestItem = 100;
        _GLOBAL.floor = 1;
        _GLOBAL.playerGold = _startGold; // Specified inside each class
    }

    public void Rogue()
    {
        _playerClass = "Rogue";

        _health = 800f;
        _damage = 60f;
        _defense = 40f;
        _dodge = 50f;
        _speed = 1.15f;
        _healthRegen = 0f;
        _lifeSteal = 0f;

        _luck = 10;

        _startGold = 150;
        _healthPotion = 0;

        // Chance of double hit up to 50% (5% * level), but it will only deal 70% of damage
        _passive["cap"] = 50f; // Cap to stop the multiplier from increasing
        _passive["levelMultiplyer"] = 5f; // Multiplier, goes up until it hits the cap
        _passive["damageReduction"] = 0.7f; // Stat specific to the passive
    }
    


    public void Warrior()
    {
        _playerClass = "Warrior";

        _health = 1300f;
        _damage = 100f;
        _defense = 60f;
        _dodge = 10f;
        _speed = 0.8f;
        _healthRegen = 0f;
        _lifeSteal = 0f;

        _luck = 10;

        _startGold = 150;
        _healthPotion = 0;

        // Armor on top of health equal to up to 10% (1% * level) of health that regenerates every fight, while active defense is up 25% (5%x5)
        _passive["cap"] = 0.1f;
        _passive["levelMultiplyer"] = 0.01f;
    }

    public void TimeLord()
    {
        _playerClass = "TimeLord";

        _health = 900f;
        _damage = 75f;
        _defense = 50f;
        _dodge = 25f;
        _speed = 0.9f;
        _healthRegen = 0f;
        _lifeSteal = 0f;

        _luck = 20;

        _startGold = 50;
        _healthPotion = 1;

        // Slows enemy up to 20% (2% * level)
        _passive["cap"] = 0.2f;
        _passive["levelMultiplyer"] = 0.02f;
    }

    public void Berserker()
    {
        _playerClass = "Berserker";

        _health = 1100f;
        _damage = 90f;
        _defense = 50f;
        _dodge = 15f;
        _speed = 0.85f;
        _healthRegen = 0.5f;
        _lifeSteal = 0f;

        _luck = 10;

        _startGold = 50;
        _healthPotion = 0;

        // Speed increases relative to health lost, speed capped at -> capMaximumSpeed
        _passive["capMinimumHealth"] = 0.7f; // Capped at 7 0% increased speed over original
    }
}
/*
Rogue - Stat, speed DONE

Double hit chance per attack
Second hit damage capped at 70%
5% per level, cap 50%


Warrior - Stat, defense TO DO

Armor equal to max health
1% per level up to 10%
While Armor is active, defense is 25% higher


Time Lord - Stat, luck DONE

Enemy speed decreases
2% per level capped at 20% 


Berserker - Stat, Damage DONE

Speed increases when life decreases
Speed increase caps when life is at 20% <=




-----------New classes ideas
>Stone Man<
Description: Slow character, very tough, cannot dodge
Passive: Defense increases when he looses health (similart to Berskerker)

>Undead<
Description: Standard solder, nothing outsanding ragarding stats
Passive: 

>Hedgehog mutant<
Description: A mutated Hegehog that is qutie fast  but deals very little damage, his best attack is his defense
Passive: Thorns, reflects part of the damage received to the enemy
*/
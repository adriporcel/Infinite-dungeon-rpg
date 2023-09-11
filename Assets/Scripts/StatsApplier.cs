using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StatsApplier : MonoBehaviour
{
    // This script applies the stats taken from PlayerClasses.cs or Creatures.cs to the Player and Enemy
    // GameObects during the game

    public string playerName;
    
    public int level, xp, healthPotion, gold, luck;
    public int healthMultiplier, damageMultiplier, speedMultiplier, defenseMultiplier, dodgeMultiplier, healthRegenMultiplier, lifeStealMultiplier, luckMultiplier;
    public float damModifierMax, damModifierMin, healthRegen, lifeSteal;

    public float currentHealth;
    [HideInInspector] public int levelOneCap = 100;
    [HideInInspector] public float levelCapMod = 0.15f;
    public TextMeshProUGUI damageDisplay, speedDisplay, defenseDisplay, dodgeDisplay, healthRegenDisplay, lifeStealDisplay, luckDisplay, nameAndLevel, passiveDisplay;

    Vector3 _enemySpawner, _enemySpawnerDestination;
    public Slider healthBar, armourBar;
    public GameObject armourBarObject;

    float _healthUpdateTick; // TODO change to gameTick, from CONSTANTS file
    
    float _health, _damage, _speed, _defense, _dodge, _creatureCurrentHealth, _passive;

    public float healthMultiplied, damageMultiplied, speedMultiplied, defenseMultiplied, dodgeMultiplied, healthRegenMultiplied, lifeStealMultiplied, luckMultiplied, armour;
    string _playerClass;
    Creatures _creaturesList;
    GameManager _gameManager;

    Color _greenDisplayColour = new Color32(43, 194, 48, 255);
    Color _whiteDisplayColour = new Color32(255, 255, 255, 255);

    GLOBAL _GLOBAL;

    bool _warriorReduceDefensePassive = true;

    private void Start()
    {
        _GLOBAL = FindObjectOfType<GLOBAL>();
        if (gameObject.name == "Player(Clone)") // Player Start
        {
            GetPlayerPrefs();

            _playerClass = _GLOBAL.playerClass;

            GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + _playerClass);
            GameObject.Find("PassiveIcon").GetComponent<Image>().sprite = Resources
                                          .Load<Sprite>("Icons/Passives/" + _playerClass + "Passive");

            nameAndLevel.SetText(playerName + "  Lvl " + level);

            if (_playerClass == "Warrior")
            {
                defenseMultiplied = _defense * 1.25f;
                defenseDisplay.color = _greenDisplayColour;

                armourBarObject.SetActive(true);
                armour = healthMultiplied * Mathf.Min(_GLOBAL.passive["levelMultiplyer"] * level, _GLOBAL.passive["cap"]);

                armourBar.maxValue = armour;
                armourBar.value = armour;
            }
        }
        else if (gameObject.name == "Enemy(Clone)") // Enemy Start
        {
            _creaturesList = GetComponent<Creatures>();
            _creaturesList.CreaturePicker(); // This chooses a random creature and saves to PlayerPrefs

            GetCreaturePrefs();
            currentHealth = _health;
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Creatures/" + playerName);

            _enemySpawner = GameObject.Find("EnemySpawner").transform.position;
            _enemySpawnerDestination = GameObject.Find("EnemySpawnerDestination").transform.position;

            nameAndLevel.SetText(playerName);
        }

        _gameManager = FindObjectOfType<GameManager>();

        healthBar.maxValue = healthMultiplied;
        healthBar.value = currentHealth;
        
        UpdateStatsOnScreen();
    }

    private void Update()
    {
        if (_gameManager.gamePaused)
            return;

        if (gameObject.name != "Player(Clone)") // Enemy specific Update / Only used for Spawn Animation
        {
            // Smooth spawn animation for enemy
            if (transform.position.x == _enemySpawnerDestination.x && !_gameManager.gameReady)
            {
                _gameManager.gameReady = true;
            }

            else if (transform.position.x >= _enemySpawnerDestination.x - 1f)
            {
                transform.position = _enemySpawnerDestination;
            }
            else if (transform.position != _enemySpawnerDestination)
            {
                transform.position = Vector3.Lerp(transform.position, _enemySpawnerDestination, _GLOBAL.timeMultiplier * 5f * Time.deltaTime);
            }
        }

        if (healthRegenMultiplied != 0 && _gameManager.gameReady)
        {
            if (currentHealth > 0 && currentHealth < healthMultiplied)
            {
                currentHealth += healthMultiplied * (healthRegenMultiplied / 1000) * Time.deltaTime;
            }
        }

        // Update Health bar UI
        _healthUpdateTick -= Time.deltaTime;

        if (_healthUpdateTick <= 0f)
        {
            healthBar.value = currentHealth;
            _healthUpdateTick = 0.2f;

            if (gameObject.name == "Player(Clone)")
            {
                if (_playerClass == "Berserker") // Berserker passive speed increase on health lost is calculated here
                {
                    float speedMultiplied = _speed + (_speed * (speedMultiplier / 100f));
                    this.speedMultiplied = speedMultiplied + (speedMultiplied * Mathf.Min(1 - currentHealth / healthMultiplied, 1 + _GLOBAL.passive["capMinimumHealth"] * speedMultiplied));
                    _passive = (Mathf.Min(1 - currentHealth / healthMultiplied, 1 + _GLOBAL.passive["capMinimumHealth"] * speedMultiplied)) * 100;
                    
                    speedDisplay.SetText(this.speedMultiplied.ToString("f2"));
                    passiveDisplay.SetText(_passive.ToString("f0") + "%");
                    
                    if (currentHealth != healthMultiplied)
                    {
                        speedDisplay.color = _greenDisplayColour;
                    }
                    else
                    {
                        speedDisplay.color = _whiteDisplayColour;
                    }
                }
                else if (_playerClass == "Warrior")
                {
                    armourBar.value = armour;

                    // Defense boost is lost after passive armour is depleted
                    if (armourBar.value <= 0 && _warriorReduceDefensePassive)
                    {
                        defenseDisplay.color = _whiteDisplayColour;
                        GetPlayerPrefs();
                        UpdateStatsOnScreen();
                        _warriorReduceDefensePassive = false;
                    }
                }
            }
        }
    }

    void UpdateStatsOnScreen()
    {
        damageDisplay.SetText(damageMultiplied.ToString("f0"));

        if (gameObject.name == "Enemy(Clone)" && _GLOBAL.playerClass == "TimeLord")
        {
            float speedReduction = Mathf.Min(_GLOBAL.passive["levelMultiplyer"] * _GLOBAL.level, _GLOBAL.passive["cap"]);
            speedDisplay.SetText((speedMultiplied - speedReduction).ToString("f2"));
            speedDisplay.color = new Color32(184, 11, 11, 255); // Red colour
        }
        else
        {
            speedDisplay.SetText(speedMultiplied.ToString("f2"));
        }

        defenseDisplay.SetText(defenseMultiplied.ToString("f0"));
        dodgeDisplay.SetText(dodgeMultiplied.ToString("f0"));
        healthRegenDisplay.SetText(healthRegenMultiplied.ToString("f0"));
        lifeStealDisplay.SetText(lifeStealMultiplied.ToString("f0"));

        if (gameObject.name == "Player(Clone)")
        {
            int levelWithCapAtTen = Mathf.Min(level, 10);

            luckDisplay.SetText(luckMultiplied.ToString("f0"));
            switch (_playerClass)
            {
                case "Rogue":
                    _passive = _GLOBAL.passive["levelMultiplyer"] * levelWithCapAtTen;
                    break;
                case "Warrior":
                    _passive = (_GLOBAL.passive["levelMultiplyer"] * levelWithCapAtTen) * 100;
                    break;
                case "TimeLord":
                    _passive = (_GLOBAL.passive["levelMultiplyer"] * levelWithCapAtTen) * 100;
                    break;
                case "Berserker":
                    _passive = 0;
                    break;
            }
            passiveDisplay.SetText(_passive.ToString() + "%");
        }
    }

    void GetCreaturePrefs()
    {
            playerName = _GLOBAL.creatureName;
            _health = _GLOBAL.creatureHealth;
            _damage = _GLOBAL.creatureDamage;
            _defense = _GLOBAL.creatureDefense;
            _dodge = _GLOBAL.creatureDodge;
            _speed = _GLOBAL.creatureSpeed;
            healthRegen = _GLOBAL.creatureHealthRegen;
            lifeSteal = _GLOBAL.creatureLifeSteal;

            level = _GLOBAL.creatureLevel;
            xp = _GLOBAL.creatureXp;
            damModifierMax = _GLOBAL.creatureDamModifierMax;
            damModifierMin = _GLOBAL.creatureDamModifierMin;

            // Warning....crappy solution ahead
            healthMultiplied = _health;
            damageMultiplied = _damage;
            speedMultiplied = _speed;
            defenseMultiplied = _defense;
            dodgeMultiplied = _dodge;
            healthRegenMultiplied = healthRegen;
            lifeStealMultiplied = lifeSteal;
    }

    void GetPlayerPrefs()
    {
        playerName = _GLOBAL.playerName;
        _health = _GLOBAL.health;
        currentHealth = _GLOBAL.currentHealth;
        _damage = _GLOBAL.damage;
        _speed = _GLOBAL.speed;
        _defense = _GLOBAL.defense;
        _dodge = _GLOBAL.dodge;
        healthRegen = _GLOBAL.healthRegen;
        lifeSteal = _GLOBAL.lifeSteal;
        luck = _GLOBAL.luck;

        healthMultiplier = _GLOBAL.healthMultiplier;
        damageMultiplier = _GLOBAL.damageMultiplier;
        speedMultiplier = _GLOBAL.speedMultiplier;
        defenseMultiplier = _GLOBAL.defenseMultiplier;
        dodgeMultiplier = _GLOBAL.dodgeMultiplier;
        healthRegenMultiplier = _GLOBAL.healthRegenMultiplier;
        lifeStealMultiplier = _GLOBAL.lifeStealMultiplier;
        luckMultiplier = _GLOBAL.luckMultiplier;
        healthPotion = _GLOBAL.healthPotion;


        level = _GLOBAL.level;
        xp = _GLOBAL.xp;
        gold = _GLOBAL.playerGold;
        damModifierMax = _GLOBAL.damModifierMax;
        damModifierMin = _GLOBAL.damModifierMin;

        // Applies multipliers, this is accessed by GameManager to carry out fights
        healthMultiplied = (_health + (_health * (healthMultiplier / 100f)));
        damageMultiplied = _damage + (_damage * (damageMultiplier / 100f));
        speedMultiplied = _speed + (_speed * (speedMultiplier / 100f));
        defenseMultiplied = _defense + (_defense * (defenseMultiplier / 100f));
        dodgeMultiplied = _dodge + (_dodge * (dodgeMultiplier / 100f));
        healthRegenMultiplied = healthRegen + (healthRegen * (healthRegenMultiplier / 100f));
        lifeStealMultiplied = lifeSteal + (lifeSteal * (lifeStealMultiplier / 100f));
        luckMultiplied = luck + (luck * (luckMultiplier / 100f));

        if (_GLOBAL.levelUpHealing == 1) // Only used at game start
        {
            _GLOBAL.levelUpHealing = 0;
            currentHealth = healthMultiplied;
        }
    }

    public void SetPlayerPrefs()
    {
        _GLOBAL.playerName = playerName;
        _GLOBAL.health = _health;
        _GLOBAL.currentHealth = currentHealth;
        _GLOBAL.damage = _damage;
        _GLOBAL.speed = _speed;
        _GLOBAL.defense = _defense;
        _GLOBAL.dodge = _dodge;
        _GLOBAL.healthRegen = healthRegen;
        _GLOBAL.lifeSteal = lifeSteal;
        _GLOBAL.luck = luck;

        _GLOBAL.level = level;
        _GLOBAL.xp = xp;
        _GLOBAL.playerGold = gold;
        _GLOBAL.damModifierMax = damModifierMax;
        _GLOBAL.damModifierMin = damModifierMin;
    }

    public void AddEarnedXp(int xpEarned)
    {
        xp += xpEarned;

        if (xp >= levelOneCap * level * level * levelCapMod)
        {
            level++;
            // currentHealth = healthMultiplied;
            //currentHealth = 0; // This alternative makes it work even after increasing health stat on levelUpPanel
            _GLOBAL.levelUpAvailable = 1;
        }
    }

    public void AddEarnedGold(int goldEarned)
    {
        _GLOBAL.playerGold = goldEarned;
    }
}
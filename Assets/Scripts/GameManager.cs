using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject combatText, playerPrefab, enemyPrefab, levelUpSkillWindow;
    public GameObject shopCanavs, shop, gameOverUI;
    public Transform playerSpawnPoint, enemySpawnPoint, canvas;
    public Toggle autoPotionToggle;

    public bool gameReady, gamePaused;
    public float globalDodgeModifier = 75f; // 100 = cap at 49.999... / Result is directly proportional to the modifier
    public TextMeshProUGUI floorLevel, healthPotionIndicator;
    public int floor;

    int _healthPotionValue = 250; // Health points to heal
    
    GameObject _playerObject, _enemyObject;

    Transform _combatTextPlayerSpawner, _combatTextEnemySpawner, _healTextPlayerSpawner, _healTextEnemySpawner;

    StatsApplier _player, _enemy;

    GameObject _text;

    float _playerAttackRate, _enemyAttackRate, _damageToBeDealt, _potionDelay;

    bool _doubleStriking;
    GLOBAL _GLOBAL;

    // Rogue specific
    float _rogueStrikeMultiplier = 1;
    public int doubleStrikeChance;

    void Start()
    {

        _GLOBAL = FindObjectOfType<GLOBAL>();

        _playerObject = Instantiate(playerPrefab, canvas);
        _playerObject.transform.position = playerSpawnPoint.position;


        _enemyObject = Instantiate(enemyPrefab, canvas);
        _enemyObject.transform.position = enemySpawnPoint.position;

        _player = _playerObject.GetComponent<StatsApplier>(); // Script name: StatsApplier.cs --> checks for GameObject name to be Player(Clone)
        _enemy = _enemyObject.GetComponent<StatsApplier>(); // Script name: StatsApplier.cs
        _combatTextPlayerSpawner = GameObject.Find("CombatTextPlayerSpawner").transform;
        _combatTextEnemySpawner = GameObject.Find("CombatTextEnemySpawner").transform;
        _healTextPlayerSpawner = GameObject.Find("HealTextPlayerSpawner").transform;
        _healTextEnemySpawner = GameObject.Find("HealTextEnemySpawner").transform;

        floor = _GLOBAL.floor;
        floorLevel.SetText("Floor  " + floor.ToString());

        healthPotionIndicator.SetText(_GLOBAL.healthPotion.ToString());
        if (_GLOBAL.autoPotion == 1)
            autoPotionToggle.isOn = true;

        _GLOBAL.SaveGame();
        PlayerPrefs.SetInt("ResumeGameAvailable", 1);

        // Player passive setups

        if (_GLOBAL.playerClass == "Rogue")
        {
            doubleStrikeChance = (int)Mathf.Min(_GLOBAL.passive["levelMultiplyer"] * _GLOBAL.level, _GLOBAL.passive["cap"]);
        }
    }


    private void Update()
    {
        if (!gameReady || gamePaused) // Makes GameManager wait for the enemy frame animation to slide into place before starting attack
            return;

        _playerAttackRate -= Time.deltaTime * _GLOBAL.timeMultiplier;
        _enemyAttackRate -= Time.deltaTime * _GLOBAL.timeMultiplier;
        _potionDelay -= Time.deltaTime * _GLOBAL.timeMultiplier;

        // Death Check
        if (_player.currentHealth <= 0)
            PlayerDeath();
        else if (_enemy.currentHealth <= 0)
            EnemyDeath();

        if (autoPotionToggle.isOn)
        {
            if (_player.currentHealth <= _player.healthMultiplied * 0.1) // Auto heal at 10% health or less
                PotionButton();
        }

        if (_playerAttackRate <= 0f) // Player attack turn
        {
            _playerAttackRate = (1f / _player.speedMultiplied); // Reset clock for next attack to happen

            if (Random.Range(1, 100) > Dodge(_enemy)) // Calculates if attack will be doged or will strike
            {
                _damageToBeDealt = DamageCalculation(_player, _enemy);
                _enemy.currentHealth -= _damageToBeDealt;
                DisplayCombatText(_damageToBeDealt.ToString(), "player", "-");

                // Life Steal
                if (_player.lifeSteal != 0)
                {
                    float stolenLife = Mathf.RoundToInt(_damageToBeDealt * (_player.lifeSteal / 100));
                    _player.currentHealth = Mathf.Min(_player.currentHealth + stolenLife, _player.healthMultiplied);
                    DisplayCombatText(stolenLife.ToString(), "player", "+");
                }

                _rogueStrikeMultiplier = 1; // Restarts the damage modifier for the secondary Rogue strike

                if (_GLOBAL.playerClass == "Rogue" && !_doubleStriking)
                {
                    if (Random.Range(1, 100) <= doubleStrikeChance) // Calculates the chance of double strike as per the Rogue Passive rules
                    {
                        _playerAttackRate = 0.3f;
                        _rogueStrikeMultiplier = _GLOBAL.passive["damageReduction"];
                        _doubleStriking = true;
                    }
                }
                else if (_GLOBAL.playerClass == "Rogue" && _doubleStriking)
                    _doubleStriking = false;
            }
            else DisplayCombatText("Miss", "player", "dodge"); // Life bar is updated, text displayed on opposite side
        }

        if (_enemyAttackRate <= 0f) // Enemy attack turn
        {
            if (_GLOBAL.playerClass == "TimeLord")
            {
                float speedReduction = Mathf.Min(_GLOBAL.passive["levelMultiplyer"] * _player.level, _GLOBAL.passive["cap"]);
                _enemyAttackRate = 1 / (_enemy.speedMultiplied - speedReduction);
            }
            else _enemyAttackRate = 1 / _enemy.speedMultiplied;
            
            if (Random.Range(1, 100) > Dodge(_player))
            {
                _damageToBeDealt = DamageCalculation(_enemy, _player);

                if (_GLOBAL.playerClass == "Warrior" && _player.armour > 0)
                {
                    if (_damageToBeDealt <= _player.armour)
                        _player.armour -= _damageToBeDealt;
                    else
                    {
                        int remainingDamage = Mathf.RoundToInt(_damageToBeDealt - _player.armour);
                        _player.armour = 0;

                        _player.currentHealth -= remainingDamage;
                    }
                }
                else _player.currentHealth -= _damageToBeDealt;
                _GLOBAL.currentHealth = _player.currentHealth;

                DisplayCombatText(_damageToBeDealt.ToString(), "enemy", "-");

                // Life Steal
                if (_enemy.lifeSteal != 0)
                {
                    float stolenLife = Mathf.RoundToInt(_damageToBeDealt * (_enemy.lifeSteal / 100));
                    _enemy.currentHealth = Mathf.Min(_enemy.currentHealth + stolenLife, _enemy.healthMultiplied);
                    DisplayCombatText(stolenLife.ToString(), "enemy", "+");
                }
            }
            else DisplayCombatText("Miss", "enemy", "dodge");
        }

        // Moved up to beginning of Update because it makes more sense, might cause some issue maybe that's why it was down here previously
        // Death check
        // Potion check
    }

    float Dodge(StatsApplier who)
    {
        return Mathf.RoundToInt((1f - (1f / (0.15f * who.dodgeMultiplied + 1f)) - 0.5f) * globalDodgeModifier);
    }
    
    float DamageCalculation(StatsApplier who, StatsApplier whoAgainst)
    {
        float rangeMin = who.damageMultiplied - (who.damageMultiplied * who.damModifierMin);
        float rangeMax = who.damageMultiplied + (who.damageMultiplied * who.damModifierMax);
        _damageToBeDealt = Random.Range(rangeMin, rangeMax);
        _damageToBeDealt = _damageToBeDealt / ((100f + whoAgainst.defenseMultiplied) / 100f);
        
        return Mathf.RoundToInt(_rogueStrikeMultiplier * _damageToBeDealt); // rogueStrikeMultiplier only applies to Rogues
    }

    void DisplayCombatText(string number, string who, string type)
    {
        // Spawn Text
        _text = Instantiate(combatText, GameObject.Find("Canvas").transform);

        if (who == "player")
        {
            if (type == "+")
                _text.transform.position = _healTextPlayerSpawner.position; // Text is displayed on the same side
            else
                _text.transform.position = _combatTextEnemySpawner.position; // Text is displayed on opposite side

            _text.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        }
        if (who == "enemy")
        {
            if (type == "+")
                _text.transform.position = _healTextEnemySpawner.position;
            else
                _text.transform.position = _combatTextPlayerSpawner.position;
            _text.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
        }

        // Assign Text
        if (number == "Miss")
            _text.GetComponent<TextMeshProUGUI>().SetText(number);
        else
            _text.GetComponent<TextMeshProUGUI>().SetText(type + number);

        // Assign Colour
        if (type == "+")
            _text.GetComponent<TextMeshProUGUI>().color = new Color32(43, 194, 48, 255); // Green (default is red)
        else if (type == "dodge")
            _text.GetComponent<TextMeshProUGUI>().color = new Color32(201, 84, 16, 255); // Orange
    }

    public void PotionButton()
    {
        if (_potionDelay <= 0f)
        {
            if (_player.healthPotion > 0 && _player.currentHealth > 0)
            {
                _potionDelay = 5f;
                _player.healthPotion -= 1;
                _player.currentHealth = Mathf.Clamp(_player.currentHealth + _healthPotionValue, 0, _player.healthMultiplied); // Player cannot heal more than max health
                healthPotionIndicator.SetText(_player.healthPotion.ToString());
                _GLOBAL.healthPotion = _player.healthPotion;
            }
        }

    }

    public void AutoPotionToggle()
    {
        if (!autoPotionToggle.isOn)
            _GLOBAL.autoPotion = 0;
        else if (autoPotionToggle.isOn)
            _GLOBAL.autoPotion = 1;
    }

    int EarnGold()
    {
        int rng = Random.Range(1, 100);
        int playerCappedLuck = Mathf.RoundToInt((1f - (1f / (0.15f * _player.luck + 1f)) - 0.5f) * 100f);
        if (playerCappedLuck <= rng)
        {
            return Mathf.RoundToInt(floor * (_player.level / 2));
        }
        return 0;
    }

    public void EnemyDeath()
    {
        floor++;
        _GLOBAL.floor = floor;
        _player.gold += EarnGold();

        _player.AddEarnedXp(_enemy.xp); // Checks for level up and sets ---> levelUpAvailable = 1  for new scene to load below
        _player.SetPlayerPrefs();

        int cheapestShopItem = _GLOBAL.shopCheapestItem;

        if (floor <= 25)
        {
            if (_player.gold >= cheapestShopItem && floor % 5 == 0)
                _GLOBAL.openShop = 1;
        }
        else if (floor > 25)
        {
            if (_player.gold >= cheapestShopItem && floor % 10 == 0)
                _GLOBAL.openShop = 1;
        }

        if (_GLOBAL.levelUpAvailable == 1 || _GLOBAL.openShop == 1)
            SceneManager.LoadScene("GameBreak");
        else SceneManager.LoadScene("Fight");
    }

    public void PlayerDeath()
    {
        //_GLOBAL.playerIsDead = 1;
        gamePaused = true;
        PlayerPrefs.SetInt("ResumeGameAvailable", 0);
        gameOverUI.transform.SetAsLastSibling();
        gameOverUI.SetActive(true);
    }

    public void SaveButton()
    {
        FindObjectOfType<GLOBAL>().SaveGame();
    }
}

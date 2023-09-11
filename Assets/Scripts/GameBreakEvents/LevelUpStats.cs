using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpStats : MonoBehaviour
{
    public TextMeshProUGUI playerNameText, playerLevelText, healthDisplay, damageDisplay, speedDisplay, defenseDisplay, dodgeDisplay, healthRegenDisplay, lifeStealDisplay, luckDisplay, remainingPoints;

    [HideInInspector] public string playerClass;
    [HideInInspector] public int pointsAvailable;

    string _playerName;
    int _level, _luck;

    float _health, _damage, _speed, _defense, _dodge, _healthRegen, _lifeSteal;
    float _time;
    float _timeReset = 0.1f;

    GLOBAL GLOBAL;

    private void Start()
    {
        GLOBAL = FindObjectOfType<GLOBAL>();

        _playerName = GLOBAL.playerName;
        _level = GLOBAL.level;

        playerClass = GLOBAL.playerClass;

        _health = GLOBAL.health;
        _damage = GLOBAL.damage;
        _speed = GLOBAL.speed;
        _defense = GLOBAL.defense;
        _dodge = GLOBAL.dodge;
        _healthRegen = GLOBAL.healthRegen;

        _lifeSteal = GLOBAL.lifeSteal;
        _luck = GLOBAL.luck;
        // Only need to be set once
        playerNameText.SetText(_playerName);
        playerLevelText.SetText(_level.ToString());

        pointsAvailable = 5;
    }

    private void Update()
    {
        _time -= Time.deltaTime;

        if (_time <= 0)
        {
            healthDisplay.SetText(_health.ToString("f0"));
            damageDisplay.SetText(_damage.ToString("f0"));
            defenseDisplay.SetText(_defense.ToString("f0"));
            speedDisplay.SetText(_speed.ToString("f2"));
            dodgeDisplay.SetText(_dodge.ToString("f0"));
            healthRegenDisplay.SetText(_healthRegen.ToString("f0"));
            lifeStealDisplay.SetText(_lifeSteal.ToString("f0"));
            luckDisplay.SetText(_luck.ToString());

            remainingPoints.SetText(pointsAvailable.ToString());

            _time = _timeReset;
        }
    }

    public void ContinueButton()
    {
        if (pointsAvailable <= 0)
        {
            SaveStats();
            if (GLOBAL.openShop == 1)
            {
                FindObjectOfType<GameBreakManager>().InstantiatePanel("shop");
                gameObject.SetActive(false);
            }
            else FindObjectOfType<GameBreakManager>().ResumeGame();
        }
    }

    public void ReassignButton()
    {
        FindObjectOfType<GameBreakManager>().ReloadGameBreak();
    }

    void SaveStats()
    {
        GLOBAL.levelUpAvailable = 0; // Makes it so that GameBreakEvent does not show level up panel until next level up

        GLOBAL.health = _health;
        GLOBAL.damage = _damage;
        GLOBAL.speed = _speed;
        GLOBAL.defense = _defense;
        GLOBAL.dodge = _dodge;
        GLOBAL.healthRegen = _healthRegen;
        GLOBAL.lifeSteal = _lifeSteal;
        GLOBAL.luck = _luck;
    }

    public void AddStatHealth()
    {
        if (pointsAvailable > 0)
        {
            _health += 50;
            pointsAvailable--;
        }
    }

    public void AddStatDamage()
    {
        if (pointsAvailable > 0)
        {
            _damage += 11;
            pointsAvailable--;
        }
    }

    public void AddStatDefense()
    {
        if (pointsAvailable > 0)
        {
            _defense += 8;
            pointsAvailable--;
        }
    }

    public void AddStatDodge()
    {
        if (pointsAvailable > 0)
        {
            _dodge += 6;
            pointsAvailable--;
        }
    }

    public void AddStatSpeed()
    {
        if (pointsAvailable > 0)
        {
            if (_speed < 2f)
                _speed += 0.05f;
            else if (_speed >= 2f)
                _speed += 0.02f;
            else if (_speed >= 3f)
                _speed += 0.01f;

            pointsAvailable--;
        }
    }

    public void AddStatHealthRegen()
    {
        if (pointsAvailable > 0)
        {
            _healthRegen += 2f;
            pointsAvailable--;
        }
    }

    public void AddStatLifeSteal()
    {
        if (pointsAvailable > 0)
        {
            _lifeSteal += 3f;
            pointsAvailable--;
        }
    }

    public void AddStatLuck()
    {
        if (pointsAvailable > 0)
        {
            _luck += 15;
            pointsAvailable--;
        }
    }
}


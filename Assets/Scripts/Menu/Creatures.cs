using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creatures : MonoBehaviour
{
    public GameObject confirmation;

    string _creature;
    float _health, _damage, _defense, _dodge, _speed, _damModifierMax, _damModifierMin, _healthRegen, _lifeSteal;

    int _xp, _rng, _floor, _playerLevel;

    public void CreaturePicker()
    {

        _floor = FindObjectOfType<GameManager>().floor;
        _playerLevel = FindObjectOfType<GLOBAL>().level;

        _rng = Random.Range(0, 4);
        switch (_rng)
        {
            case 0:
                Skeleton();
                break;
            case 1:
                Orc();
                break;
            case 2:
                Rat();
                break;
            case 3:
                Slime();
                break;
        }

        StatsMultiplyer();
        GLOBAL GLOBAL = FindObjectOfType<GLOBAL>();
        GLOBAL.creatureName = _creature;
        GLOBAL.creatureHealth = _health;
        GLOBAL.creatureDamage = _damage;
        GLOBAL.creatureDefense = _defense;
        GLOBAL.creatureDodge = _dodge;
        GLOBAL.creatureSpeed = _speed;

        GLOBAL.creatureHealthRegen = _healthRegen;
        GLOBAL.creatureLifeSteal = _lifeSteal;

        GLOBAL.creatureDamModifierMax = _damModifierMax;
        GLOBAL.creatureDamModifierMin = _damModifierMin;
        GLOBAL.creatureLevel = _floor;
        GLOBAL.creatureXp = _xp;
    }

    void StatsMultiplyer()
    {
        int floorMultiplier = Mathf.RoundToInt(_floor * 0.5f);

        if (_floor <= 50)
            floorMultiplier = Mathf.RoundToInt(_floor * 0.75f);
        else if (_floor > 50 && _floor <= 100)
            floorMultiplier = _floor;
        else if (_floor > 100 && _floor <= 200)
            floorMultiplier = Mathf.RoundToInt(_floor * 1.25f);
        else if (_floor > 200)
            floorMultiplier = Mathf.RoundToInt(_floor * 1.5f);

        _health += 30 * (_playerLevel + floorMultiplier);
        _damage += 3 * (_playerLevel + floorMultiplier);
        _defense += 2 * (_playerLevel + floorMultiplier);
        _dodge += 2 * (_playerLevel + floorMultiplier);
        _speed += 0.01f * (_playerLevel + floorMultiplier);

        if (_healthRegen != 0)
            _healthRegen += Mathf.RoundToInt(_playerLevel);
        if (_lifeSteal != 0)
            _lifeSteal += Mathf.RoundToInt(_playerLevel);

        _damModifierMax += 0.01f * (_playerLevel + floorMultiplier);
        _damModifierMin += 0.005f * (_playerLevel + floorMultiplier);
        _xp += (_playerLevel + (_floor / 2));
    }

    public void Skeleton()
    {
        _creature = "Skeleton";

        _health = 300f;
        _damage = 35f;
        _defense = 10f;
        _dodge = 30f;
        _speed = 1f;
        _healthRegen = 0f;
        _lifeSteal = 1f;

        _damModifierMax = 0.4f;
        _damModifierMin = 0.05f;
        _xp = 25;
    }

    public void Orc()
    {
        _creature = "Orc";

        _health = 500f;
        _damage = 55f;
        _defense = 30f;
        _dodge = 10f;
        _speed = 0.55f;
        _healthRegen = 0.1f;
        _lifeSteal = 0f;

        _damModifierMax = 0.4f;
        _damModifierMin = 0.1f;
        _xp = 40;
    }

    public void Rat()
    {
        _creature = "Rat";

        _health = 120f;
        _damage = 20f;
        _defense = 0f;
        _dodge = 70f;
        _speed = 1.1f;
        _healthRegen = 0.5f;
        _lifeSteal = 4f;

        _damModifierMax = 0.3f;
        _damModifierMin = 0.03f;
        _xp = 15;
    }

    public void Slime()
    {
        _creature = "Slime";

        _health = 190f;
        _damage = 25f;
        _defense = 5f;
        _dodge = 30f;
        _speed = 0.8f;
        _healthRegen = 1f;
        _lifeSteal = 0f;

        _damModifierMax = 0.25f;
        _damModifierMin = 0.02f;
        _xp = 10;
    }
}

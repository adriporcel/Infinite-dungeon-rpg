using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI healthPriceText, damagePriceText, speedPriceText, defensePriceText, dodgePriceText, healthRegenPriceText, lifeStealPriceText, luckPriceText, potionsPriceText;
    public TextMeshProUGUI heathMultiplierText, damageMultiplierText, speedMultiplierText, defenseMultiplierText, dodgeMultiplierText, healthRegenMultiplierText, lifeStealMultiplierText, luckMultiplierText, potionsText;

    int _gold;
    int _healthPrice, _damagePrice, _speedPrice, _defensePrice, _dodgePrice, _healthRegenPrice, _lifeStealPrice, _luckPrice, _potionsPrice;
    int _healthMultiplier, _damageMultiplier, _speedMultiplier, _defenseMultiplier, _dodgeMultiplier, _healthRegenMultiplier, _lifeStealMultiplier, _luckMultiplier, healthPotion;

    GLOBAL _GLOBAL;


    private void Start()
    {
        _GLOBAL = FindObjectOfType<GLOBAL>();

        // Load all variables
        _gold = _GLOBAL.playerGold;
        // Player Multipliers
        _healthMultiplier = _GLOBAL.healthMultiplier;
        _damageMultiplier = _GLOBAL.damageMultiplier;
        _speedMultiplier = _GLOBAL.speedMultiplier;
        _defenseMultiplier = _GLOBAL.defenseMultiplier;
        _dodgeMultiplier = _GLOBAL.dodgeMultiplier;
        _healthRegenMultiplier = _GLOBAL.healthRegenMultiplier;
        _lifeStealMultiplier = _GLOBAL.lifeStealMultiplier;
        _luckMultiplier = _GLOBAL.luckMultiplier;
        healthPotion = _GLOBAL.healthPotion;

        SetPrices();

        // Screen text set
        goldText.SetText("Gold: " + _gold.ToString());
        // Stats
        heathMultiplierText.SetText(_healthMultiplier.ToString() + "%");
        damageMultiplierText.SetText(_damageMultiplier.ToString() + "%");
        speedMultiplierText.SetText(_speedMultiplier.ToString() + "%");
        defenseMultiplierText.SetText(_defenseMultiplier.ToString() + "%");
        dodgeMultiplierText.SetText(_dodgeMultiplier.ToString() + "%");
        healthRegenMultiplierText.SetText(_healthRegenMultiplier.ToString() + "%");
        lifeStealMultiplierText.SetText(_lifeStealMultiplier.ToString() + "%");
        luckMultiplierText.SetText(_luckMultiplier.ToString() + "%");
        potionsText.SetText(healthPotion.ToString());
        // Prices
        healthPriceText.SetText(_healthPrice.ToString());
        damagePriceText.SetText(_damagePrice.ToString());
        speedPriceText.SetText(_speedPrice.ToString());
        defensePriceText.SetText(_defensePrice.ToString());
        dodgePriceText.SetText(_dodgePrice.ToString());
        healthRegenPriceText.SetText(_healthRegenPrice.ToString());
        lifeStealPriceText.SetText(_lifeStealPrice.ToString());
        luckPriceText.SetText(_luckPrice.ToString());
        potionsPriceText.SetText(_potionsPrice.ToString());
    }

    private void SetPrices()
    {
        _healthPrice = (100 * ((_healthMultiplier / 10) * 2)) + 100;
        _damagePrice = Mathf.Max((100 * ((_damageMultiplier / 10) * 2)), 100);
        _speedPrice = Mathf.Max((100 * ((_speedMultiplier / 10) * 2)), 100);
        _defensePrice = Mathf.Max((100 * ((_defenseMultiplier / 10) * 2)), 100);
        _dodgePrice = Mathf.Max((100 * ((_dodgeMultiplier / 10) * 2)), 100);
        _healthRegenPrice = Mathf.Max((100 * ((_healthRegenMultiplier / 10) * 2)), 100);
        _lifeStealPrice = Mathf.Max((100 * ((_lifeStealMultiplier / 10) * 2)), 100);
        _luckPrice = Mathf.Max((100 * ((_luckMultiplier / 10) * 2)), 100);
        _potionsPrice = Mathf.Max((100 * healthPotion), 100);

        _GLOBAL.shopCheapestItem = Mathf.Min(_healthPrice, _damagePrice, _speedPrice, _defensePrice, _dodgePrice, _healthRegenPrice, _lifeStealPrice, _luckPrice, _potionsPrice);
    }

    public void ContinueButton()
    {
        SaveAll();
        FindObjectOfType<GameBreakManager>().ResumeGame();
    }

    public void ReloadButton()
    {
        FindObjectOfType<GameBreakManager>().ReloadGameBreak();
    }

    void SaveAll()
    {
        _GLOBAL.openShop = 0;

        _GLOBAL.playerGold = _gold;

        _GLOBAL.healthMultiplier = _healthMultiplier;
        _GLOBAL.damageMultiplier = _damageMultiplier;
        _GLOBAL.speedMultiplier = _speedMultiplier;
        _GLOBAL.defenseMultiplier = _defenseMultiplier;
        _GLOBAL.dodgeMultiplier = _dodgeMultiplier;
        _GLOBAL.healthRegenMultiplier = _healthRegenMultiplier;
        _GLOBAL.lifeStealMultiplier = _lifeStealMultiplier;
        _GLOBAL.luckMultiplier = _luckMultiplier;
        _GLOBAL.healthPotion = healthPotion;
    }

    // Shop items Button Logic
    public void BuyHealthButton()
    {
        if (_gold >= _healthPrice)
        {
            _gold -= _healthPrice;
            _healthMultiplier += 10;

            SetPrices();

            heathMultiplierText.SetText(_healthMultiplier.ToString() + "%");
            healthPriceText.SetText(_healthPrice.ToString());
            goldText.SetText("Gold: " + _gold.ToString());
        }
    }

    public void BuyDamageButton()
    {
        if (_gold >= _damagePrice)
        {
            _gold -= _damagePrice;
            _damageMultiplier += 10;

            SetPrices();

            damageMultiplierText.SetText(_damageMultiplier.ToString() + "%");
            damagePriceText.SetText(_damagePrice.ToString());
            goldText.SetText("Gold: " + _gold.ToString());
        }
    }
   
    public void BuySpeedButton()
    {
        if (_gold >= _speedPrice)
        {
            _gold -= _speedPrice;
            _speedMultiplier += 10;

            SetPrices();

            speedMultiplierText.SetText(_speedMultiplier.ToString() + "%");
            speedPriceText.SetText(_speedPrice.ToString());
            goldText.SetText("Gold: " + _gold.ToString());
        }
    }
    
    public void BuyDefenseButton()
    {
        if (_gold >= _defensePrice)
        {
            _gold -= _defensePrice;
            _defenseMultiplier += 10;

            SetPrices();

            defenseMultiplierText.SetText(_defenseMultiplier.ToString() + "%");
            defensePriceText.SetText(_defensePrice.ToString());
            goldText.SetText("Gold: " + _gold.ToString());
        }
    }
    
    public void BuyDodgeButton()
    {
        if (_gold >= _dodgePrice)
        {
            _gold -= _dodgePrice;
            _dodgeMultiplier += 10;

            SetPrices();

            dodgeMultiplierText.SetText(_dodgeMultiplier.ToString() + "%");
            dodgePriceText.SetText(_dodgePrice.ToString());
            goldText.SetText("Gold: " + _gold.ToString());
        }
    }
    
    public void BuyHealthRegenButton()
    {
        if (_gold >= _healthRegenPrice)
        {
            _gold -= _healthRegenPrice;
            _healthRegenMultiplier += 10;

            SetPrices();

            healthRegenMultiplierText.SetText(_healthRegenMultiplier.ToString() + "%");
            healthRegenPriceText.SetText(_healthRegenPrice.ToString());
            goldText.SetText("Gold: " + _gold.ToString());
        }
    }
    
    public void BuyLifeStealButton()
    {
        if (_gold >= _lifeStealPrice)
        {
            _gold -= _lifeStealPrice;
            _lifeStealMultiplier += 10;

            SetPrices();

            lifeStealMultiplierText.SetText(_lifeStealMultiplier.ToString() + "%");
            lifeStealPriceText.SetText(_lifeStealPrice.ToString());
            goldText.SetText("Gold: " + _gold.ToString());
        }
    }
    
    public void BuyLuckButton()
    {
        if (_gold >= _luckPrice)
        {
            _gold -= _luckPrice;
            _luckMultiplier += 10;

            SetPrices();

            luckMultiplierText.SetText(_luckMultiplier.ToString() + "%");
            luckPriceText.SetText(_luckPrice.ToString());
            goldText.SetText("Gold: " + _gold.ToString());
        }
    }
    
    public void BuyPotionsButton()
    {
        if (_gold >= _potionsPrice)
        {
            _gold -= _potionsPrice;
            healthPotion++;

            SetPrices();

            potionsText.SetText(healthPotion.ToString());
            potionsPriceText.SetText(_potionsPrice.ToString());
            goldText.SetText("Gold: " + _gold.ToString());
        }
    }
}

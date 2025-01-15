using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;

    Damageable playerdamageable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) Debug.Log("no player");
        playerdamageable = player.GetComponent<Damageable>();
    }
    void Start()
    {
        healthSlider.value = CalculateSliderPercentage(playerdamageable.Health,playerdamageable.Maxhealth);
        healthBarText.text = " Health " + playerdamageable.Health + " / " + playerdamageable.Maxhealth;
    }

    private void OnEnable()
    {
        playerdamageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }
    private void OnDisable()
    {
        playerdamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth/maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = " Health " + newHealth + " / " + maxHealth;
    }
}

﻿using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;
    public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
    public Image m_FillImage;                           // The image component of the slider.
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    public GameObject m_ExplosionPrefab;

    private AudioSource m_ExplosionAudio;
    private ParticleSystem m_ExplosionParticles;
    private float m_CurrentHealth;
    private bool m_Dead;

    private void Awake()
    {
        // Instantiate the explosion prefab and get a reference to the particle system on it.
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        // Get a reference to the audio source on the instantiated prefab.
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();
        // Disable the prefab so it can be activated when it's required.
        m_ExplosionParticles.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;
        SetHealthUI();
    }

    public void TakeDamage(float amount)
    {
        m_CurrentHealth -= amount;
        SetHealthUI();
        if (m_CurrentHealth <= 0f && !m_Dead) OnDeath();
    }

    private void SetHealthUI()
    {
        m_Slider.value = m_CurrentHealth;
        // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }

    private void OnDeath()
    {
        m_Dead = true;
        // Move the instantiated explosion prefab to the tank's position and turn it on.
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        gameObject.SetActive(false);
    }
}

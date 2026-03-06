using UnityEngine;
using UnityEngine.UI;

public class UI_Boss : MonoBehaviour
{
    public Entity entity;
    public CharacterStats myStats;
    public Slider slider;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHp;
    }

    private void OnEnable()
    {
        myStats.onHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        if (myStats != null)
            myStats.onHealthChanged -= UpdateHealthUI;
    }

}

using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private RectTransform rectTransform;
    private CharacterStats myStats => GetComponentInParent<CharacterStats>();
    private Slider slider;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
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
        entity.onFilped += FilpUI;
        myStats.onHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        if(entity != null)
            entity.onFilped -= FilpUI;
        if(myStats != null)
            myStats.onHealthChanged -= UpdateHealthUI;
    }

    private void FilpUI() => rectTransform?.Rotate(0, 180, 0);

}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class СollDownManager: MonoBehaviour
{
    [Header("Кнопки")]
    [SerializeField] private Button actionButton;
    [SerializeField] private Image targetButtonImage;

    [Header("Настройки")]
    [SerializeField] private Color targetColor;
    [SerializeField] private float cooldown = 3f;

    void Start()
    {
        // Привязываем клик к методу
        if (actionButton != null) actionButton.onClick.AddListener(UseAbility);
    }

    private void UseAbility()
    {
        if (targetButtonImage != null) targetButtonImage.color = targetColor;

        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        actionButton.interactable = false;

        yield return new WaitForSeconds(cooldown);

        actionButton.interactable = true;
    }
}
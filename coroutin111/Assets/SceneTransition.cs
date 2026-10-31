using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;              // чёрная картинка для затемнения
    [SerializeField] private string nextSceneName = "GamePlay"; // имя второй сцены
    [SerializeField] private float fadeDuration = 1f;      // длительность затемнения в секундах

    private bool isLoading;

    private void Start()
    {
        // Проверяем: если на этой сцене картинка изначально включена и чёрная (Alpha > 0.9),
        // значит, мы только что загрузились и нужно плавно выйти из темноты!
        if (fadeImage != null && fadeImage.color.a > 0.9f)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            // Иначе (для первой сцены) делаем её прозрачной
            SetAlpha(0f);
            if (fadeImage != null) fadeImage.raycastTarget = false;
        }
    }

    // Этот метод вешается на кнопку (OnClick) на первой сцене
    public void GoToNextScene()
    {
        if (isLoading) return; // защита от повторных нажатий
        StartCoroutine(FadeAndLoad());
    }

    // Корутина для ПЕРВОЙ сцены (затемнение и загрузка)
    private IEnumerator FadeAndLoad()
    {
        isLoading = true;
        fadeImage.raycastTarget = true; // блокируем кнопки во время перехода

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            SetAlpha(Mathf.Clamp01(timer / fadeDuration));
            yield return null;
        }

        SetAlpha(1f);

        // Загружаем сцену
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    // Корутина для ВТОРОЙ сцены (выход из темноты)
    private IEnumerator FadeIn()
    {
        fadeImage.raycastTarget = true;
        SetAlpha(1f); // Начинаем с абсолютно черного экрана

        float timer = fadeDuration;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            SetAlpha(Mathf.Clamp01(timer / fadeDuration));
            yield return null;
        }

        SetAlpha(0f);
        fadeImage.raycastTarget = false;
    }

    private void SetAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = alpha;
            fadeImage.color = c;
        }
    }
}
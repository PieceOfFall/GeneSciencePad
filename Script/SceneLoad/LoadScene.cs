using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public UnityEvent loadEvents;

    public List<Image> fadeInImgs;

    public float fadeDuration = 4f;

    public float waitDuration = 1f;

    public string loadSceneName;

    private Image bgImg;
    void Start()
    {
        bgImg = GetComponent<Image>();
        StartCoroutine(FadeInLoad());
        loadEvents.Invoke();
    }

    private IEnumerator FadeInLoad()
    {
        float fadeInDuration = fadeDuration * 0.7f;
        float fadeOutDuration = fadeDuration * 0.3f;

        // 淡入
        float elapsedFadeInTime = 0;
        while (elapsedFadeInTime < fadeInDuration)
        {
            float progress = elapsedFadeInTime / fadeInDuration;

            fadeInImgs.ForEach(image =>
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.SmoothStep(0f, 1f, progress));
                });
            elapsedFadeInTime += Time.deltaTime;

            yield return null;
        }
        
        fadeInImgs.ForEach(image =>
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        });

        // 暂停展示
        yield return new WaitForSeconds(waitDuration);

        // 淡出
        float elapsedFadeOutTime = 0;
        while (elapsedFadeOutTime < fadeOutDuration)
        {
            float progress = elapsedFadeOutTime / fadeOutDuration;

            fadeInImgs.ForEach(image =>
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.SmoothStep(1f, 0f, progress));
            });
            elapsedFadeOutTime += Time.deltaTime;

            yield return null;
        }


        SceneManager.LoadScene(loadSceneName);

        yield return null;
    }
}

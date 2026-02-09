using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;     // Canvas上のFade Image
    [SerializeField] private float fadeOutTime = 0.8f;
    [SerializeField] private float fadeInTime  = 0.6f;
    [SerializeField] private AudioSource seSource;
    [SerializeField] private AudioClip doorSe;

    private bool isLoading;

    private void Awake()
    {
        // シーン切替後もこのオブジェクトを残す（黒幕を持ち越す）
        DontDestroyOnLoad(gameObject);

        // 念のため最初は透明
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            var c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    public void LoadScene(string sceneName)
    {
        if (isLoading) return;
        isLoading = true;

        if (seSource != null && doorSe != null)
        seSource.PlayOneShot(doorSe);

        StartCoroutine(LoadSceneWithFade(sceneName));
    }

    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        // 1) 透明→黒（フェードアウト）
        yield return Fade(0f, 1f, fadeOutTime);

        // 2) 黒いままシーン切替
        yield return SceneManager.LoadSceneAsync(sceneName);

        // 3) 次のシーンで黒→透明（フェードイン）
        yield return Fade(1f, 0f, fadeInTime);

        // 4) 役目終了。残しておくと次の遷移で二重になるので消す
        Destroy(gameObject);
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        if (fadeImage == null) yield break;

        Color c = fadeImage.color;
        float t = 0f;

        c.a = from;
        fadeImage.color = c;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(from, to, Mathf.Clamp01(t / duration));
            c.a = a;
            fadeImage.color = c;
            yield return null;
        }

        c.a = to;
        fadeImage.color = c;
    }
}

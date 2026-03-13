using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 4f;

    [SerializeField]
    private float maxAlpha = .5f;

    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private GameObject restartButton;

    public static GameOver Instance;


    private void Start()
    {
        fadeImage.gameObject.SetActive(false);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // I don't actually care about keeping this instance the only one.
            // When loading a new scene, the GameOver of the previous scene is still Instance
            // even though it doesn't really exist anymore
            Destroy(Instance.gameObject);
            Instance = this;
        }
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DoGameOver()
    {
        StartCoroutine(HandleFadeOut());
    }


    private IEnumerator HandleFadeOut()
    {

        fadeImage.gameObject.SetActive(true); // Enable the image before fading

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(restartButton);
        }

        float elapsedTime = 0f;
        Color fadeColor = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Clamp01(elapsedTime / fadeDuration) * maxAlpha; // Increase alpha
            fadeImage.color = fadeColor;
            yield return null;
        }

        yield return null;
    }
}


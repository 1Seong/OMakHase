using System.Collections;
using TMPro;
using UnityEngine;

public class RecipePopupManager : MonoBehaviour
{
    public static RecipePopupManager instance;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject newImage;
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float showDuration = 3f;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void Show(string recipeName)
    {
        if(UnlockManager.instance.IsRecipeUnlocked(recipeName))
            newImage.SetActive(false);
        else
            newImage.SetActive(true);

        tmp.text = recipeName + "(을/를) 완성했습니다!";
        StartCoroutine(ShowCoroutine());
    }
    
    private IEnumerator ShowCoroutine()
    {
        canvasGroup.alpha = 0;
        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            canvasGroup.alpha = i / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(showDuration);
        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            canvasGroup.alpha = 1 - (i / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
}

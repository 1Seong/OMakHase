using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditRoll : MonoBehaviour
{

    RectTransform rectTransform;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {

        StartCoroutine(rollCredit());

    }

    IEnumerator rollCredit() {

        yield return new WaitForSeconds(3.0f);

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, transform.position);

        while (screenPos.y <= Screen.height) {
            Debug.Log("rolling");
            rectTransform.Translate(Vector3.up * Time.deltaTime * 100.0f);
            yield return null;
            screenPos = RectTransformUtility.WorldToScreenPoint(null, transform.position);

        }

        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("StartScene");
    }
}

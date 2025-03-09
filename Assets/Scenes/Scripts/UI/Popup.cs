using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class Popup : UISlide
{
    public enum Mode { SlideLerp, SlideExpon, Fade }

    private Action PopIn;
    private Action PopOut;

    [SerializeField] private float _fadeTime = 0.3f;
    [SerializeField] private float _popDuration = 4.0f;
    [SerializeField] private Mode _mode;
    private Mode _Mode
    {
        get => _mode;
        set
        {
            _mode = value;
            Debug.Log(_mode);
            switch(_mode)
            {
                case Mode.SlideLerp:
                    Debug.Log("case slidelerp");
                    PopIn = () => StartCoroutine(SlideInLerp());
                    PopOut = () => StartCoroutine(SlideOutLerp());
                    break;
                case Mode.SlideExpon:
                    PopIn = () => StartCoroutine(SlideIn());
                    PopOut = () => StartCoroutine(SlideOut());
                    break;
                case Mode.Fade:
                    PopIn = () => StartCoroutine(FadeIn());
                    PopOut = () => StartCoroutine(FadeOut());
                    break;
            }
        }
    }

    public void OnClickBase()
    {
        if (CookManager.instance.baseIngred != Ingredient.Base.noCondition)
            return;

        var textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = "베이스 재료를 선택하지 않았습니다!";
        PopInAndOut();
    }

    public void OnClickMain()
    {
        if (CookManager.instance.meatfish != Ingredient.MeatFish.noCondition || CookManager.instance.vege != Ingredient.Vege.noCondition)
            return;

        var textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = "주재료를 선택하지 않았습니다!";
        PopInAndOut();
    }

    public void OnClickCook()
    {
        if (CookManager.instance.cook != Ingredient.Cook.noCondition)
            return;

        var textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = "주재료를 선택하지 않았습니다!";
        PopInAndOut();
    }

    public void PopInAndOut()
    {
        Debug.Log("PopInOut");
        PopInAct();
        Invoke("PopOutAct", _popDuration);
    }

    public void PopInAct()
    {
        PopIn.Invoke();
    }

    public void PopOutAct()
    {
        PopOut.Invoke();
    }

    protected override void Awake()
    {
        mainImage = GetComponent<RectTransform>();

        _Mode = _mode;
       
        hiddenPosition = new Vector2(0f, 100f);
        visiblePosition = mainImage.anchoredPosition;

        // 패널을 숨김
        if (_Mode == Mode.SlideLerp || _Mode == Mode.SlideExpon)
            mainImage.anchoredPosition = hiddenPosition;

    }

    protected override void Start()
    {
       
    }

    protected override IEnumerator SlideIn()
    {
       
        isActing = true;

        while (mainImage.anchoredPosition.y > visiblePosition.y + 0.2f)
        {
            mainImage.anchoredPosition += (visiblePosition - mainImage.anchoredPosition) * speed * Time.deltaTime;

            yield return null;
        }

        mainImage.anchoredPosition = visiblePosition;
       
        isActing = false;
    }

    protected override IEnumerator SlideOut()
    {
      
        isActing = true;

        while (mainImage.anchoredPosition.y < hiddenPosition.y - 0.2f)
        {
            mainImage.anchoredPosition += (hiddenPosition - mainImage.anchoredPosition) * speed * Time.deltaTime;

            yield return null;
        }

        mainImage.anchoredPosition = hiddenPosition;
    
        isActing = false;
    }

    private IEnumerator SlideInLerp()
    {
        Debug.Log("Lerp");
        isActing = true;
        
        var t = 0f;

        while (mainImage.anchoredPosition.y >= visiblePosition.y)
        {
            mainImage.anchoredPosition = Vector2.Lerp(hiddenPosition, visiblePosition, t);
            t += Time.deltaTime;

            yield return null;
        }

        mainImage.anchoredPosition = visiblePosition;

        isActing = false;
    }

    private IEnumerator SlideOutLerp()
    {
        isActing = true;
        var t = 0f;

        while (mainImage.anchoredPosition.y <= hiddenPosition.y)
        {
            mainImage.anchoredPosition = Vector2.Lerp(visiblePosition, hiddenPosition, t);
            t += Time.deltaTime;

            yield return null;
        }

        mainImage.anchoredPosition = hiddenPosition;
       
        isActing = false;
    }

    private IEnumerator FadeIn()
    {
        isActing = true;
       
        var image = GetComponent<Image>();
        var text = GetComponentInChildren<TextMeshProUGUI>();

        for (var i = 0f; i <= _fadeTime; i += Time.deltaTime)
        {
            image.tintColor = new Color(0, 0, 0, Mathf.Lerp(0, 1f, i / _fadeTime));
            text.color = new Color(0, 0, 0, Mathf.Lerp(0, 1f, i / _fadeTime));

            yield return null;
        }

       

        isActing = false;
    }

    private IEnumerator FadeOut()
    {
        isActing = true;
        var image = GetComponent<Image>();
        var text = GetComponentInChildren<TextMeshProUGUI>();

        for (var i = 0f; i <= _fadeTime; i += Time.deltaTime)
        {
            image.tintColor = new Color(0, 0, 0, Mathf.Lerp(1f, 0f, i / _fadeTime));
            text.color = new Color(0, 0, 0, Mathf.Lerp(1f, 0f, i / _fadeTime));

            yield return null;
        }


        
        isActing = false;
    }
}

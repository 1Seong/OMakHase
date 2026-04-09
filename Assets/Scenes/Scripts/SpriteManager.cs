using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Instance;

    [SerializeField]
    float fadeSpeed;


    [SerializeField]
    private List<Sprite> randomSpritePool;

    [SerializeField]
    private Dictionary<Sprite, bool> spriteDuplicationPool;

    public Image targetImage;         // UI Image 컴포넌트
    public Sprite newSprite;          // 바꿀 스프라이트

    public void Awake()
    {
        Instance = this;

        spriteDuplicationPool = new Dictionary<Sprite, bool>();
        for (int i = 0; i < randomSpritePool.Count; i++)
        {
            spriteDuplicationPool.Add(randomSpritePool[i], false);
        }
    }

    public void Start()
    {

    }

    bool CheckSameSprite(Image p_Image, Sprite p_Sprite)
    {
        if (p_Image.sprite == p_Sprite)
        {
            return true;
        }
        else {
            return false;
        }
    }

    public IEnumerator SpriteChangeCoroutine(string p_SpriteName)
    {


        newSprite = Resources.Load<Sprite>("Characters/" + p_SpriteName);

        if (!CheckSameSprite(targetImage, newSprite)) { 
            Color t_color = targetImage.color;
            t_color.a = 0;
            targetImage.color = t_color;

            targetImage.sprite = newSprite;

            while (t_color.a < 1) {

                t_color.a += fadeSpeed;
                targetImage.color = t_color;
                yield return null;

            }
        }

    }

    public IEnumerator SpriteChangeCoroutine(Sprite sprite)
    {


        newSprite = sprite;

        if (!CheckSameSprite(targetImage, newSprite))
        {
            Color t_color = targetImage.color;
            t_color.a = 0;
            targetImage.color = t_color;

            targetImage.sprite = newSprite;

            while (t_color.a < 1)
            {

                t_color.a += fadeSpeed;
                targetImage.color = t_color;
                yield return null;

            }
        }

    }

    /*
    public void GetRandomSprite()
    {
        if (randomSpritePool.Count == 0)
        {
            Debug.LogError("스프라이트 풀이 비어있다");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, randomSpritePool.Count);
        targetImage.sprite = randomSpritePool[randomIndex];
    }
    */




    // 중복 안되는 스프라이트 가져오기
    public Sprite GetUniqueSprite()
    {
        // 아직 사용되지 않은 스프라이트들만 모음
        List<Sprite> availableSprites = new List<Sprite>();

        foreach (var pair in spriteDuplicationPool)
        {
            if (!pair.Value)
                availableSprites.Add(pair.Key);
        }

        // 전부 사용됐을 경우 예외 처리
        if (availableSprites.Count == 0)
        {
            Debug.LogWarning("모든 Sprite가 사용됨. Duplication Pool 리셋");
            ResetSpriteDuplicationPool();
            return GetUniqueSprite();
        }

        // 랜덤 선택
        Debug.Log("랜덤 스프라이트 가져옴");
        Sprite selected = availableSprites[UnityEngine.Random.Range(0, availableSprites.Count)];
        spriteDuplicationPool[selected] = true;

        return selected;
    }

    public void ResetSpriteDuplicationPool()
    {

        Debug.LogWarning("랜덤 손님 시작  Duplication Pool 리셋");
        List<Sprite> keys = new List<Sprite>(spriteDuplicationPool.Keys);

        foreach (var key in keys)
        {
            spriteDuplicationPool[key] = false;
        }
    }
}

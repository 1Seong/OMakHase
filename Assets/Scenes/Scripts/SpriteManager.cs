using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Instance;

    [SerializeField]
    float fadeSpeed;


    [SerializeField]
    private List<Sprite> randomSpritePool;

    public Image targetImage;         // UI Image 컴포넌트
    public Sprite newSprite;          // 바꿀 스프라이트

    public void Awake()
    {
        Instance = this;
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

}

using System;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;
    [SerializeField] private Texture2D defaultTex;
    [SerializeField] private Texture2D clickTex;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            Cursor.SetCursor(clickTex, Vector2.zero, CursorMode.Auto);
        else if(Input.GetMouseButtonUp(0))
            Cursor.SetCursor(defaultTex, Vector2.zero, CursorMode.Auto);
    }
}

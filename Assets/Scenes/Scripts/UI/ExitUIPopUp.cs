using UnityEngine;
using UnityEngine.SceneManagement;


public class ExitUIPopUp : MonoBehaviour
{
    [SerializeField] GameObject ExitUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "StartScene" && Input.GetKeyDown(KeyCode.Escape))
        {
            if (ExitUI.activeSelf == true)
            {
                ExitUI.SetActive(false);
            }
            else {
                ExitUI.SetActive(true);
            }
        }
    }
}

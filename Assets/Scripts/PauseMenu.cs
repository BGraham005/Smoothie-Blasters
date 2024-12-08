using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject Cam;
    private CamManager script;
    [SerializeField] private Slider SenseToggle;

    void Start()
    {
        SensitivityToggle();
        Cursor.visible = false;
    }
    
    public void Pause(InputAction.CallbackContext context)
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
    }

    public void Resume()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SensitivityToggle()
    {
        script = Cam.GetComponent<CamManager>();
        script.mouseSense.x = SenseToggle.value;
        script.mouseSense.y = SenseToggle.value;
    }
}

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private TextMeshProUGUI StartText;
    [SerializeField] private GameObject Cam;
    private CamManager script;
    [SerializeField] private Slider SenseToggle;
    [SerializeField] private GameObject Cannon;
    private CannonBrain CannonScript;

    void Start()
    {
        SensitivityToggle();
        StartText.alpha = 0;
        Cursor.visible = false;
        CannonScript = Cannon.GetComponent<CannonBrain>();
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

    public void WaveText()
    {
        StartText.alpha = 1;
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        int frames = 24;
        yield return new WaitForSeconds(1);
        while (frames > 0)
        {
            StartText.alpha = StartText.alpha - 0.04f;
            frames--;
            yield return new WaitForSeconds(0.04f);
        }
        StartText.alpha = 0;
        CannonScript.Randomizer();
        yield return null;
    }

}

using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyTimer : MonoBehaviour
{
    private string fulltime;
    private string normaltime;
    private string[] parser = new string[2];
    [SerializeField] private GameObject Self;
    [SerializeField] private TextMeshPro Timer;
    [SerializeField] private GameObject Cannon;
    private CannonBrain CannonScript;
    // Move Z positively to go farther
    void Start()
    {
        CannonScript = Cannon.GetComponent<CannonBrain>();
        Timer.text = "60";
    }

    public void InitiateTimer()
    {
        StartCoroutine(RunTimer());
    }

    public void ResetTimer()
    {

    }

    public IEnumerator RunTimer()
    {
        float startingtime = Time.time + 60;
        while (Time.time < startingtime && CannonScript.WaveActive == true)
        {
            fulltime = Convert.ToString(startingtime-Time.time);
            parser = fulltime.Split(".");
            normaltime = parser[0];
            Timer.text = normaltime;

            Self.transform.localScale =
            new Vector3(10/(startingtime-Time.time+1),
            10/(startingtime-Time.time+1),
            10/(startingtime-Time.time+1));
            
            yield return null;
        }
        if (Time.time >= startingtime)
        {
            CannonScript.GameOver();
            yield return null;
        }
        else if (CannonScript.WaveActive == false)
        {
            CannonScript.PreNewWave();
            yield return null;
        }
        yield return null;
    }

}

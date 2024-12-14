using System.Collections;
using UnityEngine;
using Unity.VisualScripting;
using System.Linq;

public class CannonBrain : MonoBehaviour
{
    [SerializeField] private SpriteRenderer Screen1;
    [SerializeField] private SpriteRenderer Screen2;
    [SerializeField] private SpriteRenderer Screen3;
    public Sprite[] SpriteList;
    public int[] FruitsToGet;
    public bool[] FruitsOwned;
    public int[] FruitsRemaining;
    public int OnFruitNum;
    private int counter = 0;
    public bool WaveActive;
    [SerializeField] private GameObject MainUI;
    private PauseMenu UIScript;
    [SerializeField] private GameObject Enemy;
    private EnemyTimer EnemyScript;
    // Spawning randomization
    [SerializeField] private GameObject[] FruitSpots;
    private FruitSpot[] FruitScripts;
    
    void Start()
    {
        //SpriteList = new Sprite[] {banana,blueberry,strawberry};
        FruitsToGet = new int[] {0,0,0};
        WaveActive = false;
        FruitsOwned = new bool[] {false,false,false};
        OnFruitNum = 0;
        FruitsRemaining = new int[3];
        SetFruit();
        UIScript = MainUI.GetComponent<PauseMenu>();
        EnemyScript = Enemy.GetComponent<EnemyTimer>();
        SpriteList = Resources.LoadAll<Sprite>("Textures/Fruit");
        Screen1.sprite = null;
        Screen2.sprite = null;
        Screen3.sprite = null;
        StartCoroutine(NewWaveAnim());
        //Randomizer();
    }

    void Update()
    {
        if (FruitsOwned[0] == true && FruitsOwned[1] == true && FruitsOwned[2] == true && WaveActive == true)
        {
            Debug.Log("Comepleted wave");
            WaveActive = false;
            FruitsOwned = new bool[] {false,false,false};
            FruitsRemaining = new int[3];
            OnFruitNum = 0;
            //StartCoroutine(NewWaveAnim());
        }
    }

    private void SetFruit()
    {
        FruitScripts = new FruitSpot[FruitSpots.Length];
        while (counter < FruitSpots.Length)
        {
            var TargetFruit = FruitSpots[counter];
            var CurScript = TargetFruit.GetComponent<FruitSpot>();
            FruitScripts.SetValue(CurScript,counter);
            counter++;
        }
        counter = 0;
    }

    private void SpawnFruit()
    {
        while (counter < FruitSpots.Length)
        {
            FruitScripts[counter].MyFruit = UnityEngine.Random.Range(0,SpriteList.Length);
            FruitScripts[counter].SetMyImg();
            counter++;
        }
        counter = 0;
        // Impossible seed prevention
        bool firstclear = false;
        bool secondclear = false;
        int[] locations = new int[3];
        locations[0] = UnityEngine.Random.Range(0,SpriteList.Length);
        while (firstclear == false)
        {
            locations[1] = UnityEngine.Random.Range(0,SpriteList.Length);
            if (locations[1] != locations[0])
            {
                firstclear = true;
            }
        }
        while (secondclear == false)
        {
            locations[2] = UnityEngine.Random.Range(0,SpriteList.Length);
            if (locations[2] != locations[0] && locations[2] != locations[1])
            {
                secondclear = true;
            }
        }
        FruitScripts[locations[0]].MyFruit = FruitsToGet[0];
        FruitScripts[locations[0]].SetMyImg();
        FruitScripts[locations[1]].MyFruit = FruitsToGet[1];
        FruitScripts[locations[1]].SetMyImg();
        FruitScripts[locations[2]].MyFruit = FruitsToGet[2];
        FruitScripts[locations[2]].SetMyImg();
        WaveActive = true;
        EnemyScript.InitiateTimer();
    }

    public void Randomizer()
    {
        while (counter < FruitsToGet.Length)
        {
            FruitsToGet[counter] = UnityEngine.Random.Range(0,SpriteList.Length);
            counter++;
        }
        counter = 0;
        Debug.Log("Fruits to get: "+FruitsToGet[0]+FruitsToGet[1]+FruitsToGet[2]);
        FruitsRemaining = FruitsToGet;
        StartCoroutine(RandomizerAnim());
    }

    public void GameOver()
    {
        Debug.Log("Game Over.");
    }

    public void PreNewWave()
    {
        StartCoroutine(NewWaveAnim());
    }

    private IEnumerator RandomizerAnim()
    {
        while (counter <= 20)
        {
            Screen1.sprite = SpriteList[UnityEngine.Random.Range(0,SpriteList.Length)];
            Screen2.sprite = SpriteList[UnityEngine.Random.Range(0,SpriteList.Length)];
            Screen3.sprite = SpriteList[UnityEngine.Random.Range(0,SpriteList.Length)];
            if (counter <= 10) yield return new WaitForSeconds(0.04f);
            if (counter >= 11 && counter <= 16) yield return new WaitForSeconds(0.08f);
            if (counter >= 17) yield return new WaitForSeconds(0.2f);
            counter++;
        }
        yield return new WaitForSeconds(0.5f);
        Screen1.sprite = SpriteList[FruitsToGet[0]];
        Screen2.sprite = SpriteList[FruitsToGet[1]];
        Screen3.sprite = SpriteList[FruitsToGet[2]];
        counter = 0;
        yield return new WaitForSeconds(0.5f);
        SpawnFruit();
        yield return null;
    }

    private IEnumerator NewWaveAnim()
    {
        yield return new WaitForSeconds(1);
        UIScript.WaveText();
        yield return null;
    }
}

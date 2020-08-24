using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Set In Inspector")]
    public GameObject Canvas;
    public GameObject endLevelPanel;
    public FallItemSO fiSO;
    public Text levelText;
    public Text caughtItemsText;
    public GameObject UIIconPrefab;
    
    [Header("Set Dynamically")]
    public List<Image> spritesImg = new List<Image>();
    public List<Image> BGImages = new List<Image>();
    public List<GameObject> UIIconsClearPrefabs = new List<GameObject>();

    //Number of caught items to control witch icon on UI is now need to change
    private int iconsCount = 0;

    //When level ends set the plate(Player) to inactive state
    public delegate void OnPlayerIsPassive();
    public static event OnPlayerIsPassive onPlayerPassive;
    
    
    private void Awake()
    {
        Plate.onItemFallType += UpdateUI;
       CatchMe.onLevelBeated += LevelBeatedUI;

    }

    private void Start()
    {
        for (int i = 0; i < fiSO.ItemsToCatch; i++)
        {
            UIIconsClearPrefabs.Add(UIIconPrefab);
        }
        
        DisplayClearIcons();
        levelText.text = "Level: " + fiSO.currentLevel;
    }

    public void DisplayClearIcons()
    {
        int step = 150;
        for (int i = 0; i < UIIconsClearPrefabs.Count; i++)
        {
            GameObject filledIcon = Instantiate(UIIconPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            
            spritesImg.Add(filledIcon.gameObject.transform.GetChild(2).GetComponent<Image>());
            BGImages.Add(filledIcon.gameObject.transform.GetChild(1).GetComponent<Image>());
            filledIcon.transform.SetParent(Canvas.transform);
            filledIcon.transform.position = new Vector3(0,2000f-step,0);
            step += 150;
        }
    }

    public void UpdateUI(Sprite icon)
    {
        spritesImg[iconsCount].sprite = icon;
        spritesImg[iconsCount].color = Color.white;
        BGImages[iconsCount].color = Color.cyan;
        iconsCount++;
    }

    public void LevelBeatedUI(int currentLevel, int caughtItems)
    {
        onPlayerPassive?.Invoke();
        endLevelPanel.SetActive(true);
        caughtItemsText.text = "Caught Items: " + caughtItems;
    }

    
    public void NextLevelButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetProgressButtonIsPressed()
    {
        fiSO.ResetProgress();
    }
    
    private void OnDisable()
    { 
        Plate.onItemFallType -= UpdateUI;
        CatchMe.onLevelBeated -= LevelBeatedUI;
    }
}

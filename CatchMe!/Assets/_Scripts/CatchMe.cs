using System;
using System.Collections;
using _Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Main script, add to main camera
/// </summary>
public class CatchMe : MonoBehaviour
{
    public delegate void OnLevelBeated(int currentLevel, int collectedItems);
    public static event OnLevelBeated onLevelBeated;

    [Header("Set in Inspector")] public FallItemSO fiSO;

    private void Awake()
    {
        Plate.onItemFall += CheckWinCondition;
        SetupLevel();
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    public void SetupLevel()
    {
        if (fiSO.currentLevel % fiSO.extraItemWhenPassLevels == 0) fiSO.ItemsToCatch++;
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            Instantiate(fiSO.GetItemPrefab(), GetRandomPositionForItem(), Quaternion.identity);
        }
    }
    

    public Vector3 GetRandomPositionForItem()
    {

        Vector3 rPos = new Vector3(Random.Range(-1.8f, 1.8f),8, Random.Range(-6.6f, -2.4f));
        Vector3 pos = Camera.main.WorldToViewportPoint(rPos);
        pos.x = Mathf.Clamp(pos.x, 0.1f, 0.9f);
        Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);
        newPos.y = 8;

        return newPos;
    }

    public void CheckWinCondition(int itemsCaught)
    {
        if (itemsCaught >= fiSO.ItemsToCatch)
        {
            onLevelBeated?.Invoke(fiSO.currentLevel, itemsCaught);
           fiSO.currentLevel++;
        }
    }

    private void OnDestroy()
    {
        Plate.onItemFall -= CheckWinCondition;
    }
}

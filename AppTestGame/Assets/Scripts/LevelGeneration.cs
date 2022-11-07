using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [Header("Prefabs for level")]
    [SerializeField] private GameObject levelPrefab;
    [SerializeField] private GameObject obstraclePrefab;
    [Header("Settings level")]
    [SerializeField] private float levelLength;
    [SerializeField] private float speedLevel;

    private List<GameObject> levelObjects = new List<GameObject>();
    private List<GameObject> obstracles = new List<GameObject>();

    //Road settings
    private enum Difficulty { Easy = 35, Medium = 55, Hard = 65 };
    private int currentDifficulty;
    private float maxSpeed = 100f;
    private float increaseSpeed = 7f;
    private float distanceToNewSpawn;

    //Obstracle settings 
    private int maxObsctracle = 6;
    private float distanceObsctracle = 100f;

    private IEnumerator MoveRoad()
    {
        while (GameManager.Instance.IsGame)
        {
            for (int x = 0; x < levelObjects.Count; x++)
                levelObjects[x].transform.Translate(-Vector3.forward * speedLevel * Time.deltaTime);

            if (levelObjects[levelObjects.Count - 1].transform.position.z <= -distanceToNewSpawn))
            {
                if (speedLevel >= maxSpeed)
                    speedLevel = maxSpeed;
                else
                    speedLevel += increaseSpeed;

                CreateRoad();
            }

            if (levelObjects[0].transform.position.z <= -levelLength)
            {
                for (int x = 0; x < maxObsctracle; x++)
                    obstracles.Remove(obstracles[x]);

                Destroy(levelObjects[0]);
                levelObjects.Remove(levelObjects[0]);
            }

            yield return null;
        }

        yield break;
    }
    private void CreateRoad()
    {
        Vector3 pos = Vector3.zero;

        if (levelObjects.Count > 0)
            pos = levelObjects[levelObjects.Count - 1].transform.position + new Vector3(0, 0, levelLength);

        GameObject lvlObject = Instantiate(levelPrefab, pos, Quaternion.identity);
        lvlObject.transform.SetParent(transform);
        levelObjects.Add(lvlObject);

        CreateObstracles(lvlObject);
    }
    private void CreateObstracles(GameObject parent)
    {
        float maxPosYspawn = 15f;
        float minPosYspawn = -45f;

        Vector3 pos = new Vector3(0, Random.Range(minPosYspawn, maxPosYspawn), distanceObsctracle);

        for (int x = 0; x < maxObsctracle; x++)
        {
            if (obstracles.Count > 0)
                pos = new Vector3(0, Random.Range(minPosYspawn, maxPosYspawn), obstracles[obstracles.Count - 1].transform.position.z + distanceObsctracle);

            GameObject obstracleObject = Instantiate(obstraclePrefab, pos, Quaternion.Euler(0, 90, 0));
            obstracleObject.transform.SetParent(parent.transform);
            obstracles.Add(obstracleObject);
        }
    }
    private float GetDistanceNewSpawn(float distance)
    {
        distance = levelLength / 2;
        return distance;
    }
    private void SetSpeed()
    {
        currentDifficulty = GameManager.Instance.CurrentDifficulty();

        switch (currentDifficulty)
        {
            case 1:
                speedLevel = (float)Difficulty.Easy;
                break;
            case 2:
                speedLevel = (float)Difficulty.Medium;
                break;
            case 3:
                speedLevel = (float)Difficulty.Hard;
                break;
            default:
                speedLevel = (float)Difficulty.Easy;
                break;
        }
    }

    private void ClearLevel()
    {
        for (int x = 0; x < levelObjects.Count; x++)
            Destroy(levelObjects[x]);

        speedLevel = 0;
        obstracles.Clear();
        levelObjects.Clear();
    }
    private void OnEnable()
    {
        SetSpeed();
        GetDistanceNewSpawn(distanceToNewSpawn);
        CreateRoad();

        StartCoroutine(MoveRoad());
    }
    private void OnDisable()
    {
        ClearLevel();
    }
}

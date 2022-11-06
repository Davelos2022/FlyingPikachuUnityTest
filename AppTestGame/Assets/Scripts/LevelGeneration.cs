using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [Header("Prefabs for level")]
    [SerializeField] private GameObject LevelPrefab;
    [SerializeField] private GameObject ObstraclePrefab;
    [Header("Settings level")]
    [SerializeField] private float speedLevel;

    private List<GameObject> buildings = new List<GameObject>();
    private List<GameObject> obstracles = new List<GameObject>();

    //Road settings
    private int currentDifficulty;
    private enum Difficulty { Easy = 35, Medium = 55, Hard = 65 };
    private float maxSpeed = 80f;
    private float distanceToDestroy = 650f;
    private float disnanceToNewRoad = 250f;
    private float increaseSpeed = 8f;
    //Obstracle settings
    private int maxObsctracle = 6;
    private float distanceObsctracle = 100f;

    private IEnumerator MoveRoad()
    {
        while (GameManager.Instance.IsGame)
        {
            for (int x = 0; x < buildings.Count; x++)
                buildings[x].transform.Translate(-Vector3.forward * speedLevel * Time.deltaTime);

            if (buildings[buildings.Count - 1].transform.position.z < -disnanceToNewRoad)
                CreateRoad();

            if (buildings[0].transform.position.z <= -distanceToDestroy)
            {
                for (int x = 0; x < maxObsctracle; x++)
                    obstracles.Remove(obstracles[x]);

                Destroy(buildings[0]);
                buildings.Remove(buildings[0]);
            }

            yield return null;
        }

        yield break;
    }

    private void CreateRoad()
    {
        if (speedLevel >= maxSpeed)
            speedLevel = maxSpeed;
        else
            speedLevel += increaseSpeed;

        Vector3 pos = Vector3.zero;

        if (buildings.Count > 0)
            pos = buildings[buildings.Count - 1].transform.position + new Vector3(0, 0, distanceToDestroy);

        GameObject lvlObject = Instantiate(LevelPrefab, pos, Quaternion.identity);
        lvlObject.transform.SetParent(transform);
        buildings.Add(lvlObject);

        CreateObstracles(lvlObject);
    }

    private void CreateObstracles(GameObject parent)
    {
        float maxPosYspawn = 12f;
        float minPosYspawn = -20f;

        Vector3 pos = new Vector3(0, Random.Range(minPosYspawn, maxPosYspawn), distanceObsctracle);

        for (int x = 0; x < maxObsctracle; x++)
        {
            if (obstracles.Count > 0)
                pos = new Vector3(0, Random.Range(minPosYspawn, maxPosYspawn), obstracles[obstracles.Count - 1].transform.position.z + distanceObsctracle);

            GameObject obstracleObject = Instantiate(ObstraclePrefab, pos, Quaternion.Euler(0, 90, 0));
            obstracleObject.transform.SetParent(parent.transform);
            obstracles.Add(obstracleObject);
        }
    }

    private void SetSpeed(int check)
    {
        currentDifficulty = GameManager.Instance.CurrentDifficulty();

        switch (check)
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
        for (int x = 0; x < buildings.Count; x++)
            Destroy(buildings[x]);

        for (int x = 0; x < obstracles.Count; x++)
            Destroy(obstracles[x]);

        speedLevel = 0;
        obstracles.Clear();
        buildings.Clear();
    }

    private void OnEnable()
    {
        SetSpeed(currentDifficulty);
        CreateRoad();
        StartCoroutine(MoveRoad());
    }

    private void OnDisable()
    {
        ClearLevel();
    }
}

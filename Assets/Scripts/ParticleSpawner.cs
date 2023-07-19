using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public GameObject Particle;
    //public float spawnTimeTolerance = 0.05f; // Adjust this value as needed

    private List<ParticleData> particleDataList;
    private Dictionary<int, ParticleSpawnInfo> particleSpawnInfoMap;
    private Dictionary<int, StationData> stationMap;

    private class ParticleSpawnInfo
    {
        public int spawnedCount;
        public float lastSpawnTime;

        public ParticleSpawnInfo()
        {
            spawnedCount = 0;
            lastSpawnTime = 0f;
        }
    }

    private class StationData
    {
        public Vector2 position;
        public float probability;

        public StationData(Vector2 position, float probability)
        {
            this.position = position;
            this.probability = probability;
        }
    }

    private void Start()
    {
        particleDataList = GameObject.Find("Databases").GetComponent<LoadCSV>().LoadParticleData();
        particleSpawnInfoMap = new Dictionary<int, ParticleSpawnInfo>();
        stationMap = new Dictionary<int, StationData>();
        FillStationMap(); // Fill the stationMap dictionary
    }

    private void Update()
    {
        float currentTime = Time.time;

        foreach (ParticleData particleData in particleDataList)
        {
            if (IsWithinTimeRange(particleData, currentTime) && CanSpawnParticle(particleData))
            {
                SpawnParticle(particleData);
                UpdateSpawnInfo(particleData);
            }
        }
    }

    private bool IsWithinTimeRange(ParticleData particleData, float currentTime)
    {
        return currentTime >= particleData.timeStart && currentTime <= particleData.timeEnd;
    }

    private bool CanSpawnParticle(ParticleData particleData)
    {
        int hashCode = particleData.GetHashCode();

        if (!particleSpawnInfoMap.ContainsKey(hashCode))
        {
            particleSpawnInfoMap[hashCode] = new ParticleSpawnInfo();
        }

        ParticleSpawnInfo spawnInfo = particleSpawnInfoMap[hashCode];

        return spawnInfo.spawnedCount < particleData.amount &&
               Time.time >= spawnInfo.lastSpawnTime + GetSpawnInterval(particleData);
    }

    private void SpawnParticle(ParticleData particleData)
    {
        StationData spawnStation = stationMap[particleData.idSpawn];

        float totalProbability = 0f;
        foreach (KeyValuePair<int, StationData> stationEntry in stationMap)
        {
            totalProbability += stationEntry.Value.probability;
        }

        float randomValue = Random.Range(0f, totalProbability);
        float cumulativeProbability = 0f;
        StationData deathStation = null;

        foreach (KeyValuePair<int, StationData> stationEntry in stationMap)
        {
            cumulativeProbability += stationEntry.Value.probability;
            if (randomValue <= cumulativeProbability)
            {
                deathStation = stationEntry.Value;
                break;
            }
        }

        if (deathStation != null)
        {
            GameObject particleObj = Instantiate(Particle, spawnStation.position, Quaternion.identity);
            ParticleScript particleScript = particleObj.GetComponent<ParticleScript>();
            particleScript.SetParticleData(spawnStation.position, deathStation.position);
        }
    }

    private void UpdateSpawnInfo(ParticleData particleData)
    {
        int hashCode = particleData.GetHashCode();

        if (!particleSpawnInfoMap.ContainsKey(hashCode))
        {
            particleSpawnInfoMap[hashCode] = new ParticleSpawnInfo();
        }

        ParticleSpawnInfo spawnInfo = particleSpawnInfoMap[hashCode];
        spawnInfo.spawnedCount++;
        spawnInfo.lastSpawnTime = Time.time;
    }

    private void FillStationMap()
    {
        // Fill the stationMap dictionary with station IDs, coordinates, and probabilities
        stationMap.Add(1, new StationData(new Vector2(40f, 0f), 2.6f));
        stationMap.Add(2, new StationData(new Vector2(39.42463641f, 6.760032813f), 2.2f));
        stationMap.Add(3, new StationData(new Vector2(37.71509782f, 13.32559179f), 1.9f));
        stationMap.Add(4, new StationData(new Vector2(34.92056453f, 19.50779775f), 1.7f));
        stationMap.Add(5, new StationData(new Vector2(31.12143017f, 25.12879989f), 2.4f));
        stationMap.Add(6, new StationData(new Vector2(26.42698894f, 30.02689221f), 4f));
        stationMap.Add(7, new StationData(new Vector2(20.97229134f, 34.06116551f), 3.9f));
        stationMap.Add(8, new StationData(new Vector2(14.91425911f, 37.11556109f), 1.7f));
        stationMap.Add(9, new StationData(new Vector2(8.4271708f, 39.10220956f), 3.1f));
        stationMap.Add(10, new StationData(new Vector2(1.697648128f, 39.96395865f), 2.3f));
        stationMap.Add(11, new StationData(new Vector2(-5.08071279f, 39.67601741f), 3.4f));
        stationMap.Add(12, new StationData(new Vector2(-11.71291085f, 38.24666939f), 4f));
        stationMap.Add(13, new StationData(new Vector2(-18.00814979f, 35.71703433f), 2.6f));
        stationMap.Add(14, new StationData(new Vector2(-23.78532705f, 32.15988521f), 4.3f));
        stationMap.Add(15, new StationData(new Vector2(-28.87824376f, 27.67755476f), 4.9f));
        stationMap.Add(16, new StationData(new Vector2(-33.14038597f, 22.39899145f), 1.8f));
        stationMap.Add(17, new StationData(new Vector2(-36.44913962f, 16.47604993f), 2.9f));
        stationMap.Add(18, new StationData(new Vector2(-38.70931788f, 10.07912246f), 1.7f));
        stationMap.Add(19, new StationData(new Vector2(-39.85589954f, 3.392236979f), 0.8f));
        stationMap.Add(20, new StationData(new Vector2(-39.85589954f, -3.392236979f), 2f));
        stationMap.Add(21, new StationData(new Vector2(-38.70931788f, -10.07912246f), 1.3f));
        stationMap.Add(22, new StationData(new Vector2(-36.44913962f, -16.47604993f), 1.4f));
        stationMap.Add(23, new StationData(new Vector2(-33.14038597f, -22.39899145f), 2.6f));
        stationMap.Add(24, new StationData(new Vector2(-28.87824376f, -27.67755476f), 1.5f));
        stationMap.Add(25, new StationData(new Vector2(-23.78532705f, -32.15988521f), 3f));
        stationMap.Add(26, new StationData(new Vector2(-18.00814979f, -35.71703433f), 4.5f));
        stationMap.Add(27, new StationData(new Vector2(-11.71291085f, -38.24666939f), 2.4f));
        stationMap.Add(28, new StationData(new Vector2(-5.08071279f, -39.67601741f), 2.8f));
        stationMap.Add(29, new StationData(new Vector2(1.697648128f, -39.96395865f), 2.3f));
        stationMap.Add(30, new StationData(new Vector2(8.4271708f, -39.10220956f), 2.1f));
        stationMap.Add(31, new StationData(new Vector2(14.91425911f, -37.11556109f), 1.7f));
        stationMap.Add(32, new StationData(new Vector2(20.97229134f, -34.06116551f), 5.9f));
        stationMap.Add(33, new StationData(new Vector2(26.42698894f, -30.02689221f), 3.3f));
        stationMap.Add(34, new StationData(new Vector2(31.12143017f, -25.12879989f), 2.8f));
        stationMap.Add(35, new StationData(new Vector2(34.92056453f, -19.50779775f), 2f));
        stationMap.Add(36, new StationData(new Vector2(37.71509782f, -13.32559179f), 3.5f));
        stationMap.Add(37, new StationData(new Vector2(39.42463641f, -6.760032813f), 2.5f));
    }

    private float GetSpawnInterval(ParticleData particleData)
    {
        return (particleData.timeEnd - particleData.timeStart) / particleData.amount;
    }
}

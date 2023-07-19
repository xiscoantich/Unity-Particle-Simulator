using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCSV : MonoBehaviour
{
    public Item blankItem;
    public List<Item> itemDatabase = new List<Item>();

    public List<ParticleData> LoadParticleData()
    {
        // Clear the database
        itemDatabase.Clear();

        // Read CSV file
        List<Dictionary<string, object>> data = CSVReader.Read("DataBike");
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].ContainsKey("time start"))
            {
                float timeStart = float.Parse(data[i]["time start"].ToString());
                float timeEnd = float.Parse(data[i]["time end"].ToString());
                int amount = int.Parse(data[i]["amount"].ToString());
                int idSpawn = int.Parse(data[i]["id spawn"].ToString());
                
                AddItem(timeStart, timeEnd, amount, idSpawn);
            }
        }
        return ConvertItemsToParticleData();
    }

    void AddItem(float timeStart, float timeEnd, int amount, int idSpawn)
    {
        Item tempItem = new Item(blankItem);

        tempItem.timeStart = timeStart;
        tempItem.timeEnd = timeEnd;
        tempItem.amount = amount;
        tempItem.idSpawn = idSpawn;
   
        itemDatabase.Add(tempItem);
    }




    List<ParticleData> ConvertItemsToParticleData()
    {
        List<ParticleData> particleDataList = new List<ParticleData>();

        foreach (Item item in itemDatabase)
        {
            ParticleData particleData = new ParticleData()
            {
                timeStart = item.timeStart,
                timeEnd = item.timeEnd,
                amount = item.amount,
                idSpawn = item.idSpawn,
            };

            particleDataList.Add(particleData);
        }

        return particleDataList;
    }

}
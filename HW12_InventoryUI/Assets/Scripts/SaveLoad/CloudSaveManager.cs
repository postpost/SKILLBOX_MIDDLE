using System.Linq;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityGoogleDrive;
using UnityGoogleDrive.Data;

public class CloudSaveManager: MonoBehaviour 
{
    private EntityManager em;
    private EntityQuery query;

    private void Start()
    {
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        query = em.CreateEntityQuery(typeof(HealthData));
    }

    //LoadToGoogleDriveCloud

    public void SyncToCloud()
    {
        using var entities = query.ToComponentDataArray<HealthData>(Allocator.Temp); //entities data ONLY!
        SaveData data = new SaveData()
        {
            healthDataList = entities.ToArray().ToList()
        };

        string json = JsonUtility.ToJson(data, true);
        GoogleDriveTools.SmartUpload("HealthData.json", json, ()
            =>
        {
            Debug.Log("File \"HealthData.json\" was loaded to Cloud");
        }
        );
    }

    public void SyncFromCloud()
    {
        //טשול פאיכ ס ID
        GoogleDriveFiles.List().Send().OnDone += fileList =>
        {
            var file = fileList.Files.FirstOrDefault(x => x.Name == "HealthData.json" && x.Trashed !=true);
            if (file != null)
            {
                GoogleDriveTools.Download(file.Id, (downloadedFile) =>
                {
                    string jsonContent = System.Text.Encoding.UTF8.GetString(downloadedFile.Content);
                    ApplyJsonToEntity(jsonContent);
                });
            }
            else
                Debug.Log("\"HealthData.json\" was not found on Cloud");
        };
    }

    private void ApplyJsonToEntity(string json)
    {
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        var saveMap = data.healthDataList.ToDictionary(x => x.EntityID);

        using var entities = query.ToEntityArray(Allocator.Temp); //for ID only WITHOUT data

        foreach (var entity in entities)
        {
            int currentId = em.GetComponentData<HealthData>(entity).EntityID;
            if(saveMap.TryGetValue(currentId, out var savedData))
            {
                em.SetComponentData(entity, savedData);
                Debug.Log($"Player {currentId} got health: {savedData.Value}");
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class SaveGameUnils:MonoBehaviour
{
    private EntityManager em;
    private EntityQuery query;

    private void Start()
    {
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        query = em.CreateEntityQuery(typeof(HealthData));
    }
    public async void OnSaveGameClicked()
    {
        try
        {
            await SaveGame();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex, this); //полное описание и цепочка
        }
    }

    public async void OnLoadGameClicked()
    {
        try
        {
            await LoadGame();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex, this);
        }
    }

    private async Awaitable SaveGame()
    { 
        using var healthDataEntities = query.ToComponentDataArray<HealthData>(Allocator.TempJob);
        //сразу выделяет нужный размер
        SaveData data = new SaveData
        {
            healthDataList = new List<HealthData>(healthDataEntities.Length)

        };
        data.healthDataList.AddRange(healthDataEntities); //копирует данные (не лучший вариант для множества данных)
        string path = System.IO.Path.Combine(Application.persistentDataPath, "save.json");
        string tempPath = path + ".tmp";

        await Awaitable.BackgroundThreadAsync(); //уходит в фоновый поток
      

        try
        {
            //switch data to JSON string (data size may be large)
            string json = JsonUtility.ToJson(data, true);
            //HEAVY job (write to the disk)
            System.IO.File.WriteAllText(tempPath, json);

            //atomic save
            if(System.IO.File.Exists(path)) System.IO.File.Replace(tempPath, path, null); //prev saved file
            else System.IO.File.Move(tempPath, path); //first save
        }

            catch (Exception)
        {
            if(System.IO.File.Exists(tempPath)) System.IO.File.Delete(tempPath);
            throw; //back to catch in OnSaveGameClicked
        }

        await Awaitable.MainThreadAsync();
        Debug.Log($"Data was serialized. Path: {Application.persistentDataPath}");
    }

    private async Awaitable LoadGame()
    {
        //path to our saved path
        string path = System.IO.Path.Combine(Application.persistentDataPath, "save.json");
        if(!System.IO.File.Exists(path))
        {
            //Debug.LogWarning("Save file was not found");
            throw new System.IO.FileNotFoundException("LOAD_GAME:Save file was not found");
        }
        //Background launch
        await Awaitable.BackgroundThreadAsync();
        //read text from DISK to RAM
        string json = System.IO.File.ReadAllText(path);
        if(string.IsNullOrEmpty(json))
        {
            throw new Exception("LOAD_GAME: Save file is empty");
        }
        //deserialize to get objects of SaveData
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if(data?.healthDataList == null)
        {
            throw new Exception($"LOAD_GAME:JsonUtility.FromJson<SaveData>(json) is NULL");
        }

        //ECS sync
        var savedMap = data.healthDataList.ToDictionary(x => x.EntityID);

        //back to the Main Thread
        await Awaitable.MainThreadAsync();
        if (!em.World.IsCreated) return;

        using var healthDataEntities= query.ToEntityArray(Allocator.TempJob);

        foreach (Entity entity in healthDataEntities)
        {
            int currentID = em.GetComponentData<HealthData>(entity).EntityID;

            if (savedMap.TryGetValue(currentID, out var savedData))
            {
                em.SetComponentData(entity, savedData);
            }
            else
            {
                Debug.LogWarning($"Current ID {currentID} is not found in saved file: {path}");
                //throw new KeyNotFoundException($"Current ID {currentID} is not found in saved file: {path}");
            }
        }
    }

    public void OpenSavedPath()
    {
        Application.OpenURL(Application.persistentDataPath);
    }


    public void CloudSave()
    {
        var entities = query.ToComponentDataArray<HealthData>(Allocator.Temp);

        SaveData data = new SaveData();
        data.healthDataList.AddRange(entities.ToArray());
        string json = JsonUtility.ToJson(data, true);

        GoogleDriveTools.SmartUpload("TestECS.json", json, () =>
        {
            Debug.Log("File \"TestECS.json\" was loaded to GoogleDrive");
        });
    }
}

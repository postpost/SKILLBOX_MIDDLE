using System;
using System.Collections.Generic;
using System.Text;
using UnityGoogleDrive;
using UnityGoogleDrive.Data;
using UnityEngine;


public static class GoogleDriveTools
{
    public static void GetFileList(Action<List<File>> onLoaded)
    {
        GoogleDriveFiles.List().Send().OnDone += fileList =>
        {
            onLoaded?.Invoke(fileList.Files);
        };
    }

    public static void Updload(string jsonContent, Action<File> OnDone)
    {
        var file = new UnityGoogleDrive.Data.File
        {
            Name = "GameData.json",
            Content = Encoding.UTF8.GetBytes(jsonContent)
        };
        GoogleDriveFiles.Create(file).Send().OnDone += uploadedFile =>
        {
            UnityEngine.Debug.Log("File was saved in GoogleDrive!");
            OnDone?.Invoke(file);
        };
    }

    public static void Download(string fileID, Action<File> onDataReceived)
    {
        GoogleDriveFiles.Download(fileID).Send().OnDone += file =>
        {
            onDataReceived?.Invoke(file);
        };
    }

    //other way to upload
    public static void SmartUpload(string fileName, string jsonContent, Action onDone)
    {
        //find if same file exists
        // Q -- Google request idioma
        var request = GoogleDriveFiles.List();
        request.Q = $"name = '{fileName}' and trashed = false";

        request.Send().OnDone += fileList =>
        {
            if (fileList == null || fileList.Files == null)
            {
                Debug.LogError("Облако прислало пустой ответ. Проверь авторизацию (403 error).");
                return;
            }

            var content = Encoding.UTF8.GetBytes(jsonContent);
            if (fileList.Files.Count > 0 && fileList.Files != null)
            {
                //update ID
                string foundID = fileList.Files[0].Id;
                var updateFile = new File
                {
                    Content = content,
                };
                GoogleDriveFiles.Update(foundID, updateFile).Send().OnDone += _ =>
                {
                    Debug.Log($"File {fileName} was updated");
                    onDone?.Invoke();
                };
            }
            else
            {
                //create new
                var newFile = new File
                {
                    Name = fileName,
                    Content = content
                };
                GoogleDriveFiles.Create(newFile).Send().OnDone += _ =>
                {
                    Debug.Log($"new file {fileName} was created");
                    onDone?.Invoke();
                };
            }
        };
    }
}

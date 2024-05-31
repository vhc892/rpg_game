using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

public class FileDataHandler
{
    private String dataDirPath = "";
    private String dataFileName = "";
    private String secretKey = "";

    public FileDataHandler(string dataDirPath, string dataFileName, string secretKey)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.secretKey = secretKey;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string jsonToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        jsonToLoad = reader.ReadToEnd();
                    }
                }

                GameDataWithMAC gameDataWithMAC = JsonUtility.FromJson<GameDataWithMAC>(jsonToLoad);
                loadedData = gameDataWithMAC.gameData;
            }
            catch (Exception e)
            {
                Debug.LogError("Error occurred when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }


    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);
            string mac = CalculateMAC(dataToStore);  // Calculate MAC

            GameDataWithMAC gameDataWithMAC = new GameDataWithMAC
            {
                gameData = data,
                mac = mac
            };

            string jsonToStore = JsonUtility.ToJson(gameDataWithMAC, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(jsonToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred when trying to save data to file: " + fullPath + "\n" + e);
        }
    }


    private string CalculateMAC(string data)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey))) //secretKey from string to array
        {
            byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hashValue);
        }
    }

    public bool VerifyMAC(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if (File.Exists(fullPath))
        {
            try
            {
                string jsonToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        jsonToLoad = reader.ReadToEnd();
                    }
                }

                GameDataWithMAC gameDataWithMAC = JsonUtility.FromJson<GameDataWithMAC>(jsonToLoad);
                string dataToVerify = JsonUtility.ToJson(gameDataWithMAC.gameData, true);
                string storedMac = gameDataWithMAC.mac;

                string calculatedMac = CalculateMAC(dataToVerify);

                return storedMac == calculatedMac;
            }
            catch (Exception e)
            {
                Debug.LogError("Error occurred when trying to verify MAC: " + fullPath + "\n" + e);
            }
        }
        return false;
    }

}


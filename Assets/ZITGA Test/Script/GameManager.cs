
using UnityEngine;
using System.IO;

public class GameManager : Singleton<GameManager>
{
    public void Save()
    {
        string path = Application.persistentDataPath + "/Map";
        foreach (Stage s in Map.instance.transform.GetComponentsInChildren<Stage>())
        {
            StageData data = new StageData(s.index, s.isLocked, s.rate);
            File.WriteAllText(path+"/"+s.index.ToString()+".json", JsonUtility.ToJson(data));
            Debug.Log("Stage " + s.index+" save path: "+ path + "/" + s.index.ToString() + ".json");
        }
    }

    public void Load()
    {
        Debug.Log("Loading...");
        string path = Application.persistentDataPath + "/Map";
        for(int i=0;i<Map.instance.content.childCount;i++)
        {
            Stage stage = Map.instance.content.GetChild(i).GetComponent<Stage>();
            string s= path + "/" + stage.index.ToString() + ".json";
            if (File.Exists(s))
            {
                StageData data = JsonUtility.FromJson<StageData>(File.ReadAllText(s));
                stage.index = data.index;
                stage.isLocked = data.isLocked;
                stage.rate = data.rate;
                Debug.Log("Loaded Stage " + stage.index + " data");
            }
            else
                Debug.LogWarning("Stage " + stage.index + " data not found");
        }
    }

    protected override void Awake()
    {
        base.Awake();  
        Instantiate(Resources.Load<Map>("Map"), GameObject.Find("Canvas").transform);
        Map.instance.Randomize();
        string path = Application.persistentDataPath + "/Map";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Save();
        }  
        Load();

    }
}

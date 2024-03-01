using Ketra;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : Singleton<UI>
{
    public Button find, auto,menu;
    public TextMeshProUGUI stage;
    public Image popup;

    protected override void Awake()
    {
        base.Awake();
        find.onClick.AddListener(ShowPath);
        auto.onClick.AddListener(Bug.instance.MoveToTarget);
        menu.onClick.AddListener(ToMenu);
        stage.text = "Stage " + MazeGenerator.instance.index;
    }
    private void ToMenu()
    {
       Map.instance.gameObject.SetActive(true);
        Stage s = Map.instance.content.GetChild(Mathf.Clamp(MazeGenerator.instance.index, 0, Map.instance.maxStage)).GetComponent<Stage>();
        Map.instance.content.GetChild(Mathf.Clamp(MazeGenerator.instance.index-1, 0, Map.instance.maxStage)).GetComponent<Stage>().rate=Random.Range(1,4); 
       if(s.isLocked)
        {
            s.isLocked = false;
            GameManager.instance.Save();
            Debug.Log("Unlocked stage " + s.index);
        }
       DestroyImmediate(MazeGenerator.instance.gameObject);
       DestroyImmediate(gameObject);
    }
    void ShowPath()
    {
        MazeGenerator.instance.ShowPath();
    }
}

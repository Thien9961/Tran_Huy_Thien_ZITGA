
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Map : Singleton<Map>
{
    public int star=0;
    public int maxStage;
    public Button reset;
    public TextMeshProUGUI starTxt;
    public Transform content;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        reset.onClick.AddListener(ResetAll);

    }

    public void Randomize()
    {
        GameObject g = Resources.Load<GameObject>("Stage");
        Stage stage;
        var n = Random.Range(1, maxStage + 1);
        for (int i = 1; i <= maxStage; i++)
        {
            stage = Instantiate(g, content).GetComponent<Stage>();
            if (i <= n)
            {
                stage.isLocked = false;
                stage.rate=Random.Range(1, 4);
            }   
            stage.index = i;
        }
    }
    public void ResetAll()
    {
        foreach (Transform t in content)
            if (t != content.GetChild(0))
                t.GetComponent<Stage>().isLocked = true;
            else
                t.GetComponent<Stage>().rate = -1;
        GameManager.instance.Save();
    }

    public void UpdateStar()
    {
        int count = 0;
        foreach (Transform t in content)
            foreach (GameObject star in t.GetComponent<Stage>().stars)
                if (star.activeSelf)
                    count++;
                else break;
        starTxt.text = count.ToString();
    }

}

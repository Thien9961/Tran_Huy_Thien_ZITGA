
using Ketra;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(EventTrigger))]
public class Stage : MonoBehaviour
{
    public int index { get { return int.Parse(name); } set
        {
            if (value == 1)
            {
                name = value.ToString();
                indexTxt.fontSize = 18;
                indexTxt.text = "Tutorial";
            }
            else
            {
                name = value.ToString();
                indexTxt.fontSize = 36;
                indexTxt.text = value.ToString();
            }
        } }
    public TextMeshProUGUI indexTxt;
    public GameObject web;
    public int rate { get
        {
            int i = 0;
            foreach (GameObject g in stars)
                if (g.activeSelf)
                    i++;
                else
                    return i;
            return -1;
        }
        set
        {
            try
            {
                if (isLocked == false)
                {
                    for (int i = 0; i < stars.Length; i++)
                    {
                        if (i < Mathf.Clamp(value, -1, stars.Length))
                        {
                            stars[i].SetActive(true);
                        }
                        else if (stars[i].activeSelf)
                        {
                            stars[i].SetActive(false);
                        }
                    }
                    Map.instance.UpdateStar();
                }
                else
                    throw new StageNotUnlockedException("Stage is not unlocked, cannot rate it.");
            }
            catch(StageNotUnlockedException e)
            {
                Debug.LogWarning(e.Message);
            }

        }
    }
    public GameObject[] stars;
    public bool isLocked 
    { get { return web.activeSelf; } set 
        {
            if (value == true)
                rate = -1;
            web.SetActive(value);
        } 
    }

    public void Enter()
    {
        if (!isLocked)
        {
            Map.instance.gameObject.SetActive(false);
            MazeGenerator newMaze = Instantiate(Resources.Load<GameObject>("Maze")).GetComponent<MazeGenerator>();
            newMaze.Generate(index);
            Instantiate(Resources.Load<GameObject>("UI"),GameObject.Find("Canvas").transform);
        }   
    }
}

public class StageNotUnlockedException : Exception
{
    public StageNotUnlockedException(string msg) : base(msg)
    {

    }
}


using Ketra;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

[RequireComponent(typeof(PolygonCollider2D))]
public class Bug : Singleton<Bug>
{
    public float travelTime = 5;

    public void CompleteStage()
    {
        UI.instance.popup.gameObject.SetActive(true);
    }



    public void MoveToTarget()
    {
        List<Vector3> p = new List<Vector3>();
        foreach (GameObject obj in MazeGenerator.instance.path)
        {
            p.Add(obj.transform.position);
        }
        transform.DOPath(p.ToArray(),travelTime);
        Invoke(nameof(CompleteStage), travelTime);
    }

}

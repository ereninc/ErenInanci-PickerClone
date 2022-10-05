using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadModel : ObjectModel
{
    public int Id;
    [SerializeField] private List<PickableModel> pickables;
    public Transform PickableParent;

    public void OnSpawn(Vector3 pos)
    {
        transform.localPosition = pos;
        this.SetActiveGameObject(true);
    }

    [EditorButton]
    public void E_SetRoadSample() 
    {
        RoadDataModel dataModel = new RoadDataModel();
        dataModel.Pickables = new List<PickableDataModel>();

        PickableModel[] pickables = PickableParent.GetComponentsInChildren<PickableModel>(true);

        for (int i = 0; i < pickables.Length; i++)
        {
            PickableDataModel data = new PickableDataModel();
            data.Position = pickables[i].transform.position;
            dataModel.Pickables.Add(data);
        }

        RoadController controller = FindObjectOfType<RoadController>();
        controller.SampleRoads.Add(dataModel);
    }
}
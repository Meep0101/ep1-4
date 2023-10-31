using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ToolType{
    None,
    Eraser
}


[CreateAssetMenu (fileName = "Tool", menuName = "LevelBuilding/Create Tool")]
public class BuildingTool : BuildingObjectBase
{
    [SerializeField] private ToolType toolType;

    public void Use(Vector3Int position){
        ToolController tc = ToolController.GetInstance();

        switch(toolType){
            case ToolType.Eraser:
                tc.Eraser(position);
                break;
            default:
                    Debug.LogError("Tool Type not set");
                break;
        }
    }
}

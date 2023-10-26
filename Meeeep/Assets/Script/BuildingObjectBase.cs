using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Category {
    Wall,
    Floor
}



[CreateAssetMenu (fileName = "Buildable", menuName = "BuildingObjects/Create Buildable")]
public class BuildingObjectBase : ScriptableObject {
    [SerializeField] BuildingCategory category;
    [SerializeField] UICategory uiCategory;
    [SerializeField] TileBase tileBase;
    [SerializeField] PlaceType placeType;

    public TileBase TileBase {
        get {
            return tileBase;
        }
    }

    public PlaceType PlaceType{
        get {
            return placeType == PlaceType.None ? category.PlaceType : placeType;
        }
    }

    public BuildingCategory Category {
        get {
            return category;
        }
    }

    public UICategory UICategory{
        get {
            return UICategory;
        }
    }

}
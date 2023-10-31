using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

public class BuildingCreator : Singleton<BuildingCreator> {
    [SerializeField] Tilemap previewMap,
    defaultMap;

    //if one of those maps contains a tile at the position, place is not allowed
    [SerializeField] List<Tilemap> forbidPlacingWithMaps;
    
    PlayerInput playerInput;

    TileBase tileBase;
    BuildingObjectBase selectedObj;

    Camera _camera;

    Vector2 mousePos;
    Vector3Int currentGridPosition;
    Vector3Int lastGridPosition;

    bool holdActive;
    Vector3Int holdStartPosition;

    BoundsInt bounds; //contains 4 corners

    protected override void Awake () {
        base.Awake();
        playerInput = new PlayerInput ();
        _camera = Camera.main;
    }

    private void OnEnable () {
        playerInput.Enable ();

        playerInput.Gameplay.MousePosition.performed += OnMouseMove;

        playerInput.Gameplay.MouseLeftClick.performed += OnLeftClick;
        playerInput.Gameplay.MouseLeftClick.started += OnLeftClick;
        playerInput.Gameplay.MouseLeftClick.canceled += OnLeftClick;

        playerInput.Gameplay.MouseRightClick.performed += OnRightClick;


        
    }

    private void OnDisable () {
        playerInput.Disable ();

        playerInput.Gameplay.MousePosition.performed -= OnMouseMove;

        playerInput.Gameplay.MouseLeftClick.performed -= OnLeftClick;
        playerInput.Gameplay.MouseLeftClick.started -= OnLeftClick;
        playerInput.Gameplay.MouseLeftClick.canceled -= OnLeftClick;


        playerInput.Gameplay.MouseRightClick.performed -= OnRightClick;
    }

    private BuildingObjectBase SelectedObj {
        set {
            selectedObj = value;

            tileBase = selectedObj != null ? selectedObj.TileBase : null;

            UpdatePreview ();
        }
    }
    private Tilemap tilemap {
        get {
            if (selectedObj != null && selectedObj.Category != null && selectedObj.Category.Tilemap != null){
                return selectedObj.Category.Tilemap;
            }
            return defaultMap;
        }
    }



    private void Update () {
        // if something is selected - show preview
        if (selectedObj != null) {
            Vector3 pos = _camera.ScreenToWorldPoint (mousePos);
            Vector3Int gridPos = previewMap.WorldToCell (pos);

            if (gridPos != currentGridPosition) {
                lastGridPosition = currentGridPosition;
                currentGridPosition = gridPos;

                UpdatePreview ();

                if (holdActive){
                    HandleDrawing ();
                }
            }
        }
    }

    private void OnMouseMove (InputAction.CallbackContext ctx) {
        mousePos = ctx.ReadValue<Vector2> ();
    }

    private void OnLeftClick (InputAction.CallbackContext ctx) { 
        if (selectedObj != null && !EventSystem.current.IsPointerOverGameObject()){
            if (ctx.phase == InputActionPhase.Started){
                if(ctx.interaction is SlowTapInteraction) {
                    holdActive = true;
                }

                    if(ctx.interaction is TapInteraction){
                        holdStartPosition = currentGridPosition;
                    }
                
                HandleDrawing();
            } else {
                if (ctx.interaction is SlowTapInteraction || ctx.interaction is TapInteraction && ctx.phase == InputActionPhase.Performed){
                    
                    holdActive = false;
                    HandleDrawRelease();
                } else if (ctx.interaction is TapInteraction && ctx.phase == InputActionPhase.Performed){
                    HandleDrawRelease();
                }

            }
        }
        
        //Debug.Log(ctx.interaction + " / " + ctx.phase); //detects taps/clicks

        /*if (selectedObj != null && !EventSystem.current.IsPointerOverGameObject())
        {
            //HandleDrawing ();
        }*/
    }

    private void OnRightClick (InputAction.CallbackContext ctx) {
        SelectedObj = null;
    }

    public void ObjectSelected (BuildingObjectBase obj) {
        SelectedObj = obj;
    }

    private void UpdatePreview () {
        // Remove old tile if existing
        previewMap.SetTile (lastGridPosition, null);

        if (!IsForbidden(currentGridPosition)){
            
        
        // Set current tile to current mouse positions tile
        previewMap.SetTile (currentGridPosition, tileBase);
        }
    }

    private bool IsForbidden(Vector3Int pos) {

        List<BuildingCategory> restrictedCategories = selectedObj.PlacementRestrictions;
        List<Tilemap>restrictedMaps = restrictedCategories.ConvertAll(category => category.Tilemap);

        List<Tilemap> allMaps = forbidPlacingWithMaps.Concat(restrictedMaps).ToList();
        return allMaps.Any(map => {
            return map.HasTile(pos);
        });
    }

    private void HandleDrawing () {
        if (selectedObj != null) {
            switch (selectedObj.PlaceType) {

                case PlaceType.Single:
                default:
                    DrawItem(tilemap, currentGridPosition, tileBase);
                    break;
                case PlaceType.Line:
                LineRenderer();
                    break;
                case PlaceType.Rectangle:
                    RectangleRenderer ();
                    break;    
            }
        }
    }

    private void HandleDrawRelease(){
        if (selectedObj != null) {
            switch (selectedObj.PlaceType) {
                case PlaceType.Line:
                    
                
                case PlaceType.Rectangle:
                    DrawBounds (tilemap);
                    previewMap.ClearAllTiles();
                    break;    
            }
        }

    }

    private void RectangleRenderer() {
        //render preview on UI map
        previewMap.ClearAllTiles ();

        bounds.xMin = currentGridPosition.x < holdStartPosition.x ? currentGridPosition.x : holdStartPosition.x;
        bounds.xMax = currentGridPosition.x > holdStartPosition.x ? currentGridPosition.x : holdStartPosition.x;
        bounds.yMin = currentGridPosition.y < holdStartPosition.y ? currentGridPosition.y : holdStartPosition.y;
        bounds.yMin = currentGridPosition.y > holdStartPosition.y ? currentGridPosition.y : holdStartPosition.y;
    
        DrawBounds (previewMap);
    }

    private void LineRenderer(){
        previewMap.ClearAllTiles();
        float diffX = Mathf.Abs (currentGridPosition.x - holdStartPosition.x);
        float diffY = Mathf.Abs (currentGridPosition.y - holdStartPosition.y);
        
        bool lineIsHorizontal = diffX >= diffY;

        if (lineIsHorizontal){
            bounds.xMin = currentGridPosition.x < holdStartPosition.x ? currentGridPosition.x : holdStartPosition.x;
            bounds.xMax = currentGridPosition.x > holdStartPosition.x ? currentGridPosition.x : holdStartPosition.x;
            bounds.yMin = holdStartPosition.y;
            bounds.yMax = holdStartPosition.y;
            
        }else {
            bounds.xMin = holdStartPosition.x;
            bounds.xMax = holdStartPosition.x;
            bounds.yMin = currentGridPosition.y < holdStartPosition.y ? currentGridPosition.y : holdStartPosition.y;
            bounds.yMax = currentGridPosition.y > holdStartPosition.y ? currentGridPosition.y : holdStartPosition.y;
            
        }
        DrawBounds (previewMap);
    }

    private void DrawBounds (Tilemap map){
        for (int x = bounds.xMin; x <= bounds.xMax; x++ ) {
            for (int y = bounds.yMin; y <= bounds.yMax; y++){
                DrawItem (map, new Vector3Int (x, y, 0), tileBase);
            }
        }
    }

    private void DrawItem(Tilemap map, Vector3Int position, TileBase tileBase) {
       if (selectedObj.GetType() == typeof(BuildingTool)){
            //it is a tool

            BuildingTool tool = (BuildingTool)selectedObj;

            tool.Use(position);

       } else if (!IsForbidden(position)){
        map.SetTile (position, tileBase);
       }
    }

}
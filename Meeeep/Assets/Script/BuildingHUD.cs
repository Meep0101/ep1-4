using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingHUD : Singleton<BuildingHUD>
{
    
    [SerializeField] List<UICategory> categories;
    [SerializeField] Transform wrapperElement;
    [SerializeField] GameObject categoryPrefab;
    [SerializeField] GameObject itemPrefab;

    Dictionary<UICategory, GameObject> uiElements = new Dictionary<UICategory, GameObject>();
    private void Start(){
        BuildUI();

    }

    private void BuildUI(){
    foreach (UICategory cat in categories){
        if (!uiElements.ContainsKey(cat)) {
                // instantiate new entry
                var inst = Instantiate(categoryPrefab, Vector3.zero, Quaternion.identity);
                inst.transform.SetParent(wrapperElement, false);

                uiElements[cat] = inst;
        }

        // set visible name
            Text text = uiElements[cat].GetComponentInChildren<Text>();
            text.text = cat.name;

            // set name in hierarchy
            uiElements[cat].name = cat.name;

            // set index  
            uiElements[cat].transform.SetSiblingIndex(cat.SiblingIndex);

            // set color
            Image img = uiElements[cat].GetComponentInChildren<Image>();
            img.color = cat.BackgroundColor;

    }
    
    }
}


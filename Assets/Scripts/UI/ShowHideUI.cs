using UnityEngine;

public class ShowHideUI : MonoBehaviour
{
    [SerializeField] KeyCode toggleKey = KeyCode.Escape;
    [SerializeField] GameObject uiContainer = null;

     Inventory _inventory;
     InventoryItem _item;
    
    void Start()
    {
        uiContainer.SetActive(false);
    }
    
    void Update()
    {
       
        if (Input.GetKeyDown(toggleKey))
        {
           
            uiContainer.SetActive(!uiContainer.activeSelf);
            
        }
        map();
        
    }

    void map()
    {
        if (Input.GetKeyDown(toggleKey))
        { 
            if (_inventory.HasItem(_item) && _item.GetDisplayName() == "Map")
            {

                uiContainer.SetActive(!uiContainer.activeSelf);

            }
        }
    }
}
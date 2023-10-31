using System.Collections;
using UnityEngine;
public class SavingWrapper : MonoBehaviour
{
    [SerializeField] KeyCode saveKey = KeyCode.S;
    [SerializeField] KeyCode loadKey = KeyCode.L;
    [SerializeField] KeyCode deleteKey = KeyCode.Delete;
    const string defaultSaveFile = "save";
    
    void Awake() 
    {
        StartCoroutine(LoadLastScene());
    }

    IEnumerator LoadLastScene() {
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
    }

    void Update() {
        if (Input.GetKeyDown(saveKey))
        {
            Save();
        }
        if (Input.GetKeyDown(loadKey))
        {
            Load();
        }
        if (Input.GetKeyDown(deleteKey))
        {
            Delete();
        }
    }

    public void Load()
    {
        StartCoroutine(GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile));
    }

    public void Save()
    {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }

    public void Delete()
    {
        GetComponent<SavingSystem>().Delete(defaultSaveFile);
    }
}
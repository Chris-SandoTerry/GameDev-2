using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour
{
    [Serializable]
    public struct CursorMapping
    {
        public CursorType type;
        public Texture2D texture;
        public Vector2 hotspot;
    }

    [SerializeField] CursorMapping[] cursorMappings = null;
    [SerializeField] float raycastRadius = 1f;

    void Update()
    {
        if (InteractWithUI()) return;
        if (InteractWithComponent()) return;

        SetCursor(CursorType.None);
    }

    bool InteractWithUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            SetCursor(CursorType.UI);
            return true;
        }
        return false;
    }

    bool InteractWithComponent()
    {
        RaycastHit[] hits = RaycastAllSorted();
        foreach (RaycastHit hit in hits)
        {
            IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
            foreach (IRaycastable raycastable in raycastables)
            {
                if (raycastable.HandleRaycast(this))
                {
                    SetCursor(raycastable.GetCursorType());
                    return true;
                }
            }
        }
        return false;
    }

    RaycastHit[] RaycastAllSorted()
    {
        RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
        float[] distances = new float[hits.Length];
        for (int i = 0; i < hits.Length; i++)
        {
            distances[i] = hits[i].distance;
        }
        Array.Sort(distances, hits);
        return hits;
    }

    void SetCursor(CursorType type)
    {
        CursorMapping mapping = GetCursorMapping(type);
        Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
    }

    CursorMapping GetCursorMapping(CursorType type)
    {
        foreach (CursorMapping mapping in cursorMappings)
        {
            if (mapping.type == type)
            {
                return mapping;
            }
        }
        return cursorMappings[0];
    }

    static Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}
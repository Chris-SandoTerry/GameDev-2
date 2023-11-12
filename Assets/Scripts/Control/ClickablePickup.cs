using UnityEngine;

[RequireComponent(typeof(Pickup))]
public class ClickablePickup : MonoBehaviour, IRaycastable
{
    Pickup _pickup;
    PlayerInputSystemController _playerInputSystem;

    void Awake()
    {
        _playerInputSystem = FindObjectOfType<PlayerInputSystemController>();
        _pickup = GetComponent<Pickup>();
    }

    public CursorType GetCursorType()
    {
        if (_pickup.CanBePickedUp())
        {
            return CursorType.Pickup;
        }
        else
        {
            return CursorType.FullPickup;
        }
    }

    public bool HandleRaycast(CursorController callingController)
    {
        if (_playerInputSystem._playerInputs.Player.Select.WasPressedThisFrame())
        {
            _pickup.PickupItem();
        }
        return true;
    }
}
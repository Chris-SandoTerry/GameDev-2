public interface IRaycastable
{
    CursorType GetCursorType();
    bool HandleRaycast(CursorController callingController);
}
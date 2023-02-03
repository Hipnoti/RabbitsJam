using UnityEngine;

[CreateAssetMenu(menuName = "Yellow Rhymes/Game Input Settings",
    fileName = "Game Input Settings")]
public class GameInputSettings : ScriptableObject
{
    [Header("Movement")]
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;
}

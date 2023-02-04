using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Yellow Rhymes/Game Input Settings",
    fileName = "Game Input Settings")]
public class GameInputSettings : ScriptableObject
{
    [Header("Player 1")] [FormerlySerializedAs("upKey")]
    public KeyCode playerOneUpKey;
    [FormerlySerializedAs("downKey")] public KeyCode playerOneDownKey;
    [FormerlySerializedAs("leftKey")] public KeyCode playerOneLeftKey;
    [FormerlySerializedAs("rightKey")] public KeyCode playerOneRightKey;
    [FormerlySerializedAs("useKey")] public KeyCode playerOneUseKey;
    public KeyCode playerOneAlternativeUseKey;

    [Header("Player 2")] 
    public KeyCode playerTwoUpKey;
    public KeyCode playerTwoDownKey;
    public KeyCode playerTwoLeftKey;
    public KeyCode playerTwoRightKey;
    public KeyCode playerTwoUseKey;
    public KeyCode playerTwoAlternativeUseKey;
}

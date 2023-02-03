using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private readonly Vector3 upVector = Vector3.forward;
    private readonly Vector3 downVector = Vector3.back;
    private readonly Vector3 leftVector = Vector3.left;
    private readonly Vector3 rightVector = Vector3.right;
    private readonly Vector3 zeroVector = Vector3.zero;
    
    [SerializeField] private GameInputSettings inputSettings;
    [SerializeField] private GeneralGameSettings gameSettings;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (GameHelper.acceptInput)
        {
            Vector3 moveVector = Vector3.zero;
            moveVector += Input.GetKey(inputSettings.upKey) ? upVector : zeroVector;
            moveVector += Input.GetKey(inputSettings.downKey) ? downVector : zeroVector;
            moveVector += Input.GetKey(inputSettings.leftKey) ? leftVector : zeroVector;
            moveVector += Input.GetKey(inputSettings.rightKey) ? rightVector : zeroVector;
            transform.Translate(moveVector * (Time.deltaTime * gameSettings.basePlayerSpeed));
        }
    }
}

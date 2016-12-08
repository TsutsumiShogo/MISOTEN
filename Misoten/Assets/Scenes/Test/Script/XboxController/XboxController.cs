﻿using UnityEngine;
using System.Collections;

public class XboxController : MonoBehaviour
{

    // log arrai
    private static bool[] stickUp = new bool[3];
    private static bool[] stickDown = new bool[3];
    private static bool[] stickLeft = new bool[3];
    private static bool[] stickRight = new bool[3];

    // Use this for initialization
    void Start()
    {

    }

    //---------------------------------------------------------------
    // Get LeftStick Trigger Up
    //---------------------------------------------------------------
    public static bool GetLeftTriggerUp(int playerNo)
    {
        if (!stickUp[playerNo])
        {
            // スティック倒し
            if (GetLeftY(playerNo) <= -1.0f){
                stickUp[playerNo] = true;
                stickDown[playerNo] = false;
                return true;
            }
        }else{
            if (GetLeftY(playerNo) > -1.0f)
                stickUp[playerNo] = false;
        }
        return false;
    }

    //---------------------------------------------------------------
    // Get LeftStick Trigger Down
    //---------------------------------------------------------------
    public static bool GetLeftTriggerDown(int playerNo)
    {
        if (!stickDown[playerNo])
        {
            // スティック倒し
            if (GetLeftY(playerNo) >= 1.0f){
                stickDown[playerNo] = true;
                stickUp[playerNo] = false;
                return true;
            }
        }
        else{
            if (GetLeftY(playerNo) < 1.0f)
                stickDown[playerNo] = false;
        }
        return false;
    }

    //---------------------------------------------------------------
    // Get LeftStick Trigger Left
    //---------------------------------------------------------------
    public static bool GetLeftTriggerLeft(int playerNo)
    {
        float AxisX = GetLeftX(playerNo);
        if (!stickLeft[playerNo])
        {
            // スティック倒し
            if ( AxisX >= 1.0f )
            {
                stickLeft[playerNo] = true;
                stickRight[playerNo] = false;
                return true;
            }
        }
        else
        {
            if ( AxisX < 1.0f)
                stickLeft[playerNo] = false;
        }
        return false;
    }

    //---------------------------------------------------------------
    // Get LeftStick Trigger Right
    //---------------------------------------------------------------
    public static bool GetLeftTriggerRight(int playerNo)
    {
        float AxisX = GetLeftX(playerNo);
        if (!stickRight[playerNo])
        {
            // スティック倒し
            if (AxisX <= -1.0f)
            {
                stickRight[playerNo] = true;
                stickLeft[playerNo] = false;
                return true;
            }
        }
        else
        {
            if (AxisX > -1.0f)
                stickRight[playerNo] = false;
        }
        return false;
    }

    //---------------------------------------------------------------
    // Get LeftStick Axis Y
    //---------------------------------------------------------------
    public static float GetLeftY(int playerNo)
    {
        float AxisY = Input.GetAxis("Joystick" + playerNo.ToString() + "AxisY");
        //Debug.Log(AxisY.ToString());
        return AxisY;
    }

    //---------------------------------------------------------------
    // Get LeftStick Axis X
    //---------------------------------------------------------------
    public static float GetLeftX(int playerNo)
    {
        float AxisX = Input.GetAxis("Joystick" + playerNo.ToString() + "AxisX");
        
        return AxisX;
    }

    //---------------------------------------------------------------
    // Get RightStick Axis Y
    //---------------------------------------------------------------
    public static float GetRightY(int playerNo)
    {
        return Input.GetAxis("Mouse Y");
    }

    //---------------------------------------------------------------
    // Get RightStick Axis X
    //---------------------------------------------------------------
    public static float GetRightX()
    {
        return Input.GetAxis("Mouse X");
    }

    //---------------------------------------------------------------
    // GetButtonA 
    // @arugment : プレイヤーID 1～3
    //---------------------------------------------------------------
    public static bool GetButtonA(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Joystick1Button0))
                {
                  
                    return true;
                }
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.Joystick2Button0))
                {
                    
                    return true;
                }
                    
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Joystick3Button0))
                {
                  
                    return true;
                }
                break;
        }
        return false;
    }
    //---------------------------------------------------------------
    // GetButtonA_All
    // 全コントローラーの入力を検知
    // @arugment : none
    //---------------------------------------------------------------
    public static bool GetButtonA_All()
    {
        if(Input.GetKeyDown(KeyCode.Joystick1Button0))
            return true;
        if(Input.GetKeyDown(KeyCode.Joystick1Button1))
            return true;
        if(Input.GetKeyDown(KeyCode.Joystick1Button2))
            return true;

        return false;
    }
    //---------------------------------------------------------------
    // GetButtonHoldA 
    // @arugment : プレイヤーID 1～3
    //---------------------------------------------------------------
    public static bool GetButtonHoldA(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKey(KeyCode.Joystick1Button0)){
                    return true;
                }
                break;
            case 1:
                if (Input.GetKey(KeyCode.Joystick2Button0)){                  
                    return true;
                }
                break;
            case 2:
                if (Input.GetKey(KeyCode.Joystick3Button0)){                 
                    return true;
                }
                break;
        }
        return false;
    }
    
    
    //---------------------------------------------------------------
    // GetButtonB 
    //---------------------------------------------------------------
    public static bool GetButtonB(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Joystick1Button1))
                    return true;
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.Joystick2Button1))
                    return true;
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Joystick3Button1))
                    return true;
                break;
        }
        return false;
    }
    //---------------------------------------------------------------
    // GetButtonHoldB
    //---------------------------------------------------------------
    public static bool GetButtonHoldB(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKey(KeyCode.Joystick1Button1))
                    return true;
                break;
            case 1:
                if (Input.GetKey(KeyCode.Joystick2Button1))
                    return true;
                break;
            case 2:
                if (Input.GetKey(KeyCode.Joystick3Button1))
                    return true;
                break;
        }
        return false;
    }
    //---------------------------------------------------------------
    // GetButtonX
    //---------------------------------------------------------------
    public static bool GetButtonX(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Joystick1Button2))
                    return true;
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.Joystick2Button2))
                    return true;
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Joystick3Button2))
                    return true;
                break;
        }
        return false;
    }
    //---------------------------------------------------------------
    // GetButtonHoldX
    //---------------------------------------------------------------
    public static bool GetButtonHoldX(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKey(KeyCode.Joystick1Button2))
                    return true;
                break;
            case 1:
                if (Input.GetKey(KeyCode.Joystick2Button2))
                    return true;
                break;
            case 2:
                if (Input.GetKey(KeyCode.Joystick3Button2))
                    return true;
                break;
        }
        return false;
    }
    //---------------------------------------------------------------
    // GetButtonY 
    //---------------------------------------------------------------
    public static bool GetButtonY(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Joystick1Button3))
                    return true;
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.Joystick2Button3))
                    return true;
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Joystick3Button3))
                    return true;
                break;
        }
        return false;
    }
    //---------------------------------------------------------------
    // GetButtonHoldY 
    //---------------------------------------------------------------
    public static bool GetButtonHoldY(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKey(KeyCode.Joystick1Button3))
                    return true;
                break;
            case 1:
                if (Input.GetKey(KeyCode.Joystick2Button3))
                    return true;
                break;
            case 2:
                if (Input.GetKey(KeyCode.Joystick3Button3))
                    return true;
                break;
        }
        return false;
    }
    //---------------------------------------------------------------
    // GetButtonL
    //---------------------------------------------------------------
    public static bool GetButtonL(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Joystick1Button4))
                    return true;
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.Joystick2Button4))
                    return true;
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Joystick3Button4))
                    return true;
                break;
        }
        return false;
    }
    //---------------------------------------------------------------
    // GetButtonHoldL
    //---------------------------------------------------------------
    public static bool GetButtonHoldL(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKey(KeyCode.Joystick1Button4))
                    return true;
                break;
            case 1:
                if (Input.GetKey(KeyCode.Joystick2Button4))
                    return true;
                break;
            case 2:
                if (Input.GetKey(KeyCode.Joystick3Button4))
                    return true;
                break;
        }
        return false;
    }
    //---------------------------------------------------------------
    // GetButtonR
    //---------------------------------------------------------------
    public static bool GetButtonR(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Joystick1Button5))
                    return true;
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.Joystick2Button5))
                    return true;
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Joystick3Button5))
                    return true;
                break;
        }
        return false;
    }
    //---------------------------------------------------------------
    // GetButtonHoldR
    //---------------------------------------------------------------
    public static bool GetButtonHoldR(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKey(KeyCode.Joystick1Button5))
                    return true;
                break;
            case 1:
                if (Input.GetKey(KeyCode.Joystick2Button5))
                    return true;
                break;
            case 2:
                if (Input.GetKey(KeyCode.Joystick3Button5))
                    return true;
                break;
        }
        return false;
    }
    //---------------------------------------------------------------
    // GetButtonBack
    //---------------------------------------------------------------
    public static bool GetButtonBack(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Joystick1Button6))
                    return true;
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.Joystick2Button6))
                    return true;
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Joystick3Button6))
                    return true;
                break;
        }
        return false;
    }
    //---------------------------------------------------------------
    // GetButtonHoldBack
    //---------------------------------------------------------------
    public static bool GetButtonHoldBack(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKey(KeyCode.Joystick1Button6))
                    return true;
                break;
            case 1:
                if (Input.GetKey(KeyCode.Joystick2Button6))
                    return true;
                break;
            case 2:
                if (Input.GetKey(KeyCode.Joystick3Button6))
                    return true;
                break;
        }
        return false;
    }
    //---------------------------------------------------------------
    // GetButtonStart
    //---------------------------------------------------------------
    public static bool GetButtonStart(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Joystick1Button7))
                    return true;
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.Joystick2Button7))
                    return true;
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Joystick3Button7))
                    return true;
                break;
        }
        return false;
    }
    //---------------------------------------------------------------
    // GetButtonHoldStart
    //---------------------------------------------------------------
    public static bool GetButtonHoldStart(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                if (Input.GetKey(KeyCode.Joystick1Button7))
                    return true;
                break;
            case 1:
                if (Input.GetKey(KeyCode.Joystick2Button7))
                    return true;
                break;
            case 2:
                if (Input.GetKey(KeyCode.Joystick3Button7))
                    return true;
                break;
        }
        return false;
    }


}
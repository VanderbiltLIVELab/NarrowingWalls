using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringToFloat : MonoBehaviour {
    private float foutput;

    UnityEngine.TouchScreenKeyboard keyboard;
    public static string keyboardText = "";

    //private CapsuleCollider cc;

    public void changeCapsuleHeight(string sinput)
    {
        foutput = float.Parse(sinput);
        GetComponent<CapsuleCollider>().height = foutput;
    }

    void Start()
    {
        //cc = GetComponent<CapsuleCollider>();
        // Single-line textbox with title
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad, false, false, false, false, "Type in your shoulder width...");
    }

    public void ChangeShoulderWidth ()
    {
        if (TouchScreenKeyboard.visible == false && keyboard != null)
        {
            if (keyboard.status == TouchScreenKeyboard.Status.Done)
            {
                keyboardText = keyboard.text;
                changeCapsuleHeight(keyboardText);
                keyboard = null;
            }
        }
    }
}

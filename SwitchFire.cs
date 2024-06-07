using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchFire : MonoBehaviour
{
    public void switchFire(GameObject fire )
    {
        fire.SetActive( fire.activeSelf );
    }
}

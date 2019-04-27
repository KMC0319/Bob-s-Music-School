using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAnimationTest : MonoBehaviour
{
    SpriteRenderer MainSpriteRenderer;

    public GameObject GameObject;
    public SpriteRenderer spRenderer;

    // Use this for initialization
    void Start()
    {
      

    }

    void TestUiAnimationEvent()
    {
        spRenderer.color = new Color(1,1,1,1);

    }
}
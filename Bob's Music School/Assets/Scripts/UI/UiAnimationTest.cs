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

        GameObject = GameObject.Find("UiAnimationTest2");

        GameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

    }

    void TestUiAnimationEvent()
    {
        spRenderer.color = new Color(1,1,1,1);

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatorIndex : MonoBehaviour {

    Animator animator;
    private float upInterval;
    private float downInterval;
    [SerializeField] private float delaykey = 0.1f;
    private int Nowcursor;
    public int TitleIndex;
    public Text text;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        TitleIndex = 0;
    }

    // Update is called once per frame
    void Update () {
        if (TitleIndex == 0 && Input.GetButtonDown("A"))
        {
            animator.SetInteger("TitleIndex", 1);
            TitleIndex = 1;

        }else if (TitleIndex == 1 && Input.GetButtonDown("A"))
        {
            animator.SetInteger("TitleIndex", 2);
            TitleIndex = 2;

        }

        if (TitleIndex == 1 && Input.GetButtonDown("B"))
        {
            animator.SetInteger("TitleIndex", 0);
            TitleIndex = 0;
            Nowcursor = 0;

        }


            ////////////////////////////////////////////////////////////////////////////////

            if (Input.GetAxisRaw("Vertical") > 0)
        {

            if (upInterval == -0.1f) upInterval = delaykey * 0.9f;
            upInterval += Time.deltaTime;
            if (upInterval > delaykey)
            {
                if (TitleIndex == 1)
                {
                    Nowcursor++;
                }
                  


                upInterval = 0;
            }

        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            if (downInterval == -0.1f) downInterval = delaykey * 0.9f;
            downInterval += Time.deltaTime;
            upInterval = 0;
            if (downInterval > delaykey)
            {
                if (TitleIndex == 1)
                {
                    Nowcursor--;
                }
                downInterval = 0;
            }
        }
        else
        {
            upInterval = -0.1f;
            downInterval = -0.1f;
        }

        if (Nowcursor < -2)
        {
            Nowcursor = -2;
        }
        if (Nowcursor > 2)
        {
            Nowcursor = 2;
        }

        animator.SetInteger("Nowcursor", Nowcursor);

        text.text = "カーソル" + Nowcursor + "深度" + TitleIndex;


    }
}

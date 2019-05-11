using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnimatorIndex : MonoBehaviour {

    Animator animator;
    private float upInterval;
    private float downInterval;
    private float delaykey = 0.18f;
    private int Nowcursor;
    public int TitleIndex;
   // public Text text;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        TitleIndex = 0;
    }

    // Update is called once per frame
    void Update() {
        if (TitleIndex == 0 && Input.GetButtonDown("A"))
        {
            animator.SetInteger("TitleIndex", 1);
            TitleIndex = 1;

        } else if (TitleIndex == 1 && Nowcursor == 0 && Input.GetButtonDown("A"))
        {
            animator.SetInteger("TitleIndex", 2);
            TitleIndex = 2;
            SceneManager.LoadScene("test");

        }
        else if (TitleIndex == 1 && Nowcursor == 1 && Input.GetButtonDown("A"))
        {
            animator.SetInteger("TitleIndex", 3);
            TitleIndex = 3;

        }
      

        if (TitleIndex == 1 && Input.GetButtonDown("B"))
        {
            animator.SetInteger("TitleIndex", 0);
            TitleIndex = 0;
            Nowcursor = 0;

        }
        else if (TitleIndex == 3 && Input.GetButtonDown("B"))
        {
            animator.SetInteger("TitleIndex", 1);
            TitleIndex = 1;
            Nowcursor = 1;

        }


        ////////////////////////////////////////////////////////////////////////////////

        if (Input.GetAxisRaw("Vertical") > 0)
        {

            if (upInterval == -0.1f) upInterval = delaykey * 0.9f;
            upInterval += Time.deltaTime;
            if (upInterval > delaykey)
            {
                if (TitleIndex == 1|| TitleIndex == 3)
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
                if (TitleIndex == 1||TitleIndex == 3)
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

        if(3 == TitleIndex && Nowcursor < -6)
        { 
                Nowcursor = -6;
        }
        else if (3 == TitleIndex && Nowcursor > 1)
        {
            Nowcursor = 1;
        }
        else if (1 == TitleIndex && Nowcursor < -2)
        {
            Nowcursor = -2;
        }else if (1 == TitleIndex && Nowcursor > 2)
        {
            Nowcursor = 2;
        }

        animator.SetInteger("Nowcursor", Nowcursor);

       // text.text = "カーソル" + Nowcursor + "深度" + TitleIndex;


    }
}

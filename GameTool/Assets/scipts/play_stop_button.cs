using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class play_stop_button : MonoBehaviour
{
    Animator ani;

    public void Onclick()
    {
        GameObject G = GameObject.FindGameObjectWithTag("Player");
        ani = G.GetComponent<Animator>();

        if (ani.speed > 0f)
        {
            ani.speed = 0;
        }
        else
        {
            ani.speed = 1;
        }
    }
}

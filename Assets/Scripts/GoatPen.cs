using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatPen : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var goatBack = collision.GetComponent<GoatBack>();
        if (goatBack == null) return;

        goatBack.DetachAllKids();
    }
}

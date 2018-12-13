using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboGridCell : MonoBehaviour
{

    [Header("GridCell Properties")]
    private bool _occupied;

    private SpriteRenderer squareSprite;

    private void Awake()
    {
        _occupied = false;
        squareSprite = GetComponent<SpriteRenderer>();
    }

    public void SetOccupied(bool occupied)
    {
        _occupied = occupied;
    }

    public bool GetOccupied()
    {
        return _occupied;
    }

    public void StartFading()
    {
        //StartCoroutine("Fading");
        Color c = squareSprite.material.color;
        c.a = 0.5f;
        squareSprite.material.color = c;
    }

    public void StopFading()
    {
        //StopCoroutine("Fading");
        ResetColor();
    }
    
    IEnumerator Fading()
    {
        do
        {
            for (float f = 0.05f; f <= 1; f += 0.05f)
            {
                Color c = squareSprite.material.color;
                c.a = f;
                squareSprite.material.color = c;
                yield return new WaitForSeconds(0.05f);
            }

            for (float f = 1f; f >= -0.05f; f -= 0.05f)
            {
                Color c = squareSprite.material.color;
                c.a = f;
                squareSprite.material.color = c;
                yield return new WaitForSeconds(0.05f);
            }
        } while (true);
    }

    private void ResetColor()
    {
        Color c = squareSprite.material.color;
        c.a = 1;
        squareSprite.material.color = c;
    }
}

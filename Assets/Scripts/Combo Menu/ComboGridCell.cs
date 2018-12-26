using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboGridCell : MonoBehaviour { 

    public Transform target;

    [SerializeField]
    private bool _occupied;

    //Offset of the GridCell w.r.t. the Target (MainCamera)
    private Vector3 offset;

    private SpriteRenderer _squareSprite;

    private void Awake()
    {
        _occupied = false;
        _squareSprite = GetComponent<SpriteRenderer>();

        //Set the current Offset w.r.t. the Target (MainCamera)
        offset = target.position - transform.position + Vector3.forward*20;
    }
    

    public void Update()
    {   
        //Update the current position of the Grid given Target (Main Camera) position
        //and offset w.r.t the Target (Main Camera)
        transform.position = target.position + offset;
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
        Color c = _squareSprite.material.color;
        c.a = 0.5f;
        _squareSprite.material.color = c;
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
                Color c = _squareSprite.material.color;
                c.a = f;
                _squareSprite.material.color = c;
                yield return new WaitForSeconds(0.05f);
            }

            for (float f = 1f; f >= -0.05f; f -= 0.05f)
            {
                Color c = _squareSprite.material.color;
                c.a = f;
                _squareSprite.material.color = c;
                yield return new WaitForSeconds(0.05f);
            }
        } while (true);
    }

    private void ResetColor()
    {
        Color c = _squareSprite.material.color;
        c.a = 1;
        _squareSprite.material.color = c;
    }
}

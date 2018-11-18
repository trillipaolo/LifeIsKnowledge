using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    private SpriteRenderer sprite;

	private void Awake () {

        //Initialize 
        sprite = GetComponent<SpriteRenderer>();
        Color c = sprite.material.color;
        c.a = 0f;
        sprite.material.color = c;

        StartCoroutine("FadingSlow");
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void StartFadingEffect()
    {
        StartCoroutine("FadingFast");
    }

    public void StopFadingEffect()
    {
        StopCoroutine("FadingFast");
        ResetColorSprite();
    }

    public void FromFastToSlow ()
    {
        StopCoroutine("FadingFast");
        ResetColorSprite();
        StartCoroutine("FadingSlow");
    }

    public void FromSlowToFast()
    {
        StopCoroutine("FadingSlow");
        ResetColorSprite();
        StartCoroutine("FadingFast");
    }

    private void ResetColorSprite()
    {
        Color c = sprite.material.color;
        c.a = 1f;
        sprite.material.color = c;
    }

    IEnumerator FadingSlow ()
    {
        do
        {
            for (float f = 0.05f; f <= 1; f += 0.05f)
            {
                Color c = sprite.material.color;
                c.a = f;
                sprite.material.color = c;
                yield return new WaitForSeconds(0.05f);
            }

            for (float f = 1f; f >= -0.05f; f -= 0.05f)
            {
                Color c = sprite.material.color;
                c.a = f;
                sprite.material.color = c;
                yield return new WaitForSeconds(0.05f);
            }
        } while (true);
    }

    IEnumerator FadingFast()
    {
        do
        {
            for (float f = 0.1f; f <= 1; f += 0.1f)
            {
                Color c = sprite.material.color;
                c.a = f;
                sprite.material.color = c;
                yield return new WaitForSeconds(0.05f);
            }

            for (float f = 1f; f >= -0.1f; f -= 0.1f)
            {
                Color c = sprite.material.color;
                c.a = f;
                sprite.material.color = c;
                yield return new WaitForSeconds(0.05f);
            }
        } while (true);
    }


}

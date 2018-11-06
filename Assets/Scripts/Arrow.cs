using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public SpriteRenderer sprite;
    public bool active;


	// Use this for initialization
	private void Awake () {
        sprite = GetComponent<SpriteRenderer>();
        Color c = sprite.material.color;
        c.a = 0f;
        sprite.material.color = c;

        FadingEffect();
    }
	
	// Update is called once per frame
	void Update () {
        
	}
    

    IEnumerator Fading ()
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
        } while (active);
    }

    private void FadingEffect()
    {
        if (active)
        {
            StartCoroutine("Fading");
        }
        else
        {
            StopCoroutine("Fading");
            Color c = sprite.material.color;
            c.a = 1f;
            sprite.material.color = c;
        }
    }
}

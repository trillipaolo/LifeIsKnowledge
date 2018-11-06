using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArrow : MonoBehaviour {

    public SpriteRenderer sprite;
    public bool active = true;


	// Use this for initialization
	private void Awake () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FadeEffect()
    {
        sprite = GetComponent<SpriteRenderer>();
        Color c = sprite.material.color;
        c.a = 0f;
        sprite.material.color = c;
    }

    IEnumerator FadeIn ()
    {
        for (float f = 0.05f; f <= 1; f += 0.05f)
        {
            Color c = sprite.material.color;
            c.a = f;
            sprite.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator FadeOut ()
    {
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = sprite.material.color;
            c.a = f;
            sprite.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }
}

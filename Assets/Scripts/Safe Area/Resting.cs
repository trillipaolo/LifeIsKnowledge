using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resting : MonoBehaviour {

    [Header("Joel Reference")]
    public GameObject target;

    [Header("HealthBar Reference")]
    public GameObject healthBar;

    //Joypad booleans
    private bool _stickUp = false;

    //Slider reference
    private Slider _healthSlider;

    public void Start()
    {
        _healthSlider = healthBar.GetComponent<Slider>();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            bool _upInputButton = Input.GetButtonDown("Teleport");
            bool _upInputAxis = Input.GetAxis("Teleport") > 0.75f;

            if ((_upInputButton || _upInputAxis) && !_stickUp)
            {
                Debug.Log("Trying to rest");
                Rest();
            }

            _stickUp = (_upInputButton || _upInputAxis);
        }
    }

    private void Rest()
    {
        transform.Find("Background").gameObject.SetActive(true);
        DisableJoel();
        target.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        Time.timeScale = 0f;
        target.GetComponent<Animator>().SetBool("isSleeping", true);
        StartCoroutine("Heal");
    }

    IEnumerator Heal()
    {
        for (; target.GetComponentInChildren<JoelHealth>().currentHealth < target.GetComponentInChildren<JoelHealth>().joelHealth.health;)
        {
            int tmpValue = target.GetComponentInChildren<JoelHealth>().currentHealth + 10;

            if (tmpValue < target.GetComponentInChildren<JoelHealth>().joelHealth.health)
            {
                target.GetComponentInChildren<JoelHealth>().currentHealth = tmpValue;
                _healthSlider.value = tmpValue;
            }
            else
            {
                target.GetComponentInChildren<JoelHealth>().currentHealth = target.GetComponentInChildren<JoelHealth>().joelHealth.health;
                _healthSlider.value = target.GetComponentInChildren<JoelHealth>().joelHealth.health;
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }

        target.GetComponent<Animator>().SetBool("isSleeping", false);
        Time.timeScale = 1f;
        target.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        EnableJoel();
        transform.Find("Background").gameObject.SetActive(false);
    }

    private void DisableJoel()
    {
        Debug.Log("Disabling Joel");
        target.GetComponent<PlayerInput>().DisableInput();
        target.GetComponent<PlayerPhysics>().DisableMovement();
    }

    private void EnableJoel()
    {
        target.GetComponent<PlayerInput>().EnableInput();
        target.GetComponent<PlayerPhysics>().EnableMovement();
    }
}

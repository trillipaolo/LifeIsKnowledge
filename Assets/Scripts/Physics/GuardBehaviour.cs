using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardBehaviour : EnemyBehaviour
{
    public bool hasArmor = true;
    [HideInInspector] public bool joelSmashes = false;
    private JoelAttack joel;
    private GuardMovement _movementScript;
    public GameObject destroyedArmor;
    private GameObject _armor;

    void Awake()
    {
        _movementScript = transform.GetComponent<GuardMovement>();
        _unlockScript = GameObject.FindWithTag("Player").GetComponentInParent<JoelUnlockCombos>();

        _healthBar = GetComponentInChildren<Slider>();

        if (_healthBar != null)
        {
            _healthBar.maxValue = health;
            _healthBar.value = health;
        }

        //

        joel = GameObject.FindWithTag("Player").GetComponentInParent<JoelAttack>();
    }

    private void Start()
    {
        audioManager = AudioManager.instance;
    }

    private void Update()
    {
        if (joel.smashes)
        {
            joelSmashes = true;
        }
        else
        {
            joelSmashes = false;
        }
    }

    public override void TakeDamage(Collider2D collider, float baseDamage, bool unique, int comboNum, int attackNum)
    {
        if (health > 0)
        {
            float multiplier = GetMultiplier(collider);
            float damage = ComputeDamage(baseDamage, multiplier, unique);
            Color color = ComputeColor(multiplier, unique);
            if (!hasArmor)
            {
                audioManager.Play(enemyHitSound);

                Vector3 textPosition = (Vector3) collider.offset + transform.position;
                FloatingTextController.CreateFloatingText(damage.ToString(), textPosition, color);

                health -= damage;

                if (health <= 0)
                {
                    Debug.Log("Hello");
                    Die();
                }

                _healthBar.value = health;
            }
            else if (joelSmashes)
            {
                Debug.Log("You have smashed Armor");
                audioManager.Play(enemyHitSound);
                hasArmor = false;
                Instantiate(destroyedArmor, transform.position, Quaternion.identity);
                _armor = transform.Find("Armor").gameObject;
                _armor.SetActive(false);
            }
        }
    }
    public override void Die() {
        for (int i = 0; i < colliders.Length; i++) {
            colliders[i].enabled = false;
        }
        _unlockScript.KilledEnemy(enemyType);
        _movementScript.isDead = true;
        _healthBar.GetComponentInChildren<Image>().color = Color.clear;
    }
}
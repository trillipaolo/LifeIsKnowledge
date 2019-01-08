using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardBehaviour : EnemyBehaviour
{
    public bool hasArmor = true;
    [HideInInspector] public bool joelSmashes = false;
    private JoelAttack joel;
    private GuardMovement _movScript;
    public GameObject destroyedArmor;
    private GameObject _armor;

    public string guardHitNoArmorSound = "GuardHitNoArmor";
    public string guardHitWArmorSound = "GuardHitWArmor";

    void Awake()
    {
        _movScript = transform.GetComponent<GuardMovement>();
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
                audioManager.Play(guardHitNoArmorSound);

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
                audioManager.Play(guardHitWArmorSound);
                // Regular Guard Settings
                _movScript.moveSpeed = 4f;
                _movScript.timeBetweenAttacks = 3f;
                hasArmor = false;
                Instantiate(destroyedArmor, transform.position, Quaternion.identity);
                _armor = transform.Find("Armor").gameObject;
                _armor.SetActive(false);
            }
            else
            {
                audioManager.Play(guardHitWArmorSound);
                Vector3 textPosition = (Vector3) collider.offset + transform.position;
                damage = 0f;
                FloatingTextController.CreateFloatingText(damage.ToString(), textPosition, new Color(204,204,204));
            }
        }
    }

    public override void Die()
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        _unlockScript.KilledEnemy(enemyType);
        _movScript.isDead = true;
        _healthBar.GetComponentInChildren<Image>().color = Color.clear;
    }
}
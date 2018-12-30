using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMenuManager : MonoBehaviour {

    [Header("Scriptable Objects: All enemies in the game")]
    public JoelEnemiesKilled enemyList;

    [Header("Scrollbar Menu Properties")]
    public GameObject menuSlider;

    private List<GameObject> _menuSliders;

    private void OnEnable()
    {   
        //Initialize List of the Sliders
        _menuSliders = new List<GameObject>();

        InitializeScrollbarMenu();
    }

    private void OnDisable()
    {   
        //Destroy the List of Enemy in the menuSlider
        foreach (GameObject g in _menuSliders)
        {
            Destroy(g);
        }

        _menuSliders = new List<GameObject>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void InitializeScrollbarMenu()
    {
        for (int i = 0; i < enemyList.enemies.Length; i++)
        {
            GameObject slider = Instantiate(menuSlider) as GameObject;
            slider.SetActive(true);

            SingleEnemySlider singleEnemySlider = slider.GetComponent<SingleEnemySlider>();
            singleEnemySlider.SetEnemyName(enemyList.enemies[i].name);
            singleEnemySlider.SetBarFilling(enemyList.enemies[i].timesUnlockCombo, enemyList.enemies[i].timesKilled);

            slider.transform.SetParent(menuSlider.transform.parent, false);
            _menuSliders.Add(slider);
        }
    }
}

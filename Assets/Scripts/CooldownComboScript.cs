using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownComboScript : MonoBehaviour {

    public GameObject cooldownPrefab;
    public Canvas canvas;

    private JoelAttack _attack;
    private Transform _comboBar;
    private Combo[] _lastCombo;

    // contains values from 0 to 1, if 0 the corresponding combo is unlocked
    private float[] _cooldownCombo = new float[3];
    [SerializeField]
    private float _canvasDistance = 100;

    private GameObject[] _canvasCooldowns = new GameObject[0];
    private Image[] _unlocking;
    private Image[] _unlockedFrame;

    private void Awake() {
        _attack = GetComponent<JoelAttack>();
        _comboBar = canvas.transform.Find("ComboBar");

        InitializeComboCooldown();
    }

    private void InitializeComboCooldown() {
        // initialize _cooldownComboArray
        _cooldownCombo = new float[_attack._lastTimeUsed.Length];
        for (int i = 0; i < _cooldownCombo.Length; i++) {
            _cooldownCombo[i] = 0;
        }

        // destroy old cooldownPrefabs
        for(int i = 0; i < _canvasCooldowns.Length; i++) {
            Destroy(_canvasCooldowns[i]);
        }

        // compute position for new cooldownPrefabs
        float[] xDistance = new float[_cooldownCombo.Length];
        float offset = (xDistance.Length % 2) == 0 ? 0 : -_canvasDistance/2;
        for(int i = 0; i < xDistance.Length; i++) {
            float temp = i - Mathf.Floor(xDistance.Length / 2);
            xDistance[i] = offset + _canvasDistance * temp;
        }

        // add new cooldownPrefabs
        _canvasCooldowns = new GameObject[xDistance.Length];
        _unlocking = new Image[xDistance.Length];
        _unlockedFrame = new Image[xDistance.Length];
        for (int i = 0; i < xDistance.Length; i++) {
            _canvasCooldowns[i] = Instantiate(cooldownPrefab);
            _canvasCooldowns[i].transform.SetParent(_comboBar);

            // set the position
            _canvasCooldowns[i].GetComponent<RectTransform>().localPosition = new Vector2(xDistance[i],0);
            // set the combo image
            _canvasCooldowns[i].transform.Find("ComboTile").GetComponent<Image>().sprite = _attack.joelCombos.combos[i].cooldownImage;
            // set the button image
            _canvasCooldowns[i].transform.Find("FirstKey").GetComponent<Image>().sprite = _attack.joelCombos.combos[i].cooldownKey;

            // set reference to rotating cooldown
            _unlocking[i] = _canvasCooldowns[i].transform.Find("Unlocking").GetComponent<Image>();
            // set reference to unlocked frame
            _unlockedFrame[i] = _canvasCooldowns[i].transform.Find("UnlockedFrame").GetComponent<Image>();
        }
    }
    
    private void Update () {
        if (CombosChanged()) {
            InitializeComboCooldown();
        }

        UpdateComboCooldown();
	}

    private bool CombosChanged() {
        if(_lastCombo == null || _attack.joelCombos.combos == null) {
            _lastCombo = _attack.joelCombos.combos;
            return false;
        }

        if(_attack.joelCombos.combos.Length != _lastCombo.Length) {
            _lastCombo = _attack.joelCombos.combos;
            return false;
        }

        for(int i = 0; i < _attack.joelCombos.combos.Length; i++) {
            if(_attack.joelCombos.combos[i].comboName != _lastCombo[i].comboName) {
                _lastCombo = _attack.joelCombos.combos;
                return false;
            }
        }

        _lastCombo = _attack.joelCombos.combos;
        return true;
    }

    private void UpdateComboCooldown() {
        // update _cooldownCombo values
        if (_cooldownCombo.Length == _attack._lastTimeUsed.Length && _cooldownCombo.Length == _attack.joelCombos.combos.Length) {
            for (int i = 0; i < _cooldownCombo.Length; i++) {
                _cooldownCombo[i] = 1 - Mathf.Min(1,(Time.timeSinceLevelLoad - _attack._lastTimeUsed[i]) / _attack.joelCombos.combos[i].cooldown);
            }
        }

        // show new values in HUD
        for(int i = 0; i < _canvasCooldowns.Length; i++) {
            _unlocking[i].fillAmount = _cooldownCombo[i];

            if(_cooldownCombo[i] == 0) {
                // if combo can be used show frame
                _unlockedFrame[i].enabled = true;
            } else {
                // hide frame
                _unlockedFrame[i].enabled = false;
            }
        }
    }
}

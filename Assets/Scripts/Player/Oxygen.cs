using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour
{
    [SerializeField] private Slider oxygenSlider; // Improve the UI for this later.

    [SerializeField] private float _oxygenTimer = 100;
    [SerializeField] private float _maxOxygen = 100;
    [SerializeField] private float _oxygenDecreaseRate = 5f;
    [SerializeField] private float _oxygenRegenRate = 10f;

    [SerializeField] private DiverController _player;
    private bool _isUnderWater;

    private void Start()
    {
        if (oxygenSlider != null)
        {
            oxygenSlider.maxValue = _maxOxygen;
            oxygenSlider.value = _oxygenTimer;
        }
    }

    void Update()
    {
        UpdateOxygen();
        UpdateOxygenUI();
        CheckIfNoOxygenLeft();
    }

    public void UpdateOxygen()
    {
        _isUnderWater = _player.transform.position.y < _player.waterLevel;

        if (_isUnderWater) // If under water, decrease Oxygen
        {
            _oxygenTimer -= _oxygenDecreaseRate * Time.deltaTime;
            _oxygenTimer = Mathf.Max(_oxygenTimer, _maxOxygen);
        }
        else // If above, increase.
        {
            _oxygenTimer += _oxygenRegenRate * Time.deltaTime;
            _oxygenTimer = Mathf.Min(_oxygenTimer, _maxOxygen);
        }

        Debug.Log("Oxygen left: " + _oxygenTimer);
    }

    public void CheckIfNoOxygenLeft()
    {
        if (_oxygenTimer <= 0)
        {
            // Diver takes damage here.
        }
    }

    private void UpdateOxygenUI()
    {
        if (oxygenSlider != null)
        {
            oxygenSlider.value = _oxygenTimer;
        }
    }
}
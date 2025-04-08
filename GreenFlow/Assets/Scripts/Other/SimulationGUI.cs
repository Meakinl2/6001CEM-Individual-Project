using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimulationGUI : MonoBehaviour
{
    public TextMeshProUGUI vehicleCounter;
    public Button increase1;
    public Button increase10;
    public Button increase100;
    public Button decrease1;
    public Button decrease10;
    public Button decrease100;

    VehicleManager vehicleManager;

    void Start()
    {
        vehicleManager = GetComponent<VehicleManager>();
        UpdateCounter();
        increase1.onClick.AddListener(IncreaseCount1);
        increase10.onClick.AddListener(IncreaseCount10);
        increase100.onClick.AddListener(IncreaseCount100);
        decrease1.onClick.AddListener(DecreaseCount1);
        decrease10.onClick.AddListener(DecreaseCount10);
        decrease100.onClick.AddListener(DecreaseCount100);
    }

    void IncreaseCount1()
    {
        vehicleManager.maxVehicles ++;
        UpdateCounter();
    }

    void IncreaseCount10()
    {
        vehicleManager.maxVehicles += 10;
        UpdateCounter();
    }

    void IncreaseCount100()
    {
        vehicleManager.maxVehicles += 100;
        UpdateCounter();
    }

    void DecreaseCount1()
    {
        if (vehicleManager.maxVehicles < 2) vehicleManager.maxVehicles = 0;
        else vehicleManager.maxVehicles --;
        UpdateCounter();
    }

    void DecreaseCount10()
    {
        if (vehicleManager.maxVehicles < 11) vehicleManager.maxVehicles = 0;
        else vehicleManager.maxVehicles -= 10;
        UpdateCounter();
    }

    void DecreaseCount100()
    {
        if (vehicleManager.maxVehicles < 101) vehicleManager.maxVehicles = 0;
        else vehicleManager.maxVehicles -= 100;
        UpdateCounter();
    }

    void UpdateCounter() 
    {
        vehicleCounter.text = vehicleManager.maxVehicles.ToString();
    }
}

using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject[] images;
    [SerializeField]
    private GameObject sphere;
    [SerializeField]
    private GameObject player1Buttons;
    [SerializeField]
    private GameObject player2Buttons;

    private NetworkVariable<bool> electricImage = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> waterImage = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> airImage = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> fireImage = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> earthImage = new NetworkVariable<bool>(false);

    private ulong cliendID;

    private void Start()
    {
        electricImage.Value = images[0].activeInHierarchy;
        waterImage.Value = images[1].activeInHierarchy;
        airImage.Value = images[2].activeInHierarchy;
        fireImage.Value = images[3].activeInHierarchy;
        earthImage.Value = images[4].activeInHierarchy;
    }

    public override void OnNetworkSpawn()
    {
        cliendID = NetworkManager.Singleton.LocalClientId;

        if (cliendID == 0)
        {
            player2Buttons.SetActive(false);
        }
        else if (cliendID == 1)
        {
            player1Buttons.SetActive(false);
        }
    }

    public void Electric()
    {
        Debug.Log("Electric");
        DisableImagesRpc(0);
        SpawnShereRpc();
    }

    public void Water()
    {
        Debug.Log("Water");
        DisableImagesRpc(1);
        SpawnShereRpc();
    }

    public void Air()
    {
        Debug.Log("Air");
        DisableImagesRpc(2);
        SpawnShereRpc();
    }

    public void Fire()
    {
        Debug.Log("Fire");
        DisableImagesRpc(3);
        SpawnShereRpc();
    }

    public void Earth()
    {
        Debug.Log("Earth");
        DisableImagesRpc(4);
        SpawnShereRpc();
    }

    [Rpc(SendTo.Server)]
    private void DisableImagesRpc(int element)
    {
        electricImage.Value = false;
        waterImage.Value = false;
        airImage.Value = false;
        fireImage.Value = false;
        earthImage.Value = false;

        switch(element)
        {
            case 0:
                electricImage.Value = true;
                break;
            case 1:
                waterImage.Value = true;
                break;
            case 2:
                airImage.Value = true;
                break;
            case 3:
                fireImage.Value = true;
                break;
            case 4:
                earthImage.Value= true;
                break;
            default:
                break;
        }
    }

    [Rpc(SendTo.Server)]
    private void SpawnShereRpc()
    {
        GameObject sphereObject = Instantiate(sphere);
        sphereObject.GetComponent<NetworkObject>().Spawn(true);
    }

    private void OnEnable()
    {
        electricImage.OnValueChanged += ElectricImageChanged;
        waterImage.OnValueChanged += WaterImageChanged;
        airImage.OnValueChanged += AirImageChanged;
        fireImage.OnValueChanged += FireImageChanged;
        earthImage.OnValueChanged += EarthImageChanged;
    }

    private void OnDisable()
    {
        electricImage.OnValueChanged -= ElectricImageChanged;
        waterImage.OnValueChanged -= WaterImageChanged;
        airImage.OnValueChanged -= AirImageChanged;
        fireImage.OnValueChanged -= FireImageChanged;
        earthImage.OnValueChanged -= EarthImageChanged;
    }

    private void ElectricImageChanged(bool previous, bool current)
    {
        images[0].SetActive(current);
    }

    private void WaterImageChanged(bool previous, bool current)
    {
        images[1].SetActive(current);
    }

    private void AirImageChanged(bool previous, bool current)
    {
        images[2].SetActive(current);
    }

    private void FireImageChanged(bool previous, bool current)
    {
        images[3].SetActive(current);
    }

    private void EarthImageChanged(bool previous, bool current)
    {
        images[4].SetActive(current);
    }
}

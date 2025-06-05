using NUnit.Framework;
using System.Collections.Generic;
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

    [SerializeField]
    private List<Button> player1ButtonsList;
    [SerializeField]
    private List<Button> player2ButtonsList;

    private NetworkVariable<bool> electricImage = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> waterImage = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> airImage = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> fireImage = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> earthImage = new NetworkVariable<bool>(false);

    private NetworkVariable<bool> player1Played = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> player2Played = new NetworkVariable<bool>(false);
    private NetworkVariable<ElementPicked> player1Element = new NetworkVariable<ElementPicked>(ElementPicked.None);
    private NetworkVariable<ElementPicked> player2Element = new NetworkVariable<ElementPicked>(ElementPicked.None);

    private NetworkVariable<int> player1Score = new NetworkVariable<int>(0);
    private NetworkVariable<int> player2Score = new NetworkVariable<int>(0);

    private NetworkVariable<int> turn = new NetworkVariable<int>(1);

    private ulong cliendID;

    public enum ElementPicked
    {
        None,
        Electric,
        Water,
        Air,
        Fire,
        Earth
    }

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
        PickElement(ElementPicked.Electric, cliendID);
        SpawnShereRpc();
    }

    public void Water()
    {
        Debug.Log("Water");
        DisableImagesRpc(1);
        PickElement(ElementPicked.Water, cliendID);
        SpawnShereRpc();
    }

    public void Air()
    {
        Debug.Log("Air");
        DisableImagesRpc(2);
        PickElement(ElementPicked.Air, cliendID);
        SpawnShereRpc();
    }

    public void Fire()
    {
        Debug.Log("Fire");
        DisableImagesRpc(3);
        PickElement(ElementPicked.Fire, cliendID);
        SpawnShereRpc();
    }

    public void Earth()
    {
        Debug.Log("Earth");
        DisableImagesRpc(4);
        PickElement(ElementPicked.Earth, cliendID);
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
    private void PickElement(ElementPicked element, ulong player)
    {
        if (player == 0)
        {
            player1Played.Value = true;
            player1Element.Value = element;
            foreach (Button button in player1ButtonsList)
            {
                button.interactable = false;
            }
        }
        else if (player == 1)
        {
            player2Played.Value = true;
            player2Element.Value = element;
            foreach (Button button in player2ButtonsList)
            {
                button.interactable = false;
            }
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
        //Elements Picked
        electricImage.OnValueChanged += ElectricImageChanged;
        waterImage.OnValueChanged += WaterImageChanged;
        airImage.OnValueChanged += AirImageChanged;
        fireImage.OnValueChanged += FireImageChanged;
        earthImage.OnValueChanged += EarthImageChanged;

        //Turn Changed
        turn.OnValueChanged += TurnChanged;
    }

    private void OnDisable()
    {
        //Elements Picked
        electricImage.OnValueChanged -= ElectricImageChanged;
        waterImage.OnValueChanged -= WaterImageChanged;
        airImage.OnValueChanged -= AirImageChanged;
        fireImage.OnValueChanged -= FireImageChanged;
        earthImage.OnValueChanged -= EarthImageChanged;

        //Turn Changed
        turn.OnValueChanged += TurnChanged;
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

    private void TurnChanged(int previous, int current)
    {
        player1Played.Value = false;
        player2Played.Value = false;
        player1Element.Value = ElementPicked.None;
        player2Element.Value = ElementPicked.None;

        foreach (Button button in player1ButtonsList)
        {
            button.interactable = true;
        }

        foreach (Button button in player2ButtonsList)
        {
            button.interactable = true;
        }
    }
}

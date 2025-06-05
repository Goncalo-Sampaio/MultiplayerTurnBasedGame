using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject[] images1;
    [SerializeField]
    private GameObject[] images2;
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

    [SerializeField]
    private GameObject you;
    [SerializeField]
    private GameObject player2;
    [SerializeField]
    private GameObject you2;
    [SerializeField]
    private GameObject player1;
    [SerializeField]
    private GameObject youWon;
    [SerializeField]
    private GameObject youLose;

    [SerializeField]
    private TextMeshProUGUI player1UIScore;
    [SerializeField]
    private TextMeshProUGUI player2UIScore;

    private NetworkVariable<bool> electricImage = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> waterImage = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> airImage = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> fireImage = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> earthImage = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> electricImage2 = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> waterImage2 = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> airImage2 = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> fireImage2 = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> earthImage2 = new NetworkVariable<bool>(false);

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
        electricImage.Value = images1[0].activeInHierarchy;
        waterImage.Value = images1[1].activeInHierarchy;
        airImage.Value = images1[2].activeInHierarchy;
        fireImage.Value = images1[3].activeInHierarchy;
        earthImage.Value = images1[4].activeInHierarchy;
        electricImage2.Value = images2[0].activeInHierarchy;
        waterImage2.Value = images2[1].activeInHierarchy;
        airImage2.Value = images2[2].activeInHierarchy;
        fireImage2.Value = images2[3].activeInHierarchy;
        earthImage2.Value = images2[4].activeInHierarchy;
    }

    public override void OnNetworkSpawn()
    {
        cliendID = NetworkManager.Singleton.LocalClientId;

        if (cliendID == 0)
        {
            player2Buttons.SetActive(false);
            you.SetActive(true);
            player2.SetActive(true);
            you2.SetActive(false);
            player1.SetActive(false);
        }
        else if (cliendID == 1)
        {
            player1Buttons.SetActive(false);
            you.SetActive(false);
            player2.SetActive(false);
            you2.SetActive(true);
            player1.SetActive(true);
        }
    }

    public void Electric()
    {
        Debug.Log("Electric");
        DisableImagesRpc(0, cliendID);
        PickElementRpc(ElementPicked.Electric, cliendID);
        SolveTurnRpc();
        SpawnShereRpc();
    }

    public void Water()
    {
        Debug.Log("Water");
        DisableImagesRpc(1, cliendID);
        PickElementRpc(ElementPicked.Water, cliendID);
        SolveTurnRpc();
        SpawnShereRpc();
    }

    public void Air()
    {
        Debug.Log("Air");
        DisableImagesRpc(2, cliendID);
        PickElementRpc(ElementPicked.Air, cliendID);
        SolveTurnRpc();
        SpawnShereRpc();
    }

    public void Fire()
    {
        Debug.Log("Fire");
        DisableImagesRpc(3, cliendID);
        PickElementRpc(ElementPicked.Fire, cliendID);
        SolveTurnRpc();
        SpawnShereRpc();
    }

    public void Earth()
    {
        Debug.Log("Earth");
        DisableImagesRpc(4, cliendID);
        PickElementRpc(ElementPicked.Earth, cliendID);
        SolveTurnRpc();
        SpawnShereRpc();
    }

    [Rpc(SendTo.Server)]
    private void DisableImagesRpc(int element, ulong player)
    {
        if (player == 0)
        {
            electricImage.Value = false;
            waterImage.Value = false;
            airImage.Value = false;
            fireImage.Value = false;
            earthImage.Value = false;

            switch (element)
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
                    earthImage.Value = true;
                    break;
                default:
                    break;
            }
        }
        else if (player == 1)
        {
            electricImage2.Value = false;
            waterImage2.Value = false;
            airImage2.Value = false;
            fireImage2.Value = false;
            earthImage2.Value = false;

            switch (element)
            {
                case 0:
                    electricImage2.Value = true;
                    break;
                case 1:
                    waterImage2.Value = true;
                    break;
                case 2:
                    airImage2.Value = true;
                    break;
                case 3:
                    fireImage2.Value = true;
                    break;
                case 4:
                    earthImage2.Value = true;
                    break;
                default:
                    break;
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void PickElementRpc(ElementPicked element, ulong player)
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

    [Rpc(SendTo.Server)]
    private void SolveTurnRpc()
    {
        if (player1Played.Value && player2Played.Value)
        {
            CompareChoices();
        }
    }

    private void CompareChoices()
    {
        // Player one picks ELECTRIC
        if (player1Element.Value == ElementPicked.Electric)
        {
            //Player 1 Wins
            if (player2Element.Value == ElementPicked.Water || player2Element.Value == ElementPicked.Air)
            {
                player1Score.Value++;
            }
            //Player 2 Wins
            else if (player2Element.Value == ElementPicked.Fire || player2Element.Value == ElementPicked.Earth)
            {
                player2Score.Value++;
            }
        }
        // Player one picks WATER
        else if (player1Element.Value == ElementPicked.Water)
        {
            //Player 1 Wins
            if (player2Element.Value == ElementPicked.Air || player2Element.Value == ElementPicked.Fire)
            {
                player1Score.Value++;
            }
            //Player 2 Wins
            else if (player2Element.Value == ElementPicked.Earth || player2Element.Value == ElementPicked.Electric)
            {
                player2Score.Value++;
            }
        }
        // Player one picks AIR
        else if (player1Element.Value == ElementPicked.Air)
        {
            //Player 1 Wins
            if (player2Element.Value == ElementPicked.Fire || player2Element.Value == ElementPicked.Earth)
            {
                player1Score.Value++;
            }
            //Player 2 Wins
            else if (player2Element.Value == ElementPicked.Electric || player2Element.Value == ElementPicked.Water)
            {
                player2Score.Value++;
            }
        }
        // Player one picks FIRE
        else if (player1Element.Value == ElementPicked.Fire)
        {
            //Player 1 Wins
            if (player2Element.Value == ElementPicked.Earth || player2Element.Value == ElementPicked.Electric)
            {
                player1Score.Value++;
            }
            //Player 2 Wins
            else if (player2Element.Value == ElementPicked.Water || player2Element.Value == ElementPicked.Air)
            {
                player2Score.Value++;
            }
        }
        // Player one picks EARTH
        else if (player1Element.Value == ElementPicked.Earth)
        {
            //Player 1 Wins
            if (player2Element.Value == ElementPicked.Electric || player2Element.Value == ElementPicked.Water)
            {
                player1Score.Value++;
            }
            //Player 2 Wins
            else if (player2Element.Value == ElementPicked.Air || player2Element.Value == ElementPicked.Fire)
            {
                player2Score.Value++;
            }
        }

        if (player1Score.Value == 10)
        {
            youWon.SetActive(true);
        }
        else if (player2Score.Value == 10)
        {
            youLose.SetActive(true);
        }
        else
        {
            turn.Value++;
        }
    }

    private void OnEnable()
    {
        //Elements Picked
        electricImage.OnValueChanged += ElectricImageChanged;
        waterImage.OnValueChanged += WaterImageChanged;
        airImage.OnValueChanged += AirImageChanged;
        fireImage.OnValueChanged += FireImageChanged;
        earthImage.OnValueChanged += EarthImageChanged;
        electricImage2.OnValueChanged += ElectricImage2Changed;
        waterImage2.OnValueChanged += WaterImage2Changed;
        airImage2.OnValueChanged += AirImage2Changed;
        fireImage2.OnValueChanged += FireImage2Changed;
        earthImage2.OnValueChanged += EarthImage2Changed;

        //Turn Changed
        turn.OnValueChanged += TurnChanged;

        //Updating the scores
        player1Score.OnValueChanged += Player1ScoreChanged;
        player2Score.OnValueChanged += Player2ScoreChanged;
    }

    private void OnDisable()
    {
        //Elements Picked
        electricImage.OnValueChanged -= ElectricImageChanged;
        waterImage.OnValueChanged -= WaterImageChanged;
        airImage.OnValueChanged -= AirImageChanged;
        fireImage.OnValueChanged -= FireImageChanged;
        earthImage.OnValueChanged -= EarthImageChanged;
        electricImage2.OnValueChanged -= ElectricImage2Changed;
        waterImage2.OnValueChanged -= WaterImage2Changed;
        airImage2.OnValueChanged -= AirImage2Changed;
        fireImage2.OnValueChanged -= FireImage2Changed;
        earthImage2.OnValueChanged -= EarthImage2Changed;

        //Turn Changed
        turn.OnValueChanged -= TurnChanged;

        //Updating the scores
        player1Score.OnValueChanged -= Player1ScoreChanged;
        player2Score.OnValueChanged -= Player2ScoreChanged;
    }

    private void ElectricImageChanged(bool previous, bool current)
    {
        images1[0].SetActive(current);
    }

    private void WaterImageChanged(bool previous, bool current)
    {
        images1[1].SetActive(current);
    }

    private void AirImageChanged(bool previous, bool current)
    {
        images1[2].SetActive(current);
    }

    private void FireImageChanged(bool previous, bool current)
    {
        images1[3].SetActive(current);
    }

    private void EarthImageChanged(bool previous, bool current)
    {
        images1[4].SetActive(current);
    }

    private void ElectricImage2Changed(bool previous, bool current)
    {
        images2[0].SetActive(current);
    }

    private void WaterImage2Changed(bool previous, bool current)
    {
        images2[1].SetActive(current);
    }

    private void AirImage2Changed(bool previous, bool current)
    {
        images2[2].SetActive(current);
    }

    private void FireImage2Changed(bool previous, bool current)
    {
        images2[3].SetActive(current);
    }

    private void EarthImage2Changed(bool previous, bool current)
    {
        images2[4].SetActive(current);
    }

    private void TurnChanged(int previous, int current)
    {
        if (cliendID == 0)
        {
            player1Played.Value = false;
            player2Played.Value = false;
            player1Element.Value = ElementPicked.None;
            player2Element.Value = ElementPicked.None;
        }

        foreach (Button button in player1ButtonsList)
        {
            button.interactable = true;
        }

        foreach (Button button in player2ButtonsList)
        {
            button.interactable = true;
        }
    }

    private void Player1ScoreChanged(int previous, int current)
    {
        player1UIScore.text = player1Score.Value.ToString();
    }

    private void Player2ScoreChanged(int previous, int current)
    {
        player2UIScore.text = player2Score.Value.ToString();
    }
}

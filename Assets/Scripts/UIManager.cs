using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject networkButtons;
    [SerializeField]
    private GameObject[] images1;
    [SerializeField]
    private GameObject[] images2;
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

    private NetworkVariable<bool> blockedImage = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> blockedImage2 = new NetworkVariable<bool>(false);

    private NetworkVariable<bool> player1Played = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> player2Played = new NetworkVariable<bool>(false);
    private NetworkVariable<ElementPicked> player1Element = new NetworkVariable<ElementPicked>(ElementPicked.None);
    private NetworkVariable<ElementPicked> player2Element = new NetworkVariable<ElementPicked>(ElementPicked.None);

    private NetworkVariable<int> player1Score = new NetworkVariable<int>(0);
    private NetworkVariable<int> player2Score = new NetworkVariable<int>(0);

    private NetworkVariable<int> turn = new NetworkVariable<int>(1);

    private ulong clientID;

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
        clientID = NetworkManager.Singleton.LocalClientId;

        if (clientID == 0)
        {
            player2Buttons.SetActive(false);
            you.SetActive(true);
            player2.SetActive(true);
            you2.SetActive(false);
            player1.SetActive(false);
        }
        else if (clientID == 1)
        {
            player1Buttons.SetActive(false);
            you.SetActive(false);
            player2.SetActive(false);
            you2.SetActive(true);
            player1.SetActive(true);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Electric()
    {
        Debug.Log("Electric");
        DisableImagesRpc(0, clientID);
        PickElementRpc(ElementPicked.Electric, clientID);
        SolveTurnRpc();
    }

    public void Water()
    {
        Debug.Log("Water");
        DisableImagesRpc(1, clientID);
        PickElementRpc(ElementPicked.Water, clientID);
        SolveTurnRpc();
    }

    public void Air()
    {
        Debug.Log("Air");
        DisableImagesRpc(2, clientID);
        PickElementRpc(ElementPicked.Air, clientID);
        SolveTurnRpc();
    }

    public void Fire()
    {
        Debug.Log("Fire");
        DisableImagesRpc(3, clientID);
        PickElementRpc(ElementPicked.Fire, clientID);
        SolveTurnRpc();
    }

    public void Earth()
    {
        Debug.Log("Earth");
        DisableImagesRpc(4, clientID);
        PickElementRpc(ElementPicked.Earth, clientID);
        SolveTurnRpc();
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
                    blockedImage.Value = true;
                    electricImage.Value = true;
                    break;
                case 1:
                    blockedImage.Value = true;
                    waterImage.Value = true;
                    break;
                case 2:
                    blockedImage.Value = true;
                    airImage.Value = true;
                    break;
                case 3:
                    blockedImage.Value = true;
                    fireImage.Value = true;
                    break;
                case 4:
                    blockedImage.Value = true;
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
                    blockedImage2.Value = true;
                    electricImage2.Value = true;
                    break;
                case 1:
                    blockedImage2.Value = true;
                    waterImage2.Value = true;
                    break;
                case 2:
                    blockedImage2.Value = true;
                    airImage2.Value = true;
                    break;
                case 3:
                    blockedImage2.Value = true;
                    fireImage2.Value = true;
                    break;
                case 4:
                    blockedImage2.Value = true;
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
    private void SolveTurnRpc()
    {
        if (player1Played.Value && player2Played.Value)
        {
            blockedImage.Value = false;
            blockedImage2.Value = false;
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
            FinishGameRpc(true);
        }
        else if (player2Score.Value == 10)
        {
            FinishGameRpc(false);
        }
        else
        {
            StartCoroutine(ShowResultsCoroutine());
        }
    }

    [Rpc(SendTo.Everyone)]
    private void FinishGameRpc(bool player1won)
    {
        if (clientID == 0)
        {
            if (player1won)
            {
                youWon.SetActive(true);
            }
            else
            {
                youLose.SetActive(true);
            }
        }
        else if (clientID == 1)
        {
            if (player1won)
            {
                youLose.SetActive(true);
            }
            else
            {
                youWon.SetActive(true);
            }
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
        blockedImage.OnValueChanged += BlockedImageChanged;
        blockedImage2.OnValueChanged += BlockedImage2Changed;

        //Turn Changed
        turn.OnValueChanged += TurnChanged;

        //Updating the scores
        player1Score.OnValueChanged += Player1ScoreChanged;
        player2Score.OnValueChanged += Player2ScoreChanged;

        player1Played.OnValueChanged += player1PlayedChanged;
        player2Played.OnValueChanged += player2PlayedChanged;
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
        blockedImage.OnValueChanged -= BlockedImageChanged;
        blockedImage2.OnValueChanged -= BlockedImage2Changed;

        //Turn Changed
        turn.OnValueChanged -= TurnChanged;

        //Updating the scores
        player1Score.OnValueChanged -= Player1ScoreChanged;
        player2Score.OnValueChanged -= Player2ScoreChanged;

        player1Played.OnValueChanged -= player1PlayedChanged;
        player2Played.OnValueChanged -= player2PlayedChanged;
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

    private void BlockedImageChanged(bool previous, bool current)
    {
        images1[5].SetActive(current);
    }

    private void BlockedImage2Changed(bool previous, bool current)
    {
        images2[5].SetActive(current);
    }

    private void TurnChanged(int previous, int current)
    {
        if (clientID == 0)
        {
            player1Played.Value = false;
            player2Played.Value = false;
            player1Element.Value = ElementPicked.None;
            player2Element.Value = ElementPicked.None;

            electricImage.Value = false;
            waterImage.Value = false;
            airImage.Value = false;
            fireImage.Value = false;
            earthImage.Value = false;
            electricImage2.Value = false;
            waterImage2.Value = false;
            airImage2.Value = false;
            fireImage2.Value = false;
            earthImage2.Value = false;
        }
    }

    private void player1PlayedChanged(bool previous, bool current)
    {
        if (current)
        {
            foreach (Button button in player1ButtonsList)
            {
                button.interactable = false;
            }
        }
        else
        {
            foreach (Button button in player1ButtonsList)
            {
                button.interactable = true;
            }
        }
    }

    private void player2PlayedChanged(bool previous, bool current)
    {
        if (current)
        {
            foreach (Button button in player2ButtonsList)
            {
                button.interactable = false;
            }
        }
        else
        {
            foreach (Button button in player2ButtonsList)
            {
                button.interactable = true;
            }
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

    private IEnumerator ShowResultsCoroutine()
    {
        yield return new WaitForSeconds(3);
        turn.Value++;
    }
}

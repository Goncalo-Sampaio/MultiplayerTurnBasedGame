using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] images;

    public void Electric()
    {
        Debug.Log("Electric");
        DisableImages();
        images[0].SetActive(true);
    }

    public void Water()
    {
        Debug.Log("Water");
        DisableImages();
        images[1].SetActive(true);
    }

    public void Air()
    {
        Debug.Log("Air");
        DisableImages();
        images[2].SetActive(true);
    }

    public void Fire()
    {
        Debug.Log("Fire");
        DisableImages();
        images[3].SetActive(true);
    }

    public void Earth()
    {
        Debug.Log("Earth");
        DisableImages();
        images[4].SetActive(true);
    }

    private void DisableImages()
    {
        foreach (GameObject image in images)
        {
            image.SetActive(false);
        }
    }
}

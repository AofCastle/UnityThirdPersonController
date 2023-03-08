using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasInfoController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private ThirdPersonShooterController playerController;
    private int currentAmmo, gunAmmo, maxAmmo;
    [SerializeField] private Text currentText, gunText, maxText;
    private void Awake()
    {
        playerController=player.GetComponent<ThirdPersonShooterController>();
        currentAmmo = playerController.GetCurrentAmmo();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasInfoController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private ThirdPersonShooterController playerController;
    private int currentAmmo, gunAmmo, maxAmmo;
    [SerializeField] private TMP_Text currentText, gunText, maxText;
    private void Awake()
    {
        playerController=player.GetComponent<ThirdPersonShooterController>();
        currentAmmo = playerController.GetCurrentAmmo();
        gunAmmo = playerController.GetGunAmmo();
        maxAmmo = playerController.GetMaxAmmo();
        UpdateCanvas();
    }

    private void Update()
    {//No es la forma �ptima de actualizar esto, pero es la m�s r�pida de implementar
        currentAmmo = playerController.GetCurrentAmmo();
        gunAmmo = playerController.GetGunAmmo();
        maxAmmo = playerController.GetMaxAmmo();
        UpdateCanvas();
    }

    private void UpdateCanvas()
    {
        currentText.text = currentAmmo.ToString();
        gunText.text = gunAmmo.ToString();
        maxText.text = maxAmmo.ToString();
    }
}

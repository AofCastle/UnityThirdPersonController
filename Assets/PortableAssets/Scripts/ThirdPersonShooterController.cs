using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;

    [SerializeField] private int maxAmmo, pistolAmmo, rifleAmmo;
    private int currentAmmo, currentWeaponIndex;

    [SerializeField] private LayerMask aimColliderLayerMask = new();

    [SerializeField] private Transform debugRaycastTransform;
    [SerializeField] private GameObject vfxGoodHit, vfxBadHit;

    private StarterAssetsInputs assetsInputs;
    private ThirdPersonController thirdPersonController;

    private void Awake()
    {
        assetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();

        currentAmmo = pistolAmmo;
        currentWeaponIndex = 0;
    }
    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f);
        Transform hitTransform = null;
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugRaycastTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }


        if (assetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
        }

        if (CanShootWeapon())
        {
            if (hitTransform != null)
            {
                //Hit something
                if (hitTransform.GetComponent<BulletTarget>() != null)
                {
                    //Hit hittable
                    Instantiate(vfxGoodHit, debugRaycastTransform.position, Camera.main.transform.rotation);
                }
                else
                {
                    //Did not hit hittable
                    Instantiate(vfxBadHit, debugRaycastTransform.position, Camera.main.transform.rotation);
                }
            }
            else
            {
                //Hit nothing
            }
            currentAmmo--;
            Debug.Log("Ammo after shooting is " + currentAmmo);
        }

        if (assetsInputs.reload)
        {
            assetsInputs.reload = false;
            ReloadWeapon();
        }

        if(assetsInputs.swap)
        {
            assetsInputs.swap = false;
            SwapWeapon();
        }
    }

    private void ReloadWeapon()
    {
        int bulletsToReload;
        switch (currentWeaponIndex)
        {
            case 1:
                bulletsToReload = rifleAmmo;
                break;
            default:
                bulletsToReload = pistolAmmo;
                break;
        }
        if (currentAmmo < bulletsToReload)//Si tienes menos balas que las que el cargador debería tener
        {
            if (bulletsToReload < maxAmmo)//Recarga en función al cargador. El arma aquí no es relevante porque ya se ha decidido
            {
                currentAmmo = bulletsToReload;
                maxAmmo -= bulletsToReload;
            }
            else
            {
                currentAmmo = maxAmmo;
                maxAmmo = 0;
            }
        }
    }

    private void SwapWeapon()
    {
        if (currentWeaponIndex < 1)
        {
            currentWeaponIndex = 1;
        }
        else
        {
            currentWeaponIndex = 0;
        }
        maxAmmo += currentAmmo;
        currentAmmo = 0;
        ReloadWeapon();
    }

    private bool CanShootWeapon()
    {
        if (assetsInputs.shoot)
        {
            if(currentWeaponIndex < 1)
            {
                assetsInputs.shoot = false;
            }
            else
            {
                assetsInputs.shoot = false;
                //TODO: comprobar si sigue pulsado el botón y añadir un temporizador para separar los disparos unas décimas de segundo
            }
            return currentAmmo > 0;
        }
        else
        {
            return false;
        }
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetGunAmmo()
    {
        switch (currentWeaponIndex)
        {
            case 1:
                return rifleAmmo;
            default:
                return pistolAmmo;
        }
    }

    public int GetMaxAmmo()
    {
        return maxAmmo;
    }
}

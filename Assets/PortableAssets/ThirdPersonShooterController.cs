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
    [SerializeField] private LayerMask aimColliderLayerMask = new();

    [SerializeField] private Transform debugRaycastTransform;
    [SerializeField] private GameObject vfxGoodHit, vfxBadHit;

    private StarterAssetsInputs assetsInputs;
    private ThirdPersonController thirdPersonController;

    private void Awake()
    {
        assetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
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

        if (assetsInputs.shoot)
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
            assetsInputs.shoot = false;
        }
    }
}

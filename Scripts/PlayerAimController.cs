using System.Collections;
using UnityEngine;

public class PlayerAimController : MonoBehaviour
{ 
    [SerializeField]
    GameObject _mainCamera;
    [SerializeField]
    GameObject _aimCamera;
    [SerializeField]
    GameObject _aimReticle;

    PlayerShooting _playerShooting;

    void Start()
    {
        _playerShooting = GetComponent<PlayerShooting>();
    }

    void Update()
    {
        if (_playerShooting.IsAiming && !_aimCamera.activeInHierarchy)
        {
            _mainCamera.SetActive(false);
            _aimCamera.SetActive(true);

            //Allow time for the camera to blend before enabling the UI
            StartCoroutine(ShowReticle());
        }
        else if(!_playerShooting.IsAiming && !_mainCamera.activeInHierarchy)
        {
            _mainCamera.SetActive(true);
            _aimCamera.SetActive(false);
            _aimReticle.SetActive(false);
        }       
    }

    IEnumerator ShowReticle()
    {
        yield return new WaitForSeconds(0.25f);
        _aimReticle.SetActive(enabled);
    }
}

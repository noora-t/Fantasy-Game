using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField]
    float fireRate = 1;
    [SerializeField]
    GameObject _arrow;
    [SerializeField]
    GameObject _arrowPrefab;
    [SerializeField]
    GameObject _firePoint;

    Animator _animator;

    float _timer;

    bool _isAiming;

    public bool IsAiming => _isAiming;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _timer += Time.deltaTime;
        
        if (Input.GetMouseButton(1))
        {
            _isAiming = true;
            _arrow.SetActive(true);
            _animator.SetBool("IsAiming", true);

            if (Input.GetMouseButtonDown(0))
            {
                if (_timer >= fireRate) {
                    _timer = 0f;

                    _arrow.SetActive(false);
                    _animator.SetTrigger("Fire");

                    GameObject projectile = Instantiate(_arrowPrefab);
                    projectile.transform.forward = _firePoint.transform.forward;
                    projectile.transform.position = _firePoint.transform.position + _firePoint.transform.forward;
                    projectile.transform.rotation = _firePoint.transform.rotation;
                    projectile.GetComponent<ArrowController>().Fire();
                }
            }
        }
        else 
        {
            _animator.SetBool("IsAiming", false);
            _isAiming = false;
        }        
    }
}

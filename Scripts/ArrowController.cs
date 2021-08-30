using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] float _force = 100;
    [SerializeField] int _damage = 1;
    [SerializeField] AudioSource _arrowInAirSound;
    [SerializeField] AudioSource _arrowHitSound;

    Rigidbody _arrowRigidbody;

    private void Awake()
    {
        _arrowRigidbody = GetComponent<Rigidbody>();
        _arrowRigidbody.centerOfMass = transform.position;
    }

    public void Fire()
    {
        _arrowInAirSound.Play();
        _arrowRigidbody.AddForce(transform.forward * (_force * Random.Range(1.3f, 1.7f)), ForceMode.Impulse);       
    }

    public void OnCollisionEnter(Collision collision)
    {
        _arrowHitSound.Play();

        if (collision.gameObject.name != "Player")
        {
            _arrowRigidbody.isKinematic = true;

            var damageable = collision.gameObject.GetComponent<IDamageable<int>>();
            if (damageable == null)
                return;

            damageable.TakeDamage(_damage);
        }

        GameObject.Destroy(gameObject, 5f);
    }
}

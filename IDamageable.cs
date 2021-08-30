
public interface IDamageable<T>
{
    void TakeDamage(T damageAmount);

    void ModifyHealth(T amount);
}

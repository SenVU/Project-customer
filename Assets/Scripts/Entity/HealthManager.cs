using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] protected float health = 10;
    [SerializeField] bool despawnOnDeath;
    [SerializeField] string runOnDeathFunction;

    public enum DamageSource
    {
        Unknown,
        Wolf,
        Hunger
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead())
        {
            OnDeath(DamageSource.Unknown);
            if (despawnOnDeath) Destroy(gameObject);
            if (runOnDeathFunction != null && runOnDeathFunction != "") SendMessage(runOnDeathFunction);
        }
    }

    public bool IsDead() { return health <= 0; }
    public void Damage(float damage, DamageSource source)
    {
        health -= damage;
        Debug.Log("Damage dealt (" + damage + ") health left (" + health + ") source ("+source+")");
        if(IsDead()) OnDeath(source);
    }
    public void Heal(float heal) { health += heal; }

    protected virtual void OnDeath(DamageSource source) { }
}

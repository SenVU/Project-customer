using UnityEngine;

[RequireComponent(typeof(DeathManager))]
public class PlayerHealthManager : HealthManager
{
    [SerializeField] PseudoDictionary<DamageSource, DeathManager.DeathReason> damageToDeathReason;
    DeathManager deathManager;  
    private void Start()
    {
        deathManager = GetComponent<DeathManager>();
    }

    protected override void OnDeath(DamageSource source)
    {
        deathManager.StartDeathCountdown(damageToDeathReason[source]);
        base.OnDeath(source);
    }
}

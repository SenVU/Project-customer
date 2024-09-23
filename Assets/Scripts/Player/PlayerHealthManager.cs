using UnityEngine;

public class PlayerHealthManager : HealthManager
{
    [SerializeField] PseudoDictionary<DamageSource, DeathManager.DeathReason> damageToDeathReason;
    DeathManager deathManager;  
    private void Start()
    {
        deathManager = GameObject.Find("DeathManagers").GetComponent<DeathManager>();
    }

    protected override void OnDeath(DamageSource source)
    {
        base.OnDeath(source);
    }
}

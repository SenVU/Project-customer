using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] float health = 20;
    [SerializeField] bool despawnOnDeath;

    // Update is called once per frame
    void Update()
    {
        if (despawnOnDeath && IsDead()) Destroy(gameObject);
    }

    public bool IsDead() { return health <= 0; }
    public void Damage(float damage) { health -= damage; }
    public void Heal(float heal) { health += heal; }
}

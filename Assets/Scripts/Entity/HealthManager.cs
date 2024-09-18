using OpenCover.Framework.Model;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] float health = 20;
    [SerializeField] bool despawnOnDeath;
    [SerializeField] string runOnDeathFunction;

    // Update is called once per frame
    void Update()
    {
        if (despawnOnDeath && IsDead()) Destroy(gameObject);
        if (runOnDeathFunction != null && runOnDeathFunction!="") SendMessage(runOnDeathFunction);
    }

    public bool IsDead() { return health <= 0; }
    public void Damage(float damage)
    {
        health -= damage;
        Debug.Log("Damage dealt (" + damage + ") health left (" + health + ")");
    }
    public void Heal(float heal) { health += heal; }
}

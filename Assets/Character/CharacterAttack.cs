using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public float attackPower;
    public float cooldown;
    public float criticalChance;
    public float attackRange;
    public string enemyTag; // Tag musuh yang akan diserang

    private float lastAttackTime;

    private void Start()
    {
        lastAttackTime = -cooldown; // Set nilai awal untuk memungkinkan serangan langsung
    }

    private void Update()
    {
        // Untuk tujuan pengujian, Anda dapat memanggil PerformAttack() ketika suatu input tertentu (misalnya, klik mouse) terdeteksi.
        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }
    }

    public void PerformAttack()
    {
        if (Time.time - lastAttackTime > cooldown)
        {
            lastAttackTime = Time.time;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

            foreach (var hitCollider in hitColliders)
            {
                // Cek apakah objek yang terkena memiliki komponen EnemyHealth dan tag yang sesuai
                if (hitCollider.CompareTag(enemyTag))
                {
                    EnemyHealth enemyHealthComponent = hitCollider.GetComponent<EnemyHealth>();
                    if (enemyHealthComponent != null)
                    {
                        float damage = CalculateDamage();
                        bool isCritical = IsCriticalHit();

                        // Panggil fungsi TakeDamageEnemy pada komponen EnemyHealth
                        enemyHealthComponent.TakeDamageEnemy(damage, isCritical);
                    }
                }
            }
        }
    }

    private float CalculateDamage()
    {
        // Anda dapat mengimplementasikan perhitungan kerusakan yang lebih canggih berdasarkan attackPower, dll.
        return attackPower;
    }

    private bool IsCriticalHit()
    {
        // Periksa apakah serangan menghasilkan pukulan kritis berdasarkan criticalChance
        float randomValue = Random.value;
        return randomValue < criticalChance;
    }
}

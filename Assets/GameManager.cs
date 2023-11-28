using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class Entity
    {
        public string name;
        public int power;
        public GameObject prefab; // Menambahkan prefab sebagai objek Entity
    }

    public List<Entity> characters;
    public List<Entity> enemies;

    private int currentRound = 1;

    void Start()
    {
        StartRound();
    }

    void StartRound()
    {
        // Gabungkan karakter dan musuh menjadi satu list
        List<Entity> allEntities = new List<Entity>(characters);
        allEntities.AddRange(enemies);

        // Urutkan list berdasarkan kekuatan secara descending
        List<Entity> sortedEntities = allEntities.OrderByDescending(entity => entity.power).ToList();

        // List untuk menyimpan urutan pergerakan
        List<string> movementOrder = new List<string>();

        // Loop untuk menentukan urutan pergerakan
        foreach (Entity entity in sortedEntities)
        {
            GameObject entityObject = Instantiate(entity.prefab); // Instantiate objek prefab
            entityObject.name = entity.name; // Beri nama objek sesuai nama karakter/musuh
            // Lakukan sesuatu dengan objek seperti menambahkan ke dalam daftar atau menggerakkan objek
            // ...

            if (characters.Contains(entity))
            {
                movementOrder.Add($"Character {characters.IndexOf(entity) + 1}");
            }
            else
            {
                movementOrder.Add($"Enemy {enemies.IndexOf(entity) + 1}");
            }
        }

        // Tampilkan urutan pergerakan di konsol
        Debug.Log($"Round {currentRound} - Urutan Pergerakan:");
        foreach (string entity in movementOrder)
        {
            Debug.Log(entity);
        }

        // Akhir putaran, lanjutkan ke putaran berikutnya (dalam contoh ini, penambahan 1 ke putaran saat ini)
        currentRound++;
        // Tambahkan logika atau panggil fungsi berikutnya untuk melanjutkan permainan
    }
}

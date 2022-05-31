using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    int enemiesInScene = 0;
    int attackerID;
    [SerializeField] List<GameObject> enemiesList;
    public int AttackerID { get => attackerID; }
    public List<GameObject> EnemiesList { get => enemiesList; set => enemiesList = value; }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public event Action onEnemyAttack;
    public event Action onEnenmyEndAttack;
    public void enemyAttack(int id)
    {
        if(onEnemyAttack != null)
        {
            this.attackerID = id;
            onEnemyAttack();
        }
    }
    public void enemyEndAttack()
    {
        if (onEnenmyEndAttack != null)
        {
            this.attackerID = 0;
            onEnenmyEndAttack();
        }
    }
}

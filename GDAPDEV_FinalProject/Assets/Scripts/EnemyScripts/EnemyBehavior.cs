using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    enum EnemyColor { red = 0, green = 1, blue = 2 };
    public enum Direction { right, left, up, down, forward, back };

    [SerializeField] public Multiplier atkDamage;

    [SerializeField] Material materialR;
    [SerializeField] Material materialG;
    [SerializeField] Material materialB;
    [SerializeField] Renderer enemyRend;

    [SerializeField] public float health = 30.0f;
    [SerializeField] public GameObject turret;
    [SerializeField] EnemyColor enemyColor = EnemyColor.red;
    [SerializeField] public float bulletDamage = 5.0f;
    [SerializeField] float moveSpeed = 5.0f;
    //[SerializeField] float timeAlive = 20.0f;

    [Header("Particle System with Sound")]
    [SerializeField] public ParticleSystem deathParticles;

    Action<EnemyBehavior> action;
    Vector3 moveDir = Vector3.zero;
    //float timerDeath;

    public float Health { get { return health; } }
    void Start()
    {
        InitEnemyData();
        SetEnemyParams();
    }

    public virtual void Update()
    {
        gameObject.transform.position = gameObject.transform.position + (moveDir * moveSpeed * Time.deltaTime);
        //timerDeath -= Time.deltaTime;

        if(health <= 0)
        {
            deathParticles.Play();
            StartCoroutine(Death());

            //added the score after death
           

        }
        /*if(timerDeath <= 0)
        {
            action(this);
        }*/
        if (transform.position.z < 0)
        {
            turret.SetActive(false);
        }
        if (transform.position.z > 75 || transform.position.z < -10 || transform.position.y > 30 || transform.position.y < -15 || transform.position.x > 60 || transform.position.x < -60)
        {
            action(this);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7 && enemyColor == EnemyColor.red)
        {
            health -= bulletDamage * 3;
        }
        else if(collision.gameObject.layer == 8 && enemyColor == EnemyColor.green)
        {
            health -= bulletDamage * 3;
        }
        else if(collision.gameObject.layer == 9 && enemyColor == EnemyColor.blue)
        {
            health -= bulletDamage * 3;
        }
        else
        {
            health -= bulletDamage;
        }
    }

    public void Init(Action<EnemyBehavior> act)
    {
        action = act;
    }

    public void SetEnemyParams()
    {
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                enemyColor = EnemyColor.red;
                enemyRend.material = materialR;
                break;
            case 1:
                enemyColor = EnemyColor.green;
                enemyRend.material = materialG;
                break;
            case 2:
                enemyColor = EnemyColor.blue;
                enemyRend.material = materialB;
                break;
        }
        
        //timerDeath = timeAlive;
    }

    public void ResetHealth()
    {
        health = 30.0f;
    }

    public void MoveToDirection(Direction dir)
    {
        if (dir == Direction.down) moveDir = Vector3.down;
        if (dir == Direction.right) moveDir = Vector3.right;
        if (dir == Direction.left) moveDir = Vector3.left;
        if (dir == Direction.back) moveDir = Vector3.back;
        if (dir == Direction.up) moveDir = Vector3.up;
        if (dir == Direction.forward) moveDir = Vector3.forward;
    }
    public void MoveToDistanceThenTurn()
    {

    }

    public virtual void InitEnemyData()
    {
        List<int> currData = new List<int>();
        currData = GameData.Instance.retrieveCurrentData();
        bulletDamage = bulletDamage + (currData [3] * atkDamage.attackMultiplier);
    }

    public IEnumerator Death()
    {
        yield return new WaitForSeconds(0.1f);
        action(this);
        GameData.Instance.UpdateScore(100); //default scoring
        Debug.Log($"Current Tracker :");

    }
}

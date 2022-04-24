using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] SOAudioStats audioStats;
    NavMeshAgent agent;
    GameObject player;
    Animator animator;
    Transform transform;
    bool canAttack = true;
    bool inState = false;
    int isMovingHash = Animator.StringToHash("isMoving"); // moving hash
    int isLandingHash = Animator.StringToHash("isLanding"); // landing hash
    int rightStepHash = Animator.StringToHash("rightStep");
    int leftStepHash = Animator.StringToHash("leftStep");
    int backStepHash = Animator.StringToHash("backStep");
    int frontStepHash = Animator.StringToHash("frontStep");
    int attackHash = Animator.StringToHash("Attack");

    Vector3 prevPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        EnemyManager.instance.onEnemyAttack += stopAttackingHandler;
        EnemyManager.instance.onEnenmyEndAttack += canAttackHandler;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            lookAtPlayer();
            if(distanceToPlayer() >= 1.5f)
            {
                followPlayerOnBeat();
            }
            else
            {
                // turn off agent control
                //agent.destination = transform.position;
                agent.isStopped = true;
                animator.SetBool(isLandingHash, false);
                animator.SetBool(isMovingHash, false);
                // choose between a right or left side step or an attack if nobody is attacking.
                AttackOrRotate();
            }
        }
    }
    
    void lookAtPlayer()
    {
        //if (!audioStats.canPerformAction)
        //{
            transform.LookAt(new Vector3(player.transform.position.x,transform.position.y, player.transform.position.z));
        //}
    }

    void AttackOrRotate()
    {
        Vector3 pos = Vector3.zero;
        Transform closestEnemy = transform;
        if (EnemyManager.instance.EnemiesList.Count > 1)
        {
            closestEnemy = GetClosestEnemy(this.gameObject, EnemyManager.instance.EnemiesList);
            if (Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(closestEnemy.position.x, closestEnemy.position.z)) < 1)
            {
                Debug.DrawLine(new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z), new Vector3(closestEnemy.position.x, closestEnemy.transform.position.y + 0.5f, closestEnemy.transform.position.z));
            }
                //float pos = AngleDir(transform.forward, new Vector3(closestEnemy.position.x, closestEnemy.transform.position.y + 0.5f, closestEnemy.transform.position.z), transform.up);
            pos = transform.TransformPoint(new Vector3(closestEnemy.position.x, closestEnemy.transform.position.y + 0.5f, closestEnemy.transform.position.z));
        }

        if (!inState)
        {
            int choice = 1;
            if (canAttack)
            {
                choice = Random.Range(0, 20);
            }
            //if (audioStats.onBeat())
            //{
            switch (choice)
                {
                    case 0: // Take Steps
                        inState = true;
                        prevPlayerPosition = player.transform.position;
                        EnemyManager.instance.enemyAttack(this.gameObject.GetInstanceID());
                        animator.SetTrigger(attackHash);
                        StartCoroutine(startAttackAnimation());
                        StartCoroutine(endAttack());
                    break;
                    default: // attack
                             //Debug.Log(" I am "+ this.name + " my closest enemy : " +);
                             // if hotrizontal
                    if (EnemyManager.instance.EnemiesList.Count > 1)
                    {
                        if (Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(closestEnemy.position.x, closestEnemy.position.z)) < 1)
                        {
                            if (Mathf.Abs(pos.x - transform.position.x) < Mathf.Abs(pos.z - transform.position.z))
                            {
                                if (pos.x < 0)
                                {
                                    animator.SetBool(rightStepHash, true);
                                    inState = true;
                                    StartCoroutine(endStep(rightStepHash, 0.5f));
                                }
                                else//right
                                {
                                    animator.SetBool(leftStepHash, true);
                                    inState = true;
                                    StartCoroutine(endStep(leftStepHash, 0.5f));
                                }
                            }
                            else
                            {
                                //Debug.Log(this.name + " " + closestEnemy.name);
                                ////if (Mathf.Abs(pos.z - transform.position.x))
                                ////{
                                /////back
                                if (pos.z < 0) // back
                                {
                                    animator.SetBool(frontStepHash, true);
                                    inState = true;
                                    StartCoroutine(endStep(frontStepHash, 0.5f));
                                }
                                else
                                { //front
                                    animator.SetBool(backStepHash, true);
                                    inState = true;
                                    StartCoroutine(endStep(backStepHash, 0.5f));
                                }
                                //}
                            }
                        }
                        else
                        {
                            if (choice > 10)
                            {
                                animator.SetBool(rightStepHash, true);
                                inState = true;
                                StartCoroutine(endStep(rightStepHash, 0.5f));
                            }
                            else
                            {
                                animator.SetBool(leftStepHash, true);
                                inState = true;
                                StartCoroutine(endStep(leftStepHash, 0.5f));
                            }
                        }
                    }
                    else
                    {
                        if (choice > 10)
                        {
                            animator.SetBool(rightStepHash, true);
                            inState = true;
                            StartCoroutine(endStep(rightStepHash, 0.5f));
                        }
                        else
                        {
                            animator.SetBool(leftStepHash, true);
                            inState = true;
                            StartCoroutine(endStep(leftStepHash, 0.5f));
                        }
                    }
                    break;
                }
            //}
        }
    }
    IEnumerator endStep(int hash, float time)
    {
        yield return new WaitForSeconds(audioStats.BeatsToSeconds(time));
        animator.SetBool(hash, false);
        inState = false;
    }
    IEnumerator endStepAnd(int hash, float time , int scndHash, float scndTime)
    {
        yield return new WaitForSeconds(audioStats.BeatsToSeconds(time));
        animator.SetBool(hash, false);
        animator.SetBool(scndHash, true);
        StartCoroutine(endStep(scndHash, scndTime));
    }
    IEnumerator endAttack()
    {
        yield return new WaitForSeconds(audioStats.BeatsToSeconds(1f));
        //tell others that you are ended the attack
        EnemyManager.instance.enemyEndAttack();
        //and that you are no langer the attacker
        inState = false;
        animator.SetBool(backStepHash, true);
        inState = true;
        StartCoroutine(endStep(backStepHash, 0.5f));
    }
    IEnumerator startAttackAnimation()
    {
        yield return new WaitForSeconds(audioStats.BeatsToSeconds(0.4f));
        DOTween.To(x => transform.position = new Vector3(x, transform.position.y, transform.position.z), transform.position.x, prevPlayerPosition.x, audioStats.BeatsToSeconds(0.25f));
        DOTween.To(x => transform.position = new Vector3(transform.position.x, transform.position.y, x), transform.position.z, prevPlayerPosition.z, audioStats.BeatsToSeconds(0.25f));
    }
    void followPlayerOnBeat()
    {
        if (!inState)
        {
            animator.SetBool(rightStepHash, false);
            animator.SetBool(leftStepHash, false);
            if (agent.velocity.magnitude < 0.15f)
            {
                animator.SetBool(isMovingHash, false);
                // can it land ? based on the position between it and the player.
                if (distanceToPlayer() > 2.5f)
                {
                    animator.SetBool(isLandingHash, true);
                }
                else
                {
                    animator.SetBool(isLandingHash, false);
                }
            }
            else
            {
                if (animator.GetBool(isLandingHash) == true)
                {
                    animator.SetBool(isLandingHash, false);
                }
                animator.SetBool(isMovingHash, true);

            }
            if (distanceToPlayer() > 2.5f)
            {
                if (!audioStats.canPerformAction)
                {
                    agent.destination = player.transform.position;
                    if (agent.isStopped)
                    {
                        agent.isStopped = false;
                    }

                }
                else
                {
                    agent.destination = transform.position;
                    if (!agent.isStopped)
                    {
                        agent.isStopped = true;
                    }
                }
            }
            else
            {
                agent.destination = player.transform.position;
                if (agent.isStopped)
                {
                    agent.isStopped = false;
                }
            }
        }
    }
    float distanceToPlayer()
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }

    void stopAttackingHandler()
    {
        Debug.Log("Me : " + this.gameObject.GetInstanceID() + " , Him : " + EnemyManager.instance.AttackerID);
        if (!(this.gameObject.GetInstanceID() == EnemyManager.instance.AttackerID))
        {
            canAttack = false;
        }
    }
    void canAttackHandler()
    {
        canAttack = true;
    }

    Transform GetClosestEnemy(GameObject self, List<GameObject> enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in enemies)
        {
            if (potentialTarget.GetInstanceID() != self.GetInstanceID())
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget.transform;
                }
            }
        }

        return bestTarget;
    }
    /**
     @return -1 left
            1 right
            0 forward or backward
     */
    
}

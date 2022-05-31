using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class EnemyScript : MonoBehaviour
{
    //[SerializeField] SOGameEvent hitPlayerEvent;
    [SerializeField] SOAudioStats audioStats;
    [SerializeField] public GameObject weaponRef;
    [SerializeField] private float attackLength;
    LinkedList<BufferCollider> collidersList = new LinkedList<BufferCollider>();
    [SerializeField] bool debugTrail = false;
    BoxCollider weaponCollider;
    NavMeshAgent agent;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject hitVfxPrefab;
    GameObject hitVfxInstance = null;
    Animator animator;
    Transform transform;
    [SerializeField] LayerMask hitLayers;
    Damageable combatStats;
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
    private int maxCollidersCount = 10;

    public GameObject Player { get => player; set => player = value; }

    // Start is called before the first frame update
    void Start()
    {
        EnemyManager.instance.onEnemyAttack += stopAttackingHandler;
        EnemyManager.instance.onEnenmyEndAttack += canAttackHandler;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        transform = GetComponent<Transform>();
        weaponCollider = (BoxCollider) weaponRef.GetComponent<Collider>();
        combatStats = GetComponent<Damageable>();

        attackLength = audioStats.BeatsToSeconds(0.25f);
    }

   
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (!inState)
            {
                lookAtPlayer();
                if (distanceToPlayer() >= 1.5f)
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
            if (debugTrail)
            {
                checkTrail();
            }
            //DestroyVfxIfAlive();
        }
    }

    IEnumerator DestroyVfx()
    {
        yield return new WaitForSeconds(audioStats.BeatsToSeconds(1.0f));
        Destroy(hitVfxInstance);
        hitVfxInstance = null;
    }
    private void checkTrail()
    {
        // save left hand collider position
        BufferCollider b = new BufferCollider();
        b.size = weaponCollider.size;
        b.position = weaponCollider.transform.position + weaponCollider.transform.TransformDirection(weaponCollider.center);
        b.rotation = weaponCollider.transform.rotation;

        collidersList.AddFirst(b);

        if(collidersList.Count > maxCollidersCount)
        {
            collidersList.RemoveLast();
        }

        Collider[] hits = Physics.OverlapBox(b.position, b.size / 2, b.rotation, hitLayers, QueryTriggerInteraction.Ignore);

        foreach(Collider hit in hits)
        {
            if (hit.gameObject.activeSelf)
            {
                Damageable damageable = hit.GetComponent<Damageable>();
                if (damageable != null)
                {
                    damageable.ApplyDamage(this.combatStats.Damage, this.gameObject);
                    Transform hitTransform = hit.transform;
                    hitTransform.position = new Vector3(hitTransform.position.x, 0.5f, hitTransform.position.z);
                    hitVfxInstance = Instantiate(hitVfxPrefab, weaponCollider.transform.position, Quaternion.identity);
                    hitVfxInstance.GetComponent<VisualEffect>().Play();
                    StartCoroutine(DestroyVfx());
                }
            }
            break;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        foreach (BufferCollider b in collidersList)
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.TRS(b.position, b.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, b.size);
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
                        startAttack();
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
    // ATTACK
    void startAttack()
    {
        inState = true;
        prevPlayerPosition = player.transform.position;
        EnemyManager.instance.enemyAttack(this.gameObject.GetInstanceID());
        animator.SetTrigger(attackHash);
        StartCoroutine(endAttack());
    }
    public void onAttackAnimationStart()
    {
        DOTween.To(x => transform.position = new Vector3(x, transform.position.y, transform.position.z), transform.position.x, prevPlayerPosition.x, attackLength);
        DOTween.To(x => transform.position = new Vector3(transform.position.x, transform.position.y, x), transform.position.z, prevPlayerPosition.z, attackLength);
        debugTrail = true;
    }
    public void onAttackAnimationEnd()
    {
        debugTrail = false;
    }
    IEnumerator endAttack()
    {
        yield return new WaitForSeconds(0.25f);
        //tell others that you are ended the attack
        EnemyManager.instance.enemyEndAttack();
        //and that you are no langer the attacker
        inState = false;
        //animator.SetBool(backStepHash, true);
        //inState = true;
        //StartCoroutine(endStep(backStepHash, 0.5f));
    }
    // ATTACK END

    //IEnumerator startAttackAnimation()
    //{
    //    yield return new WaitForSeconds(0.25f);
        
    //}
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
    struct BufferCollider
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 size;
    }
}

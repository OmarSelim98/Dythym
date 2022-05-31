using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDamageable : MonoBehaviour,Damageable
{
    [SerializeField]
    private SODamageable _stats;
    private int _currentHealth;
    private int _damage;
    private PlayerStateMachine _player;
    public int CurrentHealth { get => _currentHealth;  set => _currentHealth = value; }
    [SerializeField]
    SODamageable Damageable.Stats => _stats;

    public int Damage => _damage;

    // Start is called before the first frame update
    void Awake()
    {
        _damage = _stats.Damage;
        _currentHealth = _stats.Health;
        _player = GetComponent<PlayerStateMachine>();
    }


    public void ApplyDamage(int damage, GameObject other)
    {
        _player.DisableCharacterController();
        // play damaged animation
        _player.PlayerAnimator.SetInteger(_player.IsDamagedHash, 1);
        _player.IsBeingDamaged = true;
        // apply damage
        _currentHealth -= damage;

        // get attacker direction and look at him
        Vector3 heading = other.transform.position  - _player.gameObject.transform.position;
        Vector3 direction = heading.normalized;

        _player.transform.LookAt(new Vector3(direction.x,0.5f,direction.z));

        Vector3 newPosition = -1 * _player.transform.forward * 4;
        newPosition = new Vector3(newPosition.x, _player.transform.position.y, newPosition.z);

        Debug.DrawLine(_player.transform.position, newPosition);

        // go back a little
        _player.PlayerTransform.DOMove(newPosition, _player.DamageDuration);
        //DOTween.To(()=> _player.transform.position , x => _player.transform.position = x , newPosition, _player.DamageDuration);

        StartCoroutine(StopAttack());
    }

    IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(_player.AudioStats.BeatsToSeconds(_player.DamageDuration));
        _player.EnableCharacterController();
        _player.IsBeingDamaged = false;
        _player.PlayerAnimator.SetInteger(_player.IsDamagedHash, 0);
    }
}

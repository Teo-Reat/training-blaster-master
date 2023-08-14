using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;
    private Player target;

    public event UnityAction<Enemy> OnDeath;
    public Player Target => target;

    public void Init(Player target) => this.target = target;
}
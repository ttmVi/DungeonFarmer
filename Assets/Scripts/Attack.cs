using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Enemies Configuration")]
    [SerializeField] private string[] objectiveTags;
    [SerializeField] private float damagePower;

    public virtual void Attacking(GameObject target, float damage)
    {
        if (objectiveTags.Contains(target.tag))
        {
            target.GetComponent<Health>().IsAttacked(damage);
        }
    }
}

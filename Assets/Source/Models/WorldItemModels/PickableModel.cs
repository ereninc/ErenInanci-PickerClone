using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableModel : ObjectModel
{
    [SerializeField] private Rigidbody rigidb;

    public void OnSpawn(Vector3 position) 
    {
        restartPhysicValues();
        transform.position = position;
        transform.rotation = Quaternion.identity;
        this.SetActiveGameObject(true);
    }

    public void OnEnterPassArea()
    {
        ParticlesController.SetParticle(0, transform.position);
        this.SetActiveGameObject(false);
    }

    private void restartPhysicValues()
    {
        rigidb.velocity = Vector3.zero;
        rigidb.angularVelocity = Vector3.zero;
    }
}
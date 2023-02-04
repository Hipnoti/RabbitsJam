using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CannonEntity : GameEntity
{
  
    
    public ParticleSystem conffetiParticle;
    public BunnyController bunnyPrefab;
    public Transform bunnySpawnPoint;
    
    public void LaunchBunny()
    {
        conffetiParticle.Play();
        BunnyController instadBunny = Instantiate(bunnyPrefab, bunnySpawnPoint.position, quaternion.identity);
        instadBunny.navMeshAgent.enabled = false;
        instadBunny.enabled = false;
        instadBunny.canvasObject.SetActive(false);
        Destroy(instadBunny);
        instadBunny.tag = "Untagged";

        Rigidbody rb = instadBunny.GetComponent<Rigidbody>();
        rb.isKinematic = false;        
        rb.AddForce(bunnySpawnPoint.forward * 1500f);

        ToggleOutline(false);
        //  rb.AddTorque(instadBunny.transform.forward * 500f);
    }
}

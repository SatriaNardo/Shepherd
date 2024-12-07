using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BarnManager : MonoBehaviour
{
    [SerializeField] private Transform SuctionPointA;
    [SerializeField] private Transform SuctionPointB;
    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip sheepSound;
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Sheep"))
        {
            collision.GetComponent<NavMeshAgent>().enabled = false;
            StartCoroutine(sheepGoIn(collision.transform));
        
        }
    }
    private IEnumerator sheepGoIn(Transform sheep)
    {
        float suctionSpeed = 1f;
        float t = 0;
        float yPosition = -0.5f;
        Vector3 initialPosition = new Vector3(sheep.position.x, yPosition, sheep.position.z);
        Vector3 targetPosition = new Vector3 (SuctionPointA.position.x, yPosition, SuctionPointA.position.z);

        while(t < 1f)
        {
            t += Time.deltaTime * suctionSpeed;
            sheep.position = Vector3.Lerp(initialPosition, targetPosition, t);
            
            yield return null;
        }
        t = 0;
        initialPosition = new Vector3(sheep.position.x, yPosition, sheep.position.z);
        targetPosition = new Vector3(SuctionPointB.position.x, yPosition, SuctionPointB.position.z);
        while (t < 1f)
        {
            t += Time.deltaTime * (suctionSpeed * 2);
            sheep.position = Vector3.Lerp(initialPosition, targetPosition, t);
            
            yield return null;
        }
        audioSource.PlayOneShot(sheepSound);
        sheep.gameObject.SetActive(false);
        
    }
}

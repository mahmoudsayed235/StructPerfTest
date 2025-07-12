using UnityEngine;
using System.Collections.Generic;

public class ActorContainer : MonoBehaviour
{
    public ActorBehavior ActorPrefab;
    public int TotalActors = 10000;


    private List<ActorBehavior> createdActors= new List<ActorBehavior>();
    [SerializeField]
    private DebugOverlay debugOverlay;

    void Start()
    {
        for (int i = 0; i < TotalActors; i++)
        {
            var actor = Instantiate(ActorPrefab);
            createdActors.Add(actor);
            actor.transform.position = Random.insideUnitSphere * 100f;

            if (i % 3 == 0)
                actor.Type = ActorBehavior.ActorType.GemHunter;
            else if (i % 3 == 1)
            {
                actor.Type = ActorBehavior.ActorType.ColorHunter;
                actor.DesiredColor = "Blue"; 
            }
            else
                actor.Type = ActorBehavior.ActorType.MarbleHunter;
        }
        debugOverlay.SetActors(createdActors);
    }
}

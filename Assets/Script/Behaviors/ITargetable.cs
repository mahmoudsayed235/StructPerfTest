using UnityEngine;

public interface ITargetable
{
    bool WasClaimed { get; }
    Vector3 Position { get; }
    bool IsChosen { get; set; }
    string TypeTag { get; }   
    string ColorTag { get; }  
    void Claim();
}

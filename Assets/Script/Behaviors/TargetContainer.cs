using System.Collections.Generic;
using UnityEngine;

public class TargetContainer : MonoBehaviour
{
    public static TargetContainer Instance { get; private set; }

    private readonly Dictionary<string, List<ITargetable>> _targetsByTag = new();


    void Awake()
    {
        Instance = this;
    }
    public void RegisterTarget(ITargetable target)
    {
        string key = target.TypeTag ?? target.ColorTag;
        if (!_targetsByTag.ContainsKey(key))
            _targetsByTag[key] = new List<ITargetable>();

        _targetsByTag[key].Add(target);
    }
 

    public void UnregisterTarget(ITargetable target)
    {
        target.IsChosen = false;

        string key = target.TypeTag ?? target.ColorTag;
        if (_targetsByTag.TryGetValue(key, out var list))
        {
            list.Remove(target);

            if (list.Count == 0)
            {
                _targetsByTag.Remove(key);
            }
        }
    }

    public ITargetable GetClosestTarget(Vector3 position, string filter = null)
    {
        ITargetable closest = null;
        float closestDist = float.MaxValue;

        if (!_targetsByTag.TryGetValue(filter ?? "", out var candidates)) return null;

        foreach (var target in candidates)
        {
            if (target.WasClaimed || target.IsChosen) continue;

            float dist = (target.Position - position).sqrMagnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = target;
            }
        }

        if (closest != null)
            closest.IsChosen = true;

        return closest;
    }


}

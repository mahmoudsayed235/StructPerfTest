using UnityEngine;

public class ActorBehavior : MonoBehaviour
{
    public enum ActorType
    {
        MarbleHunter,
        GemHunter,
        ColorHunter
    }

    public ActorType Type;
    public string DesiredColor;

    private enum State { Idle, Hunting }
    private State _state = State.Idle;

    private ITargetable _target;

    private const float MoveSpeed = 10f;
    private const float ClaimDistance = 0.1f;

    private float searchCooldown = 0f;
    private const float SearchInterval = 0.25f;
    public void MeasureUpdate()
    {
        if (_state == State.Idle)
        {
            if (searchCooldown <= 0f)
            {
                AcquireTarget();
                searchCooldown = SearchInterval;
            }
            else
            {
                searchCooldown -= Time.deltaTime;
            }
        }
        else if (_state == State.Hunting)
        {
            MoveToTarget();
        }
    }


    private string GetFilter()
    {
        return Type switch
        {
            ActorType.MarbleHunter => "Marble",
            ActorType.GemHunter => "Gem",
            ActorType.ColorHunter => DesiredColor,
            _ => null
        };
    }
    public bool isCatch = false;
    void AcquireTarget()
    {
        _target = TargetContainer.Instance.GetClosestTarget(transform.position, GetFilter());

        if (_target != null)
        {
            _state = State.Hunting;
        }
    }

    private float reacquireCooldown = 0f;
    private const float ReacquireDelay = 0.25f; 

    void MoveToTarget()
    {
        if (_target == null || _target.WasClaimed)
        {
            if (reacquireCooldown <= 0f)
            {
                _target = TargetContainer.Instance.GetClosestTarget(transform.position, GetFilter());
                reacquireCooldown = ReacquireDelay;

                if (_target == null)
                {
                    _state = State.Idle;
                    return;
                }
            }
            else
            {
                reacquireCooldown -= Time.deltaTime;
                return; 
            }
        }

        Vector3 direction = _target.Position - transform.position;
        float distSqr = direction.sqrMagnitude;
        float claimDistSqr = ClaimDistance * ClaimDistance;

        if (distSqr < claimDistSqr)
        {
            _target.Claim();
            _target = null;
            return;
        }
        transform.position += (direction / Mathf.Sqrt(distSqr)) * GameConfig.Instance.ActorSpeed * Time.deltaTime;

    }
}

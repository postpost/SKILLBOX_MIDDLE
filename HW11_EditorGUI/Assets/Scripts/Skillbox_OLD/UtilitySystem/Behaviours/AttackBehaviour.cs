using DG.Tweening;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class AttackBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] private float attackDistance = 5f; //не ставить в 1f!
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private int jumpNum = 4;
    [SerializeField] private float jumpDuration = 0.7f;
    [SerializeField] private float attackCooldown = 1.5f;

    [SerializeField] private LineRenderer line;
    
    private NavMeshAgent agent;
    private AIAgent aiAgent;
    private bool isJumping = false;
    private Vector3 originalScale;
    private float sqrtDistance;

    public void Behave(Vector3 targetPos)
    {
        float currentSqrtDist = (targetPos - transform.position).sqrMagnitude;
        float jumpThreshold = 2.5f * 2.5f;
        if(currentSqrtDist < jumpThreshold && !isJumping)
        {
            //StartCoroutine(SquashJumpAttack(targetPos));
            StartCoroutine(SquashJumpAttackDoTween(targetPos));
        }
        else
        {
            if(agent !=null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.destination = targetPos;
            }
        }
    }

    public float Evaluate(Vector3 targetPos)
    {
        sqrtDistance = (transform.position - targetPos).sqrMagnitude;

        float sqrtAttackDistance = attackDistance * attackDistance;
        float score = Mathf.Clamp01(1 - (sqrtDistance / sqrtAttackDistance)); //нормализуем дистанцию
        
        if(sqrtDistance > sqrtAttackDistance) score = 0; //если дальше чем дистанция атаки, то 0
        //Debug.Log($"SqrDist: {sqrtDistance:F2} | SqrLimit: {sqrtAttackDistance:F2}");
        return score;
    }

    public void Init()
    {
        transform.localScale = Vector3.one;
        agent = GetComponent<NavMeshAgent>();
        originalScale = transform.localScale;
        aiAgent = GetComponent<BehaviourManager>().GetAIAgent;
    }

    private IEnumerator SquashJumpAttack(Vector3 targetPos)
    {
        isJumping = true;
        if(aiAgent !=null)
            aiAgent.IsBusy = true;
        SwitchNavMesh(false);

        Vector3 start = transform.position;
        //1. squash (перед прыжком)
        
        // ---------------------------LINE RENDERER---------------------------
        if(line != null)
        {
            line.enabled = true;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, targetPos);
        }
        // -------------------------------------------------------------------

        float prepareTime = 0.2f;
        float t = 0f;
        while(prepareTime > t)
        {
            t += Time.deltaTime;
            float squashAmount = Mathf.Lerp(1f, 0.4f, t / prepareTime);
            transform.localScale = new Vector3(
                                originalScale.x * 1.2f, 
                                originalScale.y * squashAmount, 
                                originalScale.z * 1.2f);
            yield return null;
        }

        //2. stretch
        t = 0f;
        while(jumpDuration > t)
        {
            t += Time.deltaTime;
            float p = t / jumpDuration;

            Vector3 currentPos = Vector3.Lerp(start, targetPos, p);
            currentPos.y += math.sin(p * math.PI) * jumpForce;
            transform.position = currentPos;
            transform.localScale = new Vector3(originalScale.x * 0.8f, originalScale.y * 1.3f, originalScale.z * 0.8f);
            yield return null;
        }

        //3. landing and hit
        SwitchNavMesh(true);
        transform.localScale = originalScale;
        Debug.Log("<color=red>ATTACK!</color>");
        yield return new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(attackCooldown);
        if (aiAgent != null)
            aiAgent.IsBusy = false;
        isJumping = false;
    }

    private void SwitchNavMesh(bool on)
    {
        if (on)
        {
            agent.enabled = true;
            //Finds the nearest point based on the NavMesh within a specified range.
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position); //new pos to translate the agent to
            }

            if (agent.isOnNavMesh)
                agent.isStopped = false;
        }
        else
        {
            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
                agent.isStopped = true; //отключаем NavMeshAgent, чтобы не мешал прыжку

            agent.enabled = false;
        }
    }

    private void DoTweenJump(Vector3 targetPos)
    {
        Sequence jumpSeq = DOTween.Sequence();

        Vector3 squashedScale = new Vector3(originalScale.x * 1.2f, originalScale.y * 0.4f, originalScale.z * 1.2f);
        Vector3 strechedScale = new Vector3(originalScale.x * 0.8f, originalScale.y * 1.3f, originalScale.z * 0.8f);

        jumpSeq.Append(transform.DOScale(squashedScale, 0.2f)).
            Append(transform.DOScale(originalScale, 0.1f)).
            AppendCallback(() =>
            {
                transform.DOLocalJump(targetPos, jumpForce, jumpNum, jumpDuration);
                transform.DOPunchScale(strechedScale, jumpDuration, 5, 1);
            });
    }

    private IEnumerator SquashJumpAttackDoTween(Vector3 targetPos)
    {
        isJumping = true;
        if (aiAgent != null)
            aiAgent.IsBusy = true;
        SwitchNavMesh(false);

        DoTweenJump(targetPos);

        SwitchNavMesh(true);
        //transform.localScale = originalScale;
        Debug.Log("<color=red>ATTACK!</color>");
        yield return new WaitForSeconds(attackCooldown);
        if (aiAgent != null)
            aiAgent.IsBusy = false;
        isJumping = false;
    }
}

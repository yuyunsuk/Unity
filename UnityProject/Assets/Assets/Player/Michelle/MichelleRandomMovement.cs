using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MichelleRandomMovement : MonoBehaviour
{
    public float moveSpeed = 3.0f; // NPC의 이동 속도
    public float walkSpeed = 1.0f; // 걷기 속도
    public float runSpeed = 3.0f; // 달리기 속도
    public float changeDirectionInterval = 2.0f; // 방향 변경 간격
    public float moveRadius = 10.0f; // NPC가 이동할 반경
    public float animationEndMultiplier = 1.3f; // 애니메이션 종료 시 이동 거리 배수

    private Vector3 targetPosition;
    private Vector3 lastDirection; // 마지막 이동 방향
    private float timeSinceLastChange;
    private Animator animator;
    private bool isMoving;

    // 애니메이션 상태 이름
    private string[] animationStates = { "Idle", "DIdle", "LAround", "CWalk", "Talk" };

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        SetRandomTargetPosition();
    }

    void ApplyRandomAnimation()
    {
        if (animator != null)
        {
            // AnimationStates 랜덤으로 선택
            int randomIndex = Random.Range(0, animationStates.Length);

            // 랜덤으로 선택된 애니메이션 상태 이름 가져오기
            string selectedAnimation = animationStates[randomIndex];

            // 선택된 애니메이션 상태를 플레이
            animator.Play(selectedAnimation);
        }
    }

    void Update()
    {
        if (animator != null)
        {
            // NPC의 이동 상태에 따라 애니메이션 전환
            isMoving = Vector3.Distance(transform.position, targetPosition) > 0.1f;

            if (isMoving)
            {
                float distance = Vector3.Distance(transform.position, targetPosition);
                float speed = moveSpeed; // 기본 속도 설정

                string animationState = "CWalk"; // 기본 애니메이션 상태

                // string animationState = "Idle"; // 기본 애니메이션 상태

                //if (distance > 6.5f)
                //{
                //    speed = runSpeed; // 빠르게 이동할 때
                //    animationState = "Run";
                //}
                //else
                //{
                //    speed = walkSpeed; // 천천히 이동할 때 // 
                //    animationState = "Walk";
                //}

                // animator.Play(animationState); // 애니메이션 상태 전환
                // animator.SetFloat("Speed", speed); // 애니메이션 속도 설정
                // MoveTowardsTarget(speed); // 속도를 인자로 전달

                // 애니메이션이 끝났는지 체크
                if (IsAnimationFinished())
                {
                    ApplyRandomAnimation();
                    // MoveInLastDirection();
                    // SetRandomTargetPosition(); // 애니메이션이 끝났을 때 새로운 타겟 설정
                    timeSinceLastChange = 0.0f; // 방향 변경 간격 초기화
                }
            }
            else
            {
                animator.Play("Idle"); // 정지 상태에서 'Idle' 애니메이션
                animator.SetFloat("Speed", 0); // 정지 상태에서 애니메이션 속도 0
            }
        }

        timeSinceLastChange += Time.deltaTime;
        if (timeSinceLastChange >= changeDirectionInterval)
        {
            SetRandomTargetPosition();
            timeSinceLastChange = 0.0f;
        }
    }

    private void SetRandomTargetPosition()
    {
        // moveRadius 범위 내의 임의 방향 벡터를 생성
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;

        // y축을 0으로 설정하여 수평 이동만 고려
        randomDirection.y = 0;

        // 현재 오브젝트의 위치를 기준으로 타겟 위치 계산
        targetPosition = transform.position + randomDirection;
        lastDirection = (targetPosition - transform.position).normalized; // 마지막 이동 방향 저장
    }

    private void MoveTowardsTarget(float speed)
    {
        // NPC가 targetPosition으로 부드럽게 이동하도록
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 현재 위치와 타겟 위치 사이의 방향을 계산
        Vector3 directionToTarget = targetPosition - transform.position;

        // 타겟 방향을 바라보는 회전 설정
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // 부드러운 회전을 위한 Slerp
        }

        // 목표에 가까워지면 새로운 목표를 설정
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetRandomTargetPosition();
        }
    }

    private bool IsAnimationFinished()
    {
        // 현재 애니메이션 상태를 가져옴
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // 애니메이션이 끝났는지 체크
        return stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0);
    }

    private void MoveInLastDirection()
    {
        // 마지막 이동 방향으로 위치를 이동시킵니다.
        // 이동 거리 배수 적용
        Vector3 moveVector = lastDirection * animationEndMultiplier;
        transform.position += moveVector;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent; // 네비게이션 메쉬의 줄임말로, 게임 월드에서 걸어다닐 수 있는 구역을 설정하고 다룰 수 있게 해주는 유니티 빌트인 시스템
    public Transform[] waypoints;     // 2개 포인트 사이를 왔다갔다함

    int m_CurrentWaypointIndex;

    void Start ()
    {
        navMeshAgent.SetDestination (waypoints[0].position);
    }

    void Update ()
    {
        if(navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance) // navMeshAgent
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination (waypoints[m_CurrentWaypointIndex].position);
        }
    }
}

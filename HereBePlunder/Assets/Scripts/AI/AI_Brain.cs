using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AI_Brain : MonoBehaviour
{
    [Header("Character Controller")]
    [SerializeField] private KRB_CharacterController _controller;

    [Header("Navigation")]
    [SerializeField] private NavMeshAgent _agent;
    private NavMeshPath _path;

    [Header("DEBUG")]
    [SerializeField] private Character _target;

    private Vector2 _movement = Vector2.zero;

    private void Awake()
    {
        if (_agent == null)
        {
            Debug.LogWarning(this.name + " doesn't have a navmeshagent.");
            return;
        }

        _agent.updatePosition = false;
        _agent.updateRotation = false;

        _path = new NavMeshPath();
    }

    private void Update()
    {
        if (_target != null)
        {
            SetMovementTowards(_target.gameObject.transform.position);
        }
    }

    private void SetMovementTowards(Vector3 destination)
    {
        if (Vector3.Distance(_agent.nextPosition, transform.position) >= 3f)
        {
            _agent.Warp(transform.position);
        }
        else
        {
            _agent.nextPosition = transform.position;
        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, 5f, NavMesh.AllAreas))
        {
            NavMesh.CalculatePath(transform.position, hit.position, NavMesh.AllAreas, _path);
        }
        else
        {
            _movement = Vector2.zero;
            HandleMovementInputs();
            return;
        }

        if (_path.corners.Length > 0)
        {
            Vector3 pathDirection = new Vector3(_path.corners[1].x - transform.position.x, 0, _path.corners[1].z - transform.position.z);
            pathDirection = pathDirection.normalized;

            _movement.y = pathDirection.z;
            _movement.x = pathDirection.x;
            HandleMovementInputs();
        }
        else
        {
            _movement = Vector2.zero;
            HandleMovementInputs();
        }
    }

    private void HandleMovementInputs()
    {
        PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

        // Build the CharacterInputs struct
        characterInputs.MoveAxisForward = _movement.y;
        characterInputs.MoveAxisRight = _movement.x;

        // Apply inputs to character
        _controller.SetInputs(ref characterInputs);
    }
}

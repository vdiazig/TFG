using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ManagerLocalSceneUseCase : MonoBehaviour
{
    [SerializeField] private Vector3 playerStartPosition;
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        Instantiate(playerPrefab, playerStartPosition, quaternion.identity);
    }

}
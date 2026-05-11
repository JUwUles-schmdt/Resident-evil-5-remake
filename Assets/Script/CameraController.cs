using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Targets")]
    public List<Transform> players = new List<Transform>();

    [Header("Position")]
    public Vector3 offset = new Vector3(0, 10, -10);
    public float smoothSpeed = 5f;

    [Header("Collision")]
    public LayerMask wallMask;
    public float minDistance = 2f;

    [Header("Zoom (optionnel)")]
    public float minFOV = 60f;
    public float maxFOV = 80f;
    public float maxDistance = 20f;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (players.Count == 0) return;

        Vector3 center = GetCenter();
        Vector3 desiredPos = center + offset;

        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

    }

    Vector3 GetCenter()
    {
        Vector3 sum = Vector3.zero;
        foreach (var p in players)
            sum += p.position;

        return sum / players.Count;
    }

    public void AddPlayer(Transform player)
    {
        if (!players.Contains(player))
            players.Add(player);
    }

}
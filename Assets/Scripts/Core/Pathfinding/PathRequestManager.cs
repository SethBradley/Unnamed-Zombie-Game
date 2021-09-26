using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;
    static PathRequestManager instance;
    Pathfinding pathfinding;
    bool isProcessingPath;
    private void Awake() {
        instance = this;
        pathfinding = this.GetComponent<Pathfinding>();
    }
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart,pathEnd,callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void StartFindPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        Debug.Log("Done Processing Path");
        TryProcessNext();
    }

    struct PathRequest
        {
            public Vector3 pathStart;
            public Vector3 pathEnd;
            public Action<Vector3[], bool> callback;

            public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
            {
                //Huh.. okay, I guess structs have have their own constructors? That makes sense since they are technically classes?
                pathStart = _start;
                pathEnd = _end;
                callback = _callback;

                //So when I create a PathRequest struct i pass in params and set the fields equal to them.
            }
        }

    internal void FinishedProcessingPath(Vector3[] waypoints, bool pathSuccess)
    {
		currentPathRequest.callback(waypoints,pathSuccess);
		isProcessingPath = false;
		TryProcessNext();
    }
}

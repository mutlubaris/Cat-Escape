using System;
using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator[] pathsInOrder;
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;
        private Vector3 oldPos;

        void Start() {
            pathCreator = pathsInOrder[0];
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;
                oldPos = transform.position;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                if (oldPos == transform.position)
                {
                    pathCreator = pathsInOrder[Mathf.Min(Array.IndexOf(pathsInOrder, pathCreator) + 1, pathsInOrder.Length - 1)];
                    distanceTravelled = 0;
                    EventManager.OnReadyToOpenRadio.Invoke();
                }
                transform.LookAt(transform.position + (pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction) - pathCreator.path.GetPointAtDistance(distanceTravelled - speed * Time.deltaTime, endOfPathInstruction)));
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}
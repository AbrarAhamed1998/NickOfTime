using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace NickOfTime.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        public Transform target;

        public float speed;
        public float jumpSpeed;
        public float yThreshold;
        public float pickNextWaypointDist;
        public float pathCalcInterval;

        private Path aiPath;
        private int currentWaypoint = 0;
        bool reachedEndOfPath;

		[SerializeField] private Seeker seeker;
        [SerializeField] private Rigidbody2D myRigidbody;

		private void Start()
		{
			StartCoroutine(CalculatePathRoutine());
		}

		private void FixedUpdate()
		{
			//GetWayPointDirection();

		}

		private Vector2 GetWayPointDirection()
		{
			if (aiPath == null)
				return Vector2.zero;

			if (currentWaypoint >= aiPath.vectorPath.Count)
			{
				reachedEndOfPath = true;
				return Vector2.zero;
			}
			

			Vector2 horizontalDirection = ((Vector2)aiPath.vectorPath[currentWaypoint] - myRigidbody.position).normalized;
			/*Vector2 force = horizontalDirection * speed * Time.deltaTime;

			myRigidbody.AddForce(force);

			if (horizontalDirection.y > yThreshold)
			{
				myRigidbody.AddForce(jumpSpeed * Vector2.up, ForceMode2D.Impulse);
			}*/

			float distance = Vector2.Distance(myRigidbody.position, aiPath.vectorPath[currentWaypoint]);
			if (distance < pickNextWaypointDist)
				currentWaypoint++;

			return horizontalDirection;
		}

		public void OnPathComplete(Path p)
		{
            if(!p.error)
			{
                aiPath = p;
                currentWaypoint = 0;
            }
        }
        private IEnumerator CalculatePathRoutine()
		{
            while(gameObject.activeSelf)
			{
                seeker.StartPath(myRigidbody.position, target.position, OnPathComplete);

                yield return new WaitForSeconds(pathCalcInterval);
			}
		}
	}
}


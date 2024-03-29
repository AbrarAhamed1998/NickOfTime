using NickOfTime.Enemy;
using NickOfTime.Managers;
using NickOfTime.ScriptableObjects.Enemy;
using NickOfTime.UI.DialogSystem;
using NickOfTime.Utilities.PoolingSystem;
using Pathfinding;
using System;
using UnityEngine;

namespace NickOfTime.Characters
{
	public class BossTank : MonoBehaviour
    {
		[SerializeField] private Transform _gunTransform;
        [SerializeField] private Rigidbody2D _tankRigidbody;
        [SerializeField] private Seeker _tankSeeker;

        [SerializeField] private TankStats _tankStats;

        [SerializeField] private TankGun _tankGun;

        [SerializeField] private Transform _dummyCharacterHead;

        [SerializeField] private Transform _uiRoot;

        [SerializeField] private DialogPlayer _tankDialogPlayer;
        [SerializeField] private DialogSetSO _tankDialogSet;

        private PoolManager _poolManager => PersistentDataManager.instance.PoolManager;

        public TankStats TankStats => _tankStats;
        public TankGun TankGun => _tankGun;

        private bool isDestroyed;

        public Action LookAction, AttackAction, MoveAction;
        public Action<float, Vector2> TakeDamageAction;

        private Vector2 _movementDirection, _lookDirection;

        public Vector2 WaypointDirection { get; set; }

        public Transform TankTarget { get; set; } 


		#region UNITY_CALLBACKS
		void Start()
        {

        }

        void Update()
        {
            
        }

		private void OnEnable()
		{
            RegisterControlEvents();
		}

		private void OnDisable()
		{
            DeregisterControlEvents();
		}

		#endregion

		protected void RegisterControlEvents()
		{
            MoveAction += () =>
            {
                _tankRigidbody.AddForce(WaypointDirection * _tankStats.TankMovementSpeed * Time.deltaTime, ForceMode2D.Force);
            };

            LookAction += () =>
            {
                if (!isDestroyed)
                {
                    LookAtWorldPos(TankTarget, _gunTransform);
                    LookAtWorldPos(TankTarget, _dummyCharacterHead, false);
                }
            };

            AttackAction += ()=>
            {
                FireTankRound();
            };

            TakeDamageAction += (damage, direction) =>
            {
                
            };
		}

        protected void DeregisterControlEvents()
		{
            MoveAction = null;
            LookAction = null;
            AttackAction = null;
		}

        protected virtual void LookAtWorldPos(Transform targetWorldTransform, Transform rotTarget, bool isClamped = true)
        {
            if (targetWorldTransform == null)
            {
                //Debug.Log("target World Pos is null");
                return;
            }
            Vector2 worldPos = targetWorldTransform.position;
            Transform target = rotTarget;
            float y = target.position.y - worldPos.y;
            float x = target.position.x - worldPos.x;
            float localEulerY = transform.localEulerAngles.y;
            float trueYRot = localEulerY < 180f ? localEulerY : localEulerY - 360;
            float switchFactor = (trueYRot < 0 ? 0f : 180f);
            float targetAngle = (Mathf.Atan2(y, x) * Mathf.Rad2Deg) + switchFactor;
            float finalZangle = (switchFactor == 0f ? -1f : 1f) * targetAngle;

            if(isClamped)
                target.localEulerAngles = new Vector3(0f, 0f, ClampAngle(switchFactor, finalZangle));
            else
                target.localEulerAngles = new Vector3(0f, 0f, finalZangle);

            /*for (int i = 1; i < _debugLookObjects.Length; i++)
            {
                _debugLookObjects[i].transform.localEulerAngles = target.localEulerAngles;
            }*/

            Vector2 mouseDirection = (worldPos - (Vector2)this.transform.position).normalized * Vector2.right;
            Vector2 playerdirection = transform.right.normalized;
            float dotProduct = Vector2.Dot(mouseDirection, playerdirection);
            if (dotProduct < 0f)
            {
                transform.localEulerAngles += Vector3.up * 180f;
            }
        }

        // Makes sure to limit angles when character faces wither direction
        public float ClampAngle(float switchFactor, float value)
		{
            float minValue;
            float maxValue;
            if(switchFactor == 0f)
			{
                minValue = _tankStats.TankAngularLimits.minAngle;
                maxValue = _tankStats.TankAngularLimits.maxAngle;
			}
			else
			{
                minValue = _tankStats.TankAngularLimits.minAngle;
                maxValue = _tankStats.TankAngularLimits.maxAngle;
			}
            float finalVal = switchFactor == 0f ? value : value > 180f ? value - 360f : value;
            //Debug.Log($"minValue : {minValue}, maxValue : {maxValue}, value : {finalVal}");
                
            return Mathf.Clamp(finalVal, minValue, maxValue);
		}

		#region PUBLIC METHODS

        public void TankMove()
		{
            MoveAction?.Invoke();
		}

        public void TankAttack()
		{
            AttackAction?.Invoke();
		}

        public void TankLook()
		{
            LookAction?.Invoke();
		}

		#endregion


		protected void FireTankRound()
		{
            _tankDialogPlayer.AssignDialogSet(_tankDialogSet);
            _tankDialogPlayer.PlayAssignedDialogSet(0, () =>
            {
                _tankDialogPlayer.PlayAssignedDialogSet(1,
                    () =>
                    {
                        _tankGun.OnUseGun();
                    }
                );
            });
        }

        public void TakeDamage(float damage, Vector2 direction)
		{
            TakeDamageAction?.Invoke(damage, direction);
            //Debug.Log($"Taken Damage : {damage}");
		}

        protected void SetDamageSprite()
		{

		}

        protected void OnMoveInput(Vector2 input)
		{

		}

        protected void OnTankDeath()
        {

        }
		
    }
}


using NickOfTime.Characters;
using NickOfTime.Managers;
using NickOfTime.ScriptableObjects.Weapons;
using NickOfTime.Utilities.PoolingSystem;
using NickOfTime.Weapons.Projectiles;
using UnityEngine;

namespace NickOfTime.Weapons
{
	public class WeaponBase : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _mySpriteRenderer;
		[SerializeField] private Collider2D _myCollider;
		[SerializeField] private Collider2D _pickupCollider;
		[SerializeField] private Rigidbody2D _myRigidbody;
		[SerializeField] protected WeaponStatsSO _weaponStatsSO;

		[SerializeField, Layer] protected int _playerLayer;
		[SerializeField, Layer] protected int _projectileLayer;
		[SerializeField] protected Transform _barrel;


		public SpriteRenderer ItemSpriteRenderer => _mySpriteRenderer;
		public WeaponStatsSO WeaponStats => _weaponStatsSO;

		public CharacterBase WeaponOwner;


		
        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

		protected virtual void InitializePool()
		{

		}

        protected virtual void OnPickUp()
		{
            _myCollider.enabled = false;  
			_pickupCollider.enabled = false;
		}

		protected virtual void OnDrop()
		{
            _myCollider.enabled = true;
			_pickupCollider.enabled = true;
		}

		protected virtual void OnUseWeapon()
		{

		}

		protected virtual void OnReload()
		{

		}

		protected virtual void FireProjectile(string poolID)
		{
			PoolObject bulletPoolObject = PersistentDataManager.instance.PoolManager
				.GetPoolObject(poolID, null);
			BulletBase bullet = bulletPoolObject.obj.GetComponent<BulletBase>();
			bullet.SetDamageValue(_weaponStatsSO.ProjectileDamageValue);
			bullet.gameObject.SetActive(false);
			bullet.OwnerWeapon = this;
			bullet.OwnerCharacter = WeaponOwner;
			bullet.gameObject.layer = _projectileLayer;
			bullet.transform.position = _barrel.position;
			bullet.transform.rotation = _barrel.rotation;

			bullet.gameObject.SetActive(true);

			bullet.InitializeProjectile(() => {
				PersistentDataManager.instance.PoolManager.ReturnObjectToPool(bulletPoolObject);
			},
			_weaponStatsSO.MaxProjectileLifetime);
			bullet.GetComponent<Rigidbody2D>().AddForce(_barrel.right * _weaponStatsSO.ProjectileLaunchForce, ForceMode2D.Impulse);
		}

		public virtual void UseWeapon()
		{
			OnUseWeapon();
		}

		protected void OnTriggerEnter2D(Collider2D collision)
		{
			if((_weaponStatsSO.PickupMask & 1<< collision.gameObject.layer) != 0)
			{
				CharacterBase _character = collision.GetComponent<CharacterBase>();
				if(_character != null)
				{
					OnPickUp();
					_character.EquipWeapon(this);
					WeaponOwner = _character;
				}
			}
		}

		public virtual void SetProjectleLayer(bool isOwnedByPlayer)
		{
			/*if (isOwnedByPlayer)
				_projectileLayer = _weaponStatsSO.PlayerProjectileLayer;
			else*/
				_projectileLayer = _weaponStatsSO.GlobalProjectileLayer;
		}

	}

}


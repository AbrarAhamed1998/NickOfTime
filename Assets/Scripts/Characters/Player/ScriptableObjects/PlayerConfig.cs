using NickOfTime.ScriptableObjects.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.ScriptableObjects.Player
{
	[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/Player/Player Config")]
	public class PlayerConfig : CharacterBaseConfigSO
	{
		[SerializeField] protected float _jetPackRotSpeed;


		[Header("Death Variables")]
		[SerializeField] protected float _deathBodyZRot;
		[SerializeField] protected float _deathHeadZRot;
		[SerializeField] protected float _deathEffectTime;

		public float JetPackRotSpeed => _jetPackRotSpeed;
		public float DeathBodyZRot => _deathBodyZRot;
		public float DeathHeadZRot => _deathHeadZRot;
		public float DeathEffectTime => _deathEffectTime;
	}
}


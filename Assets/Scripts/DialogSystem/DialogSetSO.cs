using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.UI.DialogSystem
{
    [CreateAssetMenu(fileName = "DialogSet.asset", menuName = "Scriptable Objects/DialogSystem/DialogSetSO")]
    public class DialogSetSO : ScriptableObject
    {
		[SerializeField]
        private List<DialogSequenceParams> _dialogContentItems = new List<DialogSequenceParams>();   

        public List<DialogSequenceParams> DialogContentItems => _dialogContentItems;
    }
}



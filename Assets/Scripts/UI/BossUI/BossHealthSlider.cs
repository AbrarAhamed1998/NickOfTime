using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NickOfTime.UI
{
    public class BossHealthSlider : HealthSliderBase
    {
        [SerializeField] protected TextMeshProUGUI _bossName;


        public void SetBossName(string bossName)
		{
            _bossName.text = bossName;
		}
    }
}


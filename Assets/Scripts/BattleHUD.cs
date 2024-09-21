using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{

	public Text nameText;
	public Text levelText;

	public Text hpText;
	public Slider hpSlider;

	public Text manaText;
	public Slider manaSlider;

	public void SetHUD(Unit unit)
	{
		nameText.text = unit.unitName;
		levelText.text = "Lvl " + unit.unitLevel;
		hpSlider.maxValue = unit.maxHP;
		hpSlider.value = unit.currentHP;
		hpText.text = unit.currentHP + "/" + unit.maxHP;
		manaSlider.maxValue = unit.maxMP;
		manaSlider.value = unit.currentMP;
		manaText.text = manaText.text = unit.currentMP + "/" + unit.maxMP;
	}

	public void SetHP(int hp, int maxHP)
	{
		hpSlider.value = hp;
		hpText.text = hp + "/" + maxHP;
	}

	public void SetMP(int mp, int maxMP)
    {
		manaSlider.value = mp;
		manaText.text = mp + "/" + maxMP;
    }

}

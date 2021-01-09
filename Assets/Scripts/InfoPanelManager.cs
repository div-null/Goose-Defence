using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelManager : MonoBehaviour
{
	Button Accept;
	Text infoAboutTower, damage, radius, speed, reload;
	Text upgradeDamage, upgradeRadius, upgradeSpeed, upgradeReload;

	public void BuyTower()
	{
		upgradeDamage.gameObject.SetActive(false);
		upgradeRadius.gameObject.SetActive(false);
		upgradeSpeed.gameObject.SetActive(false);
		upgradeReload.gameObject.SetActive(false);
		Accept.GetComponent<Text>().name = "Купить";
	}

	void UpgradeTower()
	{
		Accept.GetComponent<Text>().name = "Улучшить";
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

	public Vector2 playerPosition;
	public VectorValue playerStorage;

	public GameObject playerPrefab;
	public GameObject enemyPrefab;
	public GameObject attackPanel;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	public AudioSource battleMusic;
	public AudioSource winMusic;
	public AudioSource loseMusic;

	Unit playerUnit;
	Unit enemyUnit;

	public Text dialogueText;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public BattleState state;

	public int sceneIndex;

    // Start is called before the first frame update
    void Start()
    {
		state = BattleState.START;
		StartCoroutine(SetupBattle());
    }

	IEnumerator SetupBattle()
	{
		GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
		playerUnit = playerGO.GetComponent<Unit>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
		enemyUnit = enemyGO.GetComponent<Unit>();

		dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);

		yield return new WaitForSeconds(2f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	IEnumerator PlayerDefend()
    {
		int defend = Random.Range(1, 2);

		dialogueText.text = "You brought your sword up to block, but couldn't completely stop the attack!";
		yield return new WaitForSeconds(1f);

		dialogueText.text = "You got hit for " + defend + " damage!";
		yield return new WaitForSeconds(1f);
		
		bool isDead = playerUnit.TakeDamage(defend);

		playerHUD.SetHP(playerUnit.currentHP, playerUnit.maxHP);

		yield return new WaitForSeconds(1f);

		if (isDead)
		{
			state = BattleState.LOST;
			EndBattle();
		}
		else
		{
			state = BattleState.PLAYERTURN;
			PlayerTurn();
		}
	}
	IEnumerator PlayerAttack()
	{
		bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

		enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.maxHP);
		dialogueText.text = "You swung your sword for " + playerUnit.damage + " damage!";

		ClosePanel();

		yield return new WaitForSeconds(2f);

		if(isDead)
		{
			state = BattleState.WON;
			EndBattle();
		} else
		{
			state = BattleState.ENEMYTURN;
			StartCoroutine(EnemyTurn());
		}
	}

	IEnumerator PlayerMagicAttack()
    {
		int magic = Random.Range(2, 5);
		bool isDead = enemyUnit.TakeDamage(magic);
		bool hasMana = playerUnit.CastMagic(2);

		if (hasMana)
        {
			playerHUD.SetMP(playerUnit.currentMP, playerUnit.maxMP);
			enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.maxHP);
			dialogueText.text = "You casted a spell and hit them for " + magic + " damage!";
		}
        else
        {
			dialogueText.text = "You do not have enough mana to cast this spell!";
        }

		ClosePanel();

		yield return new WaitForSeconds(2f);		

		if (isDead)
		{
			state = BattleState.WON;
			EndBattle();
		}
		else
		{
			state = BattleState.ENEMYTURN;
			StartCoroutine(EnemyTurn());
		}
	}

	IEnumerator PlayerHeal()
	{
		int potion = Random.Range(3, 8);
		playerUnit.Heal(potion);

		playerHUD.SetHP(playerUnit.currentHP, playerUnit.maxHP);
		dialogueText.text = "You used a healing potion! You healed for " + potion + " health!";

		yield return new WaitForSeconds(2f);

		state = BattleState.ENEMYTURN;
		StartCoroutine(EnemyTurn());
	}

	IEnumerator PlayerRun()
    {
		dialogueText.text = "You have run from the " + enemyUnit.unitName + "!";

		yield return new WaitForSeconds(2f);

		SceneManager.UnloadSceneAsync(sceneIndex);
    }

	IEnumerator PlayerDead()
    {
		dialogueText.text = "You were found in the forest and carried home...";

		yield return new WaitForSeconds(2f);

		SceneManager.LoadSceneAsync("Player_Home");

		playerStorage.initialValue = playerPosition;

		SceneManager.UnloadSceneAsync(sceneIndex);
		SceneManager.UnloadSceneAsync("Forest_North_1");
	}

	IEnumerator EnemyTurn()
	{
		dialogueText.text = enemyUnit.unitName + " attacks!";

		yield return new WaitForSeconds(1f);

		bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

		playerHUD.SetHP(playerUnit.currentHP, playerUnit.maxHP);

		yield return new WaitForSeconds(1f);

		if(isDead)
		{
			state = BattleState.LOST;
			EndBattle();
		} else
		{
			state = BattleState.PLAYERTURN;
			PlayerTurn();
		}

	}

	IEnumerator WaitForSound(AudioSource sound)
    {
		sound.Play();
		yield return new WaitUntil(() => sound.isPlaying == false);
	}

	void EndBattle()
	{
		if(state == BattleState.WON)
		{
			battleMusic.Stop();
			dialogueText.text = "You won the battle!";
			StartCoroutine(WaitForSound(winMusic));
			SceneManager.UnloadSceneAsync(sceneIndex);
		} else if (state == BattleState.LOST)
		{
			battleMusic.Stop();
			dialogueText.text = "You were defeated.";
			StartCoroutine(WaitForSound(loseMusic));
			StartCoroutine(PlayerDead());
		}
	}

	void PlayerTurn()
	{
		dialogueText.text = "Choose an action:";
	}

	public void OnAttackButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;
		StartCoroutine(PlayerAttack());
	}

	public void OnMagicButton()
    {
		if (state != BattleState.PLAYERTURN)
			return;
		StartCoroutine(PlayerMagicAttack());
    }

	public void OnHealButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;
		StartCoroutine(PlayerHeal());
	}

	public void OnDefendButton()
    {
		if (state != BattleState.PLAYERTURN)
			return;
		StartCoroutine(PlayerDefend());
	}

	public void OnRunButton()
    {
		if (state != BattleState.PLAYERTURN)
			return;
		StartCoroutine(PlayerRun());
    }

	public void ToggleAttackPanel()
    {
		if (state != BattleState.PLAYERTURN)
			return;

		if (attackPanel.activeSelf)
			attackPanel.SetActive(false);
		else
			attackPanel.SetActive(true);

    }

	public void ClosePanel()
    {
		attackPanel.SetActive(false);
    }

}

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class Player : NetworkBehaviour {
	[SyncVar]
	private bool isDead = false;
	public bool _isDead {
		get { return isDead;}
		protected set {isDead = value;}
	}
     
	[SerializeField] 
	private int maxHealth = 100;

	[SerializeField] 
	private int maxLives = 3;

	public RectTransform healthBar;

	
	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth;

	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;

	public void Setup () {
		wasEnabled = new bool[disableOnDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++) {
			wasEnabled[i] = disableOnDeath[i].enabled;
		}
		SetDefaults ();
	}




	/*void Update() {
		if (!isLocalPlayer)
			return;

		if (Input.GetKeyDown (KeyCode.K)) {
			RpcTakeDamage(9999);
		}
	}*/

	[ClientRpc]
	public void RpcTakeDamage (int amount) {
		if (isDead)
			return;

		currentHealth -= amount;
		Debug.Log (transform.name + " now has " + currentHealth + " health.");

		if (currentHealth <= 0) {
			Die ();
		}
	}

	void OnChangeHealth (int health)
	{
		healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
	}

	private void Die() {
		_isDead = true;

		maxLives -= 1;
		Debug.Log (transform.name + " now has " + maxLives + " lives.");
		
		if (maxLives <= 0) {
			GameOver ();
		}

		for (int i = 0; i<disableOnDeath.Length; i++) {
			disableOnDeath[i].enabled = false;
		}
		Collider col = GetComponent<Collider> ();
		if (col != null) {
			col.enabled = false;
		}
		Debug.Log (transform.name + " is dead");
		StartCoroutine (Respawn ());
	}

	private void GameOver() {
		Destroy (gameObject);
	}

	private IEnumerator Respawn(){
		yield return new WaitForSeconds (GameManager.instance.matchSettings.respawnTime);


		Transform spawnPoint = NetworkManager.singleton.GetStartPosition ();
		transform.position = spawnPoint.position;
		transform.rotation = spawnPoint.rotation;

		SetDefaults ();
		Debug.Log (transform.name + " respawned.");
	}

	public void SetDefaults() {
		_isDead = false;
		currentHealth = maxHealth;


		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath[i].enabled = wasEnabled[i];
		}
		Collider col = GetComponent<Collider> ();
		if (col != null) {
			col.enabled = true;
		}

	}
}
using UnityEngine;
using UnityEngine.Networking;


public class PlayerShooter : NetworkBehaviour {
	public PlayerWeapon weapon;
	private const string PLAYER_TAG = "Player";
	[SerializeField]
	private Camera cam;
	[SerializeField]
	private LayerMask mask;


	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	


	public Texture2D crosshairImage;


	// Use this for initialization
	void Start ()
	{

		if (cam == null){
			Debug.LogError("PlayerShoot: No camera referenced!");
			this.enabled = false;
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown("Fire1") ){
			Shoot();
		}
	}

	void OnGUI()
	{
		float xMin = (Screen.width / 2) - (crosshairImage.width / 2);
		float yMin = (Screen.height / 2) - (crosshairImage.height / 2);
		GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
	}


	[Client]
	void Shoot() {
		RaycastHit hit;


		if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, weapon.range, mask)) {
			var bullet = (GameObject)Instantiate(
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);
			bullet.GetComponent<Rigidbody>().velocity = cam.transform.forward * 100;
			Destroy (bullet, 2.0f);
			if (hit.collider.tag == PLAYER_TAG) {
			CmdPlayerShot (hit.collider.name, weapon.damage);
			}
		}
	}
	[Command]
	void CmdPlayerShot(string playerID, int damage) {
		Debug.Log (playerID + " has been shot.");
		Player player = GameManager.GetPlayer (playerID);

		player.RpcTakeDamage (damage);



	}
}

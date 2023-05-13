
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Realtime;
using Photon.Pun;



public class MultiplayerManager : MonoBehaviourPunCallbacks
{
	static public MultiplayerManager Instance;

    [SerializeField]
    private GameObject playerPrefab;


    void Start()
	{
		Instance = this;

		// in case we started this demo with the wrong scene being active, simply load the menu scene
		if (!PhotonNetwork.IsConnected)
		{
			SceneManager.LoadScene(0);

			return;
		}

		if (playerPrefab != null) 
		{ 
			StartCoroutine(DelayInitGame());
		}

	}

	void Update()
	{
		// "back" button of phone equals "Escape". quit app if that's pressed
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			//QuitApplication();
		}
	}



    #region Photon Callbacks

    /// <summary>
    /// Called when a Photon Player got connected. We need to then load a bigger scene.
    /// </summary>
    /// <param name="other">Other.</param>
    public override void OnPlayerEnteredRoom( Player other  )
	{

	}

	/// <summary>
	/// Called when a Photon Player got disconnected. We need to load a smaller scene.
	/// </summary>
	/// <param name="other">Other.</param>
	public override void OnPlayerLeftRoom( Player other)
	{

	}

	/// <summary>
	/// Called when the local player left the room. We need to load the launcher scene.
	/// </summary>
	public override void OnLeftRoom()
	{
		//SceneManager.LoadScene("PunBasics-Launcher");
	}

	#endregion

	#region Public Methods

	public bool LeaveRoom()
	{
		return PhotonNetwork.LeaveRoom();
	}

	public void QuitApplication()
	{
		Application.Quit();
	}

	#endregion

	#region Private Methods

	IEnumerator DelayInitGame()
    {
		print("waiting init");
		yield return new WaitForSeconds(1.0f);
		InitGame();
		print("inited");
    }

	void InitGame()
    {
		PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
	}
	#endregion

}

using UnityEngine;
using System.Collections;

public class TitleScript : MonoBehaviour {

	public Texture btnStartTexture;
	public Texture btnQuitTexture;

	public GUISkin mainSkin;

	static public int showCount = 0;
	// Use this for initialization
	void Start () {
	
		SelectedLevelScript.selectedLevel = 0;
		if(showCount>0)
			AdHandlerScript.showBanner();

		showCount++;
	}

	void OnGUI () {
		GUI.skin = mainSkin;
		GUI.skin.button.fontSize = 28 * Screen.height / 800;

		float btnHeight = Screen.height / 12;
		float btnWidth = Screen.width / 3;
		float startY = Screen.height / 2 + btnHeight / 2;
		float startX = Screen.width / 4;
		if (GUI.Button (new Rect(startX, startY, btnWidth, btnHeight), "  START")) {
			Application.LoadLevel("LevelScene");
		}

		if (GUI.Button (new Rect(startX, startY + btnHeight*2, btnWidth, btnHeight), "  QUIT")) {
			Application.Quit();
		}
		GUI.skin = mainSkin;
		GUI.skin.label.fontSize = 16 * Screen.height / 800;
		GUI.Label (new Rect (startX, startY+btnHeight*3.4f, btnWidth, btnHeight), "COPYRIGHT \u00A9 2014 T.MAKALAINEN");
	}
	// Update is called once per frame
	void Update () {
	
	}

	void OnApplicationQuit() {
		AdHandlerScript.DestroyAds();
	}
}

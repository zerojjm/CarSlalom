using UnityEngine;
using System.Collections;

#if !UNITY_WP8 
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
#endif

#if UNITY_WP8
using System;
using File = UnityEngine.Windows.File;
#endif

#if !UNITY_WP8 
[System.Serializable] 
#endif
public class Highscores {
	
	public int highscore1;
	public int highscore2;
	public int highscore3;
	public int highscore4;
	public int highscore5;

	public Highscores () {
		this.highscore1 = 0;
		this.highscore2 = 0;
		this.highscore3 = 0;
		this.highscore4 = 0;
		this.highscore5 = 0;
	}
}

public class HighScoreScript : MonoBehaviour {

	public Highscores highScores = new Highscores();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

#if !UNITY_WP8 
	public void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/hsdata.tm");
		bf.Serialize(file, highScores);
		file.Close();
	}

	public void Load() {
		if(File.Exists(Application.persistentDataPath + "/hsdata.tm")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/hsdata.tm", FileMode.Open);
			highScores = (Highscores)bf.Deserialize(file);
			file.Close();
		}
	}
#endif

#if UNITY_WP8 
	public void Save() {
		string[] scores = new string[3];
		scores[0] = highScores.highscore1.ToString();
		scores[1] = highScores.highscore2.ToString();
		scores[2] = highScores.highscore3.ToString();
		string scoresAsString = string.Join(",", scores);
		var bytes = System.Text.Encoding.UTF8.GetBytes(scoresAsString);
		File.WriteAllBytes (Application.persistentDataPath+ "/hsdata.tm", bytes);
	}
	
	public void Load() {
		string fileName = Application.persistentDataPath+ "/hsdata.tm";
		if(!File.Exists(fileName))
			return;
		
		var bytes = File.ReadAllBytes(fileName);
		string result = System.Text.Encoding.UTF8.GetString(bytes);
		string[] tokens = result.Split(',');
		
		int[] scores = Array.ConvertAll<string, int>(tokens, int.Parse);
		highScores.highscore1 = scores[0];
		highScores.highscore2 = scores[1];
		highScores.highscore3 = scores[2];

	}
#endif
}

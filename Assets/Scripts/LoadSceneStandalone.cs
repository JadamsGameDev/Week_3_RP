using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneStandalone : MonoBehaviour
{

    private void Start()
    {
#if UNITY_STANDALONE
		if (!Application.isEditor)
		{
			SceneManager.LoadScene("Assets/Scenes/Level1/Mechanics.unity", LoadSceneMode.Additive);
			SceneManager.LoadScene("Assets/Scenes/Level1/Zane.unity", LoadSceneMode.Additive);
		}
#endif
	}


	// Update is called once per frame
	void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //public string path;
    //private AssetBundle loadedAssets;
   // private string[] scenePaths;

    // Start is called before the first frame update
    void Start()
    {
        //loadedAssets = AssetBundle.LoadFromFile(path);
        //scenePaths = loadedAssets.GetAllScenePaths();

       // for(int ele = 0; ele < scenePaths.Length; ele++)
        //{
            //SceneManager.LoadScene(scenePaths[ele], LoadSceneMode.Additive);
        //}

        SceneManager.LoadScene("Assets/Scenes/Level1/Mechanics.unity", LoadSceneMode.Additive);
        SceneManager.LoadScene("Assets/Scenes/Level1/Zane.unity", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        
    }
}

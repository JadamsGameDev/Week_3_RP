using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

[ExecuteInEditMode]
public class SceneLoader : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Open Scenes/Open Scenes")]
    // Start is called before the first frame update
    static void OpenScenes()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Level1/Mechanics.unity", OpenSceneMode.Additive);
        EditorSceneManager.OpenScene("Assets/Scenes/Level1/Zane.unity", OpenSceneMode.Additive);


        //EditorSceneManager.OpenScene("Assets/Scenes/Level1/Mechanics.unity", OpenSceneMode.Additive);
        //EditorSceneManager.OpenScene("Assets/Scenes/Level1/Zane.unity", OpenSceneMode.Additive);
        //SceneManager.LoadScene("Assets/Scenes/Level1/Mechanics.unity", LoadSceneMode.Additive);
        //SceneManager.LoadScene("Assets/Scenes/Level1/Zane.unity", LoadSceneMode.Additive);
    }
#endif

}

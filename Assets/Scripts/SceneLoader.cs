using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string m_firstScene;
    public string m_secondScene;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(m_firstScene, LoadSceneMode.Additive);
        SceneManager.LoadScene(m_secondScene, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

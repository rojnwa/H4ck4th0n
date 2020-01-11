using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour {
    // Start is called before the first frame update
    public void LoadScene(string scene) {
        SceneManager.LoadScene(scene);
    }

    // Update is called once per frame
    public void Quit() {
        Application.Quit();
    }
}

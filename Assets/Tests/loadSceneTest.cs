using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadSceneTest : MonoBehaviour
{

    public void loadTest()
    {
        ManagerScenes.Instance.LoadScene("test3", true, () => 
        {
            Debug.Log("TEST: scene test3 load.");
        });
    }

}

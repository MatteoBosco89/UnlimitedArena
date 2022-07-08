using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerSelection
{
    public class SceneChange : MonoBehaviour
    {
        protected NetManager nm;

        private void Start()
        {
            nm = GetComponent<NetManager>();
        }

        public void selectCube()
        {
            ChangeScene("SceneB", "Cube");
        }

        public void selectSphere()
        {
            ChangeScene("SceneB", "Sphere");
        }

        void ChangeScene(string scenename, string obj)
        {
            //SceneManager.LoadScene(scenename);
            nm.ChooseMe(obj);
        }
    }
}



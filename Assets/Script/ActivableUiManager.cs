using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

namespace Character
{
    public class ActivableUiManager : MonoBehaviour
    {
        [SerializeField] GameObject currContainer;
        [SerializeField] GameObject preContainer;
        [SerializeField] GameObject nextContainer;
        [SerializeField] string activableImagesPath;
        [SerializeField] ActivableManager manager;
        protected Dictionary<string, Sprite> images = new Dictionary<string, Sprite>();
        protected Dictionary<string, bool> imageState = new Dictionary<string, bool>();
        protected string curr;
        protected string pre;
        protected string next;
        protected bool act = false;


        private void Start()
        {
            LoadActivables(); //istanza i modelli degli attivabili raccolti 
            
        }

        private void FixedUpdate()
        {
            SetCurrentModelsName();
            ShowInGui();
        }

        protected void LoadActivables()
        {
            string folderPath = Path.Combine(Application.streamingAssetsPath, activableImagesPath);
            if (!CheckDirectory(folderPath))return;  //Get path of folder
            string[] filePaths = Directory.GetFiles(folderPath, "*.png");
            for(int i = 0; i < filePaths.Length; i++) 
            {
                Texture2D t = new Texture2D(2,2);
                ImageConversion.LoadImage(t, File.ReadAllBytes(filePaths[i]));
                string name = Path.GetFileName(filePaths[i]).Replace(".png", "");
                Sprite sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 100f);
                images[name] = sprite;
            } 
        }

        protected void SetCurrentModelsName()
        {
            List<string> names = manager.GetActivablesNames();
            if(names.Count <= 0)
            {
                //Debug.Log("No activable");
                curr = "";
                pre = "";
                next = "";
            }
            else
            {
                curr = names[0];
                next = names[1];
                pre = names[2];
            }

        }
        
        private void ShowInGui()
        {
            ShowImage(curr, currContainer);
            ShowImage(pre, preContainer);
            ShowImage(next, nextContainer);
        }

        protected void ShowImage(string newImg, GameObject container)
        {
            //if (images.Count<=0) return;
           
            if (newImg != "" && images.ContainsKey(newImg)) 
            {
                //Debug.Log(newImg + "sprite");
                container.SetActive(true);
                container.GetComponent<Image>().sprite = images[newImg];
            }
            else
            {
                //Debug.Log("No sprite");
                container.SetActive(false);
            }
        }

        protected bool CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.LogError("Directory Not Found");
                return false;
            }
            return true;
        }

    }


}

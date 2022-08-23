using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class PathManager : MonoBehaviour
    {
        [SerializeField] string path;

        public string Path
        {
            get { return path; }
        }

    }
}


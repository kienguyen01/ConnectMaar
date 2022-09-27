using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Ui_handlers
{
    class WelcomeText : MonoBehaviour
    {
        public float DestroyTime = 3f;

        private void Start()
        {
            Destroy(gameObject, DestroyTime);
        }
    }
}

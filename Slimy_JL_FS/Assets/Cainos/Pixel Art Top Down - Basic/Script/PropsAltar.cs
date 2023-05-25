using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when something get into the alta, make the runes glow
namespace Cainos.PixelArtTopDown_Basic
{

    public class PropsAltar : MonoBehaviour
    {
        public List<SpriteRenderer> runes;

      
        [SerializeField]
        private Color targetColor = new Color(255,255,255,255);

  
        private void Update()
        {
         

            foreach (var r in runes)
            {
                r.color = targetColor;
            }
        }
    }
}

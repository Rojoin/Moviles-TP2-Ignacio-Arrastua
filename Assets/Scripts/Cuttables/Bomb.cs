using UnityEngine;

namespace Cuttables
{
    class Bomb : Cuttable
    {
        protected override void OnTriggerEnter(Collider other)
        {
           OnCut.Invoke(this.gameObject);
        }
    }
}
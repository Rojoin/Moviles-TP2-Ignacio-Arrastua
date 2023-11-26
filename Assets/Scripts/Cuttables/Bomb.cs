using Player;
using UnityEngine;

namespace Cuttables
{
    class Bomb : Cuttable
    {
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Blade>(out Blade player) && player.isSlicing && !isCut)
            {
                OnCut.Invoke(this.gameObject);
            }
            if (other.gameObject.CompareTag("Respawn"))
            {
                OnDespawn.Invoke(this.gameObject);
            }
        }
    }
}
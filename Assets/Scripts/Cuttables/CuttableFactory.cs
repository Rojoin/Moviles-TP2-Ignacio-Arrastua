using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Cuttables
{
    public abstract class CuttableFactory
    {
        protected List<CuttableSO> _cuttableSos;

        public abstract CuttableSO GetItem();
        
        
    }
    public class RandomCuttableFactory : CuttableFactory
    {
        public override CuttableSO GetItem()
        {
            return _cuttableSos[Random.Range(0, _cuttableSos.Count)];
        }
    }
}
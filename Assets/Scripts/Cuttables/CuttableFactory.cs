using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Cuttables
{
    public abstract class CuttableFactory
    {
        protected List<CuttableSO> _cuttableSos;

        public CuttableFactory(List<CuttableSO> cuttableSos)
        {
            _cuttableSos = new List<CuttableSO>(cuttableSos);
        }

        public abstract CuttableSO GetItem();
    }

    public class RandomCuttableFactory : CuttableFactory
    {
        public override CuttableSO GetItem()
        {
            return _cuttableSos[Random.Range(0, _cuttableSos.Count)];
        }

        public RandomCuttableFactory(List<CuttableSO> cuttableSos) : base(cuttableSos)
        {
           
        }
        public void AddToList(CuttableSO cuttableSo)
        {
            _cuttableSos.Add(cuttableSo);
        }
    }
}
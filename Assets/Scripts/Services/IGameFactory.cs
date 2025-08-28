using System.Collections.Generic;
using Components;
using UnityComponents;

namespace Services
{
    public interface IGameFactory
    {
        public Balance Balance { get; }
        public Dictionary<int, BusinessCardView> ViewById { get; }

        public void CreateBalance();
        public void CreateBusinessCards();
    }
}
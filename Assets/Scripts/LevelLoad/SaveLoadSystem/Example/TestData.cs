using System;

namespace SaveLoadSystem.Example
{
    [Serializable]
    public struct TestData
    {
        public int Price { get; private set; }
        public bool IsBought { get; private set; }

        public TestData(int price, bool isBougth)
        {
            if (price < 0)
                throw new ArgumentOutOfRangeException($"{price} can't be less, than 0!");

            Price = price;
            IsBought = isBougth;
        }
    }
}
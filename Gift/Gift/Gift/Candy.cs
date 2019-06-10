using System;
using System.Collections.Generic;

namespace HomeWork
{
    public abstract class Candy
    {
        protected abstract string NameCandy { get; }
        protected Candy(uint sugarWeight, uint totalWeight)
        {
            if (sugarWeight > totalWeight)
            {
                throw new InvalidOperationException();
            }

            SugarWeight = sugarWeight;
            TotalWeight = totalWeight;
        }

        public uint SugarWeight { get; }
        public uint TotalWeight { get; }

        public override string ToString()
        {
            return $"{NameCandy} количество сахара: {SugarWeight}г общий вес: {TotalWeight}г"; 
        }
    }
}

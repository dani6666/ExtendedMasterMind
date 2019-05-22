using System;
using System.Collections.Generic;
using System.Text;

namespace ExtendedMasterMind
{
    class Iterator
    {
        public bool IsInFinalState
        {
            get
            {
                for (int i = 0; i < Value.Length; i++)
                {
                    if(Value[i]!=_maxValue)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public int[] Value { get; }

        private readonly int _maxValue;
        public Iterator(int dimension, int maximumFieldValue)
        {
            Value = new int[dimension];
            for (int i = 0; i < dimension; i++)
            {
                Value[i] = 0;
            }

            _maxValue = maximumFieldValue - 1;
        }

        public void InterateToNextState()
        {
            if(!IsInFinalState)
            {
                UpdateAtPosition(Value.Length - 1);
            }
        }

        private void UpdateAtPosition(int position)
        {
            if(Value[position] == _maxValue)
            {
                Value[position] = 0;
                UpdateAtPosition(position - 1);
            }
            else
            {
                Value[position]++;
            }
        }
    }
}

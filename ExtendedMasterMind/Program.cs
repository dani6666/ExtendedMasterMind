using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedMasterMind
{
    class Program
    {
        public static void UserCheck(int[] guess, ref int black, ref int white, int max)
        {
            foreach (int i in guess)
            {
                Console.Write("[");
                Console.Write(i + "] ");
            }
            Console.WriteLine("  ?");
            Console.WriteLine("Black:");
            while (!Int32.TryParse(Console.ReadLine(), out black) || black < 0 || black >= max)
            {
                Console.WriteLine("You must insert a non-negative number lower than given max value");
            }
            Console.WriteLine("White:");
            while (!Int32.TryParse(Console.ReadLine(), out white) || white < 0 || white >= max)
            {
                Console.WriteLine("You must insert a non-negative number lower than given max value");
            }

        }

        static void Main(string[] args)
        {
            int numberOfFields;
            Console.WriteLine("Insert number of columns of balls/nubers in game (normally 4)");
            while(!Int32.TryParse(Console.ReadLine(), out numberOfFields) || numberOfFields < 1)
            {
                Console.WriteLine("You must insert a positive number");
            }

            int maximumNumber;
            Console.WriteLine("Insert number that field will be able to constain (normally 6)");
            while (!Int32.TryParse(Console.ReadLine(), out maximumNumber) || maximumNumber < 1)
            {
                Console.WriteLine("You must insert a positive number");
            }

            List<int[]> possibleConfigurations = new List<int[]>();
            Iterator iterator = new Iterator(numberOfFields, maximumNumber);

            possibleConfigurations.Add(iterator.Value);
            do
            {
                iterator.InterateToNextState();
                int[] arrayToAdd = new int[numberOfFields];
                Array.Copy(iterator.Value, arrayToAdd, iterator.Value.Length);
                possibleConfigurations.Add(arrayToAdd);

            } while (!iterator.IsInFinalState);


            List<int[]> configurationsToDelete = new List<int[]>();
            int blackCorrect = 0, whiteCorrect = 0, roundCount = 0;

            int[] guess = new int[numberOfFields];
            for (int i = 0; i < numberOfFields; i++)
            {
                guess[i] = 0;
            }


            while (true)
            {
                UserCheck(guess, ref blackCorrect, ref whiteCorrect, maximumNumber);
                roundCount++;

                if (blackCorrect == numberOfFields)
                {
                    Console.WriteLine("WON IN " + roundCount);
                    break;
                }

                foreach (int[] configuration in possibleConfigurations)
                {
                    int blacksInCurrentConfiguration = 0, whitesInCurrentConfiguration = 0;
                    bool[] blackedPositions = new bool[numberOfFields], whitedPositions = new bool[numberOfFields];

                    #region blacks check
                    for (int i = 0; i < numberOfFields; i++)
                    {
                        blackedPositions[i] = false;
                        whitedPositions[i] = false;

                        if (configuration[i] == guess[i])
                        {
                            blacksInCurrentConfiguration++;
                            blackedPositions[i] = true;
                        }
                    }
                    #endregion

                    #region whites check
                    for (int i = 0; i < numberOfFields; i++)
                    {
                        if (!blackedPositions[i])
                        {
                            for (int j = 0; j < numberOfFields; j++)
                            {
                                if (!whitedPositions[j] && !blackedPositions[j] && guess[i] == configuration[j])
                                {
                                    whitesInCurrentConfiguration++;
                                    whitedPositions[j] = true;
                                }
                            }
                        }
                    }
                    #endregion

                    if (blackCorrect != blacksInCurrentConfiguration || whiteCorrect != whitesInCurrentConfiguration)
                    {
                        configurationsToDelete.Add(configuration);
                    }
                }

                #region removing configurationToDelte from possibleConfigurations
                foreach (int[] t in configurationsToDelete)
                {
                    possibleConfigurations.Remove(t);
                }
                configurationsToDelete.Clear();
                #endregion

                GC.Collect();

                if (possibleConfigurations.Count() == 0)
                {
                    Console.WriteLine("CHEATING");
                    break;
                }
                else
                {
                    guess = possibleConfigurations.First();
                }
            }

            Console.ReadKey();
        }
    }


}


using System;
using System.Collections;

public static class Utility
{

    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        System.Random prng = new System.Random(seed);

        for (int i = 0; i < array.Length - 1; i++)
        {
            int random = prng.Next(i, array.Length);
            T temp = array[random];
            array[random] = array[i];
            array[i] = temp;
        }

        return array;
    }

    public static int RandomNumber(int number)
    {
        Random random = new Random();
        int seed;
        if (number == 0)
        {
            seed = random.Next(0, 50);
        }
        else
            seed = random.Next(30, 60);
        return seed;

    }
}
 


using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine;
using HutongGames.PlayMaker;

namespace RngSync;

class Generators
{
    public static Dictionary<int, System.Random> randoms = new Dictionary<int, System.Random>();
    public static string seed = "1234";

    public static void ResetRng()
    {
        randoms = new Dictionary<int, System.Random>();
    }

    public static System.Random GetRandom(GameObject go)
    {
        int hashCode = (seed + go.name + GameManager.instance.sceneName).GetHashCode();

        if (!randoms.ContainsKey(hashCode))
        {
            System.Random rand = new System.Random(hashCode);
            randoms.Add(hashCode, rand);
        }

        System.Random outValue;
        randoms.TryGetValue(hashCode, out outValue);
        return outValue;
    }

    public static System.Random GetRandom(FsmFloat[] weights)
    {
        string weightsCombined = "";
        foreach (FsmFloat f in weights)
        {
            weightsCombined += $"{f}";
        }

        int hashCode = (seed + weightsCombined + GameManager.instance.sceneName).GetHashCode();

        if (!randoms.ContainsKey(hashCode))
        {
            System.Random rand = new System.Random(hashCode);
            randoms.Add(hashCode, rand);
        }

        System.Random outValue;
        randoms.TryGetValue(hashCode, out outValue);
        return outValue;
    }

    public static float GetRandomRange(System.Random rand, float min, float max)
    {
        return (float) (rand.NextDouble() * (max - min) + min);
    }

    public static float GetRandomRange(GameObject go, float min, float max)
    {
        System.Random rand = GetRandom(go);
        return GetRandomRange(rand, min, max);
    }

    public static int GetRandomRange(GameObject go, int min, int max)
    {
        System.Random rand = GetRandom(go);
        return rand.Next(min, max + 1);
    }

    public static int GetRandomWeightedIndex(FsmFloat[] weights)
    {
        System.Random rand = GetRandom(weights);
        float num = 0f;
        foreach (FsmFloat fsmFloat in weights)
        {
            num += fsmFloat.Value;
        }
        float num2 = GetRandomRange(rand, 0f, num);
        for (int j = 0; j < weights.Length; j++)
        {
            if (num2 < weights[j].Value)
            {
                return j;
            }
            num2 -= weights[j].Value;
        }
        return -1;
    }
}

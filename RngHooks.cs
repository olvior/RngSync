using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace RngSync;

class RngHooks
{
    public static void RandomFloat_OnEnter(On.HutongGames.PlayMaker.Actions.RandomFloat.orig_OnEnter orig, RandomFloat self)
    {
        GameObject go = self.Owner;

        float result = Generators.GetRandomRange(go, self.min.Value, self.max.Value);
        self.storeResult.Value = result;
        self.Finish();
    }

    public static void RandomFloatV2_Randomise(On.HutongGames.PlayMaker.Actions.RandomFloatV2.orig_Randomise orig, RandomFloatV2 self)
    {
        GameObject go = self.Owner;

        float result = Generators.GetRandomRange(go, self.min.Value, self.max.Value);
        self.storeResult.Value = result;
    }

    public static void WaitRandom_OnEnter(On.HutongGames.PlayMaker.Actions.WaitRandom.orig_OnEnter orig, WaitRandom self)
    {
        orig(self);

        GameObject go = self.Owner;

        float time = Generators.GetRandomRange(go, self.timeMin.Value, self.timeMax.Value);
        FieldInfo timeInfo = typeof(WaitRandom).GetField("time", BindingFlags.Instance | BindingFlags.NonPublic);
        timeInfo.SetValue(self, time);
    }

    public static void RandomWait_OnEnter(On.HutongGames.PlayMaker.Actions.RandomWait.orig_OnEnter orig, RandomWait self)
    {
        orig(self);

        GameObject go = self.Owner;

        float time = Generators.GetRandomRange(go, self.min.Value, self.max.Value);
        FieldInfo timeInfo = typeof(RandomWait).GetField("time", BindingFlags.Instance | BindingFlags.NonPublic);
        timeInfo.SetValue(self, time);
    }

    public static void RandomBool_OnEnter(On.HutongGames.PlayMaker.Actions.RandomBool.orig_OnEnter orig, RandomBool self)
    {
        GameObject go = self.Owner;

        bool result = Generators.GetRandomRange(go, 0, 1) == 0;
        self.storeResult.Value = result;
        self.Finish();
    }

    public static int ActionHelpers_GetRandomWeightedIndex(On.HutongGames.PlayMaker.ActionHelpers.orig_GetRandomWeightedIndex orig, FsmFloat[] weights)
    {
        return Generators.GetRandomWeightedIndex(weights);
    }

    public static void RandomFloatEither_OnEnter(On.HutongGames.PlayMaker.Actions.RandomFloatEither.orig_OnEnter orig, RandomFloatEither self)
    {
        GameObject go = self.Owner;

        if (Generators.GetRandomRange(go, 0, 1) == 0)
        {
            self.storeResult.Value = self.value1.Value;
        }
        else
        {
            self.storeResult.Value = self.value2.Value;
        }
        self.Finish();
    }

    public static void RandomInt_OnEnter(On.HutongGames.PlayMaker.Actions.RandomInt.orig_OnEnter orig, RandomInt self)
    {
        GameObject go = self.Owner;

        if (self.inclusiveMax)
        {
            self.storeResult.Value = Generators.GetRandomRange(go, self.min.Value, self.max.Value);
        }
        else
        {
            self.storeResult.Value = Generators.GetRandomRange(go, self.min.Value, self.max.Value - 1);
        }
        self.Finish();
    }

    public static void RandomlyFlipFloat_OnEnter(On.HutongGames.PlayMaker.Actions.RandomlyFlipFloat.orig_OnEnter orig, RandomlyFlipFloat self)
    {
        GameObject go = self.Owner;

        if (Generators.GetRandomRange(go, 0, 1) == 0)
        {
            self.storeResult.Value *= -1f;
        }
        self.Finish();
    }
}

// todo: DistanceWalk, FlingFlashingGeo, FireAtTarget, IdleBuzzV3, IdleBuzzV2, IdleBuzz, WalkLeftRight,

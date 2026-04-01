using Modding;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using HutongGames.PlayMaker.Actions;
using MonoMod.RuntimeDetour;
using UnityEngine;
using UnityEngine.UI;

namespace RngSync;

internal class RngSync : Mod, IGlobalSettings<GlobalSettings>
{
    internal static RngSync Instance { get; private set; }
    public static GlobalSettings GS {get; set;} = new GlobalSettings();

    public void OnLoadGlobal(GlobalSettings s) { GS = s; }
    public GlobalSettings OnSaveGlobal() { return GS; }

    public RngSync() : base("RngSync") { }

    public override string GetVersion()
    {
        return Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }

    public override void Initialize()
    {
        Instance = this;
        On.HutongGames.PlayMaker.Actions.RandomFloat.OnEnter += RngHooks.RandomFloat_OnEnter;
        On.HutongGames.PlayMaker.Actions.RandomFloatV2.Randomise += RngHooks.RandomFloatV2_Randomise;
        On.HutongGames.PlayMaker.Actions.WaitRandom.OnEnter += RngHooks.WaitRandom_OnEnter;
        On.HutongGames.PlayMaker.Actions.RandomWait.OnEnter += RngHooks.RandomWait_OnEnter;
        On.HutongGames.PlayMaker.Actions.RandomBool.OnEnter += RngHooks.RandomBool_OnEnter;
        On.HutongGames.PlayMaker.ActionHelpers.GetRandomWeightedIndex += RngHooks.ActionHelpers_GetRandomWeightedIndex;
        On.HutongGames.PlayMaker.Actions.RandomFloatEither.OnEnter += RngHooks.RandomFloatEither_OnEnter;
        On.HutongGames.PlayMaker.Actions.RandomInt.OnEnter += RngHooks.RandomInt_OnEnter;
        On.HutongGames.PlayMaker.Actions.RandomlyFlipFloat.OnEnter += RngHooks.RandomlyFlipFloat_OnEnter;

        Log("Initialised the hooks");


        string seed = GS.RngSeedOrBlankForRandom;
        if (seed == "")
        {
            seed = $"{Guid.NewGuid().GetHashCode()}";
            Log($"Detected empty seed, using random one: {seed}");
        }
        Log($"Seed is {seed}");
        Generators.seed = seed;
    }
}


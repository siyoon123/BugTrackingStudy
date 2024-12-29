using System;
using System.Collections.Generic;
// using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

public static class RoyalValue
{
    public static readonly Vector3 Vector3Zero = Vector3.zero;
    public static readonly Vector3 Vector3One = Vector3.one;
    public static readonly Vector3 Vector3Half = new Vector3(0.5f, 0.5f, 0.5f);
    public static readonly Vector3 Vector2Zero = Vector3.zero;
    public static readonly Vector3 Vector2One = Vector3.one;
    public static readonly Vector3 Vector2Half = new Vector2(0.5f, 0.5f);

    public static readonly Quaternion RotateBase = Quaternion.identity;


    public static readonly Color ColorRed = Color.red;
    public static readonly Color ColorBlack = Color.black;
    public static readonly Color ColorWhite = Color.white;
    public static readonly Color ColorWhiteClear = new Color(1, 1, 1, 0);
    public static readonly Color ColorGray = Color.gray;
    public static readonly Color ColorDarkGray = new Color(0.23f, 0.23f, 0.23f);
    public static readonly Color ColorNormal = Color.white;
    public static readonly Color ColorMagic = new Color(0.5137255f, 0.9372549f, 1f);
    public static readonly Color ColorEpic = new Color(0.9568627f, 0.6705883f, 1f);
    public static readonly Color ColorLegend = new Color(1f, 0.8313726f, 0.3764706f);

    public static readonly Color32 Color32Black = new Color32(0, 0, 0, 255);

    // public static readonly ObscuredInt SPAmount = 10;
    // public static readonly ObscuredInt MaxDiceLevel = 7;
    // public static readonly ObscuredInt KillSPAmount = 10;
    // public static readonly ObscuredInt[] UpgradeSPTable = { 100, 200, 400, 700 };
    //
    // public static readonly CriticalData DefaultCritical = new CriticalData();
    // public static readonly ObscuredFloat bulletSpeed = 10.0f;
    //
    // public static readonly ObscuredInt GoldenTrophy = 10000;
    // public static readonly ObscuredInt GoldenTrophyInterval = 200;
    
    public static readonly TimeSpan TimeSpanZero = TimeSpan.Zero;

    // public static  int[] StartDicePoolTable = new int[] { (int)DiceIDType.DICE_ELECT, (int)DiceIDType.DICE_IRON, (int)DiceIDType.DICE_BROKEN, (int)DiceIDType.DICE_ICE, (int)DiceIDType.DICE_LOCK };
    //
    // public static  List<List<DiceIDType>> DicePoolTable = new List<List<DiceIDType>>()
    // {
    //     new List<DiceIDType>() { DiceIDType.DICE_GIFT, DiceIDType.DICE_MODIFIED_ELECT, DiceIDType.DICE_GUN },
    //     new List<DiceIDType>() { DiceIDType.DICE_INFECT, DiceIDType.DICE_SUMMON, DiceIDType.DICE_LUNAN },
    //     new List<DiceIDType>() { DiceIDType.DICE_GROWTH, DiceIDType.DICE_ADJUST, DiceIDType.DICE_BOW },
    //     new List<DiceIDType>() { DiceIDType.DICE_LANDMINE, DiceIDType.DICE_CRACK, DiceIDType.DICE_BLIZZARD },
    //     new List<DiceIDType>() { DiceIDType.DICE_JOKER, DiceIDType.DICE_MINE, DiceIDType.DICE_SWORD },
    // };
    //Start Version
    public static readonly string Version = "HGPO.10.0.0";
    //End Version

}

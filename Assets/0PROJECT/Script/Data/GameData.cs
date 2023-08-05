using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/GameData", order = 1)]
public class GameData : ScriptableObject
{
    /*
    I KEEP ALL THE DATA I NEED TO SAVE IN THE GAME WITHIN THIS SCRIPTABLE OBJECT AND CALL IT HERE WHEN NECESSARY.
    */


    [SerializeField] private float totalCoin; 
    public float TotalCoin //ENCAPSULATING TO PREVENT VALUE DOWN BELOW ZERO
    {
        get { return totalCoin; }
        set
        {
            if (value < 0) totalCoin = 0;
            else totalCoin = value;
        }
    }

    public int LevelCount;
    public int UILevelCount;

    [Space(5)]
    [Header("Structs")]
    public Values values;
    public Bools bools;
    public Lists lists;


    [Serializable]
    public struct Values
    {
        public int perfectShootCounter;
        public int HighScore;
        public int LongestPerfectStreak;
    }

    [Serializable]
    public struct Bools
    {
        public bool _isPerfectShoot;
    }

    [Serializable]
    public struct Lists
    {
        public List<int> LevelPlatformCounts;
    }



    [Button]
    void ResetData()
    {
        TotalCoin = 0;
        LevelCount = 0;
        UILevelCount = 1;
        values.perfectShootCounter = 0;
        values.HighScore = 0;
        values.LongestPerfectStreak = 0;

        bools._isPerfectShoot = false;
    }

    void ResetList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = default(T);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [SerializeField] private float totalCoin; //################## TOTAL COIN
    public float TotalCoin
    {
        get { return totalCoin; }
        set
        {
            if (value < 0) totalCoin = 0;
            else totalCoin = value;
        }
    }

    [Space(5)]
    [Header("Structs")]
    public Values values;
    public Bools bools;
    public Lists lists;


    [Serializable]
    public struct Values
    {
        public int LevelCount;
        public int perfectShootCounter;
        public int LongestPerfectShoot;
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
        values.perfectShootCounter = 0;
        
        bools._isPerfectShoot = false;
    }

    [Button]
    void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }


    void ResetList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = default(T);
        }
    }

}

[System.Serializable]
public class Levels
{

}
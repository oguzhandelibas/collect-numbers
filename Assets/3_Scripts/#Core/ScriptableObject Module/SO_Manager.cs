using System;
using System.Collections.Generic;
using CollectNumbers;
using UnityEngine;

public enum SO_Type
{
    ColorData,
    GridSignals,
    LevelSignals
}

public static class SO_Manager
{
    private static readonly Dictionary<SO_Type, string> Paths = new Dictionary<SO_Type, string>
    {
        { SO_Type.ColorData, "ColorData"},
        { SO_Type.GridSignals, "Signals/SD_GridSignals"},
        { SO_Type.LevelSignals, "Signals/SD_LevelSignals"}
    };

    private static readonly Dictionary<SO_Type, ScriptableObject> _cache = new Dictionary<SO_Type, ScriptableObject>();

    public static T Load_SO<T>() where T : ScriptableObject
    {
        var type = Get_SO_Type<T>();
        return Load_SO<T>(type);
    }
    
    private static T Load_SO<T>(SO_Type type) where T : ScriptableObject
    {
        if (_cache.TryGetValue(type, out var value))
        {
            return value as T;
        }

        if (Paths.TryGetValue(type, out string path))
        {
            T scriptableObject = Resources.Load<T>(path);
            if (scriptableObject != null)
            {
                _cache[type] = scriptableObject;
                return scriptableObject;
            }
            else
            {
                Debug.LogError($"ScriptableObject of type {type} at path {path} could not be loaded.");
            }
        }
        else
        {
            Debug.LogError($"Path for ScriptableObject of type {type} not found.");
        }

        return null;
    }

    private static SO_Type Get_SO_Type<T>() where T : ScriptableObject
    {
        if (typeof(T) == typeof(ColorData))
            return SO_Type.ColorData;
        if (typeof(T) == typeof(GridSignals))
            return SO_Type.GridSignals;
        if (typeof(T) == typeof(LevelSignals))
            return SO_Type.LevelSignals;
        
        throw new ArgumentException($"Unsupported ScriptableObject type: {typeof(T)}");
    }
}
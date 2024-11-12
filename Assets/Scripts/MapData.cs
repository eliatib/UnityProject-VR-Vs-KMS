using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapData
{
    public List<PinData> pins;
}

[Serializable]
public class PinData
{
    public string pinType;
    public Vector3 position;
}
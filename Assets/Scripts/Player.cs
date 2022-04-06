using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPropertyTakenData
{
    public List<House> houseList;
    public List<SpecialBuildings> specialBuildingList;
}
public class Player : MonoBehaviour
{
    [HideInInspector]
    public PlayerState Owner;

    public PlayerPropertyTakenData propertyTakenData;

}

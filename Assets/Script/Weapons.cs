using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Scriptable Objects/Weapons")]
public class Weapons : ScriptableObject
{
    public float cd;
    public float damage;
    public float mag;
    public float maxMag;
    public float reserve;
    public float reloadTime;

    public Weapons Clone()
    {
        return Instantiate(this);
    }
}

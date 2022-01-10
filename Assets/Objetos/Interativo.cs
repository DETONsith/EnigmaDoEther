using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Interativo", menuName ="ObjInterativo")]
public class Interativo : ScriptableObject
{
    public int id;
    public int level;

    public string value;

    public AudioClip audLook;
    public AudioClip audSolo;


    public Interativo(int aid, int alevel, string avalue, AudioClip aaudlook, AudioClip aaudsolo)
    {
        this.id = aid;
        this.level = alevel;
        this.value = avalue;
        this.audLook = aaudlook;
        this.audSolo = aaudsolo;
    }

    public void DefineValues(int aid, int alevel, string avalue, AudioClip aaudlook, AudioClip aaudsolo)
    {
        this.id = aid;
        this.level = alevel;
        this.value = avalue;
        this.audLook = aaudlook;
        this.audSolo = aaudsolo;
    }


    public int getId()
    {
        return this.id;
    }
    

    public int getLevel()
    {
        return this.level;
    }


    public string getValue()
    {
        return this.value;
    }


    public AudioClip getLook()
    {
        return this.audLook;
    }
    public AudioClip getSolo()
    {
        return this.audSolo;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipContainer : MonoBehaviour
{
    Parameters statsReference;
    UnitData unitData;

    private void Start()
    {
        statsReference = gameObject.GetComponent<Parameters>();
        unitData = gameObject.GetComponent<UnitData>();

    }


    public void Equip(Weapon w)
    {
        if(unitData.UnitWeapon != null)
            Unequip(unitData.UnitWeapon);

        unitData.UnitWeapon = w;

        statsReference.AddModifier(new ModApplication(w.hp_mod_type, StatTypes.MHP, w.hp_mod));
        statsReference.AddModifier(new ModApplication(w.mp_mod_type, StatTypes.MMP, w.mp_mod));
        statsReference.AddModifier(new ModApplication(w.atk_mod_type, StatTypes.ATK, w.atk_mod));
        statsReference.AddModifier(new ModApplication(w.def_mod_type, StatTypes.DEF, w.def_mod));
        statsReference.AddModifier(new ModApplication(w.spr_mod_type, StatTypes.SPR, w.spr_mod));
        statsReference.AddModifier(new ModApplication(w.spd_mod_type, StatTypes.SPD, w.spd_mod));
        statsReference.AddModifier(new ModApplication(w.mov_mod_type, StatTypes.MOV, w.mov_mod));
        statsReference.AddModifier(new ModApplication(w.jmp_mod_type, StatTypes.JMP, w.jmp_mod));
    }
    

    public void Equip(Armor a)
    {
        if (unitData.UnitArmor != null)
            Unequip(unitData.UnitArmor);

        unitData.UnitArmor = a;

        statsReference.AddModifier(new ModApplication(a.hp_mod_type, StatTypes.MHP, a.hp_mod));
        statsReference.AddModifier(new ModApplication(a.mp_mod_type, StatTypes.MMP, a.mp_mod));
        statsReference.AddModifier(new ModApplication(a.atk_mod_type, StatTypes.ATK, a.atk_mod));
        statsReference.AddModifier(new ModApplication(a.def_mod_type, StatTypes.DEF, a.def_mod));
        statsReference.AddModifier(new ModApplication(a.spr_mod_type, StatTypes.SPR, a.spr_mod));
        statsReference.AddModifier(new ModApplication(a.spd_mod_type, StatTypes.SPD, a.spd_mod));
        statsReference.AddModifier(new ModApplication(a.mov_mod_type, StatTypes.MOV, a.mov_mod));
        statsReference.AddModifier(new ModApplication(a.jmp_mod_type, StatTypes.JMP, a.jmp_mod));
    }


    public void Unequip(Weapon w)
    {
        statsReference.RemoveModifier(new ModApplication(w.hp_mod_type, StatTypes.MHP, w.hp_mod));
        statsReference.RemoveModifier(new ModApplication(w.mp_mod_type, StatTypes.MMP, w.mp_mod));
        statsReference.RemoveModifier(new ModApplication(w.atk_mod_type, StatTypes.ATK, w.atk_mod));
        statsReference.RemoveModifier(new ModApplication(w.def_mod_type, StatTypes.DEF, w.def_mod));
        statsReference.RemoveModifier(new ModApplication(w.spr_mod_type, StatTypes.SPR, w.spr_mod));
        statsReference.RemoveModifier(new ModApplication(w.spd_mod_type, StatTypes.SPD, w.spd_mod));
        statsReference.RemoveModifier(new ModApplication(w.mov_mod_type, StatTypes.MOV, w.mov_mod));
        statsReference.RemoveModifier(new ModApplication(w.jmp_mod_type, StatTypes.JMP, w.jmp_mod));

        unitData.UnitWeapon = null;
    }

    public void Unequip(Armor a)
    {
        statsReference.RemoveModifier(new ModApplication(a.hp_mod_type, StatTypes.MHP, a.hp_mod));
        statsReference.RemoveModifier(new ModApplication(a.mp_mod_type, StatTypes.MMP, a.mp_mod));
        statsReference.RemoveModifier(new ModApplication(a.atk_mod_type, StatTypes.ATK, a.atk_mod));
        statsReference.RemoveModifier(new ModApplication(a.def_mod_type, StatTypes.DEF, a.def_mod));
        statsReference.RemoveModifier(new ModApplication(a.spr_mod_type, StatTypes.SPR, a.spr_mod));
        statsReference.RemoveModifier(new ModApplication(a.spd_mod_type, StatTypes.SPD, a.spd_mod));
        statsReference.RemoveModifier(new ModApplication(a.mov_mod_type, StatTypes.MOV, a.mov_mod));
        statsReference.RemoveModifier(new ModApplication(a.jmp_mod_type, StatTypes.JMP, a.jmp_mod));

        unitData.UnitArmor = null;
    }
}

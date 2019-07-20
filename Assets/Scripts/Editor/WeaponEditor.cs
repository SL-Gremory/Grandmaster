using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{

    private Weapon targetWeapon;

    void OnEnable()
    {
        targetWeapon = (Weapon)this.target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginVertical();

        targetWeapon.name = EditorGUILayout.TextField("Name", targetWeapon.name);
        targetWeapon.description = EditorGUILayout.TextField("Description", targetWeapon.description);
        targetWeapon.rarity = (Rarity)EditorGUILayout.EnumPopup("Rarity", targetWeapon.rarity);

        targetWeapon.type = (WeaponType)EditorGUILayout.EnumPopup("Weapon Type", targetWeapon.type);

        GUI.enabled = IsWeaponTypeSelected();
        CheckWeaponTypeSelection();

        GUI.enabled = IsWeaponAttributeSelected();
        EnableStatsEdit();


        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(target);
    }

    private bool IsWeaponAttributeSelected()
    {
        bool a = (targetWeapon.rpAttribute != RangedPhysicalAttribute.NONE) ? true : false;
        bool b = (targetWeapon.rmAttribute != RangedMagicalAttribute.NONE) ? true : false;
        bool c = (targetWeapon.mpAttribute != MeleePhysicalAttribute.NONE) ? true : false;
        bool d = (targetWeapon.maAttribute != MeleeAuraAttribute.NONE) ? true : false;
        bool e = IsWeaponTypeSelected();


        return (e && (a || b || c || d));

    }

    private bool IsWeaponTypeSelected()
    {
        return (targetWeapon.type != WeaponType.NONE) ? true : false;
    }

    private void CheckWeaponTypeSelection()
    {
        switch (targetWeapon.type)
        {
            case WeaponType.RANGED_PHYSICAL:
                targetWeapon.rpAttribute = (RangedPhysicalAttribute)EditorGUILayout.EnumPopup("Attribute", targetWeapon.rpAttribute);
                targetWeapon.rmAttribute = RangedMagicalAttribute.NONE;
                targetWeapon.mpAttribute = MeleePhysicalAttribute.NONE;
                targetWeapon.maAttribute = MeleeAuraAttribute.NONE;
                break;
            case WeaponType.RANGED_MAGICAL:
                targetWeapon.rpAttribute = RangedPhysicalAttribute.NONE;
                targetWeapon.rmAttribute = (RangedMagicalAttribute)EditorGUILayout.EnumPopup("Attribute", targetWeapon.rmAttribute);
                targetWeapon.mpAttribute = MeleePhysicalAttribute.NONE;
                targetWeapon.maAttribute = MeleeAuraAttribute.NONE;
                break;
            case WeaponType.MELEE_PHYSICAL:
                targetWeapon.rpAttribute = RangedPhysicalAttribute.NONE;
                targetWeapon.rmAttribute = RangedMagicalAttribute.NONE;
                targetWeapon.mpAttribute = (MeleePhysicalAttribute)EditorGUILayout.EnumPopup("Attribute", targetWeapon.mpAttribute);
                targetWeapon.maAttribute = MeleeAuraAttribute.NONE;
                break;
            case WeaponType.MELEE_AURA:
                targetWeapon.rpAttribute = RangedPhysicalAttribute.NONE;
                targetWeapon.rmAttribute = RangedMagicalAttribute.NONE;
                targetWeapon.mpAttribute = MeleePhysicalAttribute.NONE;
                targetWeapon.maAttribute = (MeleeAuraAttribute)EditorGUILayout.EnumPopup("Attribute", targetWeapon.maAttribute);
                break;
        }
    }

    private void EnableStatsEdit()
    {
        targetWeapon.range = EditorGUILayout.IntField("Range", targetWeapon.range);

        targetWeapon.hp_mod = EditorGUILayout.IntField("HP", targetWeapon.hp_mod);
        targetWeapon.hp_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetWeapon.hp_mod_type);

        targetWeapon.mp_mod = EditorGUILayout.IntField("MP", targetWeapon.mp_mod);
        targetWeapon.mp_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetWeapon.mp_mod_type);

        targetWeapon.atk_mod = EditorGUILayout.IntField("ATK", targetWeapon.atk_mod);
        targetWeapon.atk_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetWeapon.atk_mod_type);

        targetWeapon.def_mod = EditorGUILayout.IntField("DEF", targetWeapon.def_mod);
        targetWeapon.def_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetWeapon.def_mod_type);

        targetWeapon.spr_mod = EditorGUILayout.IntField("SPR", targetWeapon.spr_mod);
        targetWeapon.spr_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetWeapon.spr_mod_type);

        targetWeapon.spd_mod = EditorGUILayout.IntField("SPD", targetWeapon.spd_mod);
        targetWeapon.spd_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetWeapon.spd_mod_type);

        targetWeapon.mov_mod = EditorGUILayout.IntField("MOV", targetWeapon.mov_mod);
        targetWeapon.mov_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetWeapon.mov_mod_type);

        targetWeapon.jmp_mod = EditorGUILayout.IntField("JMP", targetWeapon.jmp_mod);
        targetWeapon.jmp_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetWeapon.jmp_mod_type);
    }
}
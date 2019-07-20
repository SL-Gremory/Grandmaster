using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Armor))]
public class ArmorEditor : Editor
{

    private Armor targetArmor;

    void OnEnable()
    {
        targetArmor = (Armor)this.target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginVertical();

        targetArmor.name = EditorGUILayout.TextField("Name", targetArmor.name);
        targetArmor.description = EditorGUILayout.TextField("Description", targetArmor.description);
        targetArmor.rarity = (Rarity)EditorGUILayout.EnumPopup("Rarity", targetArmor.rarity);


        EnableStatsEdit();


        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(target);
    }


    private void EnableStatsEdit()
    {

        targetArmor.hp_mod = EditorGUILayout.IntField("HP", targetArmor.hp_mod);
        targetArmor.hp_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetArmor.hp_mod_type);

        targetArmor.mp_mod = EditorGUILayout.IntField("MP", targetArmor.mp_mod);
        targetArmor.mp_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetArmor.mp_mod_type);

        targetArmor.atk_mod = EditorGUILayout.IntField("ATK", targetArmor.atk_mod);
        targetArmor.atk_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetArmor.atk_mod_type);

        targetArmor.def_mod = EditorGUILayout.IntField("DEF", targetArmor.def_mod);
        targetArmor.def_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetArmor.def_mod_type);

        targetArmor.spr_mod = EditorGUILayout.IntField("SPR", targetArmor.spr_mod);
        targetArmor.spr_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetArmor.spr_mod_type);

        targetArmor.spd_mod = EditorGUILayout.IntField("SPD", targetArmor.spd_mod);
        targetArmor.spd_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetArmor.spd_mod_type);

        targetArmor.mov_mod = EditorGUILayout.IntField("MOV", targetArmor.mov_mod);
        targetArmor.mov_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetArmor.mov_mod_type);

        targetArmor.jmp_mod = EditorGUILayout.IntField("JMP", targetArmor.jmp_mod);
        targetArmor.jmp_mod_type = (StatModType)EditorGUILayout.EnumPopup("Mod Type", targetArmor.jmp_mod_type);
    }
}
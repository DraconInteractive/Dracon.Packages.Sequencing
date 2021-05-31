using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DI_Sequences
{
    [CustomPropertyDrawer(typeof(Sequence))]
    public class SequenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            
            //var allTypes = Enum.GetValues(typeof(Sequence.ActionType));
            //string[] allTypeNames = Enum.GetNames(typeof(Sequence.ActionType));
            var type = (Sequence.ActionType)EditorGUILayout.EnumPopup("Create Sequence: ", Sequence.ActionType.SelectType);
            if (type != Sequence.ActionType.SelectType)
            {
                //The name of the list, List<IAction> actions
                var prop = property.FindPropertyRelative("actions");

                switch (type)
                {
                    case Sequence.ActionType.SelectType:
                        break;
                    case Sequence.ActionType.Debug:
                        SerializedProperty newAction = prop.GetArrayElementAtIndex(prop.arraySize++);
                        newAction.managedReferenceValue = new DebugAction();
                        break;
                    case Sequence.ActionType.MoveTransform:
                        break;
                }
            }
        }
    }
}
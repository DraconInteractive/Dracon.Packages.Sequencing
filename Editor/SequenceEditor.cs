using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DI_Sequences
{
    //var allTypes = Enum.GetValues(typeof(Sequence.ActionType));
 //string[] allTypeNames = Enum.GetNames(typeof(Sequence.ActionType));
    [CustomPropertyDrawer(typeof(Sequence))]
    public class SequenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty prop = property.FindPropertyRelative("actions");

            Rect propRect = new Rect(position.x, position.y, position.width, position.height);
            Rect enumRect = new Rect(position.x, position.y - 30, position.width, position.height);

            EditorGUI.PropertyField(propRect, prop);

            var type = (Sequence.ActionType)EditorGUI.EnumPopup(enumRect, "Create Sequence: ", Sequence.ActionType.SelectType);
            if (type != Sequence.ActionType.SelectType)
            {                
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

            EditorGUI.EndProperty();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DI_Sequences
{ 
    [CustomPropertyDrawer(typeof(Sequence))]
    public class SequenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty prop = property.FindPropertyRelative("actions");

            float propHeight = EditorGUI.GetPropertyHeight(prop, true);
            Rect propRect = new Rect(position.x, position.y, position.width, propHeight);
            
            EditorGUI.PropertyField(propRect, prop, true);
            if (prop.isExpanded)
            {
                Rect enumRect = new Rect(position.x, position.y + propHeight + EditorGUIUtility.standardVerticalSpacing, position.width, 50);

                var type = (Sequence.ActionType)EditorGUI.EnumPopup(enumRect, "Create Sequence: ", Sequence.ActionType.SelectType);
                if (type != Sequence.ActionType.SelectType)
                {
                    SerializedProperty newAction = prop.GetArrayElementAtIndex(prop.arraySize++);
                    switch (type)
                    {
                        case Sequence.ActionType.Debug:
                            newAction.managedReferenceValue = new DebugAction();
                            break;
                        case Sequence.ActionType.MoveTransform:
                            newAction.managedReferenceValue = new MoveTransformAction();
                            break;
                        case Sequence.ActionType.ScaleTransform:
                            newAction.managedReferenceValue = new ScaleTransformAction();
                            break;
                        case Sequence.ActionType.WaitDuration:
                            newAction.managedReferenceValue = new WaitDurationAction();
                            break;
                        case Sequence.ActionType.TransferStage:
                            newAction.managedReferenceValue = new TransferStageAction();
                            break;
                        case Sequence.ActionType.UnityEvent:
                            newAction.managedReferenceValue = new UnityEventAction();
                            break;
                    }
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty prop = property.FindPropertyRelative("actions");

            return EditorGUI.GetPropertyHeight(prop, true) + EditorGUIUtility.standardVerticalSpacing + (prop.isExpanded ? EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing : 0);
        }
    }
}
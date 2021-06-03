using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
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
                Rect selectionRect = new Rect(position.x, position.y + propHeight + EditorGUIUtility.standardVerticalSpacing, position.width, 50);

                if (GUI.Button(selectionRect, new GUIContent("Add Sequence"), EditorStyles.toolbarButton))
                {
                    var dropdown = new ActionsDropdown(new AdvancedDropdownState());
                    dropdown.prop = prop;
                    dropdown.Show(selectionRect);
                }  
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty prop = property.FindPropertyRelative("actions");

            return EditorGUI.GetPropertyHeight(prop, true) + EditorGUIUtility.standardVerticalSpacing + (prop.isExpanded ? EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing : 0);
        }

        class ActionsDropdown : AdvancedDropdown
        {
            public SerializedProperty prop;
            public ActionsDropdown(AdvancedDropdownState state) : base(state)
            { }

            protected override AdvancedDropdownItem BuildRoot()
            {
                var root = new AdvancedDropdownItem("Main");

                foreach (var v in TypeCache.GetTypesDerivedFrom<SequenceAction>().ToArray())
                {
                    root.AddChild(new AdvancedDropdownItem(v.FullName));
                }
                return root;
            }

            protected override void ItemSelected(AdvancedDropdownItem item)
            {
                base.ItemSelected(item);

                Debug.Log("Selected " + item.name);
                Debug.Log("ID: " + item.id);
                SerializedProperty newAction = prop.GetArrayElementAtIndex(prop.arraySize++);
                Type t = Type.GetType(item.name);
                newAction.managedReferenceValue = Activator.CreateInstance(t);
            }
        }
    }
}
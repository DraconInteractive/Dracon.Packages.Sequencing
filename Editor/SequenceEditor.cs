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
        List<string> toAdd = new List<string>();
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty prop = property.FindPropertyRelative("actions");
            
            float propHeight = EditorGUI.GetPropertyHeight(prop, true);
            Rect propRect = new Rect(position.x, position.y, position.width, propHeight);

            EditorGUILayout.TextField(label.text);
            EditorGUI.PropertyField(propRect, prop, true);
            if (prop.isExpanded)
            {
                Rect selectionRect = new Rect(position.x, position.y + propHeight + EditorGUIUtility.standardVerticalSpacing, position.width, 50);

                if (GUI.Button(selectionRect, new GUIContent("Add Sequence"), EditorStyles.toolbarButton))
                {
                    var dropdown = new ActionsDropdown(new AdvancedDropdownState());
                    dropdown.onSelected += (x =>
                    {
                        toAdd.Add(x);
                    });
                    
                    dropdown.Show(selectionRect);
                }  
            }
            AddFromQueue(prop);
            EditorGUI.EndProperty();
        }

        public void AddFromQueue (SerializedProperty prop)
        {
            foreach (var s in toAdd)
            {
                SerializedProperty newAction = prop.GetArrayElementAtIndex(prop.arraySize++);
                Type t = GetTypesAdvanced(s);
                if (t == null)
                {
                    Debug.LogError($"Type creation attempt from {s} resulted in null type.");
                    newAction.managedReferenceValue = new SequenceAction();
                    continue;
                }
                newAction.managedReferenceValue = Activator.CreateInstance(t);
            }
            toAdd.Clear();
        }

        public Type GetTypesAdvanced (string fullName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                var types = assemblies[i].GetTypes();
                foreach (var t in types)
                {
                    if (t.FullName == fullName)
                    {
                        return t;
                    }
                }
            }
            return null;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty prop = property.FindPropertyRelative("actions");

            return EditorGUI.GetPropertyHeight(prop, true) + EditorGUIUtility.standardVerticalSpacing + (prop.isExpanded ? EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing : 0);
        }

        class ActionsDropdown : AdvancedDropdown
        {
            public delegate void OnSelected(string item);
            public OnSelected onSelected;

            public ActionsDropdown(AdvancedDropdownState state) : base(state)
            {
                
            }

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

                onSelected?.Invoke(item.name);
            }
        }
    }
}
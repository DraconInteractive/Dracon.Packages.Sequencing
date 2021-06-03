using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                Rect selectionRect = new Rect(position.x, position.y + propHeight + EditorGUIUtility.standardVerticalSpacing, position.width, 50);

                List<string> options = new List<string>() { "Select Action" };
                Array.ForEach(GetInheritedClasses(typeof(SequenceAction)), x => options.Add(x.ToString()));

                int i = EditorGUI.Popup(selectionRect, "Create Sequence: ", 0, options.ToArray());

                if (i != 0)
                {
                    SerializedProperty newAction = prop.GetArrayElementAtIndex(prop.arraySize++);
                    newAction.managedReferenceValue = GetInstance(options[i]);
                    /*
                    switch (type)
                    {
                        case Sequence.ActionType.Debug:
                            newAction.managedReferenceValue = new DebugAction();
                            break;
                    }*/
                }
            }

            EditorGUI.EndProperty();
        }

        public object GetInstance(string strFullyQualifiedName)
        {
            Type t = Type.GetType(strFullyQualifiedName);
            return Activator.CreateInstance(t);
        }

        Type[] GetInheritedClasses(Type MyType)
        {
            //if you want the abstract classes drop the !TheType.IsAbstract but it is probably to instance so its a good idea to keep it.
            return Assembly.GetAssembly(MyType).GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(MyType)).ToArray();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty prop = property.FindPropertyRelative("actions");

            return EditorGUI.GetPropertyHeight(prop, true) + EditorGUIUtility.standardVerticalSpacing + (prop.isExpanded ? EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing : 0);
        }
    }
}
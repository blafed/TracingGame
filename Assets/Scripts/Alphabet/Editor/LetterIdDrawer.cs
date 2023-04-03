// using UnityEngine;
// using UnityEditor;

// [CustomPropertyDrawer(typeof(LetterId))]
// public class LetterIdDrawer : PropertyDrawer
// {
//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//     {
//         //draws the letterId field as a popup of all the letters
//         var valueProp = property.FindPropertyRelative("value");
//         valueProp.intValue = EditorGUI.Popup(position, label.text, property.intValue, LetterUtility.listAllLetters());
//     }
// }
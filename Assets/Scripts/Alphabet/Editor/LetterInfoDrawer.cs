// using UnityEngine;
// using UnityEditor;
// using System;

// [CustomPropertyDrawer(typeof(LetterInfo))]
// public class LetterInfoDrawer : PropertyDrawer
// {


//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//     {

//         //draw letter info property that will has the following fields:
//         //letterId: int
//         //clip: AudioClip
//         //words: List<string>

//         //draws the letterId field as a popup of all the Letter

//         var letterIdProp = property.FindPropertyRelative("letterId");
//         //draw

//         var clipProp = property.FindPropertyRelative("clip");
//         var wordsProp = property.FindPropertyRelative("words");

//         var letterId = letterIdProp.intValue;
//         var clip = clipProp.objectReferenceValue as AudioClip;

//         var rect = position;
//         rect.height = EditorGUIUtility.singleLineHeight;

//         EditorGUI.BeginProperty(rect, label, property);
//         EditorGUI.BeginChangeCheck();

//         //draw letterId
//         var allLettersAsString = new string[LetterUtility.sizeSet];
//         for (int i = 0; i < LetterUtility.sizeSet; i++)
//         {
//             allLettersAsString[i] = LetterUtility.letterToChar(i).ToString();
//         }
//         letterId = EditorGUI.Popup(rect, letterId, allLettersAsString);

//         //draw clip
//         rect.y += EditorGUIUtility.singleLineHeight;
//         clip = EditorGUI.ObjectField(rect, clip, typeof(AudioClip), false) as AudioClip;





//     }
// }
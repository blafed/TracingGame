using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
[CustomPropertyDrawer(typeof(TweenInfo))]
public class TweenInfoDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        var type_prop = property.FindPropertyRelative("type");
        var type_field = new EnumField("Type", default(TweenInfo.Type));
        var type = (TweenInfo.Type)type_field.value;
        container.Add(type_field);




        switch (type)
        {
            case TweenInfo.Type.tween:
                container.Add(new PropertyField(property.FindPropertyRelative(nameof(TweenInfo.loopType))));
                container.Add(new PropertyField(property.FindPropertyRelative(nameof(TweenInfo.loops))));
                break;
            case TweenInfo.Type.invokeScripts:
                container.Add(new PropertyField(property.FindPropertyRelative(nameof(TweenInfo.startScripts))));
                break;
            case TweenInfo.Type.delay:
                container.Add(new PropertyField(property.FindPropertyRelative(nameof(TweenInfo.delay))));
                break;
        }

        return container;
    }

    // public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    // {
    //     CreatePropertyGUI(property);
    // }
}
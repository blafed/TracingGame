using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
[CanEditMultipleObjects]
[CustomEditor(typeof(TweenScript))]
public class TweenScriptEditor : Editor
{

    public override VisualElement CreateInspectorGUI()
    {
        var container = new VisualElement();

        // If you're running a recent version of the package, or 2021.2, you can use
        // InspectorElement.FillDefaultInspector(container, serializedObject, this);
        // otherwise, the code below basically does the same thing

        SerializedProperty property = serializedObject.GetIterator();
        if (property.NextVisible(true)) // Expand first child.
        {
            do
            {
                var field = new PropertyField(property);
                field.name = "PropertyField:" + property.propertyPath;

                if (property.propertyPath == "m_Script")
                {
                    if ((serializedObject.targetObject != null))
                        field.SetEnabled(false);
                }

                container.Add(field);
            }
            while (property.NextVisible(false));
        }

        return container;
    }
}
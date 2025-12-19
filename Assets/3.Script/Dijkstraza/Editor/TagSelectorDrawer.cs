using UnityEngine;
using UnityEditor; // 이 네임스페이스가 필수입니다.

[CustomPropertyDrawer(typeof(TagSelectorAttribute))]
public class TagSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 변수가 문자열(string)일 경우에만 작동하도록 함
        if (property.propertyType == SerializedPropertyType.String)
        {
            // 인스펙터에 태그 선택 필드를 그려줍니다.
            // EditorGUI.TagField는 유니티 내장 기능으로 태그 목록을 불러옵니다.
            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        }
        else
        {
            // 문자열이 아니면 기본 스타일로 그립니다.
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
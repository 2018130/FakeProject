using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;

public class KeyVariableModifier : EditorWindow
{
    private string searchKeyword = "Key";

    private struct SearchResult
    {
        public MonoBehaviour targetComponent;
        public FieldInfo fieldInfo;
        public string newValue;
    }

    private List<SearchResult> results = new List<SearchResult>();
    private Vector2 scrollPos;

    [MenuItem("Tools/Key Variable Modifier")]
    public static void ShowWindow() => GetWindow<KeyVariableModifier>("변수 수정 및 참조");

    void OnGUI()
    {
        EditorGUILayout.BeginVertical("helpbox");
        searchKeyword = EditorGUILayout.TextField("찾을 변수 키워드:", searchKeyword);

        if (GUILayout.Button("씬 내 변수 검색", GUILayout.Height(30)))
        {
            SearchVariables();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < results.Count; i++)
        {
            var res = results[i];

            // Missing 체크 (씬에서 오브젝트가 삭제된 경우 대비)
            if (res.targetComponent == null) continue;

            EditorGUILayout.BeginVertical("box");

            // --- 오브젝트 참조 섹션 ---
            EditorGUILayout.BeginHorizontal();
            // 1. 실제 오브젝트를 필드 형태로 표시 (누르면 바로 이동됨)
            EditorGUILayout.ObjectField("대상 오브젝트:", res.targetComponent.gameObject, typeof(GameObject), true);

            // 2. 명시적인 '찾기' 버튼 추가
            if (GUILayout.Button("위치 찾기", GUILayout.Width(70)))
            {
                Selection.activeGameObject = res.targetComponent.gameObject;
                EditorGUIUtility.PingObject(res.targetComponent.gameObject);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField($"스크립트: {res.targetComponent.GetType().Name} | 변수: {res.fieldInfo.Name}");

            // --- 값 수정 섹션 ---
            object currentVal = res.fieldInfo.GetValue(res.targetComponent);

            EditorGUILayout.BeginHorizontal();
            results[i] = new SearchResult
            {
                targetComponent = res.targetComponent,
                fieldInfo = res.fieldInfo,
                newValue = EditorGUILayout.TextField($"수정 (현재: {currentVal})", res.newValue)
            };

            if (GUILayout.Button("적용", GUILayout.Width(50)))
            {
                ApplyValue(results[i]);
                GUI.FocusControl(null); // 입력 후 포커스 해제
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }

        EditorGUILayout.EndScrollView();
    }

    private void SearchVariables()
    {
        results.Clear();
        MonoBehaviour[] scripts = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (var script in scripts)
        {
            if (script == null) continue;

            Type type = script.GetType();
            // 상속받은 private 필드까지 찾고 싶다면 FlattenHierarchy 추가 가능
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (field.Name.IndexOf(searchKeyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(new SearchResult
                    {
                        targetComponent = script,
                        fieldInfo = field,
                        newValue = field.GetValue(script)?.ToString() ?? ""
                    });
                }
            }
        }
    }

    private void ApplyValue(SearchResult res)
    {
        try
        {
            object convertedValue = Convert.ChangeType(res.newValue, res.fieldInfo.FieldType);
            Undo.RecordObject(res.targetComponent, "Change Variable Value");
            res.fieldInfo.SetValue(res.targetComponent, convertedValue);
            EditorUtility.SetDirty(res.targetComponent);
            Debug.Log($"<color=cyan>{res.targetComponent.name}</color>의 <color=yellow>{res.fieldInfo.Name}</color> 값이 변경됨.");
        }
        catch (Exception)
        {
            Debug.LogError($"{res.fieldInfo.FieldType.Name} 형식에 맞는 값을 입력하세요.");
        }
    }
}
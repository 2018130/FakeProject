using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class SaveStateViewerWindow : EditorWindow
{
    // 씬에서 찾은 모든 SaveState 컴포넌트를 저장할 리스트
    private SaveState[] allSaveStates;

    // 스크롤 위치를 기억하는 변수
    private Vector2 scrollPosition;

    // 1. 유니티 메뉴에 창을 여는 항목 추가
    [MenuItem("Window/Custom Tools/All SaveStates Viewer")]
    public static void ShowWindow()
    {
        // 창을 가져오거나 새로 만들고 표시합니다.
        GetWindow<SaveStateViewerWindow>("All SaveStates Viewer").Show();
    }

    // 창이 열리거나 활성화될 때 (또는 메뉴를 클릭했을 때) 호출되어 오브젝트를 검색합니다.
    private void OnEnable()
    {
        FindAllSaveStatesInScene();
    }

    // 씬의 모든 SaveState 컴포넌트를 검색하는 핵심 함수
    private void FindAllSaveStatesInScene()
    {
        // 현재 씬의 모든 SaveState 컴포넌트를 찾습니다.
        // 유니티 2020.1 이후 버전부터는 FindObjectsOfType<T>() 대신 FindObjectsOfType<T>(bool includeInactive)를 사용하는 것이 좋습니다.
        // 여기서는 기본적으로 씬에서 활성화된 오브젝트만 찾습니다.
        allSaveStates = FindObjectsByType<SaveState>(FindObjectsSortMode.None);

        // 찾은 개수를 콘솔에 표시
        Debug.Log($"씬에서 {allSaveStates.Length}개의 SaveState 오브젝트를 찾았습니다.");
    }

    // 2. 창의 GUI를 그리는 함수
    private void OnGUI()
    {
        GUILayout.Label("현재 씬의 모든 SaveState 목록", EditorStyles.boldLabel);

        // 검색 버튼
        if (GUILayout.Button("씬 오브젝트 다시 검색 (Refresh)"))
        {
            FindAllSaveStatesInScene();
        }

        EditorGUILayout.Space();

        // --- 3. 목록 표시 ---

        // 스크롤 뷰 시작
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        if (allSaveStates == null || allSaveStates.Length == 0)
        {
            EditorGUILayout.HelpBox("현재 씬에 SaveState 컴포넌트가 없습니다.", MessageType.Warning);
        }
        else
        {
            foreach (var saveState in allSaveStates)
            {
                if (saveState == null) continue; // 혹시라도 파괴된 오브젝트가 리스트에 남아있는 경우 대비

                EditorGUILayout.BeginVertical(EditorStyles.helpBox); // 각 항목을 구분하기 위해 박스로 묶음

                // 1. 오브젝트 참조 필드 (읽기 전용 표시)
                // EditorGUILayout.ObjectField를 사용하여 오브젝트를 표시합니다. 
                // 참조 필드를 통해 해당 오브젝트를 클릭하여 Inspector에서 볼 수 있습니다.
                EditorGUILayout.ObjectField(
                    "GameObject",
                    saveState.gameObject,
                    typeof(GameObject),
                    true // 씬 오브젝트
                );

                // 2. StateKey 값 표시
                // LabelField를 사용하여 읽기 전용으로 key 값을 표시합니다.
                EditorGUILayout.LabelField("🔑 State Key:", saveState.StateKey);

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(5);
            }
        }

        // 스크롤 뷰 끝
        EditorGUILayout.EndScrollView();

        // 씬 변경 감지: 씬이 변경되거나 저장될 때 자동으로 목록을 업데이트하고 싶다면 EditorApplication.hierarchyChanged 등을 사용할 수 있습니다.
    }
}
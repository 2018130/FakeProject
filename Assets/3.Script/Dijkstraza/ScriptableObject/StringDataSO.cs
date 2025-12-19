using UnityEngine;

[CreateAssetMenu(fileName = "NewStringData", menuName = "Scriptable Objects/String Data")]
public class StringDataSO : ScriptableObject
{
    // 여러 개의 텍스트를 담을 수 있는 리스트
     [TextArea(5,10)] // 인스펙터에서 입력창을 넓게 보여줍니다
    public string scriptLines;
}
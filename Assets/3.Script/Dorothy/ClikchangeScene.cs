using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ClikchangeScene : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public string sceneName;
    public float hoverScale = 1.1f;
    private bool isClicked = false;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isClicked)
        {
            SceneChangeManager.Instance.ChangeScene(SceneType.DialogueScene);
            isClicked = true;
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

public class AnimNotify : MonoBehaviour
{
    [SerializeField]
    private UnityEvent notifyEvent;

    public void Notify()
    {
        notifyEvent?.Invoke();
    }
}

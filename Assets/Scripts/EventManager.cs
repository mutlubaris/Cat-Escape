using UnityEngine;
using UnityEngine.Events;


public static class EventManager
{
    public static UnityEvent OnCatCaught = new UnityEvent();
    public static UnityEvent OnLevelComplete = new UnityEvent();
    public static TargetEvent OnRadioTurnedOn = new TargetEvent();

    public class TargetEvent : UnityEvent<GameObject> { }
}

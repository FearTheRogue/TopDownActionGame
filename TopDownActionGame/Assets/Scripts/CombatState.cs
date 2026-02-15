using System;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class CombatState : MonoBehaviour
{
    [SerializeField] private float combatHoldTime = 2.0f;

    private float timer;
    public bool InCombat => timer > 0f;

    public event Action<bool> OnCombatChanged;

    private bool lastState;

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }

        bool state = InCombat;

        if (state != lastState)
        {
            lastState = state;
            OnCombatChanged?.Invoke(state);
        }
    }

    // Enemy engages or the player takes damage
    public void NotifyCombat(string source, float extraHoldTime = -1f)
    {
        Debug.Log($"NotifyCombat source: {source}");
        float hold = extraHoldTime > 0f ? extraHoldTime : combatHoldTime;
        timer = MathF.Max(timer, hold);
    }

    public void ClearCombat()
    {
        timer = 0f;

        lastState = false;
        OnCombatChanged?.Invoke(false);
    }
}

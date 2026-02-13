using System;
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
    public void NotifyCombat(float extraHoldTime = -1f)
    {
        float hold = extraHoldTime > 0f ? extraHoldTime : combatHoldTime;
        timer = MathF.Max(timer, hold);
    }
}

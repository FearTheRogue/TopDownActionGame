using UnityEngine;
using Unity.Cinemachine;
using UnityEditor.Build;
using UnityEngine.Rendering;

public class CombatCameraZoom : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CombatState combatState;

    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineCamera exploreCam;
    [SerializeField] private CinemachineCamera combatCam;

    [Header("Priorities")]
    [SerializeField] private int explorePriority = 10;
    [SerializeField] private int combatPriority = 20;

    private void Awake()
    {
        if (combatState == null)
            combatState = FindFirstObjectByType<CombatState>();
    }

    private void OnEnable()
    {
        if (combatState != null)
            combatState.OnCombatChanged += HandleCombatChanged;

        // Apply initial state
        HandleCombatChanged(combatState != null && combatState.InCombat);
    }

    private void OnDisable()
    {
        if (combatState != null)
            combatState.OnCombatChanged -= HandleCombatChanged;
    }

    private void HandleCombatChanged(bool inCombat)
    {
        if (exploreCam == null || combatCam == null)
            return;

        Debug.Log($"Camera combat changed: {inCombat} (CombatState id {combatState.GetInstanceID()})");


        exploreCam.Priority = inCombat ? explorePriority : combatPriority;
        combatCam.Priority = inCombat ? combatPriority : explorePriority;
    }

    //private void Update()
    //{
    //    if (combatTimer > 0f)
    //    {
    //        combatTimer -= Time.deltaTime;

    //        if (combatTimer <= 0f)
    //        {
    //            SetCombat(false);
    //        }
    //    }
    //}

    //public void NotifyCombat()
    //{
    //    combatTimer = combatHoldTime;
    //    SetCombat(true);
    //}

    //public void SetCombat(bool inCombat)
    //{
    //    if (exploreCam == null || exploreCam == null)
    //        return;

    //    exploreCam.Priority = inCombat ? explorePriority : combatPriority;
    //    combatCam.Priority = inCombat ? combatPriority : explorePriority;
    //}
}

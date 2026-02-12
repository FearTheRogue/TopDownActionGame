using UnityEngine;
using Unity.Cinemachine;

public class CombatCameraZoom : MonoBehaviour
{
    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineCamera exploreCam;
    [SerializeField] private CinemachineCamera combatCam;

    [Header("Priorities")]
    [SerializeField] private int explorePriority = 10;
    [SerializeField] private int combatPriority = 20;

    [Header("Combat State")]
    [SerializeField] private float combatHoldTime = 2.0f;

    private float combatTimer;

    private void Awake()
    {
        SetCombat(false);
    }

    private void Update()
    {
        if (combatTimer > 0f)
        {
            combatTimer -= Time.deltaTime;

            if (combatTimer <= 0f)
            {
                SetCombat(false);
            }
        }
    }

    public void NotifyCombat()
    {
        combatTimer = combatHoldTime;
        SetCombat(true);
    }

    public void SetCombat(bool inCombat)
    {
        if (exploreCam == null || exploreCam == null)
            return;

        exploreCam.Priority = inCombat ? explorePriority : combatPriority;
        combatCam.Priority = inCombat ? combatPriority : explorePriority;
    }
}

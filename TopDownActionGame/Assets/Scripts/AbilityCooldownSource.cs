using System.Runtime.CompilerServices;
using UnityEngine;

public class AbilityCooldownSource : MonoBehaviour
{
    [Header("Ability")]
    [SerializeField] private MonoBehaviour abilityBehaviour;
    [SerializeField] private string id = "Dash";

    private IAbilityCooldown ability;

    public bool IsReady => ability != null && ability.IsReady;
    public float CooldownRemaining => ability != null ? ability.CooldownRemaining : 0f;
    public float CooldownDuration => ability != null ? ability.CooldownDuration : 1f;
    public string Id => id;

    private void Awake()
    {
        ability = abilityBehaviour as IAbilityCooldown;

        if (ability == null)
        {
            Debug.LogError($"{name}: Assigned behaviour does not implement IAbilityCooldown.", this);
        }
    }
}

public interface IAbilityCooldown
{
    bool IsReady { get; }
    float CooldownRemaining {  get; }
    float CooldownDuration { get; }
}
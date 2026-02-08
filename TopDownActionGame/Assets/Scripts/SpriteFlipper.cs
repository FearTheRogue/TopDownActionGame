using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    [SerializeField] private Transform visuals;

    /// <summary>
    /// Flips the sprite based on a world-space direction
    /// </summary>

    public void FaceDirection(Vector2 direction)
    {
        if (visuals == null) return;
        if (Mathf.Abs(direction.x) < 0.01f) return;

        Vector3 scale = visuals.localScale;
        scale.x = Mathf.Abs(scale.x) * (direction.x < 0 ? -1f : 1f);
        visuals.localScale = scale;
    }
}
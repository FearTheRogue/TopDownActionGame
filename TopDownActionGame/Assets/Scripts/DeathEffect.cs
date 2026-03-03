using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    [Header("FX")]
    [SerializeField] private GameObject deathFxPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool detachFxFromParent = true;

    public void Play()
    {
        if (deathFxPrefab == null)
            return;

        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion rot = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

        var fx = Instantiate(deathFxPrefab, pos, rot);

        if (detachFxFromParent)
            fx.transform.SetParent(null);
    }
}

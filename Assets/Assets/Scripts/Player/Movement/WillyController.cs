using UnityEngine;
using SceneConnectionsGenerator;

public class WillyController : MonoBehaviour
{
    public PlayMakerFSM fsm;
    private float oldAxis = 1;

    void Start()
    {
        Health.OnDeath += () => PlayMakerFSM.BroadcastEvent("DeathGlobal");
        SetStartingPosition();
    }

    public float clampAxis(float raw, int multiplier)
    {
        if (raw != 0){
            return raw > 0 ? multiplier : -1 * multiplier;
        }
        return 0;
    }

    public float clampAxisNoNull(float raw)
    {   
        if (raw == 0)
        {
            return oldAxis;
        }
        float rawInt = raw > 0 ? 1 : -1;
        oldAxis = rawInt;
        return rawInt;
    }

    public float getAngle(Vector2 pos1, Vector2 pos2)
    {
        Vector2 enemyToPlayer = (pos1 - pos2).normalized;
        return Vector2.SignedAngle(enemyToPlayer, new Vector2(0, 1)) * -1;
    }

    public Vector2 getMidPos(Vector2 pos1, Vector2 pos2)
    {
        return new Vector2((pos1.x + pos2.x) / 2, (pos1.y + pos2.y) / 2);
    }

    private void SetStartingPosition()
    {
        var sceneManager = FindObjectOfType<SceneConnectionManager>();
        if (sceneManager == null)
            return;

        var spawnObj = GameObject.Find(sceneManager.spawn);
        if (spawnObj == null)
        {
            Debug.LogWarning("Spawn point not found when loading scene...");
            return;
        }
        var newPos = spawnObj.transform.position;

        gameObject.transform.position = new Vector2(newPos.x, newPos.y);
    }
}

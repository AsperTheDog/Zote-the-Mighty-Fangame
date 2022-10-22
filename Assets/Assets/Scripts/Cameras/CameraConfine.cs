using UnityEngine;
using Cinemachine;

[ExecuteInEditMode]
public class CameraConfine : MonoBehaviour
{
    CompositeCollider2D compColl;
    CinemachineConfiner2D confiner;
    public void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner2D>();
        compColl = GetComponent<CompositeCollider2D>();
        confiner.m_BoundingShape2D = compColl;
        RegenerateCollider();        
    }

    public void RegenerateCollider()
    {
        if(confiner is null)
            confiner = FindObjectOfType<CinemachineConfiner2D>();

        foreach (var coll in gameObject.GetComponents<Collider2D>())
        {
            if (coll.GetType() != typeof(CompositeCollider2D))
            {
                DestroyImmediate(coll);
            }
        }

        foreach (var newColl in GetComponentsInChildren<PolygonCollider2D>())
        {
            if (newColl.TryGetComponent(out HiddenArea hiddenArea))
            {
                if (!hiddenArea.CanAddTrigger())
                    continue;
            }
            CopyCollider2D(newColl, gameObject); 
        }

        compColl.GenerateGeometry();
        confiner.InvalidateCache();
    }

    private void CopyCollider2D(PolygonCollider2D coll, GameObject go)
    {
        PolygonCollider2D newComp = (PolygonCollider2D)go.AddComponent(coll.GetType());
        newComp.points = (Vector2[])coll.points.Clone();

        var x = coll.gameObject.transform.localPosition.x;
        var y = coll.gameObject.transform.localPosition.y;
        newComp.offset = new Vector2(x, y);
        newComp.usedByComposite = true;
    }

    public void AddCollider2D(PolygonCollider2D coll)
    {
        CopyCollider2D(coll, gameObject);
        compColl.GenerateGeometry();
        confiner.InvalidateCache();
    }
}

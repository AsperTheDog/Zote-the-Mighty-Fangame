using UnityEngine;

public class HiddenArea : MonoBehaviour
{
    public bool isHidden = true;
    public bool onlyBlackout = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            EnableArea();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(onlyBlackout)
                DisableArea();
        }
    }

    public bool CanAddTrigger()
    {
        return !isHidden || onlyBlackout;
    }

    private void EnableArea()
    {
        if (isHidden)
        {
            isHidden = false;
            GetComponent<MeshRenderer>().enabled = false;
            if (!onlyBlackout)
            {
                FindObjectOfType<CameraConfine>().AddCollider2D(GetComponent<PolygonCollider2D>());
                //play sound
            }
        }
    }

    private void DisableArea()
    {
        isHidden = true;
        GetComponent<MeshRenderer>().enabled = true;
    }
}

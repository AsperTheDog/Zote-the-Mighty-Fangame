using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneConnectionsGenerator;

public class InspectEnterArea : MonoBehaviour
{
    public ConnectorTrigger connector;

    public void EnableConnector()
    {
        connector.gameObject.GetComponent<Collider2D>().enabled = true;
    }

}

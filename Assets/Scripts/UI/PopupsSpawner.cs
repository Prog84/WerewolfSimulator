using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))] 
public class PopupsSpawner : MonoBehaviour
{
    public void SpawnPopup(Popup popup, Vector3 position)
    {
        var spawn = Instantiate(popup.gameObject, position + Vector3.up*0.1f, Quaternion.identity);
        spawn.GetComponent<Popup>().Init(transform);
    }
}

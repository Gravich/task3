using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instanse;

    [SerializeField]
    private float RespawnTime;
    [SerializeField]
    private List<Creature> Prefabs;

    private void Start()
    {
        if (Instanse == null)
        {
            Instanse = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Respawn(Creature obj)
    {
        Debug.Log("запрос принят");
        Instanse.StartCoroutine(GoToRespawn(obj.GetType(), obj.RespawnPoint, RespawnTime, obj.RespawnData));
        Destroy(obj.gameObject);
    }

    private IEnumerator GoToRespawn(System.Type type, Vector3 pos, float time, object respawnData)
    {
        yield return new WaitForSeconds(time);
        foreach (var prefab in Prefabs)
        {
            if (prefab.GetType() == type)
            { 
                Instantiate(prefab, pos, new Quaternion()).RespawnData = respawnData;
                break;
            }
        }
    }
}
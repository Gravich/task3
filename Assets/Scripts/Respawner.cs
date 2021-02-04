#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public static Respawner Instanse;

    [SerializeField]
    private float RespTime;
    [SerializeField]
    private List<UnimmortallObject> prefabs;

    private void Start()
    {
        if (Instanse == null)
        {
            Instanse = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Respawn(UnimmortallObject obj)
    {
        Debug.Log("запрос принят");
        Instanse.StartCoroutine(GoToResp(obj.GetType(), obj.RespawnPoint, RespTime));
        Destroy(obj.gameObject);
    }

    private IEnumerator GoToResp(System.Type type, Vector3 pos, float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var prefab in prefabs)
        {
            if (prefab.GetType() == type)
            {
                Instantiate(prefab, pos, new Quaternion());
            }
        }
    }
}

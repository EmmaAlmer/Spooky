using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReroutedAudioScript : MonoBehaviour
{
    GameObject _player;
    LayerMask audioOcclusionLayerMask;
    [SerializeField] float distWhenToDestroy = 1;

    [SerializeField]
    GameObject NextInLine = null; //Bara f�r testerna 28/09

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); //kan assignas n�r denna skapas ist�llet
        audioOcclusionLayerMask = ~LayerMask.GetMask("IgnoreAudioOcclusion");
    }
    void Update()
    {
        RaycastHit hit;
        Physics.Linecast(transform.position, _player.transform.position, out hit, audioOcclusionLayerMask);

        if(hit.collider != null && hit.collider.tag == "Player")
        {
            if(Vector3.Distance(transform.position, _player.transform.position) < distWhenToDestroy)
            {
                if(NextInLine != null)
                {
                    NextInLine.active = true;
                }
                Destroy(gameObject);
            }
        }
    }
}

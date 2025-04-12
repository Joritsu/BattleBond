using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField] private GameObject trapObject;
    [SerializeField] private float waitTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            trapObject.GetComponent<Spikes_trap>().onTrigger(true, waitTime);
        }
    }
}

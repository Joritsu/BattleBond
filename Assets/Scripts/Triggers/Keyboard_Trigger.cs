using System;
using Unity.VisualScripting;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class Keyboard_Trigger : MonoBehaviour
{
    [SerializeField] private GameObject trapObject;
    [SerializeField] private float waitTime;
    private bool state = true;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            trapObject.GetComponent<Spikes_trap>().onTrigger(state, waitTime);
            state = false;
            StartCoroutine(wait(waitTime));
            state = true;
        }
    }
    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}

using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Spikes_trap : MonoBehaviour
{
    [SerializeField] public float spikeSpeed;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void onTrigger(bool state, float waitTime)
    {
        if (anim != null)
        {
            StartCoroutine(spikeStop(waitTime));
            StartCoroutine(wait(waitTime, state));
        }
    }

    IEnumerator spikeStop(float waitTime)
    {
        anim.SetTrigger("TrOpen");
        yield return new WaitForSeconds(waitTime);
        anim.SetTrigger("TrClose");
    }
    IEnumerator wait(float waitTime, bool state)
    {
        yield return new WaitForSeconds(waitTime);
        yield return state = true;
    }
<<<<<<< Updated upstream
}
=======
}
>>>>>>> Stashed changes

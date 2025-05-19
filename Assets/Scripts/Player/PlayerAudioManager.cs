using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioManager : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip jumpClip;
    public AudioClip[] idleClips;

    [Header("Idle Settings (sec)")]
    public float minIdleInterval = 5f;
    public float maxIdleInterval = 12f;

    private AudioSource _audio;
    private Animator _anim;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        StartCoroutine(IdleSFXRoutine());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    // Call this from your jump code or Animation Event
    public void PlayJumpSFX()
    {
        if (jumpClip != null)
            _audio.PlayOneShot(jumpClip);
    }

    IEnumerator IdleSFXRoutine()
    {
        while (true)
        {
            float wait = Random.Range(minIdleInterval, maxIdleInterval);
            yield return new WaitForSeconds(wait);

            // only play if in Idle state
            var state = _anim.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("Idle"))
            {
                var clip = idleClips[Random.Range(0, idleClips.Length)];
                _audio.PlayOneShot(clip);
            }
        }
    }
}

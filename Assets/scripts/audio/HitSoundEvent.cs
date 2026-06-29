using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HitSoundEvent : MonoBehaviour
{
    [Header("💥 피격 사운드")]
    public AudioClip[] hitSounds;
    public float volume = 1f;
    public bool randomPitch = true;
    public Vector2 pitchRange = new Vector2(0.9f, 1.1f);
    
    private AudioSource audioSource;
    private int lastIndex = -1;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        
        // ✅ PlayerHealth 찾아서 이벤트 구독
        PlayerHealth health = GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.OnDamaged += PlayHitSound;
            Debug.Log("💥 피격 이벤트 구독 완료!");
        }
        else
        {
            Debug.LogWarning("⚠️ PlayerHealth를 찾을 수 없습니다!");
        }
    }
    
    // ✅ 피격 시 자동 호출됨
    void PlayHitSound()
    {
        if (hitSounds == null || hitSounds.Length == 0) return;
        
        // 랜덤 클립
        AudioClip clip;
        if (hitSounds.Length == 1)
        {
            clip = hitSounds[0];
        }
        else
        {
            int index;
            do
            {
                index = Random.Range(0, hitSounds.Length);
            } while (index == lastIndex);
            lastIndex = index;
            clip = hitSounds[index];
        }
        
        if (randomPitch)
            audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        else
            audioSource.pitch = 1f;
        
        audioSource.PlayOneShot(clip, volume);
        Debug.Log($"💥 피격 소리: {clip.name}");
    }
    
    void OnDestroy()
    {
        // 이벤트 구독 해제
        PlayerHealth health = GetComponent<PlayerHealth>();
        if (health != null)
            health.OnDamaged -= PlayHitSound;
    }
}
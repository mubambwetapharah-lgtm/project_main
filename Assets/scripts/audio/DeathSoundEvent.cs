using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DeathSoundEvent : MonoBehaviour
{
    [Header("💀")]
    [SerializeField] private AudioClip[] deathSounds;
    [SerializeField] private float volume = 1f;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.9f, 1.1f);
    
    [Header("🎯")]
    [SerializeField] private PlayerHealth playerHealth;  
    
    private AudioSource audioSource;
    private int lastClipIndex = -1;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        
        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();
        
        if (playerHealth != null)
        {
            playerHealth.OnDeath += PlayDeathSound;
            Debug.Log("💀 Event Showed!");
        }
        else
        {
            Debug.LogError("❌ cannot find playerHealth script");
        }
    }
    
    void PlayDeathSound()
    {
        if (deathSounds == null || deathSounds.Length == 0)
        {
            Debug.LogWarning("⚠️ No DeathSound File!");
            return;
        }
        
        AudioClip clip = GetRandomClip();
        if (clip == null) return;
        
        audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        audioSource.PlayOneShot(clip, volume);
        
        Debug.Log($"💀 play deathsound: {clip.name}");
    }
    
    AudioClip GetRandomClip()
    {
        if (deathSounds.Length == 1) return deathSounds[0];
        
        int newIndex;
        do
        {
            newIndex = Random.Range(0, deathSounds.Length);
        } while (newIndex == lastClipIndex && deathSounds.Length > 1);
        
        lastClipIndex = newIndex;
        return deathSounds[newIndex];
    }
    
    void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnDeath -= PlayDeathSound;
    }
}
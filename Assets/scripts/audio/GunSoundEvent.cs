using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GunSoundEvent : MonoBehaviour
{
    [Header("🔫")]
    [SerializeField] private AudioClip[] shootSounds;
    [SerializeField] private float volume = 1f;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.95f, 1.05f);
    
    [Header("🎯")]
    [SerializeField] private ShootBullet shootScript; 
    
    private AudioSource audioSource;
    private int lastClipIndex = -1;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        
        if (shootScript == null)
            shootScript = GetComponent<ShootBullet>();
        
        if (shootScript != null)
        {
            shootScript.OnFire += PlayShootSound;
            Debug.Log("🔫 ShotEvent Accepted!");
        }
        else
        {
            Debug.LogError("❌ cannot find ShootBullet script!");
        }
    }
    
    void PlayShootSound()
    {
        if (shootSounds == null || shootSounds.Length == 0)
        {
            Debug.LogWarning("⚠️ No ShootSound File!");
            return;
        }
        
        AudioClip clip = GetRandomClip();
        if (clip == null) return;
        
        audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        audioSource.PlayOneShot(clip, volume);
    }
    
    AudioClip GetRandomClip()
    {
        if (shootSounds.Length == 1) return shootSounds[0];
        
        int newIndex;
        do
        {
            newIndex = Random.Range(0, shootSounds.Length);
        } while (newIndex == lastClipIndex && shootSounds.Length > 1);
        
        lastClipIndex = newIndex;
        return shootSounds[newIndex];
    }
    
    void OnDestroy()
    {
        if (shootScript != null)
            shootScript.OnFire -= PlayShootSound;
    }
}
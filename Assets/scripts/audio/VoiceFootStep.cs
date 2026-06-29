using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VoiceFootstep : MonoBehaviour
{
    [Header("Voice Audio")]
    [SerializeField] private AudioClip[] voiceClips;
    [SerializeField] private float volume = 1f;
    
    [Header("Movement Settings")]
    [SerializeField] private float stepInterval = 0.5f; 
    
    [Header("Ground Check (Optional)")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRadius = 0.2f;
    
    private AudioSource audioSource;
    private float stepTimer;
    private int lastClipIndex = -1;
    private bool wasMoving = false; 
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        
        Debug.Log("🎵 VoiceFootstep Started!");
    }
    
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        bool isMoving = Mathf.Abs(horizontal) > 0.1f;
        
        bool isGrounded = IsGrounded();
        
        if (isMoving && !wasMoving)
        {
            stepTimer = 0f; 
            Debug.Log("🚀 Movement Started! Play Sound!");
        }
        
        if (isMoving && isGrounded)
        {
            stepTimer += Time.deltaTime;
            
            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f; 
            }
        }
        else
        {
            stepTimer = 0f;
            
            if (wasMoving)
            {
                Debug.Log("⏹️ Movement Stopped! Stop Sound.");
            }
        }
        
        wasMoving = isMoving;
    }
    
    bool IsGrounded()
    {
        if (groundCheck == null) return true;
        
        bool grounded2D = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        bool grounded3D = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);
        
        return grounded2D || grounded3D;
    }
    
    void PlayFootstep()
    {
        if (voiceClips == null || voiceClips.Length == 0)
        {
            Debug.LogWarning("⚠️ No Sound File!");
            return;
        }
        
        AudioClip clipToPlay;
        
        if (voiceClips.Length > 1)
        {
            int newIndex;
            do
            {
                newIndex = Random.Range(0, voiceClips.Length);
            } while (newIndex == lastClipIndex);
            
            lastClipIndex = newIndex;
            clipToPlay = voiceClips[newIndex];
        }
        else
        {
            clipToPlay = voiceClips[0];
        }
        
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clipToPlay, volume);
        Debug.Log($"🔊 walk: {clipToPlay.name} (interval: {stepInterval}s)");
    }
    
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
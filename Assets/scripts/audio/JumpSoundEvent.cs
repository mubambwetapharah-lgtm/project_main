using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class JumpSoundEvent : MonoBehaviour
{
    [Header("🎵 점프 사운드")]
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private float volume = 1f;
    [SerializeField] private bool randomPitch = true;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.9f, 1.1f);

    [Header("🎯 연결")]
    [SerializeField] private PlayerJump playerJump;

    private AudioSource audioSource;
    private int lastIndex = -1;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (playerJump == null)
            playerJump = GetComponent<PlayerJump>();

        if (playerJump != null)
        {
            playerJump.OnJump += PlayJumpSound;
            Debug.Log("🎵 점프 이벤트 구독 완료!");
        }
        else
        {
            Debug.LogError("❌ PlayerJump 스크립트를 찾을 수 없습니다!");
        }
    }

    void PlayJumpSound()
    {
        if (jumpSounds == null || jumpSounds.Length == 0)
        {
            Debug.LogWarning("⚠️ 점프 사운드 파일이 없습니다!");
            return;
        }

        AudioClip clip;
        if (jumpSounds.Length == 1)
        {
            clip = jumpSounds[0];
        }
        else
        {
            int newIndex;
            do
            {
                newIndex = Random.Range(0, jumpSounds.Length);
            } while (newIndex == lastIndex && jumpSounds.Length > 1);
            lastIndex = newIndex;
            clip = jumpSounds[newIndex];
        }

        if (randomPitch)
            audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        else
            audioSource.pitch = 1f;

        audioSource.PlayOneShot(clip, volume);
        Debug.Log($"🎵 점프 소리: {clip.name}");
    }

    void OnDestroy()
    {
        if (playerJump != null)
            playerJump.OnJump -= PlayJumpSound;
    }
}
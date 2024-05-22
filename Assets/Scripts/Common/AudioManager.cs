
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip seClick;

    private static AudioManager singleton;

		public static bool IsInstanceValid() { return singleton != null; }

		public void Reset()
		{
			gameObject.name = typeof(AudioManager).Name;
		}

		public static AudioManager Instance
		{
			get
			{
				if (!Application.isPlaying) {
					return null;
				}
				if (singleton == null)
				{
					singleton = (AudioManager)FindObjectOfType(typeof(AudioManager));
					if (singleton == null)
					{
						AudioManager obj = Instantiate(Resources.Load<AudioManager>("Prefabs/AudioManager"));
						singleton = obj;
					}
				}

				return singleton;
			}
		}
    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayOneShot(AudioClip audioClip) {
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayClick() {
        audioSource.PlayOneShot(seClick);
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}

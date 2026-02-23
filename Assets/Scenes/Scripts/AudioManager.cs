using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum EAudioMixerType { Master, BGM, SFX }
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Source")]
    [SerializeField] private AudioSource BGMSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("Slider")]
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    [Header("BGM")]
    [SerializeField] private AudioClip[] BGMList;
    [Header("SFX")]
    [SerializeField] private AudioClip[] SFXList;

    private Dictionary<string, AudioClip> BGMDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> SFXDict = new Dictionary<string, AudioClip>();

    private bool[] isMute = new bool[3];
    private float[] audioVolumes = new float[3];

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < BGMList.Length; i++)
        {
            BGMDict.Add(BGMList[i].name, BGMList[i]);
        }

        for (int i = 0; i < SFXList.Length; i++)
        {
            SFXDict.Add(SFXList[i].name, SFXList[i]);
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Q)) {
            SFXTrackChange("sound1");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SFXTrackChange("sound2");
        }
        */
    }

    public void SetAudioVolume(EAudioMixerType audioMixerType, float volume)
    {
        // 오디오 믹서의 값은 -80 ~ 0까지이기 때문에 0.0001 ~ 1의 Log10 * 20을 한다.
        audioMixer.SetFloat(audioMixerType.ToString(), Mathf.Log10(volume) * 20);
    }

    public void SetAudioMute(EAudioMixerType audioMixerType)
    {
        int type = (int)audioMixerType;
        if (!isMute[type]) // 뮤트 
        {
            isMute[type] = true;
            audioMixer.GetFloat(audioMixerType.ToString(), out float curVolume);
            audioVolumes[type] = curVolume;
            SetAudioVolume(audioMixerType, 0.001f);
        }
        else
        {
            isMute[type] = false;
            SetAudioVolume(audioMixerType, audioVolumes[type]);
        }
    }

    //슬라이더, 버튼 이벤트 함수

    public void MasterMute()
    {
        AudioManager.Instance.SetAudioMute(EAudioMixerType.Master);
    }

    public void BGMMute()
    {
        AudioManager.Instance.SetAudioMute(EAudioMixerType.BGM);
    }

    public void SFXMute()
    {
        AudioManager.Instance.SetAudioMute(EAudioMixerType.SFX);
    }

    public void MasterChangeVolume()
    {
        AudioManager.Instance.SetAudioVolume(EAudioMixerType.Master, MasterSlider.value);
    }
    public void BGMChangeVolume()
    {
        AudioManager.Instance.SetAudioVolume(EAudioMixerType.BGM, BGMSlider.value);
    }
    public void SFXChangeVolume()
    {
        AudioManager.Instance.SetAudioVolume(EAudioMixerType.SFX, SFXSlider.value);
    }

    public void BGMTrackChange(string name) {
            BGMSource.clip = BGMDict[name];
            BGMSource.Play();
    }

    public void SFXTrackChange(string name)
    {
            SFXSource.clip = SFXDict[name];
            SFXSource.Play();
    }

}

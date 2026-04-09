using System.Collections.Generic;
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

    public void sliderInit()
    {
        InitSlider(EAudioMixerType.Master, MasterSlider);
        InitSlider(EAudioMixerType.BGM, BGMSlider);
        InitSlider(EAudioMixerType.SFX, SFXSlider);
    }

    private void InitSlider(EAudioMixerType type, Slider slider)
    {
        if (slider == null) return;

        // AudioMixer는 dB(-80~0)를 쓰고, Slider는 선형(0~1)이라서 역변환 필요
        // SetAudioVolume: dB = log10(v) * 20  ->  v = 10^(dB/20)
        if (audioMixer != null && audioMixer.GetFloat(type.ToString(), out float db))
        {
            float linear = Mathf.Pow(10f, db / 20f);          // 0.0001 ~ 1 근처
            linear = Mathf.Clamp(linear, 0.0001f, 1f);        // 0 방지(로그 변환 대비)
            slider.SetValueWithoutNotify(linear);             // 초기화 시 리스너 호출 방지
        }
        else
        {
            slider.SetValueWithoutNotify(1f);                 // 못 읽으면 기본값
        }
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

    public void SFXTrackChange(AudioSource source, string name)
    {
        source.clip = SFXDict[name];
        source.Play();
    }

}

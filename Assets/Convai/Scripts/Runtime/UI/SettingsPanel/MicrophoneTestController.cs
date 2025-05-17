using System;
using System.Collections;
using Convai.Scripts.Runtime.Addons;
using Convai.Scripts.Runtime.LoggerSystem;
using UnityEngine;

namespace Convai.Scripts.Runtime.UI
{
    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(UIMicrophoneSettings), typeof(AudioSource), typeof(MicrophoneInputChecker))]
    public class MicrophoneTestController : MonoBehaviour
    {
        private const int FREQUENCY = 44100;
        private const int MAX_RECORD_TIME = 10;

        private AudioSource _audioSource;
        private bool _isAudioPlaying;
        private bool _isRecording;
        private MicrophoneInputChecker _microphoneInputChecker;
        private Coroutine _recordTimeCounterCoroutine;
        private string _selectedDevice;
        private UIMicrophoneSettings _uiMicrophoneSettings;

        public static MicrophoneTestController Instance { get; private set; }

        public event Action OnRecordStarted;
        public event Action OnRecordCompleted;
        public event Action OnAudioClipCompleted;

        private void Awake()
        {
            if (Instance != null)
            {
                ConvaiLogger.DebugLog("<color=red> There's More Than One MicrophoneTestController </color> " + transform + " - " + Instance, ConvaiLogger.LogCategory.UI);
                Destroy(gameObject);
                return;
            }

            Instance = this;

            _uiMicrophoneSettings = GetComponent<UIMicrophoneSettings>();
            _audioSource = GetComponent<AudioSource>();
            _microphoneInputChecker = GetComponent<MicrophoneInputChecker>();
        }

        private void Start()
        {
            _uiMicrophoneSettings.GetRecordControllerButton().onClick.AddListener(RecordController);
#if !UNITY_WEBGL
            _selectedDevice = MicrophoneManager.Instance.SelectedMicrophoneName;
#endif
        }

        public void CheckMicrophoneDeviceWorkingStatus(AudioClip audioClip)
        {
            _microphoneInputChecker.IsMicrophoneWorking(audioClip);
        }

        private void RecordController()
        {
            if (_isRecording)
                StopMicrophoneTestRecording();
            else if (!_isAudioPlaying)
                StartMicrophoneTestRecording();
        }

        private void StartMicrophoneTestRecording()
        {
#if UNITY_WEBGL
            ConvaiLogger.Error("Microphone input is not supported on WebGL.", ConvaiLogger.LogCategory.UI);
#else
            if (_uiMicrophoneSettings.GetMicrophoneSelectDropdown().options.Count > 0)
            {
                _selectedDevice = MicrophoneManager.Instance.SelectedMicrophoneName;
                AudioClip recordedClip = Microphone.Start(_selectedDevice, false, MAX_RECORD_TIME, FREQUENCY);
                _audioSource.clip = recordedClip;
                CheckMicrophoneDeviceWorkingStatus(recordedClip);

                OnRecordStarted?.Invoke();
                _isRecording = true;

                _recordTimeCounterCoroutine = StartCoroutine(RecordTimeCounter());
            }
            else
            {
                ConvaiLogger.Error("No microphone devices found.", ConvaiLogger.LogCategory.UI);
            }
#endif
        }

        private void StopMicrophoneTestRecording()
        {
#if UNITY_WEBGL
            ConvaiLogger.Error("Microphone input is not supported on WebGL.", ConvaiLogger.LogCategory.UI);
#else
            if (Microphone.IsRecording(_selectedDevice))
            {
                StopCoroutine(_recordTimeCounterCoroutine);
                int position = Microphone.GetPosition(_selectedDevice);
                Microphone.End(_selectedDevice);

                TrimAudio(position);
                _audioSource.Play();
                _isAudioPlaying = true;

                OnRecordCompleted?.Invoke();
                _isRecording = false;

                StartCoroutine(AudioClipTimeCounter(_audioSource.clip.length));
            }
#endif
        }

        private IEnumerator RecordTimeCounter()
        {
            yield return new WaitForSeconds(MAX_RECORD_TIME);
            StopMicrophoneTestRecording();
        }

        private IEnumerator AudioClipTimeCounter(float length)
        {
            yield return new WaitForSeconds(length);

            OnAudioClipCompleted?.Invoke();
            _isAudioPlaying = false;
        }

        private void TrimAudio(int micRecordLastPosition)
        {
            if (_audioSource.clip == null)
            {
                ConvaiLogger.Error("AudioSource clip is null.", ConvaiLogger.LogCategory.UI);
                return;
            }

            if (micRecordLastPosition <= 0)
            {
                ConvaiLogger.Error("Microphone position is zero or negative. Cannot trim audio.", ConvaiLogger.LogCategory.UI);
                return;
            }

            AudioClip tempAudioClip = _audioSource.clip;
            int channels = tempAudioClip.channels;
            int position = micRecordLastPosition;
            float[] samplesArray = new float[position * channels];
            tempAudioClip.GetData(samplesArray, 0);
            AudioClip newClip = AudioClip.Create("RecordedSound", position * channels, channels, FREQUENCY, false);
            newClip.SetData(samplesArray, 0);
            _audioSource.clip = newClip;
        }
    }
}

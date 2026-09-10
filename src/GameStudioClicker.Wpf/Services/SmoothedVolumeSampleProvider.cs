using NAudio.Wave;

namespace GameStudioClicker.Wpf.Services
{
    // This class job is to smooth out volume changes over time
    // preventing static noise when changing volume drastically

    public sealed class SmoothedVolumeSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider _source; // Audio source
        private readonly int _channels; // Ensures L/R channels are handled correctly
        private readonly float _smoothingStep; // Controls how quickly the volume changes
        private float _currentVolume; // Moves toward _targetVolume over time
        private volatile float _targetVolume; // Changed by the slider by the user


        public SmoothedVolumeSampleProvider(
        ISampleProvider source,
        float initialVolume)
        {
            _source = source;
            _channels = source.WaveFormat.Channels;

            _currentVolume = Math.Clamp(initialVolume, 0, 1);
            _targetVolume = _currentVolume;

            // Approximately 20 milliseconds for a full 0-to-1 transition.
            _smoothingStep =
                1f / (source.WaveFormat.SampleRate * 0.02f);
        }

        public WaveFormat WaveFormat => _source.WaveFormat;

        public float Volume
        {
            get => _targetVolume;
            set => _targetVolume = Math.Clamp(value, 0, 1);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = _source.Read(buffer, offset, count);
            int end = offset + samplesRead;

            for (int frame = offset; frame < end; frame += _channels)
            {
                MoveCurrentVolumeTowardTarget();

                for (int channel = 0;
                     channel < _channels && frame + channel < end;
                     channel++)
                {
                    buffer[frame + channel] *= _currentVolume;
                }
            }

            return samplesRead;
        }

        private void MoveCurrentVolumeTowardTarget()
        {
            float targetVolume = _targetVolume;

            if (_currentVolume < targetVolume)
            {
                _currentVolume = Math.Min(
                    _currentVolume + _smoothingStep,
                    targetVolume);
            }
            else if (_currentVolume > targetVolume)
            {
                _currentVolume = Math.Max(
                    _currentVolume - _smoothingStep,
                    targetVolume);
            }
        }
    }
}

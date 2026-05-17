namespace gishadev.tools.Audio
{
    public abstract class AudioPlayer<T> : BaseAudioPlayer where T : AudioData, new()
    {
        public abstract void Play(T data);
        public abstract void Pause(T data);
        public abstract void Stop(T data);
    }
}
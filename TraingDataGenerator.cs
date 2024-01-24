using NAudio.Wave;

namespace NeuronalesNetz;

public class TraingDataGenerator
{
    private readonly List<float[]> _musicSlices = new();
    private readonly List<float[]> _speechSlices = new();

    public const int SAMPLE_LENGTH_SEC = 1;
    
    public void LoadDataFromFolder(string path, AudioType audioType, int nbrOfSlicesPreFile = 1000)
    {
        foreach (var item in Directory.GetFiles(path))
        {
            //Load audio data into buffer
            AudioFileReader reader = new AudioFileReader(item);
            ISampleProvider isp = reader.ToMono();
            float[] buffer = new float[reader.Length / 4];
            isp.Read(buffer, 0, buffer.Length);

            //Check if audio file is too short
            var lengthOfSlice = reader.WaveFormat.SampleRate * SAMPLE_LENGTH_SEC;
            if (lengthOfSlice > buffer.Length) throw new Exception("Audio file \"" + item + "\" shorter than slice length!");
            
            //Get set amount of slices with fixed length and random offset
            for (int i = 0; i < nbrOfSlicesPreFile; i++)
            {
                var sliceBuffer = new float[lengthOfSlice];
                var offset = Random.Shared.Next(0, buffer.Length - lengthOfSlice);
                var newSlice = new Span<float>(buffer).Slice(offset, lengthOfSlice).ToArray();
                if (newSlice.All(x => x == 0)) continue;
                
                if (audioType == AudioType.MUSIC)
                {
                    _musicSlices.Add(newSlice);
                }
                else
                {
                    _speechSlices.Add(newSlice);
                }
            }
        }
    }

    public (float[] data, AudioType type)[] GetTrainingSamples(int numberOfSamples)
    {
        List<(float[], AudioType)> returnData = new();

        while (returnData.Count < numberOfSamples)
        {
            var currentType = (AudioType)Random.Shared.Next(0, 2);
            if (currentType == AudioType.MUSIC && _musicSlices.Count > 0)
            {
                returnData.Add((_musicSlices[Random.Shared.Next(0, _musicSlices.Count)], currentType));
            } else if(_speechSlices.Count > 0)
            {
                returnData.Add((_speechSlices[Random.Shared.Next(0, _speechSlices.Count)], currentType));
            }
        }

        return returnData.ToArray();
    }

    public void SaveTestFile()
    {
        var slice = _musicSlices[0];
        if (slice is null) return;

        var writer = new WaveFileWriter("test.wav", WaveFormat.CreateIeeeFloatWaveFormat(44100, 1));
        writer.WriteSamples(slice, 0, slice.Length);
        writer.Close();
    }
}
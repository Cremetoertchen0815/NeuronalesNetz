using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace NeuronalesNetz;

public class TraingDataGenerator
{
    private readonly List<float[]> _musicSlices = new();
    private readonly List<float[]> _speechSlices = new();

    public const int SAMPLE_LENGTH_SEC = 1;
    
    public void LoadDataFromFolder(string path, AudioType audioType)
    {
        foreach (var item in Directory.GetFiles(path))
        {
            var reader = new AudioFileReader(item).ToSampleProvider().ToMono();
            var audioBuffer = new float[reader.WaveFormat.SampleRate * SAMPLE_LENGTH_SEC];

            int measured = 0;
            do
            {
                var currentSample = reader.Take(TimeSpan.FromSeconds(SAMPLE_LENGTH_SEC));
                measured = currentSample.Read(audioBuffer, 0, audioBuffer.Length);
                var data = new ReadOnlySpan<float>(audioBuffer).Slice(0, measured).ToArray();
                if (audioType == AudioType.MUSIC)
                {
                    _musicSlices.Add(data);
                }
                else
                {
                    _speechSlices.Add(data);
                }


            } while (measured >= audioBuffer.Length);
        }
    }

    public (float[] data, AudioType type)[] GetTrainingSamples(int numberOfSamples)
    {
        int counter = numberOfSamples;
        List<(float[], AudioType)> returnData = new();

        while (counter-- > 0)
        {
            var currentType = (AudioType)Random.Shared.Next(0, 2);
            if (currentType == AudioType.MUSIC && _musicSlices.Count > 0)
            {
                returnData.Add((_musicSlices[Random.Shared.Next(0, _musicSlices.Count)], currentType));
            } else
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
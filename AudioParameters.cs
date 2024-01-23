namespace NeuronalesNetz;

public struct AudioParameters
{
    public double Energy;
    public double Rhythm;
    public double Pitch;

    public static AudioParameters FromSample(float[] sample) => new AudioParameters() { Energy = CalculateEnergy(sample), Rhythm = CalculateRhythmScore(sample), Pitch = CalculatePitchScore(sample) };

    private static double CalculateEnergy(float[] buffer)
    {
        double energy = 0.0;

        for (int i = 0; i < buffer.Length; i++)
        {
            energy += buffer[i] * buffer[i];
        }
        
        return energy;
    }

    private static double CalculateRhythmScore(float[] buffer)
    {
        double rhythmScore = 0.0;

        // Annahme: buffer enthält Gleitkommazahlenrepräsentation des Audiosignals

        for (int i = 1; i < buffer.Length - 1; i++)
        {
            // Berechne die Differenzen zwischen aufeinanderfolgenden Abtastwerten
            double diff1 = buffer[i] - buffer[i - 1];
            double diff2 = buffer[i + 1] - buffer[i];
            
            // Wenn das aktuelle Sample größer ist als die umgebenden Samples,
            // und somit ein lokales Maximum darstellt, erhöhe den Rhythmus-Score
            if (diff1 > 0 && diff2 < 0)
            {
                rhythmScore += buffer[i] * buffer[i];
            }
        }

        return rhythmScore;
    }

    private static double CalculatePitchScore(float[] buffer)
    {
        double pitchScore = 0.0;

        // Annahme: buffer enthält Gleitkommazahlenrepräsentation des Audiosignals

        for (int i = 1; i < buffer.Length; i++)
        {
            // Berechne die Änderung der Amplitude zwischen aufeinanderfolgenden Abtastwerten
            double amplitudeChange = Math.Abs(buffer[i] - buffer[i - 1]);

            // Addiere die Änderung zur Gesamtsumme
            pitchScore += amplitudeChange;
        }

        return pitchScore;
    }
}
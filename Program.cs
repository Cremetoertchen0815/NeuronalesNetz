

using NeuronalesNetz;

var tr = new TraingDataGenerator();
tr.LoadDataFromFolder(Path.GetFullPath("songs/"), AudioType.MUSIC);
tr.LoadDataFromFolder(Path.GetFullPath("speech/"), AudioType.SPEECH);
var trainingData = tr.GetTrainingSamples(5000).Select(item => (AudioParameters.FromSample(item.data), item.type)).ToArray();

var guesses = 0;
var correctGuesses = 0;

foreach (var item in trainingData)
{
    var sampleData = new AudioModel.ModelInput()
    {
        Energy = (float)item.Item1.Energy,
        Rhythm = (float)item.Item1.Rhythm,
        Pitch = (float)item.Item1.Pitch,
    };

    //Load model and predict output
    var result = AudioModel.Predict(sampleData);
    var correctGuess = (AudioType)result.Type == item.type;
    Console.WriteLine("Guess " + guesses);
    if (correctGuess) correctGuesses++;
    guesses++;
}

Console.WriteLine($"Guessed {correctGuesses} of {guesses} correct ({(float)correctGuesses / guesses * 100}%)");


//var file = new StreamWriter("training_data.csv");

//file.WriteLine("Energy,Rhythm,Pitch,Type");
//foreach (var item in trainingData)
//{
//    file.WriteLine($"{item.Item1.Energy.ToString(System.Globalization.CultureInfo.InvariantCulture)},{item.Item1.Rhythm.ToString(System.Globalization.CultureInfo.InvariantCulture)},{item.Item1.Pitch.ToString(System.Globalization.CultureInfo.InvariantCulture)},{item.type}");
//}

//file.Close();

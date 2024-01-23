

using NeuronalesNetz;

var tr = new TraingDataGenerator();
tr.LoadDataFromFolder(Path.GetFullPath("songs/"), AudioType.MUSIC);
tr.SaveTestFile();
Console.WriteLine();






//using CSharpNet;

//var layers = new int[] { 3, 6, 3, 1 };
//var learningRate = 0.1;
//var activationFunction = ActivationFunction.Sigmoid;
//var nn = new DeepNetBuilder(layers, learningRate, activationFunction);



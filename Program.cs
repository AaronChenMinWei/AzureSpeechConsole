using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

class Program
{
	// This example requires environment variables named "SPEECH_KEY" and "SPEECH_REGION"
	static string speechKey = "e04de2b4e7a64067bf7407bd1aa85c5d";
	static string speechRegion = "southeastasia";
	static Uri speechContainerHost = new Uri("ws://20.195.37.25");

	static void OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
	{
		switch (speechRecognitionResult.Reason)
		{
			case ResultReason.RecognizedSpeech:
				Console.WriteLine($"RECOGNIZED: Text={speechRecognitionResult.Text}");
				break;
			case ResultReason.NoMatch:
				Console.WriteLine($"NOMATCH: Speech could not be recognized.");
				break;
			case ResultReason.Canceled:
				var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
				Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

				if (cancellation.Reason == CancellationReason.Error)
				{
					Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
					Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
					Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
				}
				break;
		}
	}

	async static Task Main(string[] args)
	{
		var speechConfig = SpeechConfig.FromHost(speechContainerHost);
		speechConfig.SpeechRecognitionLanguage = "en-US";

		using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
		using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

		Console.WriteLine($"Connected to {speechContainerHost.OriginalString}");
		Console.WriteLine("Speak into your microphone.");
		var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
		OutputSpeechRecognitionResult(speechRecognitionResult);
	}
}
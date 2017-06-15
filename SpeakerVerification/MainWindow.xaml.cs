using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.ProjectOxford.SpeakerRecognition;
using Microsoft.ProjectOxford.SpeakerRecognition.Contract;
using Microsoft.ProjectOxford.SpeakerRecognition.Contract.Verification;
using NAudio.Utils;
using NAudio.Wave;
using System.Runtime.CompilerServices;
using System.IO;

namespace SpeakerVerification
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SpeakerVerificationServiceClient verServiceClient;
        private WaveIn _waveIn;
        private WaveFileWriter _fileWriter;
        private Stream _stream;
        private Guid _speakerId;
        private string userPhrase;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                verServiceClient = new SpeakerVerificationServiceClient("1cb9ba6056ee4ba1be230ac93bc1358c");
                initializeRecorder();
                initializeSpeaker();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
        }


        /// <summary>
        /// Initialize the speaker information
        /// </summary>
        private async void initializeSpeaker()
        {
            try
            {
                //CreateProfileResponse response = await verServiceClient.CreateProfileAsync("en-us");
                //Console.WriteLine("Profile id :" + response.ProfileId.ToString());
                //_speakerId = response.ProfileId;
                _speakerId = Guid.Parse("72431ee5-d02a-4c6c-985c-1217088ed2ec");
                Profile profile = await verServiceClient.GetProfileAsync(_speakerId);
                remEnrollText.Text = profile.RemainingEnrollmentsCount.ToString();
                verPhraseText = null;
                refreshPhrases();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            } 

        }

        /// <summary>
        /// Initialize NAudio recorder instance
        /// </summary>
        private void initializeRecorder()
        {
            _waveIn = new WaveIn();
            _waveIn.DeviceNumber = 0;
            int sampleRate = 16000; // 16 kHz
            int channels = 1; // mono
            _waveIn.WaveFormat = new WaveFormat(sampleRate, channels);
            _waveIn.DataAvailable += waveIn_DataAvailable;
            _waveIn.RecordingStopped += waveSource_RecordingStopped;
        }

        /// <summary>
        /// A listener called when the recording stops
        /// </summary>
        /// <param name="sender">Sender object responsible for event</param>
        /// <param name="e">A set of arguments sent to the listener</param>
        private void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            _fileWriter.Dispose();
            _fileWriter = null;
            _stream.Seek(0, SeekOrigin.Begin);
            //Dispose recorder object
            _waveIn.Dispose();
            initializeRecorder();
            //enrollSpeaker(_stream);
            verifySpeaker(_stream);
        }

        /// <summary>
        /// A method that's called whenever there's a chunk of audio is recorded
        /// </summary>
        /// <param name="sender">The sender object responsible for the event</param>
        /// <param name="e">The arguments of the event object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (_fileWriter == null)
            {
                _stream = new IgnoreDisposeStream(new MemoryStream());
                _fileWriter = new WaveFileWriter(_stream, _waveIn.WaveFormat);
            }
            _fileWriter.Write(e.Buffer, 0, e.BytesRecorded);
        }

        /// <summary>
        /// Refresh the list of phrases
        /// </summary>
        private async void refreshPhrases()
        {
            Title = String.Format("Retrieving available phrases...");
            //record.IsEnabled = false;
            try
            {
                VerificationPhrase[] phrases = await verServiceClient.GetPhrasesAsync("en-us");
                foreach (VerificationPhrase phrase in phrases)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = phrase.Phrase;
                    phrasesList.Items.Add(item);
                }
                Title = String.Format("Retrieving available phrases done");
            }
            catch (PhrasesException exp)
            {
                Console.WriteLine("Cannot retrieve phrases: " + exp.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            //record.IsEnabled = true;
        }

        /// <summary>
        /// Enrolls the audio of the speaker
        /// </summary>
        /// <param name="audioStream">The audio stream</param>
        private async void enrollSpeaker(Stream audioStream)
        {
            try
            {
                Enrollment response = await verServiceClient.EnrollAsync(audioStream, _speakerId);
                
                Title = String.Format("Enrollment Done");
                remEnrollText.Text = response.RemainingEnrollments.ToString();
                //verPhraseText.Text = response.Phrase;
                userPhrase = response.Phrase.ToString();
                

                Console.WriteLine("Your phrase is : " + response.Phrase.ToString());

                if (response.RemainingEnrollments == 0)
                {
                    MessageBox.Show("You have now completed the minimum number of enrollments. You may perform verification or add more enrollments", "Speaker enrolled");
                    Console.WriteLine("Enrolled");
                }
                else
                {
                    Console.WriteLine("Enrolling");
                }
                
            }
            catch (EnrollmentException exception)
            {
                Console.WriteLine("Cannot enroll speaker: " + exception.Message);
            }
            catch (Exception gexp)
            {
                Console.WriteLine("Error: " + gexp.Message);
            }
        }


        /// <summary>
        /// Verifies the speaker by using the audio
        /// </summary>
        /// <param name="audioStream">The audio stream</param>
        private async void verifySpeaker(Stream audioStream)
        {
            try
            {
                Title = String.Format("Verifying....");
                Verification response = await verServiceClient.VerifyAsync(_stream, _speakerId);
                Title = String.Format("Verification Done.");
                statusResTxt.Text = response.Result.ToString();
                confTxt.Text = response.Confidence.ToString();
                if (response.Result == Result.Accept)
                {
                    statusResTxt.Background = Brushes.Green;
                    statusResTxt.Foreground = Brushes.White;
                }
                else
                {
                    statusResTxt.Background = Brushes.Red;
                    statusResTxt.Foreground = Brushes.White;
                }
            }
            catch (VerificationException exception)
            {
                Console.WriteLine("Cannot verify speaker: " + exception.Message);
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error : " + ex.Message);
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void recordBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WaveIn.DeviceCount == 0)
                {
                    throw new Exception("Cannot detect microphone.");
                }
                _waveIn.StartRecording();
                recordBtn.IsEnabled = false;
                stopRecordBtn.IsEnabled = true;
                Title = String.Format("Recording...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void stopRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            recordBtn.IsEnabled = true;
            stopRecordBtn.IsEnabled = false;
            _waveIn.StopRecording();
            Title = String.Format("Enrolling...");
            
        }

        private async void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Title = String.Format("Verifying....");
                Verification response = await verServiceClient.VerifyAsync(_stream, _speakerId);
                Title = String.Format("Verification Done.");
                statusResTxt.Text = response.Result.ToString();
                confTxt.Text = response.Confidence.ToString();
                if (response.Result == Result.Accept)
                {
                    statusResTxt.Background = Brushes.Green;
                    statusResTxt.Foreground = Brushes.White;
                }
                else
                {
                    statusResTxt.Background = Brushes.Red;
                    statusResTxt.Foreground = Brushes.White;
                }
            }
            catch (VerificationException exception)
            {
                Console.WriteLine("Cannot verify speaker: " + exception.Message);
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error : " + ex.Message);
            }
        }

        private async void enrollBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Enrollment response = await verServiceClient.EnrollAsync(_stream, _speakerId);

                Title = String.Format("Enrollment Done");
                remEnrollText.Text = response.RemainingEnrollments.ToString();
                verPhraseText.Text = response.Phrase;
                userPhrase = response.Phrase.ToString();


                Console.WriteLine("Your phrase: " + response.Phrase);

                if (response.RemainingEnrollments == 0)
                {
                    MessageBox.Show("You have now completed the minimum number of enrollments. You may perform verification or add more enrollments", "Speaker enrolled");
                    Console.WriteLine("Enrolled");
                }
                else
                {
                    Console.WriteLine("Enrolling");
                }
            }
            catch (EnrollmentException exception)
            {
                Console.WriteLine("Cannot enroll speaker: " + exception.Message);
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error : " + ex.Message);
            }

        }
    }
}

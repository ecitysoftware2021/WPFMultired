using Accord.Video.FFMPEG;
using AForge.Video.DirectShow;
using Emgu.CV;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WPFMultired.Classes;
using WPFMultired.Classes.UseFull;

namespace WPFMultired.ViewModel
{
    internal class MainWindowViewModel : ObservableObject, IDisposable
    {
        #region "Campos Privados"
        private static BitmapImage _image;
        private VideoFileWriter _writer;
        private bool _recording;
        VideoCapture _capture;
        private Mat _frame;
        public static string PhotoName;
        #endregion

        #region "CallBacks"
        public static Action<bool> CallBackGrabar { get; set; }
        public static Action<bool> CallBackPotho { get; set; }
        #endregion

        #region "Propiedades"
        public ObservableCollection<FilterInfo> VideoDevices { get; set; }
        public BitmapImage Image
        {
            get { return _image; }
            set { Set(ref _image, value); }
        }
        public ICommand StartRecordingCommand { get; private set; }
        public ICommand SaveSnapshotCommand { get; private set; }
        public ICommand StopSourceCommand { get; private set; }
        string VideoName = string.Empty;
        #endregion

        #region "Constructor"
        public MainWindowViewModel()
        {

            VideoDevices = new ObservableCollection<FilterInfo>();
            StartRecordingCommand = new RelayCommand(StartRecording);
            SaveSnapshotCommand = new RelayCommand(SaveSnapshot);
            StopSourceCommand = new RelayCommand(StopRecording);
            GetVideoDevices();
        }
        #endregion

        #region "Metodos"
        /// <summary>
        /// metodo para buscar las camaras disponibles
        /// </summary>
        private void GetVideoDevices(int num = 0)
        {
            try
            {
                var devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                _capture = new VideoCapture(num);
                _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Focus, 66);
                _capture.ImageGrabbed += ProcessFrame;
                _frame = new Mat();

                if (_capture != null)
                {
                    _capture.Start();
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, ex.ToString());
            }
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            try
            {
                if (_capture != null && _capture.Ptr != IntPtr.Zero)
                {
                    _capture.Retrieve(_frame, 0);
                }

                using (var bitmap = (Bitmap)_frame.Bitmap.Clone())
                {
                    var bi = bitmap.ToBitmapImage();
                    bi.Freeze();
                    Dispatcher.CurrentDispatcher.Invoke(() => Image = bi);
                }

                CallBackGrabar?.Invoke(true);

            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, ex.ToString());
            }
        }

        /// <summary>
        /// limpia la memoria
        /// </summary>
        private static void FlushMemory()
        {
            try
            {
                var proces = Process.GetCurrentProcess();
                proces.MinWorkingSet = (IntPtr)300000;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "FaceRecognition", ex, ex.ToString());
            }
        }


        public void SaveSnapshot()
        {
            try
            {
                if (Image != null)
                {
                    string pathPhoto = Path.Combine(Directory.GetCurrentDirectory(), "Photos");
                    if (!Directory.Exists(pathPhoto))
                    {
                        Directory.CreateDirectory(pathPhoto);
                    }
                    string[] files = Directory.GetFiles(pathPhoto);

                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }

                    var dialog = new SaveFileDialog();
                    dialog.FileName = Path.Combine(pathPhoto, PhotoName + ".png"); ;
                    dialog.DefaultExt = ".png";

                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(Image));

                    using (var filestream = new FileStream(dialog.FileName, FileMode.Create))
                    {
                        encoder.Save(filestream);
                    }

                    CallBackPotho?.Invoke(true);
                }
                FlushMemory();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, ex.ToString());
                CallBackPotho?.Invoke(false);
            }
        }

        /// <summary>
        /// Metodo que detiene la grabacion del video y llama al metodo que lo procesa
        /// </summary>
        private void StopRecording()
        {
            try
            {
                _capture.ImageGrabbed -= ProcessFrame;
                _recording = false;
                FlushMemory();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, ex.ToString());
            }
        }

        /// <summary>
        /// metodo para iniciar la grabacion
        /// </summary>
        private void StartRecording()
        {
            try
            {
                _writer = new VideoFileWriter();
                _recording = true;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, ex.ToString());
            }
        }

        public void Dispose()
        {
            StopRecording();
        }
        #endregion
    }
}

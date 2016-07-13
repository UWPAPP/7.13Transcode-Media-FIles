using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.MediaProperties;
using Windows.Media.Transcoding;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace _7._13Transcode_Media_FIles
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //选择要转换的视频文件
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".mp4");

            //源文件
            StorageFile source = await openPicker.PickSingleFileAsync();

            //选择要把转换的视频文件保存的位置
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
            savePicker.DefaultFileExtension = ".mp4";
            savePicker.SuggestedFileName = "New Video";

            savePicker.FileTypeChoices.Add("MPEG4", new string[] { ".mp4" });

            //目标的保存文件
            StorageFile destination = await savePicker.PickSaveFileAsync();

            //媒体编码配置文件
            MediaEncodingProfile profile = MediaEncodingProfile.CreateMp4(VideoEncodingQuality.HD720p);

            //转换文件
            MediaTranscoder transcoder = new MediaTranscoder();
            PrepareTranscodeResult prepareOp = await
                transcoder.PrepareFileTranscodeAsync(source, destination, profile);
            if (prepareOp.CanTranscode)
            {
                var transcodeOp = prepareOp.TranscodeAsync();

                transcodeOp.Progress +=
                    new AsyncActionProgressHandler<double>(TranscodeProgress);
                transcodeOp.Completed +=
                    new AsyncActionWithProgressCompletedHandler<double>(TranscodeComplete);
            }
            else
            {
                switch (prepareOp.FailureReason)
                {
                    case TranscodeFailureReason.CodecNotFound:
                        System.Diagnostics.Debug.WriteLine("Codec not found.");
                        break;
                    case TranscodeFailureReason.InvalidProfile:
                        System.Diagnostics.Debug.WriteLine("Invalid profile.");
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Unknown failure.");
                        break;
                }
            }

        }

        //转换文件执行过程中
        void TranscodeProgress(IAsyncActionWithProgress<double> asyncInfo, double percent)
        {
            // Display or handle progress info.
        }

        //转换文件结束
        void TranscodeComplete(IAsyncActionWithProgress<double> asyncInfo, AsyncStatus status)
        {
            asyncInfo.GetResults();
            if (asyncInfo.Status == AsyncStatus.Completed)
            {

            }
            else if (asyncInfo.Status == AsyncStatus.Canceled)
            {

            }
            else
            {

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UniTransnap.Class;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.DataTransfer;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.Services.MicrosoftTranslator;
using Windows.UI.ViewManagement;
using Windows.UI;


// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace UniTransnap
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class TransPage : Page
    {
        string authenticationHeaderValue = string.Empty;
        Windows.ApplicationModel.Resources.ResourceLoader rsrcs;

        string before, after;
        string transResult;
        string ja, en, zh, es, ru, hi, ar, bn, pt, de;
        DataPackage dataPackage;

        private const string SubscriptionKey = "3387e3b71ccd4a5fbbd7e8a39b3ccafb";
        //Enter here the Key from your Microsoft Translator Text subscription on http://portal.azure.com

        public TransPage()
        {
            this.InitializeComponent();

            //admAuth = new AdmAuthentication();
            dataPackage = new DataPackage();
            rsrcs = new Windows.ApplicationModel.Resources.ResourceLoader();

            initializeCombobox();

            //BeforeLanguageBox.SelectedIndex = 0;
            //AfterLanguageBox.SelectedIndex = 1;


            if (ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay))
            {
                pinpButton.Visibility = Visibility.Visible;
            }

            TransratorInitialize();

        }

        private async void TransratorInitialize()
        {
            await TranslatorService.Instance.InitializeAsync(SubscriptionKey);
        }

        private void IC()
        {
            historyCommand.IsOpen = true;
        }

        private void initializeCombobox()
        {
            var resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();

            ja = resourceLoader.GetString("ja");
            en = resourceLoader.GetString("en");
            zh = resourceLoader.GetString("zh");
            es = resourceLoader.GetString("es");
            ru = resourceLoader.GetString("ru");
            de = resourceLoader.GetString("de");
            hi = resourceLoader.GetString("hi");
            ar = resourceLoader.GetString("ar");
            bn = resourceLoader.GetString("bn");
            pt = resourceLoader.GetString("pt");

            //BeforeLanguageBox.Items.Add(ja);//ja
            //BeforeLanguageBox.Items.Add(en);//en
            //BeforeLanguageBox.Items.Add(zh);//zh
            //BeforeLanguageBox.Items.Add(es);//es
            //BeforeLanguageBox.Items.Add(ru);//ru
            //BeforeLanguageBox.Items.Add(de);//de
            //BeforeLanguageBox.Items.Add(hi);//hi
            //BeforeLanguageBox.Items.Add(ar);//ar
            //BeforeLanguageBox.Items.Add(bn);//
            //BeforeLanguageBox.Items.Add(pt);//bn

            AfterLanguageBox.Items.Add(ja);
            AfterLanguageBox.Items.Add(en);
            AfterLanguageBox.Items.Add(zh);
            AfterLanguageBox.Items.Add(es);
            AfterLanguageBox.Items.Add(ru);
            AfterLanguageBox.Items.Add(de);
            AfterLanguageBox.Items.Add(hi);
            AfterLanguageBox.Items.Add(ar);
            AfterLanguageBox.Items.Add(bn);
            AfterLanguageBox.Items.Add(pt);
        }

        /// <summary>
        /// メインファンクション
        /// </summary>
        /// 
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataStore();
        }

        /// <summary>
        /// データのセーブや読み込みなどを行う
        /// </summary>
        private void DataStore()
        {
            try
            {
                using (var db = new HistoryContext())
                {
                    HistoryList.ItemsSource = db.Historys.ToList();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private void BeforeLanguageBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
      if (//BeforeLanguageBox.SelectedIndex == 0) before = "ja";
      else if (//BeforeLanguageBox.SelectedIndex == 1) before = "en";
      else if (//BeforeLanguageBox.SelectedIndex == 2) before = "zh";
      else if (//BeforeLanguageBox.SelectedIndex == 3) before = "es";
      else if (//BeforeLanguageBox.SelectedIndex == 4) before = "ru";
      else if (//BeforeLanguageBox.SelectedIndex == 5) before = "de";
      else if (//BeforeLanguageBox.SelectedIndex == 6) before = "hi";
      else if (//BeforeLanguageBox.SelectedIndex == 7) before = "ar";
      else if (//BeforeLanguageBox.SelectedIndex == 8) before = "bn";
      else if (//BeforeLanguageBox.SelectedIndex == 9) before = "pt";
      */
        }

        private void AfterLanguageBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AfterLanguageBox.SelectedIndex == 0) after = "ja";
            else if (AfterLanguageBox.SelectedIndex == 1) after = "en";
            else if (AfterLanguageBox.SelectedIndex == 2) after = "zh";
            else if (AfterLanguageBox.SelectedIndex == 3) after = "es";
            else if (AfterLanguageBox.SelectedIndex == 4) after = "ru";
            else if (AfterLanguageBox.SelectedIndex == 5) after = "de";
            else if (AfterLanguageBox.SelectedIndex == 6) after = "hi";
            else if (AfterLanguageBox.SelectedIndex == 7) after = "ar";
            else if (AfterLanguageBox.SelectedIndex == 8) after = "bn";
            else if (AfterLanguageBox.SelectedIndex == 9) after = "pt";
        }

        private async void dialogView()
        {
            this.webDlg.Title = rsrcs.GetString("plzwrd"); var result = await this.webDlg.ShowAsync();
        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterForShare();
        }

        private void reloadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new HistoryContext())
                {
                    HistoryList.ItemsSource = db.Historys.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async private void TranslateButton_Tapped(object sender, TappedRoutedEventArgs e)
        {

            //if (//BeforeLanguageBox.SelectedItem == null || AfterLanguageBox.SelectedItem == null)
            if (AfterLanguageBox.SelectedItem == null)
            { this.webDlg.Title = "言語を選択してください"; var result = await this.webDlg.ShowAsync(); }
            else
            {
                string data;
                if (InputTextBox.Text == "") { this.webDlg.Title = "言葉を入力してください"; var result = await this.webDlg.ShowAsync(); }
                else
                {
                    /*
          Translator.LanguageServiceClient client = new Translator.LanguageServiceClient();
          // TextBoxに入力した文章を英語から日本語への翻訳を行う
          var result = await client.TranslateAsync(authenticationHeaderValue, InputTextBox.Text, before, after, null, null);
          */

                    var result = await TranslatorService.Instance.TranslateAsync(InputTextBox.Text, after);

                    var b_language = await TranslatorService.Instance.DetectLanguageAsync(InputTextBox.Text);


                    OutputTextBox.DataContext = new { op = result };

                    //History history = new History() { before_langage = (string)//BeforeLanguageBox.SelectedItem, after_langage = (string)AfterLanguageBox.SelectedItem, before_word = InputTextBox.Text, after_word = result };
                    History history = new History() { before_langage = b_language, after_langage = (string)AfterLanguageBox.SelectedItem, before_word = InputTextBox.Text, after_word = result };

                    try
                    {
                        using (var dbh = new HistoryContext())
                        {
                            dbh.Historys.Add(history);
                            dbh.SaveChanges();
                            HistoryList.ItemsSource = dbh.Historys.ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                    transResult = result;

                }
            }
        }



        private void CopyButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            dataPackage.SetText(transResult);
            Clipboard.SetContent(dataPackage);
        }

        private void copyClipboardButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            History item = (History)HistoryList.SelectedItem;

            using (var db = new HistoryContext())
            {
                var history = db.Historys.Where(x => x.Id == item.Id).FirstOrDefault();
                try
                {
                    dataPackage.SetText(history.after_word);
                    Clipboard.SetContent(dataPackage);
                }
                catch (Exception ex)
                { }
            }

        }

        private void AllDeleteButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                using (var db = new HistoryContext())
                {
                    var hs = db.Historys;
                    db.Historys.RemoveRange(hs);
                    db.SaveChanges();
                    HistoryList.ItemsSource = db.Historys.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            //BeforeLanguageBox.SelectedIndex = 0;
            AfterLanguageBox.SelectedIndex = 1;
            InputTextBox.Text = "";
            OutputTextBox.Text = "";
        }

        private void changeWordButton_Click(object sender, RoutedEventArgs e)
        {
            string b, a;

            b = InputTextBox.Text;
            a = OutputTextBox.Text;

            InputTextBox.Text = a;
            OutputTextBox.Text = b;
        }

        private void ChengeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int b = 0, a = 0, tmp = 0;

            b = //BeforeLanguageBox.SelectedIndex;
            a = AfterLanguageBox.SelectedIndex;

            //BeforeLanguageBox.SelectedIndex = a;
            AfterLanguageBox.SelectedIndex = b;

        }

        private async void DeleteButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                History item = (History)HistoryList.SelectedItems[0];
                Debug.WriteLine(item.Id);

                // レコードの削除
                using (var db = new HistoryContext())
                {
                    var person = db.Historys.Where(x => x.Id == item.Id).FirstOrDefault();
                    db.Historys.Remove(person);
                    await db.SaveChangesAsync();
                    HistoryList.ItemsSource = db.Historys.ToList();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }


        /// <summary>
        /// クリップボードへコピーなど、そこら辺を行う。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>



        private void HistoryList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            IC();
        }

        /// <summary>
        /// share
        /// </summary>
        /// 


        private void AppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.ShareTextHandler);
            DataTransferManager.ShowShareUI();

        }

        private void RegisterForShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.ShareTextHandler);
        }

        private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {

            var resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();
            /*
            string cr = resourceLoader.GetString("checkresult");
            string rs = resourceLoader.GetString("resultsend");
            string of = resourceLoader.GetString("Offence");
            string de = resourceLoader.GetString("Defence");
            string ay1 = resourceLoader.GetString("affinityresult1");
            string ay2 = resourceLoader.GetString("affinityresult2");
            string And = resourceLoader.GetString("_And");
             * */
            try
            {
                History item = (History)HistoryList.SelectedItem;
                History history;
                using (var db = new HistoryContext())
                {
                    history = db.Historys.Where(x => x.Id == item.Id).FirstOrDefault();
                }

                DataRequest request = e.Request;
                request.Data.Properties.Title = resourceLoader.GetString("trnsltrslt");
                request.Data.Properties.Description = resourceLoader.GetString("outrslt");
                if (HistoryList.SelectedItem == null) request.Data.SetText(resourceLoader.GetString("lstslct"));
                else request.Data.SetText(history.before_langage + " : " + history.before_word + " → " + history.after_langage + " : " + history.after_word);


            }
            catch { }
        }




        private async void pinpButton_Click(object sender, RoutedEventArgs e)
        {

            var appView = ApplicationView.GetForCurrentView();
            // タイトルバーにもUIを展開表示するための設定（これは必須ではありませんが、見た目的には必要）
            Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            appView.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            appView.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;


            ViewModePreferences compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            compactOptions.CustomSize = new Windows.Foundation.Size(360, 800);

            bool modeSwitched = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);
            pinpButton.Visibility = Visibility.Collapsed;
            standardButton.Visibility = Visibility.Visible;
        }

        private async void standard(object sender, RoutedEventArgs e)
        {
            /*
            var appView = ApplicationView.GetForCurrentView();
            // タイトルバーを透過表示するための設定を元に戻す
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
            appView.TitleBar.ButtonBackgroundColor = null;
            appView.TitleBar.ButtonInactiveBackgroundColor = null;
            */

            bool modeSwitched = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);
            standardButton.Visibility = Visibility.Collapsed;
            pinpButton.Visibility = Visibility.Visible;

        }


    }
}

﻿using System;
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
using Windows.UI.Core;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Diagnostics;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace UniTransnap
{
  /// <summary>
  /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
  /// </summary>
  public sealed partial class TransPage : Page
  {
    string authenticationHeaderValue = string.Empty;
    AdmAuthentication admAuth;
    Windows.ApplicationModel.Resources.ResourceLoader rsrcs;
    // AzureAuthToken authTokenSource;

    string before, after;
    ObservableCollection<History> HistView;
    string transResult;
    string ja, en, zh, es, ru, hi, ar, bn, pt, de;
    DataPackage dataPackage;

    private const string SubscriptionKey = "3387e3b71ccd4a5fbbd7e8a39b3ccafb";
    //Enter here the Key from your Microsoft Translator Text subscription on http://portal.azure.com

    public TransPage()
    {
      this.InitializeComponent();

      //admAuth = new AdmAuthentication();
      HistView = new ObservableCollection<History>();
      dataPackage = new DataPackage();
      rsrcs = new Windows.ApplicationModel.Resources.ResourceLoader();

      //admAuth.AdmAuthentication2("roob_twi", "0OGK8MPcfIGFX6BtYhCbBI5V+EBp//2E3BF95HOu4Vs=");

      initializeCombobox();

      BeforeLanguageBox.SelectedIndex = 0;
      AfterLanguageBox.SelectedIndex = 1;

      authenticationHeaderValue = Translate2();

    }

    private static string Translate2()
    {
      var authTokenSource = new AzureAuthToken(SubscriptionKey);
      var authenticationHeaderValue = string.Empty;
      try
      {
        authenticationHeaderValue = authTokenSource.GetAccessToken();
      }
      catch (HttpRequestException)
      {

        switch (authTokenSource.RequestStatusCode)
        {
          case HttpStatusCode.Unauthorized:
            //Debug.WriteLine("Request to token service is not authorized (401). Check that the Azure subscription key is valid.");
            break;
          case HttpStatusCode.Forbidden:
            //Debug.WriteLine("Request to token service is not authorized (403). For accounts in the free-tier, check that the account quota is not exceeded.");
            break;
        }
        throw;

      }
      return authenticationHeaderValue;
    }

    private void IC()
    {
      mainCommandBar.IsOpen = true;
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

      BeforeLanguageBox.Items.Add(ja);//ja
      BeforeLanguageBox.Items.Add(en);//en
      BeforeLanguageBox.Items.Add(zh);//zh
      BeforeLanguageBox.Items.Add(es);//es
      BeforeLanguageBox.Items.Add(ru);//ru
      BeforeLanguageBox.Items.Add(de);//de
      BeforeLanguageBox.Items.Add(hi);//hi
      BeforeLanguageBox.Items.Add(ar);//ar
      BeforeLanguageBox.Items.Add(bn);//
      BeforeLanguageBox.Items.Add(pt);//bn

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

      BeforeLanguageBox.Header = resourceLoader.GetString("bfrlng");
      BeforeLanguageBox.PlaceholderText = resourceLoader.GetString("bfrlng_p");
      AfterLanguageBox.Header = resourceLoader.GetString("aftlng");
      AfterLanguageBox.PlaceholderText = resourceLoader.GetString("aftlng_p");
      InputTextBox.Header = resourceLoader.GetString("bfrwrd");
      InputTextBox.PlaceholderText = resourceLoader.GetString("bfrwrd_p");
      OutputTextBox.Header = resourceLoader.GetString("aftwrd");
      OutputTextBox.PlaceholderText = resourceLoader.GetString("aftwrd_p");

      HistoryList.Header = resourceLoader.GetString("hstry");
    }

    /// <summary>
    /// メインファンクション
    /// </summary>

    private void BeforeLanguageBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (BeforeLanguageBox.SelectedIndex == 0) before = "ja";
      else if (BeforeLanguageBox.SelectedIndex == 1) before = "en";
      else if (BeforeLanguageBox.SelectedIndex == 2) before = "zh";
      else if (BeforeLanguageBox.SelectedIndex == 3) before = "es";
      else if (BeforeLanguageBox.SelectedIndex == 4) before = "ru";
      else if (BeforeLanguageBox.SelectedIndex == 5) before = "de";
      else if (BeforeLanguageBox.SelectedIndex == 6) before = "hi";
      else if (BeforeLanguageBox.SelectedIndex == 7) before = "ar";
      else if (BeforeLanguageBox.SelectedIndex == 8) before = "bn";
      else if (BeforeLanguageBox.SelectedIndex == 9) before = "pt";

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
    private void getToken()
    {
      AdmAccessToken admToken;

      try
      {
        admToken = admAuth.GetAccessToken();
        authenticationHeaderValue = "Bearer " + admToken.access_token;
      }
      catch (Exception e)
      {
        dialogView();
      }
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
      int x = HistoryList.SelectedIndex;
      //if (x != null)
      try
      {
        string y = HistView[x].before_word;
        InputTextBox.Text = y;
      }
      catch (Exception ex)
      { }
      // test(y);
    }

    async private void TranslateButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (authenticationHeaderValue == string.Empty)
      {
        try
        {
          //  authenticationHeaderValue = await authTokenSource.GetAccessTokenAsync();
          authenticationHeaderValue = Translate2();
        }
        catch
        {
          this.webDlg.Title = rsrcs.GetString("plzwrd"); var result = await this.webDlg.ShowAsync();
          return;
        }
      }

      if (BeforeLanguageBox.SelectedItem == null || AfterLanguageBox.SelectedItem == null)
      { this.webDlg.Title = "言語を選択してください"; var result = await this.webDlg.ShowAsync(); }
      else
      {
        string data;
        if (InputTextBox.Text == "") { this.webDlg.Title = "言葉を入力してください"; var result = await this.webDlg.ShowAsync(); }
        else
        {
          Translator.LanguageServiceClient client = new Translator.LanguageServiceClient();
          // TextBoxに入力した文章を英語から日本語への翻訳を行う
          var result = await client.TranslateAsync(authenticationHeaderValue, InputTextBox.Text, before, after, null, null);
          //var result = await client.TranslateAsync(authenticationHeaderValue, "Hello World", "en", "fr", null, "general");
          OutputTextBox.DataContext = new { op = result };
          HistView.Add(new History { before_langage = (string)BeforeLanguageBox.SelectedItem, after_langage = (string)AfterLanguageBox.SelectedItem, before_word = InputTextBox.Text, after_word = result });
          HistoryList.ItemsSource = HistView;
          transResult = result;
          try
          {
          }
          catch (Exception ex)
          {
          }
          // data = (string)BeforeLanguageBox.SelectedItem + "/" + (string)AfterLanguageBox.SelectedItem + "/" + InputTextBox.Text + "/" + result + "\n";
          // DataSave(data);
        }
      }
    }

    private void CopyButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      dataPackage.SetText(transResult);
      Clipboard.SetContent(dataPackage);
    }

    private void saveButton_Click(object sender, RoutedEventArgs e)
    {


      try
      {
        int x = HistoryList.SelectedIndex;
        string msg = HistView[x].before_langage + "/" + HistView[x].after_langage + "/" + HistView[x].before_word + "/" + HistView[x].after_word + "\n";
        DataSave(msg);
        /*
        IList<Object> items = HistoryList.SelectedItems as IList<Object>;
        string msg ="";
        foreach (Object item in items)
        {
          msg += items.ToString();
        }
        */
        //OutputTextBox.Text = msg;
      }
      catch (Exception ex)
      { }
      // data = (string)BeforeLanguageBox.SelectedItem + "/" + (string)AfterLanguageBox.SelectedItem + "/" + InputTextBox.Text + "/" + result + "\n";
      // DataSave(data);

    }

    private void copyClipboardButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      int x = HistoryList.SelectedIndex;
      try
      {
        dataPackage.SetText(HistView[x].after_word);
        Clipboard.SetContent(dataPackage);
      }
      catch (Exception ex)
      { }

    }

    private async void allDeleteButton_Tapped(object sender, TappedRoutedEventArgs e)
    {

      String filePath = "date1.txt";
      StorageFolder roamingFolder = ApplicationData.Current.RoamingFolder;
      try
      {
        StorageFile file = await roamingFolder.GetFileAsync(filePath);

        try
        {
          await file.DeleteAsync();
          HistoryList.ItemsSource = null;
        }
        catch { }
      }
      catch (Exception ex)
      {
        // ファイル無し
      }
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
      BeforeLanguageBox.SelectedIndex = 0;
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

      b = BeforeLanguageBox.SelectedIndex;
      a = AfterLanguageBox.SelectedIndex;

      BeforeLanguageBox.SelectedIndex = a;
      AfterLanguageBox.SelectedIndex = b;

    }


    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      DataStore();
    }

    /// <summary>
    /// データのセーブや読み込みなどを行う
    /// </summary>
    async private void DataStore()
    {
      //    string msg = null;
      String filePath = "date1.txt";
      StorageFolder roamingFolder = ApplicationData.Current.RoamingFolder;
      try
      {
        StorageFile file = await roamingFolder.GetFileAsync(filePath);
        IList<String> strList = await FileIO.ReadLinesAsync(file);
        foreach (String str in strList)
        {

          //          msg = str;
          DataRestore(str);
          //          datas.Add(str);
        }
        //DataRestore(msg);
      }
      catch (Exception ex)
      {
        // ファイル無し
      }

    }

    private void DataRestore(string msg0)
    {
      string[] msg1 = msg0.Split('/');
      HistView.Add(new History { before_langage = msg1[0], after_langage = msg1[1], before_word = msg1[2], after_word = msg1[3] });
      HistoryList.ItemsSource = HistView;
    }

    async private void DataSave(string data)
    {

      String filePath = "date1.txt";

      try
      {
        StorageFolder roamingFolder = ApplicationData.Current.RoamingFolder;
        StorageFile file = await roamingFolder.CreateFileAsync(filePath,
          CreationCollisionOption.OpenIfExists);

        //        data = "0/0/0/0/0/0\n";
        await FileIO.AppendTextAsync(file, data);
      }
      catch (Exception ex)
      { }
      //      DataReset();
    }

    async private void DeleteButton_Tapped(object sender, TappedRoutedEventArgs e)
    {


      try
      {
        int x = HistoryList.SelectedIndex;
        HistView.RemoveAt(x);
      }
      catch { }

      /*
            for (int i = 0; i < HistView.Count; i++)
            {
              HistView.RemoveAt(i);
            }
            String filePath = "date1.txt";
            StorageFolder roamingFolder = ApplicationData.Current.RoamingFolder;
            try
            {
              StorageFile file = await roamingFolder.GetFileAsync(filePath);

              try
              {
                await file.DeleteAsync();
              }
              catch { }
            }
            catch (Exception ex)
            {
              // ファイル無し
            }

          StorageFolder localFolder = ApplicationData.Current.LocalFolder;
          try
          {
            StorageFile file = await localFolder.CreateFileAsync(filePath, CreationCollisionOption.ReplaceExisting);
            datas.RemoveAt(index);
            foreach (String str in datas)
            {
              await FileIO.AppendTextAsync(file, str + '\n');
            }
          }

          catch (Exception ex)
          {

          }
          */
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
        int i = HistoryList.SelectedIndex;
        DataRequest request = e.Request;
        //          request.Data.Properties.ApplicationName = "タイプ相性チェッカー";
        request.Data.Properties.Title = resourceLoader.GetString("trnsltrslt");
        request.Data.Properties.Description = resourceLoader.GetString("outrslt");
        if (HistoryList.SelectedItem == null) request.Data.SetText(resourceLoader.GetString("lstslct"));
        else request.Data.SetText(HistView[i].before_langage + " : " + HistView[i].before_word + " → " + HistView[i].after_langage + " : " + HistView[i].after_word);

      }
      catch { }
    }


    /// <summary>
    /// 定義したクラス
    /// </summary>

    [DataContract]
    public class History
    {
      [DataMember]
      public string before_langage { get; set; }
      [DataMember]
      public string after_langage { get; set; }
      [DataMember]
      public string before_word { get; set; }
      [DataMember]
      public string after_word { get; set; }
    }


  }
}

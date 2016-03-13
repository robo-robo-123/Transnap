using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UniTransnap.Class;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 を参照してください

namespace UniTransnap
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
    string authenticationHeaderValue = null;
    AdmAuthentication admAuth;
    public MainPage()
    {
      this.InitializeComponent();

      admAuth = new AdmAuthentication();

      admAuth.AdmAuthentication2("roob_twi", "0OGK8MPcfIGFX6BtYhCbBI5V+EBp//2E3BF95HOu4Vs=");

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
        OutputTextBlock.Text = "もう少し間をおいてください．";
      }
    }


    private async void transrate()
    {
      Translator.LanguageServiceClient client = new Translator.LanguageServiceClient();
      // TextBoxに入力した文章を英語から日本語への翻訳を行う
      var result = await client.TranslateAsync(authenticationHeaderValue, InputTextBox.Text, "ja", "en", null, null);
      OutputTextBlock.Text = result;
    }

    private void TranslateButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (authenticationHeaderValue == null)
      {
        try
        {
          getToken();
        }
        catch
        {
          OutputTextBlock.Text = "もう少し間をおいてください．";
          return;
        }
      }

      transrate();
    }

    private void getTokenButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      getToken();
    }
  }
}

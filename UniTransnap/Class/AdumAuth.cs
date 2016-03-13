using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Threading;

namespace UniTransnap.Class
{
  [DataContract]
  public class AdmAccessToken
  {
    [DataMember]
    public string access_token { get; set; }
    [DataMember]
    public string token_type { get; set; }
    [DataMember]
    public string expires_in { get; set; }
    [DataMember]
    public string scope { get; set; }
  }
  public class AdmAuthentication
  {
    public static readonly string DatamarketAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
    private string clientId;
    private string clientSecret;
    private string request;
    private AdmAccessToken token;
    private Timer accessTokenRenewer;
    //Access token expires every 10 minutes. Renew it every 9 minutes only.
    private const int RefreshTokenDuration = 9;

    public AdmAuthentication()
    {
     // AdmAuthentication2(clientId, clientSecret);
    }

    public async void AdmAuthentication2(string clientId, string clientSecret)
    {
      this.clientId = clientId;
      this.clientSecret = clientSecret;
      //If clientid or client secret has special characters, encode before sending request

      this.request = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", WebUtility.UrlEncode(clientId), WebUtility.UrlEncode(clientSecret));
      Task<AdmAccessToken> tokenTask = HttpPost(DatamarketAccessUri, this.request);

      this.token = await tokenTask;
      //renew the token every specified minutes
      accessTokenRenewer = new Timer(new TimerCallback(OnTokenExpiredCallback), this, TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
    }
    public AdmAccessToken GetAccessToken()
    {
      return this.token;
    }
    private async void RenewAccessToken()
    {
      Task<AdmAccessToken> taskToken = HttpPost(DatamarketAccessUri, this.request);

      AdmAccessToken newAccessToken = await taskToken;
      //swap the new token with old one
      //Note: the swap is thread unsafe
      this.token = newAccessToken;
    }
    private void OnTokenExpiredCallback(object stateInfo)
    {
      try
      {
        RenewAccessToken();
      }
      catch (Exception ex)
      {
      }
      finally
      {
        try
        {
          accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
        }
        catch (Exception ex)
        {
        }
      }
    }


    private async void GetRSA(string requestDetails, WebRequest webRequest)
    {
      using (var strm = await webRequest.GetRequestStreamAsync())
      using (var writer = new System.IO.BinaryWriter(strm))
      {
        var bytes = Encoding.ASCII.GetBytes(requestDetails);
        writer.Write(bytes, 0, bytes.Length);
      }

    }

    private async Task<AdmAccessToken> GRA(WebRequest webRequest)
    {
      using (var webResponse = await webRequest.GetResponseAsync())
      {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
        //Get deserialized object from JSON stream
        token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());
        return token;
      }
    }


    private async Task<AdmAccessToken> HttpPost(string DatamarketAccessUri, string requestDetails)
    {
      //Prepare OAuth request 

      var webRequest = WebRequest.Create(DatamarketAccessUri);

//      HttpWebRequest webRequest2 = HttpWebRequest.CreateHttp(DatamarketAccessUri);
      
      webRequest.ContentType = "application/x-www-form-urlencoded";
      webRequest.Method = "POST";

      //byte[] bytes = Encoding.ASCII.GetBytes(requestDetails);
      // webRequest2.ContentLength = bytes.Length;
      //Task<Stream> outputStream = GetRSA();
      //GetRSA(requestDetails, webRequest);
      
      using (var strm = await webRequest.GetRequestStreamAsync())
      using (var writer = new System.IO.BinaryWriter(strm))
      {
        var bytes = Encoding.ASCII.GetBytes(requestDetails);
        writer.Write(bytes, 0, bytes.Length);
      }
      
      //using (var strm = await webRequest.GetResponseAsync())
      //Task<AdmAccessToken> token = GRA(webRequest);
      //AdmAccessToken token2 = await token;
      
      using (var webResponse = await webRequest.GetResponseAsync())
      {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
        //Get deserialized object from JSON stream
         token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());
        return token;
      }
      
//      return token;
    }
  }
}

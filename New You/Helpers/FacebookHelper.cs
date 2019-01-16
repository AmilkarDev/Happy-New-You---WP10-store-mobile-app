using Facebook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Security.Authentication.Web;

namespace New_You.Helpers
{
    class FacebookHelper
    {
        FacebookClient _fb = new FacebookClient();  
        readonly Uri _callbackUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri();  
         Uri _loginUrl;
         private const string FacebookAppId = "562246680652243";//Enter your FaceBook App ID here  
         private const string FacebookPermissions = "publish_actions";  
        public string AccessToken  
        {  
            get { return _fb.AccessToken; }  
        }  
  
        public   FacebookHelper()  
        {  
            _loginUrl = _fb.GetLoginUrl(new  
                    {  
                        client_id = FacebookAppId,  
                        redirect_uri = _callbackUri.AbsoluteUri,  
                        scope = FacebookPermissions,  
                        display = "popup",  
                        response_type = "token"  
                    });  
            Debug.WriteLine(_callbackUri);//This is useful for fill Windows Store ID in Facebook WebSite  
        }  
        private void ValidateAndProccessResult(WebAuthenticationResult result)  
        {  
            if (result.ResponseStatus == WebAuthenticationStatus.Success)  
            {  
                var responseUri = new Uri(result.ResponseData.ToString());  
                var facebookOAuthResult = _fb.ParseOAuthCallbackUrl(responseUri);  
  
                if (string.IsNullOrWhiteSpace(facebookOAuthResult.Error))  
                    _fb.AccessToken = facebookOAuthResult.AccessToken;  
                else  
                {//error de acceso denegado por cancelación en página  
                }  
            }  
            else if (result.ResponseStatus == WebAuthenticationStatus.ErrorHttp)  
            {// error de http  
            }  
            else  
            {  
                _fb.AccessToken = null;//Keep null when user signout from facebook  
            }  
        }  
        public void LoginAndContinue()  
        {  
            WebAuthenticationBroker.AuthenticateAndContinue(_loginUrl);  
        }  
        public void ContinueAuthentication(WebAuthenticationBrokerContinuationEventArgs args)  
        {  
            ValidateAndProccessResult(args.WebAuthenticationResult);  
        }  
    }
}

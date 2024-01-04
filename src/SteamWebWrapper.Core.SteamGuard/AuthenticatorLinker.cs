using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SteamWebWrapper.Core.Contracts.Entities.SteamGuard.Session;

namespace SteamWebWrapper.SteamGuard
{
    /// <summary>
    /// Handles the linking process for a new mobile authenticator.
    /// </summary>
    public class AuthenticatorLinker
    {
        /// <summary>
        /// Session data containing an access token for a steam account generated with k_EAuthTokenPlatformType_MobileApp
        /// </summary>
        private SessionData Session;

        /// <summary>
        /// Set to register a new phone number when linking. If a phone number is not set on the account, this must be set. If a phone number is set on the account, this must be null.
        /// </summary>
        public string PhoneNumber = null;
        public string PhoneCountryCode = null;

        /// <summary>
        /// Randomly-generated device ID. Should only be generated once per linker.
        /// </summary>
        public string DeviceId { get; private set; }

        /// <summary>
        /// After the initial link step, if successful, this will be the SteamGuard data for the account. PLEASE save this somewhere after generating it; it's vital data.
        /// </summary>
        public SteamGuardAccount LinkedAccount { get; private set; }

        /// <summary>
        /// True if the authenticator has been fully finalized.
        /// </summary>
        public bool Finalized = false;

        /// <summary>
        /// Set when the confirmation email to set a phone number is set
        /// </summary>
        private bool _confirmationEmailSent;

        /// <summary>
        /// Email address the confirmation email was sent to when adding a phone number
        /// </summary>
        public string ConfirmationEmailAddress;

        /// <summary>
        /// Create a new instance of AuthenticatorLinker
        /// </summary>
        /// <param name="accessToken">Access token for a Steam account created with k_EAuthTokenPlatformType_MobileApp</param>
        /// <param name="steamid">64 bit formatted steamid for the account</param>
        public AuthenticatorLinker(SessionData sessionData)
        {
            Session = sessionData;
            DeviceId = GenerateDeviceId();
        }

        /// <summary>
        /// First step in adding a mobile authenticator to an account
        /// </summary>
        public async Task<LinkResult> AddAuthenticator()
        {
            // This method will be called again once the user confirms their phone number email
            if (_confirmationEmailSent)
            {
                // Check if email was confirmed
                var isStillWaiting = await IsAccountWaitingForEmailConfirmation();
                if (isStillWaiting)
                {
                    return LinkResult.MustConfirmEmail;
                }

                // Now send the SMS to the phone number
                await SendPhoneVerificationCode();

                // This takes time so wait a bit
                await Task.Delay(2000);
            }

            // Make request to ITwoFactorService/AddAuthenticator
            var addAuthenticatorBody = new NameValueCollection();
            addAuthenticatorBody.Add("steamid", Session.SteamId.ToString());
            addAuthenticatorBody.Add("authenticator_time", (await TimeAligner.GetSteamTimeAsync()).ToString());
            addAuthenticatorBody.Add("authenticator_type", "1");
            addAuthenticatorBody.Add("device_identifier", DeviceId);
            addAuthenticatorBody.Add("sms_phone_id", "1");
            var addAuthenticatorResponseStr = await SteamWeb.PostRequest("https://api.steampowered.com/ITwoFactorService/AddAuthenticator/v1/?access_token=" + Session.AccessToken, null, addAuthenticatorBody);

            // Parse response json to object
            var addAuthenticatorResponse = JsonConvert.DeserializeObject<AddAuthenticatorResponse>(addAuthenticatorResponseStr);

            if (addAuthenticatorResponse == null || addAuthenticatorResponse.Response == null)
                return LinkResult.GeneralFailure;

            // Status 2 means no phone number is on the account
            if (addAuthenticatorResponse.Response.Status == 2)
            {
                if (PhoneNumber == null)
                {
                    return LinkResult.MustProvidePhoneNumber;
                }
                // Add phone number

                // Get country code
                var countryCode = PhoneCountryCode;

                // If given country code is null, use the one from the Steam account
                if (string.IsNullOrEmpty(countryCode))
                {
                    countryCode = await GetUserCountry();
                }

                // Set the phone number
                var res = await _setAccountPhoneNumber(PhoneNumber, countryCode);

                // Make sure it's successful then respond that we must confirm via email
                if (res != null && res.Response.ConfirmationEmailAddress != null)
                {
                    ConfirmationEmailAddress = res.Response.ConfirmationEmailAddress;
                    _confirmationEmailSent = true;
                    return LinkResult.MustConfirmEmail;
                }

                // If something else fails, we end up here
                return LinkResult.FailureAddingPhone;
            }

            if (addAuthenticatorResponse.Response.Status == 29)
                return LinkResult.AuthenticatorPresent;

            if (addAuthenticatorResponse.Response.Status != 1)
                return LinkResult.GeneralFailure;

            // Setup this.LinkedAccount
            LinkedAccount = addAuthenticatorResponse.Response;
            LinkedAccount.DeviceId = DeviceId;
            LinkedAccount.Session = Session;

            return LinkResult.AwaitingFinalization;
        }

        public async Task<FinalizeResult> FinalizeAddAuthenticator(string smsCode)
        {
            var tries = 0;
            while (tries <= 10)
            {
                var finalizeAuthenticatorValues = new NameValueCollection();
                finalizeAuthenticatorValues.Add("steamid", Session.SteamId.ToString());
                finalizeAuthenticatorValues.Add("authenticator_code", LinkedAccount.GenerateSteamGuardCode());
                finalizeAuthenticatorValues.Add("authenticator_time", TimeAligner.GetSteamTime().ToString());
                finalizeAuthenticatorValues.Add("activation_code", smsCode);
                finalizeAuthenticatorValues.Add("validate_sms_code", "1");

                string finalizeAuthenticatorResultStr;
                using (var wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    wc.Headers[HttpRequestHeader.UserAgent] = SteamWeb.MobileAppUserAgent;
                    var finalizeAuthenticatorResult = await wc.UploadValuesTaskAsync(new Uri("https://api.steampowered.com/ITwoFactorService/FinalizeAddAuthenticator/v1/?access_token=" + Session.AccessToken), "POST", finalizeAuthenticatorValues);
                    finalizeAuthenticatorResultStr = Encoding.UTF8.GetString(finalizeAuthenticatorResult);
                }

                var finalizeAuthenticatorResponse = JsonConvert.DeserializeObject<FinalizeAuthenticatorResponse>(finalizeAuthenticatorResultStr);

                if (finalizeAuthenticatorResponse == null || finalizeAuthenticatorResponse.Response == null)
                {
                    return FinalizeResult.GeneralFailure;
                }

                if (finalizeAuthenticatorResponse.Response.Status == 89)
                {
                    return FinalizeResult.BadSmsCode;
                }

                if (finalizeAuthenticatorResponse.Response.Status == 88)
                {
                    if (tries >= 10)
                    {
                        return FinalizeResult.UnableToGenerateCorrectCodes;
                    }
                }

                if (!finalizeAuthenticatorResponse.Response.Success)
                {
                    return FinalizeResult.GeneralFailure;
                }

                if (finalizeAuthenticatorResponse.Response.WantMore)
                {
                    tries++;
                    continue;
                }

                LinkedAccount.FullyEnrolled = true;
                return FinalizeResult.Success;
            }

            return FinalizeResult.GeneralFailure;
        }

        private async Task<string> GetUserCountry()
        {
            var getCountryBody = new NameValueCollection { { "steamid", Session.SteamId.ToString() } };
            var getUserCountryLink = $"https://api.steampowered.com/IUserAccountService/GetUserCountry/v1?access_token={Session.AccessToken}";
            var getCountryResponseStr = await SteamWeb.PostRequest(getUserCountryLink, null, getCountryBody);

            // Parse response json to object
            var response = JsonConvert.DeserializeObject<GetUserCountryResponse>(getCountryResponseStr);
            return response.Response.Country;
        }

        private async Task<SetAccountPhoneNumberResponse> _setAccountPhoneNumber(string phoneNumber, string countryCode)
        {
            var setPhoneNumberLink = $"https://api.steampowered.com/IPhoneService/SetAccountPhoneNumber/v1?access_token={Session.AccessToken}";
            
            var setPhoneBody = new NameValueCollection
            {
                { "phone_number", phoneNumber },
                { "phone_country_code", countryCode }
            };
            var getCountryResponseStr = await SteamWeb.PostRequest(setPhoneNumberLink, null, setPhoneBody);
            return JsonConvert.DeserializeObject<SetAccountPhoneNumberResponse>(getCountryResponseStr);
        }

        private async Task<bool> IsAccountWaitingForEmailConfirmation()
        {
            var isWaitingLink = $"https://api.steampowered.com/IPhoneService/IsAccountWaitingForEmailConfirmation/v1?access_token={Session.AccessToken}";
            var waitingForEmailResponse = await SteamWeb.PostRequest(isWaitingLink, null, null);

            // Parse response json to object
            var response = JsonConvert.DeserializeObject<IsAccountWaitingForEmailConfirmationResponse>(waitingForEmailResponse);
            return response.Response.AwaitingEmailConfirmation;
        }

        private async Task SendPhoneVerificationCode()
        {
            var verificationLink = $"https://api.steampowered.com/IPhoneService/SendPhoneVerificationCode/v1?access_token={Session.AccessToken}";
            await SteamWeb.PostRequest(verificationLink, null, null);
        }

        private static string GenerateDeviceId()
        {
            return "android:" + Guid.NewGuid();
        }
    }

    public enum FinalizeResult
    {
        BadSmsCode,
        UnableToGenerateCorrectCodes,
        Success,
        GeneralFailure
    }

    public enum LinkResult
    {
        MustProvidePhoneNumber, //No phone number on the account
        MustRemovePhoneNumber, //A phone number is already on the account
        MustConfirmEmail, //User need to click link from confirmation email
        AwaitingFinalization, //Must provide an SMS code
        GeneralFailure, //General failure (really now!)
        AuthenticatorPresent,
        FailureAddingPhone
    }

    internal class GetUserCountryResponseResponse
    {
        [JsonProperty("country")]
        public string Country { get; set; }
    }

    internal class SetAccountPhoneNumberResponse
    {
        [JsonProperty("response")]
        public SetAccountPhoneNumberResponseResponse Response { get; set; }
    }

    internal class SetAccountPhoneNumberResponseResponse
    {
        [JsonProperty("confirmation_email_address")]
        public string ConfirmationEmailAddress { get; set; }

        [JsonProperty("phone_number_formatted")]
        public string PhoneNumberFormatted { get; set; }
    }

    internal class IsAccountWaitingForEmailConfirmationResponse
    {
        [JsonProperty("response")]
        public IsAccountWaitingForEmailConfirmationResponseResponse Response { get; set; }
    }

    internal class IsAccountWaitingForEmailConfirmationResponseResponse
    {
        [JsonProperty("awaiting_email_confirmation")]
        public bool AwaitingEmailConfirmation { get; set; }

        [JsonProperty("seconds_to_wait")]
        public int SecondsToWait { get; set; }
    }

    internal class AddAuthenticatorResponse
    {
        [JsonProperty("response")]
        public SteamGuardAccount Response { get; set; }
    }

    internal class FinalizeAuthenticatorResponse
    {
        [JsonProperty("response")]
        public FinalizeAuthenticatorInternalResponse Response { get; set; }

        internal class FinalizeAuthenticatorInternalResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("want_more")]
            public bool WantMore { get; set; }

            [JsonProperty("server_time")]
            public long ServerTime { get; set; }

            [JsonProperty("status")]
            public int Status { get; set; }
        }
    }

    internal class GetUserCountryResponse
    {
        [JsonProperty("response")]
        public GetUserCountryResponseResponse Response { get; set; }
    }
}

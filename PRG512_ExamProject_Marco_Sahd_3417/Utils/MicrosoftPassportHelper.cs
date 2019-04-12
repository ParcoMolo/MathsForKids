using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using PRG512_ExamProject_Marco_Sahd_3417.Models;

namespace PRG512_ExamProject_Marco_Sahd_3417.Utils
{
    public static class MicrosoftPassportHelper
    {
        /// <summary>
        /// Checks to see if Passport is ready to be used.
        /// 
        /// Passport has dependencies on:
        ///     1. Having a connected Microsoft Account
        ///     2. Having a Windows PIN set up for that _account on the local machine
        /// </summary>
        public static async Task<bool> MicrosoftPassportAvailableCheckAsync()
        {
            bool keyCredentialAvailable = await KeyCredentialManager.IsSupportedAsync();
            if (keyCredentialAvailable == false)
            {
                // Key credential is not enabled yet as user 
                // needs to connect to a Microsoft Account and select a PIN in the connecting flow.
                Debug.WriteLine("Microsoft Passport is not setup!\nPlease go to Windows Settings and set up a PIN to use it.");
                return false;
            }

            return true;
        }
        
        public static async Task<bool> CreatePassportKeyAsync(string accountId)
        {
            KeyCredentialRetrievalResult keyCreationResult = await KeyCredentialManager.RequestCreateAsync(accountId, KeyCredentialCreationOption.ReplaceExisting);

            switch (keyCreationResult.Status)
            {
                case KeyCredentialStatus.Success:
                    Debug.WriteLine("Successfully made key");

                    // In the real world authentication would take place on a server.
                    // So every time a user migrates or creates a new Microsoft Passport account Passport details should be pushed to the server.
                    // The details that would be pushed to the server include:
                    // The public key, keyAttesation if available, 
                    // certificate chain for attestation endorsement key if available,  
                    // status code of key attestation result: keyAttestationIncluded or 
                    // keyAttestationCanBeRetrievedLater and keyAttestationRetryType
                    // As this sample has no concept of a server it will be skipped for now
                    // for information on how to do this refer to the second Passport sample

                    //For this sample just return true
                    return true;
                case KeyCredentialStatus.UserCanceled:
                    Debug.WriteLine("User cancelled sign-in process.");
                    break;
                case KeyCredentialStatus.NotFound:
                    // User needs to setup Microsoft Passport
                    Debug.WriteLine("Microsoft Passport is not setup!\nPlease go to Windows Settings and set up a PIN to use it.");
                    break;
                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// Function to be called when user requests deleting their account.
        /// Checks the KeyCredentialManager to see if there is a Passport for the current user
        /// Then deletes the local key associated with the Passport.
        /// </summary>
        public static async void RemovePassportAccountAsync(Account account)
        {
            // Open the account with Passport
            KeyCredentialRetrievalResult keyOpenResult = await KeyCredentialManager.OpenAsync(account.Username);

            if (keyOpenResult.Status == KeyCredentialStatus.Success)
            {
                // In the real world you would send key information to server to unregister
                //e.g. RemovePassportAccountOnServer(account);
            }

            // Then delete the account from the machines list of Passport Accounts
            await KeyCredentialManager.DeleteAsync(account.Username);
        }

        /// <summary>
        /// Attempts to sign a message using the Passport key on the system for the accountId passed.
        /// </summary>
        /// <returns>Boolean representing if creating the Passport authentication message succeeded</returns>
        public static async Task<bool> GetPassportAuthenticationMessageAsync(Account account)
        {
            KeyCredentialRetrievalResult openKeyResult = await KeyCredentialManager.OpenAsync(account.Username);
            // Calling OpenAsync will allow the user access to what is available in the app and will not require user credentials again.
            // If you wanted to force the user to sign in again you can use the following:
            // var consentResult = await Windows.Security.Credentials.UI.UserConsentVerifier.RequestVerificationAsync(account.Username);
            // This will ask for the either the password of the currently signed in Microsoft Account or the PIN used for Microsoft Passport.

            if (openKeyResult.Status == KeyCredentialStatus.Success)
            {
                // If OpenAsync has succeeded, the next thing to think about is whether the client application requires access to backend services.
                // If it does here you would Request a challenge from the Server. The client would sign this challenge and the server
                // would check the signed challenge. If it is correct it would allow the user access to the backend.
                // You would likely make a new method called RequestSignAsync to handle all this
                // e.g. RequestSignAsync(openKeyResult);
                // Refer to the second Microsoft Passport sample for information on how to do this.

                // For this sample there is not concept of a server implemented so just return true.
                return true;
            }
            else if (openKeyResult.Status == KeyCredentialStatus.NotFound)
            {
                // If the _account is not found at this stage. It could be one of two errors. 
                // 1. Microsoft Passport has been disabled
                // 2. Microsoft Passport has been disabled and re-enabled cause the Microsoft Passport Key to change.
                // Calling CreatePassportKey and passing through the account will attempt to replace the existing Microsoft Passport Key for that account.
                // If the error really is that Microsoft Passport is disabled then the CreatePassportKey method will output that error.
                if (await CreatePassportKeyAsync(account.Username))
                {
                    // If the Passport Key was again successfully created, Microsoft Passport has just been reset.
                    // Now that the Passport Key has been reset for the _account retry sign in.
                    return await GetPassportAuthenticationMessageAsync(account);
                }
            }

            // Can't use Passport right now, try again later
            return false;
        }
    }
}
using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using ME.Pushy.Sdk;
using ME.Pushy.Sdk.Util.Exceptions;
using Android.Util;
using Android.Content.PM;

namespace SamplePushy.XamarinAndroid
{
    [Activity (Label = "SamplePushyXamarinAndroid", LaunchMode=LaunchMode.SingleTask, MainLauncher = true, Icon = "@mipmap/ic_launcher")]
    public class MainActivity : Activity
    {
        TextView instructions;

        TextView registrationId;

        protected override async void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // Cache TextView objects
            instructions = FindViewById<TextView> (Resource.Id.instructions);
            registrationId = FindViewById<TextView> (Resource.Id.registrationId);

            // Restart the socket service, in case the user force-closed the app
            Pushy.Listen (this);

            // Register device for push notifications (async)
            var registrationResult = await RegisterDevice ();

            // Registration failed?
            if (registrationResult.Error != null)
            {
                // Write error to logcat
                Log.Error ("Pushy", "Registration failed: " + registrationResult.Error.Message);

                // Display registration failed in app UI
                instructions.SetText (Resource.String.restartApp);
                registrationId.SetText (Resource.String.registrationFailed);

                // Display error dialog
                new AlertDialog.Builder (this).SetTitle (Resource.String.registrationError)
                       .SetMessage (registrationResult.Error.Message)
                       .SetPositiveButton (GetString (Resource.String.ok), (sender, e) => { })
                       .Create ()
                       .Show ();
            }
            else
            {
                // Write registration ID to logcat
                Log.Debug ("Pushy", "Registration ID: " + registrationResult.RegistrationId);

                // Display registration ID and copy from logcat instructions
                instructions.SetText (Resource.String.copyLogcat);
                registrationId.Text = registrationResult.RegistrationId;
            }

        }

        async Task<RegistrationResult> RegisterDevice ()
        {
            var loading = new ProgressDialog (this);
            loading.SetMessage (GetString (Resource.String.registeringDevice));
            loading.SetCancelable (false);

            // Show it
            loading.Show ();

            // Prepare registration result
            var result = new RegistrationResult ();

            await Task.Run (() => {

                try
                {
                    // Get registration ID via Pushy and store it in result (this will return existing registration ID if already registered before)
                    // This cannot be run in the main thread.
                    result.RegistrationId = Pushy.Register (this);
                }
                catch (PushyException ex)
                {
                    // Store registration error in result
                    result.Error = ex;
                }

            });

            // Hide progress bar
            loading.Dismiss ();

            return result;
        }
    }
}


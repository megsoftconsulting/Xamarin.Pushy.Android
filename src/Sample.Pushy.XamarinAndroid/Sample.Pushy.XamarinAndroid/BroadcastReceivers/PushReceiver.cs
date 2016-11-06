using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;

namespace PushyBindingApp
{
    [BroadcastReceiver(Enabled=true, Exported=false)]
    [IntentFilter(new [] { "pushy.me" })]
    public class PushReceiver : BroadcastReceiver
    {
        public override void OnReceive (Context context, Intent intent)
        {
            string notificationTitle = "Pushy";
            string notificationText = "Test notification";

            // Attempt to extract the "message" property from the payload: {"message":"Hello World!"}
            if (intent.GetStringExtra ("message") != null)
            {
                notificationText = intent.GetStringExtra ("message");
            }

            // Prepare a notification with vibration, sound and lights
            var mBuilder = new Notification.Builder (context)
                    .SetSmallIcon (Android.Resource.Drawable.IcDialogInfo)
                    .SetContentTitle (notificationTitle)
                    .SetContentText (notificationText)
                    .SetLights (Color.Red, 1000, 1000)
                    .SetVibrate (new long [] { 0, 400, 250, 400 })
                   .SetSound (RingtoneManager.GetDefaultUri(RingtoneType.Notification ));

            // Get an instance of the NotificationManager service
            var mNotifyMgr = (NotificationManager)context.GetSystemService (Context.NotificationService);

            // Build the notification and display it
            mNotifyMgr.Notify (1, mBuilder.Build ());
        }
    }
}

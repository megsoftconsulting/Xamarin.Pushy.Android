using ME.Pushy.Sdk.Util.Exceptions;

namespace SamplePushy.XamarinAndroid
{
    public class RegistrationResult
    {
        public string RegistrationId { get; set; }

        public PushyException Error {get; set; }
    }
}
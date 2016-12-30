namespace Play.Flies.Client.Models.Messages
{
    public class DigitKeyPressedMessage
    {
        public DigitKeyPressedMessage(int key)
        {
            Key = key;
        }

        public int Key { get; }
    }
}
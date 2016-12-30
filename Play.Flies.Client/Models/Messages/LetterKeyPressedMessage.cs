namespace Play.Flies.Client.Models.Messages
{
    public class LetterKeyPressedMessage
    {
        public LetterKeyPressedMessage(string key)
        {
            Key = key;
        }

        public string Key { get; }
    }
}
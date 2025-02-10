using MidiaPost.CQRS.Core.Lib.Messages;



namespace MidiaPost.CQRS.Core.Lib.Events
{
    public abstract class BaseEvent : Message
    {
        protected BaseEvent(string type)
        {
            Type = type;
        }

        public string Type { get; set; }
        public int Version { get; set; }
    }

}

using System;
using System.Collections.Generic;

namespace WhatsappWebScrapper
{
    public class ChatMessages
    {
        string Person { get; set; }
        string Message { get; set; }
        string Time { get; set; }
        public ChatMessages(string person, string message, string time)
        {
            Person = person;
            Message = message;
            Time = time;
        }
        public string MessageString()
        {
            var text = String.Concat(this.Message, "--", this.Person, "--", this.Time);
            return text;
        }
    }

    public class Chat
    {
        string ChatName { get; set; }
        List<ChatMessages> Messages { get; set; }
        public Chat(string chatName)
        {
            ChatName = chatName;
            Messages = new List<ChatMessages>();
        }
        public void AddMessages(List<ChatMessages> msgs)
        {
            Messages.AddRange(msgs);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocktailProject.ClassCocktail;

namespace CocktailProject.ClassNPC
{
    public struct DayConversation
    {
        public List<TextConversation> BeforeOrder;
        public List<TextConversation> AfterServe;
        public List<TextConversation> ChitChat;
    }
    public struct TextConversation
    {
        public List<string> Conversation;
        public byte CurrentIndex;
    }

    public enum Enum_Mood
    {
        BeforeOrder,
        AfterServe,
        ChitChat
    }
    public class BaseCharacter
    {
        public SortedList<int, DayConversation> _DayConversations;
        protected int _currentConversationIndex;
        public string _Name;
        public bool HadMetPlayer;
        public byte MetPlayerTimes;
        public HashSet<Enum_TypeOfCocktail> _FavoriteTypeOfCocktail;
        protected Rectangle _SourceRectangle;

        public void SetSourceRectangle(Rectangle SouuceRec)
        {
            _SourceRectangle = new Rectangle();
        }

        public void AddConversation(int Day, DayConversation conversation)
        {
            _DayConversations.Add(Day, conversation);
        }

        public void AddTextBeforeOrder(int Day, TextConversation conversation)
        {
            if (_DayConversations.TryGetValue(Day, out DayConversation dayConversation))
            {
                dayConversation.BeforeOrder.Add(conversation);
                _DayConversations[Day] = dayConversation;
            }
        }
        public void AddTextAfterServe(int Day, TextConversation conversation)
        {
            if (_DayConversations.TryGetValue(Day, out DayConversation dayConversation))
            {
                dayConversation.AfterServe.Add(conversation);
                _DayConversations[Day] = dayConversation;
            }
        }
        public void AddTextChitChat(int Day, TextConversation conversation)
        {
            if (_DayConversations.TryGetValue(Day, out DayConversation dayConversation))
            {
                dayConversation.ChitChat.Add(conversation);
                _DayConversations[Day] = dayConversation;
            }
        }

        public string GetCoversationBeforeOrder(int Day)
        {
            if (_DayConversations.TryGetValue(Day, out DayConversation dayConversation))
            {
                if (dayConversation.BeforeOrder.Count > 0)
                {
                    var convo = dayConversation.BeforeOrder[0]; // pick the first conversation
                    if (convo.CurrentIndex < convo.Conversation.Count)
                    {
                        string text = convo.Conversation[convo.CurrentIndex];
                        convo.CurrentIndex++;
                        dayConversation.BeforeOrder[0] = convo; // put back updated struct
                        _DayConversations[Day] = dayConversation;
                        return text;
                    }
                }
            }
            return null;
        }


        //After Serve Conversation must have 3 things and will get 1 
        public string GetConversationAfterServe(int Day, Enum_CocktaillResualt result)
        {
            if (_DayConversations.TryGetValue(Day, out DayConversation dayConversation))
            {
                int index = 0;

                switch (result)
                {
                    case Enum_CocktaillResualt.Success:
                        index = 0;
                        break;
                    case Enum_CocktaillResualt.Aceptable:
                        index = 1;
                        break;
                    case Enum_CocktaillResualt.Fail:
                        index = 2;
                        break;
                }

                if (index < dayConversation.AfterServe.Count)
                {
                    var convo = dayConversation.AfterServe[0];

                    
                    string text = convo.Conversation[convo.CurrentIndex];

                    // update back
                    dayConversation.AfterServe[index] = convo;
                    _DayConversations[Day] = dayConversation;

                    return text;
                    
                }
            }

            return null;
        }

        public string GetConversationChitChat(int Day)
        {
            if (_DayConversations.TryGetValue(Day, out DayConversation dayConversation))
            {
                if (dayConversation.ChitChat.Count > 0)
                {
                    var convo = dayConversation.ChitChat[0]; // pick the first conversation
                    if (convo.CurrentIndex < convo.Conversation.Count)
                    {
                        string text = convo.Conversation[convo.CurrentIndex];
                        convo.CurrentIndex++;
                        dayConversation.ChitChat[0] = convo; // put back updated struct
                        _DayConversations[Day] = dayConversation;
                        return text;
                    }
                }
            }
            return null;
        }


        public byte GetCurrentConversationIndex() { return (byte)_currentConversationIndex; }
        public byte GetBeforeServeConversationCount(int Day)
        {
            if (_DayConversations.TryGetValue(Day, out DayConversation dayConversation))
            {
                return (byte)dayConversation.BeforeOrder.Count;
            }
            return 0;
        }
        public byte GetAfterServeConversationCount(int Day)
        {
            if (_DayConversations.TryGetValue(Day, out DayConversation dayConversation))
            {
                return (byte)dayConversation.AfterServe.Count;
            }
            return 0;
        }
        public byte GetChitChatConversationCount(int Day)
        {
            if (_DayConversations.TryGetValue(Day, out DayConversation dayConversation))
            {
                return (byte)dayConversation.ChitChat.Count;
            }
            return 0;
        }

        public float IsLastConversation() { return 0; }

    }


    
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CocktailProject.ClassCocktail;

namespace CocktailProject.ClassNPC
{
    public struct DayConversation
    {
        public List<TextConversation> BeforeOrder { get; set; }
        public List<TextConversation> ChitChat { get; set; }
    }

    public struct TextConversation
    {
        public List<string> Conversation { get; set; }
        public byte CurrentIndex { get; set; }
    }

    public class BaseCharacter
    {
        public SortedList<int, DayConversation> _DayConversations { get; set; }
            = new SortedList<int, DayConversation>();

        // Instead of one TextConversation → keep 3 slots by result
        public Dictionary<Enum_CocktaillResualt, TextConversation> AfterServe
            = new Dictionary<Enum_CocktaillResualt, TextConversation>();

        protected int _currentConversationIndex;
        public string _Name { get; set; }

        [JsonIgnore] protected string _CharaterID;
        [JsonIgnore] public bool HadMetPlayer { get; set; }
        [JsonIgnore] public byte NumberOfVisitsToDay { get; set; } // increments after finishing chit chat
        [JsonIgnore]
        public HashSet<Enum_TypeOfCocktail> _FavoriteTypeOfCocktail { get; set; }
            = new HashSet<Enum_TypeOfCocktail>();

        // -------------------- Add methods --------------------
        public void AddConversation(int Day, DayConversation conversation)
        {
            _DayConversations[Day] = conversation;
        }

        public BaseCharacter() { }

        public BaseCharacter(string Name)
        {
            this._Name = Name;
            _DayConversations = new SortedList<int, DayConversation>();
            AfterServe = new Dictionary<Enum_CocktaillResualt, TextConversation>();
            _FavoriteTypeOfCocktail = new HashSet<Enum_TypeOfCocktail>();
        }

        public void AddFavoriteTypeOfCocktail(Enum_TypeOfCocktail TypeOfCocktail)
        {
            this._FavoriteTypeOfCocktail.Add(TypeOfCocktail);
        }

        public string GetID()
        {
            return this._CharaterID;
        }

        public string SetID(string ID)
        {
            this._CharaterID = ID;
            return this._CharaterID;
        }

        public void AddDayConversationFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Load day conversations
            if (root.TryGetProperty("_DayConversations", out var dayConvObj))
            {
                foreach (var day in dayConvObj.EnumerateObject())
                {
                    int dayNum = int.Parse(day.Name);
                    var conv = JsonSerializer.Deserialize<DayConversation>(
                        day.Value.GetRawText()
                    );
                    _DayConversations[dayNum] = conv;
                }
            }

            // 🔹 Load AfterServe conversations
            if (root.TryGetProperty("AfterServe", out var afterServeObj))
            {
                foreach (var item in afterServeObj.EnumerateObject())
                {
                    if (Enum.TryParse<Enum_CocktaillResualt>(item.Name, out var result))
                    {
                        var convo = JsonSerializer.Deserialize<TextConversation>(
                            item.Value.GetRawText()
                        );
                        AfterServe[result] = convo;
                    }
                }
            }
        }


        public void AddTextBeforeOrder(int Day, TextConversation conversation)
        {
            if (!_DayConversations.ContainsKey(Day))
                _DayConversations[Day] = new DayConversation { BeforeOrder = new List<TextConversation>(), ChitChat = new List<TextConversation>() };

            _DayConversations[Day].BeforeOrder.Add(conversation);
        }

        public void AddTextAfterServe(Enum_CocktaillResualt result, TextConversation conversation)
        {
            AfterServe[result] = conversation;
        }

        public void AddTextChitChat(int Day, TextConversation conversation)
        {
            if (!_DayConversations.ContainsKey(Day))
                _DayConversations[Day] = new DayConversation { BeforeOrder = new List<TextConversation>(), ChitChat = new List<TextConversation>() };

            _DayConversations[Day].ChitChat.Add(conversation);
        }

        // -------------------- Get methods --------------------
        public string GetConversationBeforeOrder(int Day)
        {
            if (!_DayConversations.ContainsKey(Day)) return null;
            var dayConv = _DayConversations[Day];
            if (dayConv.BeforeOrder == null || dayConv.BeforeOrder.Count == 0) return null;

            int index = Math.Min(NumberOfVisitsToDay, (byte)(dayConv.BeforeOrder.Count - 1));
            var convo = dayConv.BeforeOrder[index];

            if (convo.CurrentIndex >= convo.Conversation.Count) return null;
            string line = convo.Conversation[convo.CurrentIndex];
            convo.CurrentIndex++;

            dayConv.BeforeOrder[index] = convo;
            _DayConversations[Day] = dayConv;

            return line;
        }

        public string GetConversationAfterServe(Enum_CocktaillResualt result)
        {
            if (!AfterServe.ContainsKey(result)) return null;

            var convo = AfterServe[result];
            if (convo.CurrentIndex >= convo.Conversation.Count) return null;

            string line = convo.Conversation[convo.CurrentIndex];
            convo.CurrentIndex++;
            AfterServe[result] = convo;

            return line;
        }

        public string GetConversationChitChat(int Day)
        {
            if (!_DayConversations.ContainsKey(Day)) return null;
            var dayConv = _DayConversations[Day];
            if (dayConv.ChitChat == null || dayConv.ChitChat.Count == 0) return null;

            int index = Math.Min(NumberOfVisitsToDay, (byte)(dayConv.ChitChat.Count - 1));
            var convo = dayConv.ChitChat[index];

            if (convo.CurrentIndex >= convo.Conversation.Count) { return null; }
            string line = convo.Conversation[convo.CurrentIndex];
            convo.CurrentIndex++;

            dayConv.ChitChat[index] = convo;
            _DayConversations[Day] = dayConv;

            
            return line;
        }


        public void InceaseNumberOfVisitTodya() {
            NumberOfVisitsToDay++;
        }
        public void ResetNumberOfVisitTodya()
        {
            NumberOfVisitsToDay = 0;
        }   

        public byte GetCurrentConversationIndex() => (byte)_currentConversationIndex;
    }
}

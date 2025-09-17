using CocktailProject.NPC;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailProject.Class_DialogLogic
{
    public class ConversationManager
    {
        private NPCBase activeNPC;
        private ConversationPhase currentPhase = ConversationPhase.SmallTalkBeforeOrder;
        private int dialogueIndex = 0;

        private TaggedTextRevealer AnimationText;
        private bool canSkipConversation = false;
        private bool canGoNextConversation = false;
        private bool haveDoneOrder = false; // game sets this when cocktail is served

        public void SetActiveNPC(NPCBase npc)
        {
            activeNPC = npc;
            currentPhase = ConversationPhase.SmallTalkBeforeOrder;
            dialogueIndex = 0;
            StartDialogue(currentPhase);
        }

        private string GetNextDialogue(ConversationPhase phase)
        {
            var lines = activeNPC.DialogueScripts[phase];
            if (dialogueIndex >= lines.Count) dialogueIndex = 0; // loop or reset
            return lines[dialogueIndex++];
        }

        private void StartDialogue(ConversationPhase nextPhase)
        {
            string line = GetNextDialogue(nextPhase);
            AnimationText = new TaggedTextRevealer(line, 0.1);
            AnimationText.Start();

            canSkipConversation = true;
            canGoNextConversation = false;
            currentPhase = nextPhase;
        }

        public string GetCurrentText()
        {
            if (AnimationText == null) return "";
            return AnimationText.GetVisibleText(); // ← shows animated text while revealing
        }

        public void Update(GameTime gameTime)
        {
            AnimationText.Update(gameTime);

            // Skip text animation
            if (!AnimationText.IsFinished() &&
                Core.Input.Mouse.WasButtonJustPressed(MonoGameLibrary.Input.MouseButton.Left) &&
                canSkipConversation)
            {
                AnimationText.Skip();
                canSkipConversation = false;
                canGoNextConversation = true;
                return;
            }

            // Animation finished naturally
            if (AnimationText.IsFinished())
            {
                canSkipConversation = false;
                canGoNextConversation = true;
            }

            // Advance dialogue
            if (canGoNextConversation &&
                Core.Input.Mouse.WasButtonJustPressed(MonoGameLibrary.Input.MouseButton.Left))
            {
                switch (currentPhase)
                {
                    case ConversationPhase.SmallTalkBeforeOrder:
                        StartDialogue(ConversationPhase.Ordering);
                        break;

                    case ConversationPhase.Ordering:
                        if (haveDoneOrder)
                            StartDialogue(ConversationPhase.SmallTalkAfterOrder);
                        break;

                    case ConversationPhase.SmallTalkAfterOrder:
                        StartDialogue(ConversationPhase.Complain);
                        break;

                    case ConversationPhase.Complain:
                        StartDialogue(ConversationPhase.SmallTalkBeforeOrder);
                        break;
                }
            }
        }

        // Allow outside game code to set when cocktail is done
        public void SetOrderDone() => haveDoneOrder = true;
    }
}

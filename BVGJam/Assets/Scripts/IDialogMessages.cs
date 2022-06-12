using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IDialogMessages : IEventSystemHandler {

    void playerChoosing(List<Conversation_Transition> _playerTransitions);
    void playerSpeaking(Conversation_Transition _playerChoice);
    void npcSpeaking(Conversation_NPCState _npcState);

    void closeConversation();

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IDialogMessages : IEventSystemHandler {

    void playerSpeaking(List<Conversation_Transition> _playerTransitions);
    void npcSpeaking(Conversation_NPCState _npcState);

    void closeConversation();

}

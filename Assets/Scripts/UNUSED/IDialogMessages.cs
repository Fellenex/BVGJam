using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IDialogMessages : IEventSystemHandler
{

    void playerChoosing(List<Conversation_Transition> _playerTransitions);
    void playerSpeaking(Conversation_State _playerChoice);
    void npcSpeaking(Conversation_State _statement);

    void setNextStatement(Conversation_State _state);

    void closeConversation();


    //void npcContinuesSpeaking(Conversation_State _npcState);

    bool hasMoreStatements(Conversation_State _state, Conversation_Statement _statement);

    void advanceConversation();

    //List<Conversation_Transition> getPossibleTransitions();


    //Gets the NPC state that will result from taking _transition
    Conversation_State getNewNPCState(Conversation_Transition _transition);

    //Checks to see if player meets the preconditions for a given transition option
    bool playerMeetsPreconditions(Conversation_Option _transitionOption);

    //User has selected a text option! We should display it
    void buttonClicked(Button button);
}

﻿using VRC.Udon;
using UnityEngine;

namespace KabulClient.Features.Worlds
{
    class AmongUs
    {
        public static bool worldLoaded = false;
        public static bool emergencyAnnoyEnabled = false;
        public static UdonBehaviour gameLogic = null;

        public static void Initialize(string sceneName)
        {
            // TODO: Check world ID aswell.
            if (sceneName == "Skeld")
            {
                gameLogic = GameObject.Find("Game Logic")?.GetComponent<UdonBehaviour>();

                if (gameLogic != null)
                {
                    worldLoaded = true;
                }
            }
            else
            {
                worldLoaded = false;
            }
        }

        public static void OnUpdate()
        {
            if (emergencyAnnoyEnabled)
            {
                EmergencyButton();
            }
        }

        public static void ToggleSabotageHud(bool value)
        {
            GameObject sabotageHud = GameObject.Find("Game Logic/Sabotage HUD");

            if (sabotageHud == null)
            {
                return;
            }

            sabotageHud.SetActive(value);
        }

        /// <summary>
        /// Starts an emergency meeting, this can be spammed and the results are VERY annoying to others aswell.
        /// </summary>
        public static void EmergencyButton()
        {
            CallUdonEvent("StartMeeting");
            CallUdonEvent("SyncEmergencyMeeting");
        }

        /// <summary>
        /// Calls a UDON event.
        /// </summary>
        /// <param name="eventName">The name of the event to call.</param>
        public static void CallUdonEvent(string eventName)
        {
            Udon.CallUdonEvent(gameLogic, eventName);
        }

        /// Available UDON events:
        /// Check
        /// Log
        /// SetErrorCode
        /// _start
        /// _onPlayerJoined
        /// _onPlayerLeft
        /// Btn_Start
        /// Btn_Abort
        /// Tgl_Mods
        /// SyncStart
        /// SyncCountdown
        /// SyncStartGame
        /// _update
        /// OnLocalPlayerAssignedRole
        /// KillLocalPlayer
        /// OnLocalPlayerKilled
        /// SyncVictoryB
        /// SyncVictoryM
        /// SyncAbort
        /// BoundsContainsPoint
        /// GetRandomPlayerSpawnPoint
        /// GetRandomMeetingSpawnPoint
        /// GetLocalPlayerNode
        /// Btn_Next
        /// Btn_Prev
        /// Btn_Enable
        /// Btn_Disable
        /// Btn_Home
        /// Btn_SpectatePlayer
        /// KillPlayersNearRadius
        /// ShouldShowKnives
        /// ShouldCheaterDropItem
        /// CanLocalPlayerPickUpBearTrap
        /// Tgl_Options
        /// LocalPlayerHasRole
        /// OnLocalPlayerKillsOther
        /// OnLocalPlayerReported
        /// Btn_Meeting
        /// SyncEmergencyMeeting
        /// SyncBodyFound
        /// StartMeeting
        /// OnLocalPlayerVotedFor
        /// OnPlayerVoteSubmitted
        /// Btn_SkipVoting
        /// OnPlayerWasReporter
        /// OnBodyWasFound
        /// SyncCloseVoting
        /// SyncEndVotingPhase
        /// SyncVoteResultNobody
        /// SyncVoteResultSkip
        /// SyncVoteResultTie
        /// ShowVoteResult
        /// OnLocalPlayerCompletedTask
        /// GameStartedWithDelay
        /// VisualTasksEnabled
        /// CanSeeTaskWaypoints
        /// CanDoTasks
        /// UpdateSabotageCooldowns
        /// CancelAllSabotage
        /// SomePlayerIsNear
        /// Btn_OpenSabotageMenu
        /// Btn_CloseSabotageMenu
        /// Btn_SabotageDoorsCafeteria
        /// SyncTrySabotageDoorsCafeteria
        /// SyncDoSabotageDoorsCafeteria
        /// SyncTrySabotageLights
        /// SyncBodyFound
        /// SyncRepairComms
        /// SyncRepairOxygenB
    }
}

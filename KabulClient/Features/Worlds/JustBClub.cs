using System;
using VRC.Udon;
using UnityEngine;
using UnityEngine.UI;
using MelonLoader;
using System.Collections.Generic;

namespace KabulClient.Features.Worlds
{
    class JustBClub
    {
        public class PrivateRoom
        {
            public int roomScore;
            public int roomNumber;
            public Vector3 position;
            public GameObject roomObject;

            public PrivateRoom(int roomNumber, Vector3 position, GameObject roomObject)
            {
                this.roomScore = -1;
                this.roomNumber = roomNumber;
                this.position = position;
                this.roomObject = roomObject;
            }
        }

        public static List<PrivateRoom> privateRooms = new List<PrivateRoom>();
        public static bool worldLoaded = false;
        public static bool roomsInitialized = false;

        /// <summary>
        /// Gets a room based off the room number.
        /// </summary>
        /// <param name="roomNumber">The room's number.</param>
        public static PrivateRoom GetRoomFromNumber(int roomNumber)
        {
            foreach (PrivateRoom privateRoom in privateRooms)
            {
                if (privateRoom.roomNumber == roomNumber)
                {
                    return privateRoom;
                }
            }

            return null;
        }

        public static void OnUpdate()
        {
            // Possible annoy features in "Udon/World Audio":
            // PlayUiPositive
            // PlayUiNegative
            // PlayUiOpen
            // PlayUiClose
        }

        /// <summary>
        /// Returns a score based off the room itself for how likely people are ERPing.
        /// </summary>
        /// <param name="room">The room object to check.</param>
        public static int CalculateRoomScore(PrivateRoom room)
        {
            int score = 0;

            string fixRoomName = room.roomNumber == 7 ? "VIP" : room.roomNumber.ToString();
            string externalObjectPath = $"Penthouse/Private Rooms Exterior/Room Entrances/Private Room Entrance {fixRoomName}";
            
            try
            {
                GameObject intercomButton = GameObject.Find($"{externalObjectPath}/BlueButtonWide - Intercom");
                GameObject lockIndicator = GameObject.Find($"{externalObjectPath}/Screen/Canvas/Indicators/Locked");
                
                Text occupantList = GameObject.Find($"{externalObjectPath}/Occupants Screen/Canvas/Text - Occupants")?.GetComponent<Text>();
                
                if (occupantList == null)
                {
                    MelonLogger.Error("occupantList is null!");
                    return -1;
                }

                string[] occupants = occupantList.text.Split(Environment.NewLine.ToCharArray());
                int occupantCount = occupants.Length - 1; // There's an extra newline at the end of the string.

                if (intercomButton == null || lockIndicator == null || occupantList == null)
                {
                    return -1;
                }

                // Check for a name that matches an incognito name.
                bool isIncognito = occupantList.text.Contains("❤  ########");

                // If the lock indicator is active then that means the door is locked.
                bool isLocked = lockIndicator.activeSelf;

                // The intercom button will be disabled if the room is on Do Not Disturb.
                bool isDoNotDisturb = !intercomButton.activeSelf;

                score += isLocked ? 3 : 0;                                                              // If the room is locked, add 3 to the score.

                score += isDoNotDisturb ? 2 : 0;                                                        // If the room is do not disturb, add 2 to the score.

                // TODO: If there's 2 people exactly in the room, implement a distance check between
                //       both users in the room and score based on if they're below a set threshold.
                // NOTE: You can get the two room users by checking each player in the game and find
                //       the two players closest to the GameObject position of the room itself.
                score += (occupantCount == 2) ? 2 : ((occupantCount < 4 && occupantCount > 0) ? 1 : 0); // If there's less than 4 people in the room, add 1 to the score.
                                                                                                        // If there's exactly 2 people in the room, add 2 instead.

                score += isIncognito ? 3 : 0;                                                           // If the room is in incognito, add 3 to the score.

                return score;
            }
            catch (Exception e)
            {
                GameObject.Find("Penthouse")?.SetActive(true);
                MelonLogger.Error($"Exception caught in JustBClub.CalculateRoomScore!\nMessage: {e.Message}\nSource: {e.Source}\n\nSTACKTRACE:\n{e.StackTrace}\n");
                return -1;
            }
        }

        public static void Initialize(string sceneName)
        {
            // Check if Just B Club was loaded.
            if (sceneName.Contains("jbd") || sceneName.Contains("jbc"))
            {
                worldLoaded = true;
            }
            else
            {
                worldLoaded = false;
                roomsInitialized = false;
                privateRooms.Clear();
            }

            // Store room objects if we're on Just B Club.
            if (worldLoaded && !roomsInitialized)
            {
                InitializeRooms();
            }
        }

        /// <summary>
        /// Initializes the rooms so we can generate buttons for each room.
        /// </summary>
        public static void InitializeRooms()
        {
            try
            {
                GameObject bedroomObject = GameObject.Find("Bedrooms");

                if (bedroomObject == null)
                {
                    MelonLogger.Msg("bedroomObject is null!");
                    return;
                }

                // Save the positions of the private rooms here.
                privateRooms.Add(new PrivateRoom(1, new Vector3(-217.7101f, -11.755f, 151.0652f), GameObject.Find("Bedrooms/Bedroom 1")));
                privateRooms.Add(new PrivateRoom(2, new Vector3(-217.3516f, 55.245f, -91.66356f), GameObject.Find("Bedrooms/Bedroom 2")));
                privateRooms.Add(new PrivateRoom(3, new Vector3(-119.0256f, -11.755f, 151.1068f), GameObject.Find("Bedrooms/Bedroom 3")));
                privateRooms.Add(new PrivateRoom(4, new Vector3(-116.8698f, 55.245f, -91.59067f), GameObject.Find("Bedrooms/Bedroom 4")));
                privateRooms.Add(new PrivateRoom(5, new Vector3(-18.62112f, -11.755f, 150.9862f), GameObject.Find("Bedrooms/Bedroom 5")));
                privateRooms.Add(new PrivateRoom(6, new Vector3(-17.56843f, 55.245f, -91.55622f), GameObject.Find("Bedrooms/Bedroom 6")));
                privateRooms.Add(new PrivateRoom(7, new Vector3(58.17721f, 62.3625f, -6.299268f), GameObject.Find("Bedroom VIP")));

                // Ensure that all the rooms are loaded.
                foreach (PrivateRoom privateRoom in privateRooms)
                {
                    roomsInitialized = privateRoom != null;
                }
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Exception caught in JustBClub.InitializeRooms!\nMessage: {e.Message}\nSource: {e.Source}\n\nSTACKTRACE:\n{e.StackTrace}\n");
            }
        }
    }
}

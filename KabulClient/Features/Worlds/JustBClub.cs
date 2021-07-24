using System;
using UnityEngine;
using MelonLoader;
using System.Collections.Generic;

namespace KabulClient.Features.Worlds
{
    class JustBClub
    {
        public class PrivateRoom
        {
            public int roomNumber;
            public Vector3 position;
            public GameObject roomObject;

            public PrivateRoom(int roomNumber, Vector3 position, GameObject roomObject)
            {
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

        public static void Initialize(string sceneName)
        {
            // Check if Just B Club was loaded.
            if (sceneName == "jbc-k1-rooms-manual-sync")
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
                MelonLogger.Error($"Exception caught in JustBClub.InitializeRooms()!\nMessage: {e.Message}\nSource: {e.Source}\n\nSTACKTRACE:\n{e.StackTrace}\n");
            }
        }
    }
}

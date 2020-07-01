// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the room we are currently in. If null, we aren't in any room.")]
	[HelpUrl("")]
	public class PunGetRoomProperties : PunActionBase
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("True if we are in a room.")]
		public FsmBool isInRoom;

			
		[ActionSection("room properties")]
		[UIHint(UIHint.Variable)]
		[Tooltip("the name of the room.")]
		public FsmString RoomName;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("the number of players inthe room.")]
		public FsmInt playerCount;
		
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The limit of players to this room. This property is shown in lobby, too.\n" +
		 	"If the room is full (players count == maxplayers), joining this room will fail..")]
		public FsmInt maxPlayers;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Defines if the room can be joined. If not open, the room is excluded from random matchmaking. \n" +
			"This does not affect listing in a lobby but joining the room will fail if not open.")]
		public FsmBool open;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Defines if the room is listed in its lobby.")]
		public FsmBool visible;

		[UIHint(UIHint.Variable)]
		[Tooltip("Defines if the room autocleanup is enabled")]
		public FsmBool autoCleanUp;

		[UIHint(UIHint.Variable)]
		[Tooltip("Defines the expected users in this room.")]
		[ArrayEditor(VariableType.String)]
		public FsmArray expectedUsers;

		[Tooltip("Custom Properties you have assigned to this room.")]
		[CompoundArray("Room Custom Properties", "property", "value")]
		public FsmString[] customPropertyKeys;
		[UIHint(UIHint.Variable)]
		public FsmVar[] customPropertiesValues;
		
		
		[ActionSection("Events")] 
		
				
		[Tooltip("Send this event if we are in a room.")]
		public FsmEvent isInRoomEvent;
		
		[Tooltip("Send this event if we aren't in any room.")]
		public FsmEvent isNotInRoomEvent;
		
		[Tooltip("Send this event if the room properties were found.")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if the room properties access failed, likely because we are not in a room or because a custom property was not found")]
		public FsmEvent failureEvent;
		
		public override void Reset()
		{
			
			RoomName = null;
			maxPlayers = null;
			open = null;
			visible = null;
			
			playerCount = 0;

			autoCleanUp = null;

			expectedUsers = null;

			isInRoom = null;
			isInRoomEvent = null;
			isNotInRoomEvent = null;
			
			customPropertyKeys = new FsmString[0];
			customPropertiesValues = new FsmVar[0];
			
			successEvent = null;
			failureEvent = null;
			
		}
		
		public override void OnEnter()
		{
			bool ok = getRoomProperties();
			
			
			if (ok)
			{
				Fsm.Event(successEvent);
			}else{
				Fsm.Event(failureEvent);
			}
			
			Finish();
		}
		
		
		bool getRoomProperties()
		{
			Room _room = PhotonNetwork.CurrentRoom;
			bool _isInRoom = _room!=null;
			isInRoom.Value = _isInRoom;
			
			if (_isInRoom )
			{
				if (isInRoomEvent!=null)
				{
					Fsm.Event(isInRoomEvent);
				}
			}else{
				
				if (isNotInRoomEvent!=null)
				{
					Fsm.Event(isNotInRoomEvent);
				}
				return false;
			}

			// we get the room properties
			RoomName.Value = _room.Name;
			maxPlayers.Value = _room.MaxPlayers;
			open.Value = _room.IsOpen;
			visible.Value = _room.IsVisible;
			playerCount.Value = _room.PlayerCount;
			autoCleanUp.Value = _room.AutoCleanUp;
			expectedUsers.Values = _room.ExpectedUsers;

			// get the custom properties
			int i = 0;
			foreach(FsmString key in customPropertyKeys)
			{
				if (_room.CustomProperties.ContainsKey(key.Value))
				{
					PlayMakerUtils.ApplyValueToFsmVar(this.Fsm,customPropertiesValues[i],_room.CustomProperties[key.Value]);
				}else{
					return false;
				}
				i++;
			}
			
			return true;
		}

	}
}
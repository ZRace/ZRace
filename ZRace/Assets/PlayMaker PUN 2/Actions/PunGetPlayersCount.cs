// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the count of players currently using this application. \n" +
		"This is updated on the MasterServer (only) in 5sec intervals (if any count changed).")]
	[HelpUrl("")]
	public class PunGetPlayersCount : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
        [RequiredField]
		[Tooltip("The number of players currently using this application.")]
		public FsmInt playersCount;
		
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
		
		public override void Reset()
		{
			playersCount = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			ExecuteAction();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
            ExecuteAction();
		}
		
		void ExecuteAction()
		{
			playersCount.Value = PhotonNetwork.CountOfPlayers;
			
		}

	}
}
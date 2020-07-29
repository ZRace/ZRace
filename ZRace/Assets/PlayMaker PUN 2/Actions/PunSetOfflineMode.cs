// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Defines if the PhotonNetwork runs offline or not.Offline mode can be set to re-use your multiplayer code in singleplayer game modes." +
		"\n When this is on PhotonNetwork will not create any connections and there is near to " +
		"no overhead. Mostly usefull for reusing RPC's and PhotonNetwork.Instantiate")]
	[HelpUrl("")]
	public class PhotonNetworkSetOfflineMode : FsmStateAction
	{
		[Tooltip("Define if PhotonNetwork should work offline or not.")]
		public FsmBool offlineMode;
		
		public override void Reset()
		{
			offlineMode  = null;
		}

		public override void OnEnter()
		{
			PhotonNetwork.OfflineMode = offlineMode.Value;
			
			Finish();
		}
	}
}
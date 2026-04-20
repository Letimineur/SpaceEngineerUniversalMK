using System;
using System.Collections.Generic;
using Sandbox.Game.GUI;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.Definitions;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Utils;

namespace UniversalMK.GUI
{
	public class StatPlayerHydrogenBottles : ModdedHudStatBase
	{
        protected MyDefinitionId GasDefinitionID = MyResourceDistributorComponent.HydrogenId;
		
		private const double CHECK_INTERVAL_MS = 1500.0;

		private static readonly MyGameTimer TIMER = new MyGameTimer();
		

		private double m_lastCheck;

		
	
		public StatPlayerHydrogenBottles()
		{
			base.Id = MyStringHash.GetOrCompute("player_hydrogen_bottles");
			this.m_lastCheck = 0.0;
			
			MyLog.Default.WriteLineAndConsole("[Universal MK]: created new player_hydrogen_bottles stat");
		}

		public override void Update()
		{
			if (TIMER.ElapsedTimeSpan.TotalMilliseconds - CHECK_INTERVAL_MS < this.m_lastCheck)
			{
				return;
			}
			
			this.m_lastCheck = TIMER.ElapsedTimeSpan.TotalMilliseconds;			
			IMyInventory inventory = null;
			
			try
			{
				var localHumanPlayer = MyAPIGateway.Session.LocalHumanPlayer;
				if(localHumanPlayer == null)
				{
					base.CurrentValue = 0f;
					return;
				}
				
				IMyCharacter localCharacter = localHumanPlayer.Character;
				if (localCharacter == null)
				{
					base.CurrentValue = 0f;
					return;
				}
				
				IMyInventory workInventory = localCharacter.GetInventory(0);
				if (workInventory == null)
				{
					base.CurrentValue = 0f;
					return;
				}

				inventory = workInventory;
			}
			catch(Exception e)
			{
				MyLog.Default.WriteLineAndConsole($"[Universal MK]: Exception while accessing player inventory: {e}");
				base.CurrentValue = 0f;
				return;
			}
			
			if(inventory == null)
			{
				base.CurrentValue = 0f;
				return;
			}
			float statValue = 0f;
            try
			{
				foreach(MyPhysicalInventoryItem item in inventory.GetItems())
				{
					MyObjectBuilder_GasContainerObject gasContainer = item.Content as MyObjectBuilder_GasContainerObject;
					if(gasContainer != null && gasContainer.GasLevel > 1e-06f)
					{
						MyOxygenContainerDefinition def = MyDefinitionManager.Static.GetPhysicalItemDefinition(item.Content.GetId()) as MyOxygenContainerDefinition;
						if(def != null && def.StoredGasId == GasDefinitionID)
						{
							statValue += (float)item.Amount;
						}
					}
				}
				base.CurrentValue = statValue;
			}
			catch(Exception e)
			{
				MyLog.Default.WriteLineAndConsole($"[Universal MK]: Exception while setting player oxygen bottles stat: {e.ToString()}");
				base.CurrentValue = 0f;
				if(MyAPIGateway.Session?.Player != null)
                    MyAPIGateway.Utilities.ShowNotification($"[ ERROR: Universal MK CheckOxygenBottles: {e.Message} | Send SpaceEngineers.Log to mod author ]", 10000, MyFontEnum.Red);
				return;
			}            
			return;
		}
	}
}

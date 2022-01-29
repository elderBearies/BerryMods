using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using static BerryMods.BerryMods;
namespace BerryMods
{
	class JynxArtifact : ArtifactBase
	{
		public override string ArtifactName => "Artifact of Jynxes";
		public override string ArtifactLangTokenName => "ARTIFACT_OF_JYNXES";
		public override string ArtifactDescription => "Start your run with bad luck.";
		public override Sprite ArtifactEnabledIcon => MainAssets.LoadAsset<Sprite>("RoR2UnluckyActive.png");
		public override Sprite ArtifactDisabledIcon => MainAssets.LoadAsset<Sprite>("RoR2UnluckyInactive.png");
		public override void Init(ConfigFile config)
		{
			CreateConfig(config);
			CreateLang();
			CreateArtifact();
			Hooks();
		}
		private void CreateConfig(ConfigFile config)
		{
			//TimesToPrintMessageOnStart = config.Bind<int>("Artifact: " + ArtifactName, "Times to Print Message in Chat", 5, "How many times should a message be printed to the chat on run start?");
		}
		public override void Hooks()
		{
			CharacterBody.onBodyStartGlobal += BadLuck;
		}
		private void BadLuck(CharacterBody characterBody)
        {
			if (NetworkServer.active && ArtifactEnabled && characterBody.isPlayerControlled)
			{
				characterBody.master.luck = -3 + characterBody.inventory.GetItemCount(ItemCatalog.FindItemIndex("Clover")) - characterBody.inventory.GetItemCount(ItemCatalog.FindItemIndex("LunarBadLuck"));
				characterBody.inventory.onInventoryChanged += LuckOverride;
			}

			void LuckOverride()
			{
				if (NetworkServer.active && ArtifactEnabled)
				{
					characterBody.master.luck = -3 + characterBody.inventory.GetItemCount(ItemCatalog.FindItemIndex("Clover")) - characterBody.inventory.GetItemCount(ItemCatalog.FindItemIndex("LunarBadLuck"));
				}
			}
		}
	}
}
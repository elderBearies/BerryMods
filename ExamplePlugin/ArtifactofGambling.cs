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
	class ExampleArtifact : ArtifactBase
	{
		public override string ArtifactName => "Artifact of Gambling";
		public override string ArtifactLangTokenName => "ARTIFACT_OF_GAMBLING";
		public override string ArtifactDescription => "Adds a chance for items to be duplicated, as well as a chance to lose items.";
		public override Sprite ArtifactEnabledIcon => MainAssets.LoadAsset<Sprite>("ArtifactOfGamblingOn.png");
		public override Sprite ArtifactDisabledIcon => MainAssets.LoadAsset<Sprite>("ArtifactOfGamblingOff.png");
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
			Inventory.onServerItemGiven += ItemDupeMaybe;
		}
		private void ItemDupeMaybe(Inventory inventory, ItemIndex index, int n)
        {
			ItemDef itemDef = ItemCatalog.GetItemDef(index);
			if (NetworkServer.active && ArtifactEnabled && itemDef.canRemove && !itemDef.hidden)
            {
				var weightedSelection = new WeightedSelection<System.Action>();
				weightedSelection.AddChoice(() =>
				{
					inventory.GiveItem(itemDef);
					Chat.AddMessage("You try your luck...");
				}, 40f);
				weightedSelection.AddChoice(() =>
				{
					inventory.RemoveItem(itemDef, 999);
					Chat.AddMessage("Your luck fails you...");
				}, 6.9f);
				weightedSelection.AddChoice(() =>
				{
					int stackCount = inventory.GetItemCount(index);
					inventory.GiveItem(itemDef, stackCount);
					Chat.AddMessage("You hit the jackpot!");
				}, 0.1f);
				weightedSelection.AddChoice(() =>
				{
					//this is deliberately blank, nothing is happening here.
				}, 53f);
				System.Action chosenAction = weightedSelection.Evaluate(RoR2Application.rng.nextNormalizedFloat);
				chosenAction();
			}
        }
	}
}